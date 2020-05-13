using Inventory.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory.Models
{
    public class SaleInvoiceDetail
    {
        public long SaleInvoiceDetailId { get; set; }
        public int SaleInvoiceId { get; set; }
        [IsSelected(ErrorMessage ="Please Select value")]
        public int ItemId { get; set; }
        [Required]
        public int Qty { get; set; }
        public decimal SalePrice { get; set; }
        public decimal Total { get; set; }
    }
}