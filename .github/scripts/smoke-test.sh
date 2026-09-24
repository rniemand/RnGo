#!/usr/bin/env bash
# End-to-end smoke test for a built RnGo image against the seeded MariaDB from docker/docker-compose.yaml.
# The image is trimmed, so this guards against reflection-dependent code being removed at publish time.
# Usage: .github/scripts/smoke-test.sh <image>
set -uo pipefail

image=${1:?usage: smoke-test.sh <image>}
project=rngo-smoke
app=rngo-smoke-app
port=18080
base=http://localhost:$port
compose=(docker compose -p "$project" -f "$(dirname "$0")/../../docker/docker-compose.yaml")

cleanup() {
  docker rm -f "$app" >/dev/null 2>&1
  "${compose[@]}" down -v >/dev/null 2>&1
}
trap cleanup EXIT

fail() {
  echo "FAIL: $*"
  docker logs "$app" 2>&1 | tail -40
  exit 1
}

"${compose[@]}" up -d

# TCP only becomes available once the init scripts (schema + seed data) have finished
echo "Waiting for database..."
for _ in $(seq 1 60); do
  "${compose[@]}" exec -T db mariadb --protocol=tcp -h 127.0.0.1 -urngouser -ppassword RnGo \
    -e 'SELECT 1 FROM Links LIMIT 1' >/dev/null 2>&1 && break
  sleep 2
done

docker run -d --name "$app" --network "${project}_default" -p "$port:8080" \
  -e 'ConnectionStrings__RnGo=Server=db;Uid=rngouser;Pwd=password;Database=RnGo;Allow User Variables=true;SslMode=none' \
  "$image" >/dev/null

echo "Waiting for app..."
for _ in $(seq 1 30); do
  status=$(curl -s -o /dev/null -w '%{http_code}' "$base/swagger/v1/swagger.json")
  [ "$status" = 200 ] && break
  sleep 1
done
[ "$status" = 200 ] || fail "swagger.json returned $status"

url="https://example.com/smoke/$(date +%s)"
add=$(curl -s -X POST "$base/links" -H 'Content-Type: application/json' \
  -d "{\"apiKey\":\"f7ff2316-79fd-435b-b48d-5d2d56e57828\",\"url\":\"$url\"}")
echo "POST /links -> $add"
code=$(echo "$add" | sed -n 's/.*"success":true.*"shortCode":"\([^"]*\)".*/\1/p')
[ -n "$code" ] || fail "adding a link did not return a short code"

follow=$(curl -s -o /dev/null -w '%{http_code} %{redirect_url}' "$base/f/$code")
echo "GET /f/$code -> $follow"
[ "$follow" = "302 $url" ] || fail "following the link did not redirect to $url"

unknown=$(curl -s -o /dev/null -w '%{http_code}' "$base/f/UNKNOWN999")
echo "GET /f/UNKNOWN999 -> $unknown"
[ "$unknown" = 400 ] || fail "unknown short code returned $unknown"

count=$(curl -s "$base/links/count")
echo "GET /links/count -> $count"
[ "$count" -gt 0 ] 2>/dev/null || fail "link count was '$count'"

echo "Smoke test passed"
