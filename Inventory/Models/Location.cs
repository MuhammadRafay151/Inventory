using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase;
using System.ComponentModel.DataAnnotations;
namespace Inventory.Models
{
    public class Location
    { db database = new db();
        public Int64 id { get; set; }
        [Required]
        public string name { get; set; }
        public bool IsShop { get; set; }
        public void create()
        {
            string querry = "insert into Location (Name,IsShop)values(@name,@ish)";
            SqlParm s1 = new SqlParm();
            s1.Add("name",name);
            s1.Add("ish", IsShop);
            database.ExecuteQuerry(querry,s1.GetParmList());
        }
        public void Update()
        {
            string querry = "update Location set Name=@name,IsShop=@ish where LocationId="+id;
            SqlParm s1 = new SqlParm();
            s1.Add("name", name);
            s1.Add("ish", IsShop);
            database.ExecuteQuerry(querry, s1.GetParmList());
        }
        public void Delete()
        {
            string querry = "delete from Location where LocationId="+id;
            database.ExecuteQuerry(querry);
        }
        public static List<Location> GetLocations()
        {
            db database = new db();
            List<Location> l1=new List<Location>();
            var x= database.Read("select * from location");
            foreach(System.Data.DataRow d1 in x.Tables[0].Rows)
            {
                l1.Add(new Location() { id = Convert.ToInt64(d1[0]), name = d1[1].ToString(), IsShop = Convert.ToBoolean(d1[2]) });
            }
            return l1;
        }
        public static System.Data.DataSet GetLocation(int id)
        {

            db d1 = new db();
            return d1.Read("select * from location where LocationId="+id);
           
        }
    }
}