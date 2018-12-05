using System;
using System.IO;
using GestMembrosSUD.Models;
using SQLite;

namespace GestMembrosSUD.Helpers
{
    public class Data
    {
        string dbPath;
        SQLiteConnection db;


        public Data()
        {
            dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "Members.db3");
        }

        public SQLiteConnection ListMembers()
        {
            return new SQLiteConnection(dbPath);
        }
    }
}
