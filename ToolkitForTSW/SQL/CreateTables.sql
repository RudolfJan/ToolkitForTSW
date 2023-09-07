﻿BEGIN TRANSACTION; 

CREATE TABLE IF NOT EXISTS "Editions" (
  "Id" INTEGER PRIMARY KEY NOT NULL,
  "EditionOrder" INTEGER NOT NULL,
  "EditionName" TEXT NOT NULL UNIQUE ON CONFLICT IGNORE,
   "EditionLongName" TEXT NOT NULL UNIQUE ON CONFLICT IGNORE,
  "SteamGameId" TEXT NOT NULL,
  "SteamProgramPath" TEXT NOT NULL,
  "EGSProgramPath" TEXT NOT NULL,
  "Selected" INTEGER NOT NULL DEFAULT 0,
  "Description"	TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS RadioStations (
        Id  INTEGER PRIMARY KEY NOT NULL,
        Url TEXT  NOT NULL,
        RouteName TEXT NOT NULL,
        Description TEXT NOT NULL
         );

    CREATE TABLE IF NOT EXISTS Routes (
        Id  INTEGER PRIMARY KEY NOT NULL,
        RouteName TEXT NOT NULL,
        RouteAbbrev TEXT NOT NULL UNIQUE ON CONFLICT IGNORE,
        RouteDescription  TEXT NOT NULL,
        RouteImagePath TEXT NOT NULL,
        ScenarioPlannerRouteName TEXT NUL NULL,
        ScenarioPlannerRouteString TEXT NOT NULL,
        DlcId INT,
        FOREIGN KEY("DlcId") REFERENCES "Dlcs" ON DELETE CASCADE
        );

    CREATE TABLE IF NOT EXISTS Scenarios (
        Id  INTEGER PRIMARY KEY NOT NULL,
        ScenarioName TEXT NOT NULL,
        ScenarioGuid  TEXT NOT NULL UNIQUE ON CONFLICT IGNORE,
        RouteId INTEGER NOT NULL,
        FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
        );

    CREATE TABLE IF NOT EXISTS Dlcs (
        Id  INTEGER PRIMARY KEY NOT NULL,
        DlcName TEXT NOT NULL,
        DlcDescription TEXT NOT NULL,
        DlcImagePath TEXT NOT NULL,
        DlcSource TEXT NOT NULL
        );

CREATE TABLE IF NOT EXISTS GameSaves (
        Id  INTEGER PRIMARY KEY NOT NULL,
        Activity TEXT  NOT NULL,
        RouteAbbreviation TEXT NOT NULL,
        SaveName TEXT NOT NULL,
        Description TEXT NOT NULL
        );

CREATE TABLE IF NOT EXISTS Mods (
  Id INTEGER PRIMARY KEY NOT NULL,
  ModName TEXT NOT NULL,
  FilePath TEXT UNIQUE ON CONFLICT IGNORE,
  FileName TEXT  NOT NULL,
  DLCName	TEXT NOT NULL,
  ModDescription	TEXT NOT NULL,
  ModImage	TEXT NOT NULL,
  ModSource	TEXT NOT NULL,
  ModType	TEXT NOT NULL,
  ModVersion TEXT NOT NULL DEFAULT "",
  IsInstalledSteam	INTEGER NOT NULL DEFAULT 0,
  IsInstalledEGS	INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS `ModSets` (
  `Id`	INTEGER PRIMARY KEY NOT NULL,
  ModSetName TEXT NOT NULL,
  `ModSetDescription`	TEXT  NOT NULL,
  `RouteId`	INTEGER DEFAULT null,
  FOREIGN KEY("RouteId") REFERENCES "Routes" ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS `ModModSet` (
  Id	INTEGER PRIMARY KEY NOT NULL,
  ModSetId	INTEGER DEFAULT null,
  ModId	INTEGER DEFAULT null,
  FOREIGN KEY("ModSetId") REFERENCES "ModSets" ON DELETE CASCADE
  FOREIGN KEY("ModId") REFERENCES "Mods" ON DELETE CASCADE
);


COMMIT;