using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Models
{
    public class Item
    {
        public Int64 ItemId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }

        public static List<Item> Get()
        {
            db database = new db();
            List<Item> l1 = new List<Item>();
            var x = database.Read("select * from Item");
            foreach (System.Data.DataRow d1 in x.Tables[0].Rows)
            {
                l1.Add(new Item() { ItemId = Convert.ToInt64(d1[0]), Name = d1[1].ToString(), IsActive = Convert.ToBoolean(d1[2]) ,
                PurchasePrice=Convert.ToDecimal(d1[3]),SalePrice=Convert.ToDecimal(d1[4])
                });
            }
            return l1;
        }
        public static Item Get(int id)
        {
            return null;
        }
        public void Create() { }
        public void Update() { }
        public void delete() { }
    }
}