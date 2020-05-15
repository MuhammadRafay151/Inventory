using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Inventory.CustomValidation;
using System.Data.SqlClient;
using System.Configuration;
namespace Inventory.Models
{
    public class SalesInvoice
    {
        public Int64 SaleInvoiceId { get; set; }
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
        public void Create()
        {
           var x= System.Configuration.ConfigurationManager.
       ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(x);
            DataBase.SqlParm sqlParm = new DataBase.SqlParm();
            con.Open();
            SqlTransaction t1 ;
            t1 = con.BeginTransaction();
            try
            {
                var querry = @"insert into SaleInvoice(Date,InvoiceTypeId,LocationId,PartyId,TotalAmount)values(@dt,@tid,@loc,@pid,@cb);select SCOPE_IDENTITY()
";
                sqlParm.Add("dt", Date);
                sqlParm.Add("tid", InvoiceTypeId);
                sqlParm.Add("loc", LocationId);
                sqlParm.Add("pid", PartyId);
                sqlParm.Add("cb", CalculateBill());
                var cmd = getsqlcommand(querry, sqlParm.GetParmList(), con, t1);
                SaleInvoiceId=long.Parse( cmd.ExecuteScalar().ToString());
                InsertList(con, t1);
                t1.Commit();
            }
            catch (Exception)
            {
                t1.Rollback();
            }
            con.Close();
        }
        private SqlCommand getsqlcommand(string querry, List<SqlParameter> sqlParameters,SqlConnection sqlConnection,SqlTransaction sqlTransaction)
        {
            SqlCommand cmd = new SqlCommand(querry, sqlConnection, sqlTransaction);
            foreach(var i in sqlParameters)
            {
                cmd.Parameters.Add(i);
            }
            return cmd;
        }
        private void InsertList(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            var querry = @"
INSERT INTO [dbo].[SaleInvoiceDetail]
           ([SaleInvoiceId]
           ,[ItemId]
           ,[Qty]
           ,[SalePrice]
           ,[Total])
     VALUES
           (@SaleInvoiceId,@ItemId,@Qty,@SalePrice,@Total)";
            foreach(var x in SaleInvoiceDetails)
            {
                var parm = new DataBase.SqlParm();
                parm.Add("@SaleInvoiceId",SaleInvoiceId);
                parm.Add("@ItemId",x.ItemId);
                parm.Add("@Qty",x.Qty);
                parm.Add("@SalePrice",x.SalePrice);
                parm.Add("@Total",(Decimal)x.Qty*x.SalePrice);
                getsqlcommand(querry, parm.GetParmList(), sqlConnection, sqlTransaction).ExecuteNonQuery();
            }
        }
        private Decimal CalculateBill()
        {
            Decimal total=0;
            foreach(var x in SaleInvoiceDetails)
            {
                total += x.Qty * x.SalePrice;
            }
            return total;
        }
        public static System.Data.DataSet Get()
        {
            var list = new List<SalesInvoice>();
            DataBase.db database = new DataBase.db();
          return  database.Read(@"select SaleInvoice.SaleInvoiceId,SaleInvoice.Date,InvoiceType.InvoiceType,
Location.Name as location, Party.Name as Party, SaleInvoice.TotalAmount
from SaleInvoice inner join InvoiceType on InvoiceType.InvoiceTypeId = SaleInvoice.InvoiceTypeId
inner
join Location on Location.LocationId = SaleInvoice.LocationId
inner
join Party on Party.PartyId = SaleInvoice.PartyId");


        }
    }
}