BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "EngineIniSettings" (
	"Id"	INTEGER NOT NULL,
	"SettingName"	TEXT NOT NULL UNIQUE,
	"SettingDescription"	TEXT NOT NULL,
	"MinValue"	TEXT NOT NULL,
	"MaxValue"	TEXT NOT NULL,
	"DefaultValue"	TEXT NOT NULL,
	"ValueType"	TEXT NOT NULL,
	PRIMARY KEY("Id")
);
CREATE TABLE IF NOT EXISTS "EngineIniWorkSets" (
	"Id"	INTEGER NOT NULL,
	"WorkSetName"	TEXT NOT NULL,
	"WorkSetDescription"	TEXT NOT NULL,
	PRIMARY KEY("Id")
);
CREATE TABLE IF NOT EXISTS "EngineIniWorkSetConnectors" (
	"Id"	INTEGER NOT NULL,
	"EngineIniSettingId"	INTEGER NOT NULL,
	"EngineIniWorkSetId"	INTEGER NOT NULL,
	PRIMARY KEY("Id"),
	FOREIGN KEY("EngineIniSettingId") REFERENCES "EngineIniSettings"("Id") ON DELETE CASCADE,
	FOREIGN KEY("EngineIniWorkSetId") REFERENCES "EngineIniWorkSets"("Id") ON DELETE CASCADE,
	UNIQUE("EngineIniSettingId","EngineIniWorkSetId")
);

CREATE VIEW IF NOT EXISTS SettingsView (WorkSetId, WorkSetName, SettingId, SettingName)
AS
SELECT EngineIniWorkSets.Id as WorkSetId, EngineIniWorkSets.WorkSetName as WorkSetName, EngineIniSettings.Id as SettingId, EngineIniSettings.SettingName as SettingName 
	FROM EngineIniSettings, EngineIniWorkSets, EngineIniWorkSetConnectors
	WHERE EngineIniSettings.Id= EngineIniWorkSetConnectors.EngineIniSettingId AND  EngineIniWorkSets.Id= EngineIniWorkSetConnectors.EngineIniWorkSetId;
COMMIT;
