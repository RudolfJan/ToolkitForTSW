BEGIN TRANSACTION;
DROP TABLE IF EXISTS `LiverySet`;
CREATE TABLE IF NOT EXISTS `LiverySet` (
	`Id`	INTEGER,
	`Description`	TEXT,
	`Route`	TEXT,
	PRIMARY KEY(`Id`)
);
DROP TABLE IF EXISTS `LiveryLiverySet`;
CREATE TABLE IF NOT EXISTS `LiveryLiverySet` (
	`Id`	INTEGER,
	`LiverySetId`	INTEGER DEFAULT null,
	`LiveryId`	INTEGER DEFAULT null,
	PRIMARY KEY(`Id`)
);
DROP TABLE IF EXISTS `Livery`;
CREATE TABLE IF NOT EXISTS `Livery` (
	`Id`	INTEGER,
	`Name`	TEXT,
	`FileName`	TEXT,
	`Description`	TEXT,
	`Image`	TEXT,
	`Source`	TEXT,
	`LiveryType`	TEXT,
	`ReplaceName`	TEXT
);
COMMIT;
