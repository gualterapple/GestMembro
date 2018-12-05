using System;
using SQLite;

namespace GestMembrosSUD.Models
{
    public class Model1
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
    }
}
