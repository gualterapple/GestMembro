using System;
using SQLite;

namespace GestMembrosSUD.Models
{
    [Table("Member")]
    public class Member
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
        public string Photo
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public int Age
        {
            get;
            set;
        }
        public string Capela
        {
            get;
            set;
        }

    }
}
