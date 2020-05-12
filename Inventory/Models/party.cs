using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase;
using System.ComponentModel.DataAnnotations;
namespace Inventory.Models
{
    public class party
    {
        public Int64 id { get; set; }
        [Required]
        public string name { get; set; }
        public bool IsVendor { get; set; }
        db database = new db();
        public void Create()
        {
            string querry = "insert into Party(Name,IsVendor)values(@name,@ven)";
            var x = new SqlParm();
            x.Add("name", name);
            x.Add("ven", IsVendor);
            database.ExecuteQuerry(querry, x.GetParmList());
        }
        public void delete()
        {
            database.ExecuteQuerry("delete from party where PartyId="+id);
        }
        public void Update()
        { SqlParm s1 = new SqlParm();
            s1.Add("name", name);
            s1.Add("ven", IsVendor);
            database.ExecuteQuerry("update party set Name=@name ,IsVendor=@ven where PartyId=" + id, s1.GetParmList());
        }
        public static List<party> Getparties()
        {
            string x = "select * from party";
            var db = new db();
            var data = db.Read(x);
            var parties = new List<party>();
            foreach (System.Data.DataRow row in data.Tables[0].Rows)
            {
                parties.Add(new party { id = Convert.ToInt64(row[0]), name = row[1].ToString(), IsVendor = Convert.ToBoolean(row[2]) });
            }

            return parties;
        }
        public static System.Data.DataSet GetParty(int id)
        {
            string x = "select * from party where partyid="+id;
            var db = new db();
            var data = db.Read(x);
            return data;
        }
    }
}