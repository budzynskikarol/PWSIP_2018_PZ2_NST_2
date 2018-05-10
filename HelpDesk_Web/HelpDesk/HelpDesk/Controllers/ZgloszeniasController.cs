using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelpDesk.Models;

namespace HelpDesk.Controllers
{
    public class ZgloszeniasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Zgloszenias
        public ActionResult Index()
        {
            var zgloszenias = db.Zgloszenias.Include(z => z.Kategorie).Include(z => z.Statusy);
            return View(zgloszenias.ToList());
        }

        // GET: Zgloszenias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zgloszenia zgloszenia = db.Zgloszenias.Find(id);
            if (zgloszenia == null)
            {
                return HttpNotFound();
            }
            return View(zgloszenia);
        }

        // GET: Zgloszenias/Create
        public ActionResult Create()
        {
            ViewBag.KategorieId = new SelectList(db.Kategories, "Id", "Nazwa");
            ViewBag.StatusyId = new SelectList(db.Statusys, "Id", "Nazwa");
            return View();
        }

        // POST: Zgloszenias/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nazwa,Opis,Komentarz,StatusyId,KategorieId,Uzytkownik")] Zgloszenia zgloszenia)
        {
            if (ModelState.IsValid)
            {
                db.Zgloszenias.Add(zgloszenia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategorieId = new SelectList(db.Kategories, "Id", "Nazwa", zgloszenia.KategorieId);
            ViewBag.StatusyId = new SelectList(db.Statusys, "Id", "Nazwa", zgloszenia.StatusyId);
            return View(zgloszenia);
        }

        // GET: Zgloszenias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zgloszenia zgloszenia = db.Zgloszenias.Find(id);
            if (zgloszenia == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategorieId = new SelectList(db.Kategories, "Id", "Nazwa", zgloszenia.KategorieId);
            ViewBag.StatusyId = new SelectList(db.Statusys, "Id", "Nazwa", zgloszenia.StatusyId);
            return View(zgloszenia);
        }

        // POST: Zgloszenias/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nazwa,Opis,Komentarz,StatusyId,KategorieId,Uzytkownik")] Zgloszenia zgloszenia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zgloszenia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategorieId = new SelectList(db.Kategories, "Id", "Nazwa", zgloszenia.KategorieId);
            ViewBag.StatusyId = new SelectList(db.Statusys, "Id", "Nazwa", zgloszenia.StatusyId);
            return View(zgloszenia);
        }

        // GET: Zgloszenias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zgloszenia zgloszenia = db.Zgloszenias.Find(id);
            if (zgloszenia == null)
            {
                return HttpNotFound();
            }
            return View(zgloszenia);
        }

        // POST: Zgloszenias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Zgloszenia zgloszenia = db.Zgloszenias.Find(id);
            db.Zgloszenias.Remove(zgloszenia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
