namespace RnGo.Core.Entities;

public class GenericCountEntity
{
  public long CountLong { get; set; }

  public GenericCountEntity() { }

  public GenericCountEntity(long count)
  {
    CountLong = count;
  }
}
