using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Create(SalesInvoice s1)
        {
            if (ModelState.IsValid)
            {
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


    }
}
