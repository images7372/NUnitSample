using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MediaLibrary.Models;
using MediaLibrary.Services;
using MediaLibrary.ViewModels.Recording;

namespace MediaLibrary.Controllers
{
    public class RecordingsController : Controller
    {
        [Obsolete("ロジックをServiceで実装する為、使用禁止")]
        private MediaLibraryContext db = new MediaLibraryContext();
        private IRepository _repos;

        public RecordingsController() : this (new Repository()){ }

        public RecordingsController(IRepository repos)
        {
            _repos = repos;
        }

        public ActionResult Index()
        {
            var service = RecordingService.GetInstance(_repos);
            return View(service.GetAll());
        }

        //TODO:Service経由に修正予定
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recording recording = db.Recordings.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }
            return View(recording);
        }

        // GET: Recordings/Create
        public ActionResult Create()
        {
            var service = RecordingService.GetInstance(_repos);

            return View(service.CreateCreateViewModel());
        }

        // POST: Recordings/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ReleaseDate,SelectedLabelId,SelectedArtistId,TrackTitles,Durations")]CreateViewModel recording)
        {
            var service = Services.RecordingService.GetInstance(_repos);
            if (ModelState.IsValid)
            {
                service.Insert(recording);
                return RedirectToAction("Index");
            }

            service.SetListItemSources(recording);
            return View(recording);
        }

        // GET: Recordings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var service = Services.RecordingService.GetInstance(_repos);
            if (service.IsExists(id) == false)
            {
                return HttpNotFound();
            }

            return View(service.GetCreateViewModel(id));
        }

        // POST: Recordings/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、https://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,ReleaseDate,SelectedLabelId,SelectedArtistId,TrackTitles,Durations")]CreateViewModel vm)
        {
            var service = RecordingService.GetInstance(_repos);
            if (ModelState.IsValid)
            {
                service.Update(vm);
                return RedirectToAction("Index");
            }

            service.SetListItemSources(vm);
            return View(vm);
        }

        //TODO:Service経由に修正予定
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recording recording = db.Recordings.Find(id);
            if (recording == null)
            {
                return HttpNotFound();
            }
            return View(recording);
        }

        //TODO:Service経由に修正予定
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recording recording = db.Recordings.Find(id);
            db.Recordings.Remove(recording);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repos.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
