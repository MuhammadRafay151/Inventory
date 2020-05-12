using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Inventory.Models;
using Newtonsoft.Json;
namespace Inventory.Controllers
{
    public class PartyController : Controller
    {
        // GET: Party
        public async Task<ActionResult> Index()
        {
            await Task.Run(() =>
            {
                ViewBag.data = party.Getparties();
            });

            return View();
        }

        // GET: Party/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create([Form]party p1)
        {
            try
            {
                if (ModelState.IsValid)
                    p1.Create();
                else
                {
                    return View("Index", p1);
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View("Index", p1);

            }
        }

        // GET: Party/Edit/5
        public async Task<JsonResult> Edit(int id)
        {
            System.Data.DataSet ds = null;
            await Task.Run(() =>
             {
                 ds = party.GetParty(id);
             });
          
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]),JsonRequestBehavior.AllowGet);
        }

        // POST: Party/Edit/5
        [HttpPost]
        public ActionResult Edit([Form]party p1)
        {
            try
            {
                // TODO: Add update logic here
                p1.Update();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Party/Delete/5
        public ActionResult Delete(int id)
        {
            party p1 = new party() { id = id };
            p1.delete();
            return RedirectToAction("Index");
        }

    }
}
