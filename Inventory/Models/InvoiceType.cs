using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DataBase;
namespace Inventory.Models
{
    public class InvoiceType
    {
        public Int64 id { get; set; }
        [Required]
        public string type { get; set; }
        db database = new db();
        public void Create()
        {
            string querry = "insert into InvoiceType (InvoiceType)values(@type)";
            SqlParm s1 = new SqlParm();
            s1.Add("type", type);
            database.ExecuteQuerry(querry, s1.GetParmList());
        }
        public void Update()
        {
            string querry = "update InvoiceType set InvoiceType=@type where InvoiceTypeId=" + id;
            SqlParm s1 = new SqlParm();
            s1.Add("type", type);
            database.ExecuteQuerry(querry, s1.GetParmList());
        }
        public void Delete()
        {
            database.ExecuteQuerry("delete from InvoiceType where InvoiceTypeId=" + id);
        }
        public static List<InvoiceType> get()
        {
            db database = new db();
            List<InvoiceType> l1 = new List<InvoiceType>();
            var x = database.Read("select * from InvoiceType");
            foreach (DataRow row in x.Tables[0].Rows)
            {
                l1.Add(new InvoiceType() { id = Convert.ToInt64(row[0]), type = row[1].ToString() });
            }
            
            return l1;
        }
        public static DataSet get(int id)
        {
            var x = new db();
            return x.Read("select * from InvoiceType where InvoiceTypeId="+id) ;
        }

    }
}