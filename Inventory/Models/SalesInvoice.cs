using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Inventory.CustomValidation;
namespace Inventory.Models
{
    public class SalesInvoice
    {
        public int SaleInvoiceId { get; set; }
        [Required]
        public System.DateTime Date { get; set; }
        [IsSelected(ErrorMessage = "Please Select value")]
        public long InvoiceTypeId { get; set; }
        [IsSelected(ErrorMessage = "Please Select value")]
        public long LocationId { get; set; }
        [IsSelected(ErrorMessage = "Please Select value")]
        public long PartyId { get; set; }
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Kindly Add Details")]
        public  List<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
    }
}