CREATE TABLE `ApiKeys` (
	`ApiKeyId` INT(11) NOT NULL AUTO_INCREMENT,
	`DateAddedUtc` DATETIME NOT NULL DEFAULT utc_timestamp(6),
	`Deleted` BIT(1) NOT NULL DEFAULT b'0',
	`Enabled` BIT(1) NOT NULL DEFAULT b'1',
	`ApiKey` VARCHAR(50) NOT NULL DEFAULT '' COLLATE 'utf8_general_ci',
	PRIMARY KEY (`ApiKeyId`) USING BTREE,
	INDEX `Deleted` (`Deleted`) USING BTREE,
	INDEX `Enabled` (`Enabled`) USING BTREE
)
COLLATE='utf8_general_ci'
ENGINE=InnoDB;


CREATE TABLE `Links` (
	`LinkId` BIGINT(20) NOT NULL AUTO_INCREMENT,
	`Deleted` BIT(1) NOT NULL DEFAULT b'0',
	`DateAddedUtc` DATETIME NOT NULL DEFAULT utc_timestamp(6),
	`FollowCount` INT(11) NOT NULL DEFAULT '0',
	`DateLastFollowedUtc` DATETIME NOT NULL DEFAULT utc_timestamp(6),
	`ShortCode` VARCHAR(16) NULL DEFAULT NULL COLLATE 'utf8_general_ci',
	`Url` VARCHAR(1024) NOT NULL DEFAULT '' COLLATE 'utf8_general_ci',
	PRIMARY KEY (`LinkId`) USING BTREE,
	INDEX `Deleted` (`Deleted`) USING BTREE
)
COLLATE='utf8_general_ci'
ENGINE=InnoDB;
