using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Inventory.CustomValidation;
using System.Data.SqlClient;
using System.Configuration;
using DataBase;
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
        public List<SaleInvoiceDetail> SaleInvoiceDetails { get; set; }
        public void Create()
        {
            var x = System.Configuration.ConfigurationManager.
        ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(x);
            DataBase.SqlParm sqlParm = new DataBase.SqlParm();
            con.Open();
            SqlTransaction t1;
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
                SaleInvoiceId = int.Parse(cmd.ExecuteScalar().ToString());
                InsertList(con, t1);
                t1.Commit();
            }
            catch (Exception)
            {
                t1.Rollback();
            }
            con.Close();
        }
        private SqlCommand getsqlcommand(string querry, List<SqlParameter> sqlParameters, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            SqlCommand cmd = new SqlCommand(querry, sqlConnection, sqlTransaction);
            foreach (var i in sqlParameters)
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
            foreach (var x in SaleInvoiceDetails)
            {
                var parm = new DataBase.SqlParm();
                parm.Add("@SaleInvoiceId", SaleInvoiceId);
                parm.Add("@ItemId", x.ItemId);
                parm.Add("@Qty", x.Qty);
                parm.Add("@SalePrice", x.SalePrice);
                parm.Add("@Total", (Decimal)x.Qty * x.SalePrice);
                getsqlcommand(querry, parm.GetParmList(), sqlConnection, sqlTransaction).ExecuteNonQuery();
            }
        }
        private void InsertList(List<SaleInvoiceDetail> items,SqlConnection sqlConnection, SqlTransaction sqlTransaction)
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
            foreach (var x in items)
            {
                var parm = new DataBase.SqlParm();
                parm.Add("@SaleInvoiceId", SaleInvoiceId);
                parm.Add("@ItemId", x.ItemId);
                parm.Add("@Qty", x.Qty);
                parm.Add("@SalePrice", x.SalePrice);
                parm.Add("@Total", (Decimal)x.Qty * x.SalePrice);
                getsqlcommand(querry, parm.GetParmList(), sqlConnection, sqlTransaction).ExecuteNonQuery();
            }
        }
        private Decimal CalculateBill()
        {
            Decimal total = 0;
            foreach (var x in SaleInvoiceDetails)
            {
                if (!x.Isdeleted)
                    total += x.Qty * x.SalePrice;
            }
            return total;
        }
        public void delete()
        {
            db x = new db();
            x.ExecuteQuerry("delete from saleinvoice where SaleInvoiceId=" + SaleInvoiceId);
        }
        public static System.Data.DataSet Get()
        {
            var list = new List<SalesInvoice>();
            DataBase.db database = new DataBase.db();
            return database.Read(@"select SaleInvoice.SaleInvoiceId,SaleInvoice.Date,InvoiceType.InvoiceType,
Location.Name as location, Party.Name as Party, SaleInvoice.TotalAmount
from SaleInvoice inner join InvoiceType on InvoiceType.InvoiceTypeId = SaleInvoice.InvoiceTypeId
inner
join Location on Location.LocationId = SaleInvoice.LocationId
inner
join Party on Party.PartyId = SaleInvoice.PartyId");


        }
        public static SalesInvoice GetSaleInvoice(int id)
        {
            string Querry = string.Format(@"select * from SaleInvoice where SaleInvoiceId={0}
select * from SaleInvoiceDetail where SaleInvoiceId ={0}", id);
            DataBase.db database = new DataBase.db();
            var x = database.Read(Querry);
            SalesInvoice s1 = new SalesInvoice()
            {
                SaleInvoiceId = Convert.ToInt32(x.Tables[0].Rows[0][0]),
                Date = Convert.ToDateTime(x.Tables[0].Rows[0][1]),
                InvoiceTypeId = Convert.ToInt64(x.Tables[0].Rows[0][2]),
                LocationId = Convert.ToInt64(x.Tables[0].Rows[0][3]),
                PartyId = Convert.ToInt64(x.Tables[0].Rows[0][4]),
                TotalAmount = Convert.ToDecimal(x.Tables[0].Rows[0][5])

            };
            s1.SaleInvoiceDetails = new List<SaleInvoiceDetail>();
            foreach (System.Data.DataRow row in x.Tables[1].Rows)
            {
                s1.SaleInvoiceDetails.Add(new SaleInvoiceDetail()
                {
                    SaleInvoiceDetailId = Convert.ToInt64(row[0]),
                    SaleInvoiceId = Convert.ToInt32(row[1]),
                    ItemId = Convert.ToInt32(row[2]),
                    Qty = Convert.ToInt32(row[3]),
                    SalePrice = Convert.ToInt32(row[4]),
                    Total = Convert.ToDecimal(row[5])
                });
            }
            return s1;
        }
        public static System.Data.DataSet GetSaleInvoiceDetails(int id)
        {
            db database = new db();
            return database.Read(string.Format(@"select Item.Name,SaleInvoiceDetail.Qty,SaleInvoiceDetail.SalePrice,SaleInvoiceDetail.Total
from SaleInvoiceDetail inner join Item on SaleInvoiceDetail.ItemId = Item.ItemId where SaleInvoiceDetail.SaleInvoiceId ={0};
select sum(SaleInvoiceDetail.Total)count
from SaleInvoiceDetail inner join Item on SaleInvoiceDetail.ItemId = Item.ItemId where SaleInvoiceDetail.SaleInvoiceId={0};
", id));
        }
        public void Update()
        {
            string querry = @"UPDATE [dbo].[SaleInvoice]
   SET[Date] =@dt
      ,[InvoiceTypeId] =@tid 
      ,[LocationId] = @loc
      ,[PartyId] = @pid
      ,[TotalAmount] = @cb
 WHERE SaleInvoiceId = @id";
            var x = System.Configuration.ConfigurationManager.
       ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(x);
            DataBase.SqlParm sqlParm = new DataBase.SqlParm();
            con.Open();
            SqlTransaction t1;
            t1 = con.BeginTransaction();
            try
            {
                sqlParm.Add("dt", Date);
                sqlParm.Add("tid", InvoiceTypeId);
                sqlParm.Add("loc", LocationId);
                sqlParm.Add("pid", PartyId);
                sqlParm.Add("id", SaleInvoiceId);
                sqlParm.Add("cb", CalculateBill());
                var cmd = getsqlcommand(querry, sqlParm.GetParmList(), con, t1);
                cmd.ExecuteNonQuery();
                UpdateList(con, t1);
                t1.Commit();
            }
            catch (Exception)
            {
                t1.Rollback();
            }
            con.Close();
        }
        private void UpdateList(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            var newitems = new List<SaleInvoiceDetail>();
            var querry = @"UPDATE [dbo].[SaleInvoiceDetail]
      SET 
      [ItemId] = @ItemId
      ,[Qty] = @Qty
      ,[SalePrice] = @SalePrice
      ,[Total] = @Total
 WHERE SaleInvoiceId=@SaleInvoiceId and SaleInvoiceDetailId=@SaleInvoiceDetailId";
            var querry2 = "delete from SaleInvoiceDetail where SaleInvoiceDetailId=@SaleInvoiceDetailId and SaleInvoiceId=@SaleInvoiceId";
            foreach (var x in SaleInvoiceDetails)
            {
                var parm = new DataBase.SqlParm();
                if (x.Isdeleted)
                {
                    
                    parm.Add("@SaleInvoiceId", SaleInvoiceId);
                    parm.Add("@SaleInvoiceDetailId", x.SaleInvoiceDetailId);
                    getsqlcommand(querry2, parm.GetParmList(), sqlConnection, sqlTransaction).ExecuteNonQuery();

                }
                else if(x.SaleInvoiceDetailId==0)
                {
                    newitems.Add(x);
                }
                else
                {
                    parm.Add("@ItemId", x.ItemId);
                    parm.Add("@Qty", x.Qty);
                    parm.Add("@SalePrice", x.SalePrice);
                    parm.Add("@Total", (Decimal)x.Qty * x.SalePrice);
                    parm.Add("@SaleInvoiceId", SaleInvoiceId);
                    parm.Add("@SaleInvoiceDetailId", x.SaleInvoiceDetailId);
                    getsqlcommand(querry, parm.GetParmList(), sqlConnection, sqlTransaction).ExecuteNonQuery();
                }

              
              
            }
            if(newitems.Count>0)
            {
                InsertList(newitems, sqlConnection, sqlTransaction);
                newitems = null;
            }
        }
    }
}