using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediaLibrary.Models;
using MediaLibrary.ViewModels.Recording;
using System.Web.Mvc;

namespace MediaLibrary.Services
{
    public class RecordingService
    {
        private IRepository _rep;
        private RecordingService(IRepository rep)
        {
            _rep = rep;
        }

        public static RecordingService GetInstance(IRepository rep)
        {
            return new RecordingService(rep);
        }
        
        public IQueryable<Recording> Recording
        {
            get
            {
                return _rep.GetAll<Recording>();
            }
        }

        public Recording FillById(int id)
        {
            return _rep.FindBy<Recording>(r => r.Id == id).First();
        }

        public CreateViewModel CreateCreateViewModel()
        {
            var ret = new CreateViewModel()
            {
                TrackTitles = new List<string>() { "" },
                Durations = new List<int?>() { null },
                Artists = _rep.GetAll<Artist>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                Labels = _rep.GetAll<Label>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name })
            };

            return ret;
        }

        public int Insert(CreateViewModel vm)
        {
             var entry = new Recording();
            SetValue(vm, ref entry);
            _rep.Add<Recording>(entry);
            _rep.Save();

            return entry.Id;
        }

        public CreateViewModel GetCreateViewModel(int? id)
        {
            var rec = _rep.FindBy<Recording>(r => r.Id == id).First();
            var ret =  new CreateViewModel()
            {
                Title = rec.Title,
                ReleaseDate = rec.ReleaseDate,
                SelectedArtistId = rec.Artist.Id,
                SelectedLabelId = rec.Label.Id,
                TrackTitles = rec.Tracks.Select(r => r.Title).ToList(),
                Durations = rec.Tracks.Select(r => (int?)r.Duration).ToList()
            };
            SetListItemSources(ret);
            return ret;
        }

        public bool IsExists(int? id)
        {
            return _rep.Any<Recording>(r => r.Id == id);
        }

        public void Update(CreateViewModel entry)
        {
            var target = _rep.FindBy<Recording>(r => r.Id == entry.Id).First();
            SetValue(entry, ref target);
            _rep.Save();
        }

        private void SetValue(ViewModels.Recording.CreateViewModel vm, ref Recording entry)
        {
            entry.Title = vm.Title;
            entry.ReleaseDate = vm.ReleaseDate;

            entry.Label = _rep.FindBy<Label>(r => r.Id == vm.SelectedLabelId).First();
            entry.Artist = _rep.FindBy<Artist>(r => r.Id == vm.SelectedArtistId).First();

            entry.Tracks = new List<Track>();
            foreach (var i in Enumerable.Range(0, vm.TrackTitles.Count()))
            {
                entry.Tracks.Add(new Track()
                {
                    Title = vm.TrackTitles[i],
                    Duration = (int)vm.Durations[i]
                });
            }
        }

        public void DeleteAll()
        {
            _rep.DeleteAll<Track>();
            _rep.DeleteAll<Recording>();
            _rep.Save();
        }

        /// <summary>
        /// ListItemSourceに値を設定する
        /// 
        /// Post時、ModelStateにエラーがあった場合にViewModelのListItemSourceが保持されない為
        /// Controllerで改めて設定する事を想定している
        /// </summary>
        /// <param name="vm"></param>
        public void SetListItemSources(CreateViewModel vm)
        {
            vm.Artists = _rep.GetAll<Artist>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name });
            vm.Labels = _rep.GetAll<Label>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name });
        }

        public List<Recording> GetAll()
        {
            return _rep.GetAll<Recording>().ToList();
        }
    }
}