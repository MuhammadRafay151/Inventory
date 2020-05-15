using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Inventory.Models;
namespace Inventory.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Types()
        {
            ViewBag.data = InvoiceType.get();
            return View();
        }
        [HttpPost]
        public ActionResult TypeCreate([Form]InvoiceType i1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    i1.Create();
                }
                else
                {
                    return View("Types", i1);
                }
                return RedirectToAction("Types");
            }
            catch
            {
                return View("Types", i1);
            }
        }
        public ActionResult TypeEdit(int id)
        {
            var x = InvoiceType.get(id);
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(x.Tables[0]), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TypeEdit([Form]InvoiceType i1)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    i1.Update();
                }

                return RedirectToAction("Types");
            }
            catch
            {
                return View("Types");
            }
        }
        public ActionResult TypeDelete(int id)
        {
            InvoiceType i1 = new InvoiceType() { id = id };
            i1.Delete();
            return RedirectToAction("Types");
        }

        public ActionResult Create()
        {
            ViewBag.itypes = InvoiceType.get();
            ViewBag.loc = Location.GetLocations();
            ViewBag.party = party.Getparties();
            ViewBag.items = Item.Get();
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create([Form]SalesInvoice s1)
        {
            if (ModelState.IsValid)
            {
                await Task.Run(() => {
                    s1.Create(); });
             
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.itypes = InvoiceType.get();
                ViewBag.loc = Location.GetLocations();
                ViewBag.party = party.Getparties();
                ViewBag.items = Item.Get();
                return View(s1);
            }
           
        }
        public async Task<ActionResult> Index()
        {
            System.Data.DataSet ds=null;
            await Task.Run(() => { ds=SalesInvoice.Get(); });
            return View(ds);
        }
        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Edit([Form]SalesInvoice s1)
        {
            return View();
        }

    }
}
