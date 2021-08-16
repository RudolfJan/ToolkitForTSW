using SQLiteDatabase.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolkitForTSW.DataAccess
  {
  public class DatabaseFactory: IDatabaseFactory
    {
    public int CreateStructures()
      {
      DbManager.CreateStructureElementFromFile("SQL\\CreateTables.sql");
      DbManager.CreateStructureElementFromFile("SQL\\EngineIniTables.sql");
      if (!DbManager.ColumnExists("Mods", "ModVersion"))
        {
        DbManager.CreateStructureElementFromFile("SQL\\AlterModTable.sql");
        }
      return 0;
      }
    }
  }
