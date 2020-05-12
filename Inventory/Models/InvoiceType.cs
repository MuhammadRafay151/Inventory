using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace Inventory.Models
{
    public class InvoiceType
    {
        public int id { get; set; }
        [Required]
        public string type { get; set; }
        public void Create()
        {

        }
        public void Update()
        {

        }
        public void Delete()
        {

        }
        public static List<InvoiceType> get()
        { return null;
        }
        public static DataSet get(int id)
        {
            return null;
        }

    }
}