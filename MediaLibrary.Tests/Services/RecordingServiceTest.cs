using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MediaLibrary.Models;
using MediaLibrary.Services;
using System.Web.Mvc;
using MediaLibrary.ViewModels.Recording;
using System.ComponentModel.DataAnnotations;

namespace MediaLibrary.Tests.Services
{
    [TestFixture]
    class RecordingServiceTest : ConnectionFixture
    {


        [Test]
        public void 全件削除出来る事()
        {
            var service = RecordingService.GetInstance(_repos);
            service.DeleteAll();
            _repos.Reload();

            Assert.That(_repos.GetAll<Recording>().Count(), Is.EqualTo(0));
        }

        class CreateGet : ConnectionFixture
        {
            RecordingService _service;
            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
            }

            [Test]
            public void CreateViewModel表示用のTitleがnullであること()
            {
                Assert.That(null, Is.EqualTo(_service.CreateCreateViewModel().Title));
            }

            [Test]
            public void CreateViewModel表示用のReleaseDateがnullであること()
            {
                Assert.That(null, Is.EqualTo(_service.CreateCreateViewModel().ReleaseDate));
            }

            [TestCase]
            public void Recording表示用のArtistsに登録された値があること()
            {
                SelectListItemAssert(
                    _repos.GetAll<Artist>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                    _service.CreateCreateViewModel().Artists);
            }

            [TestCase]
            public void Recording表示用のLabelsに登録された値があること()
            {
                SelectListItemAssert(
                    _repos.GetAll<Label>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                     _service.CreateCreateViewModel().Labels);
            }

            private void SelectListItemAssert(IEnumerable<SelectListItem> expected, IEnumerable<SelectListItem> actual)
            {
                var lExpected = expected.ToList();
                var lActual = actual.ToList();
                foreach (var i in Enumerable.Range(0, lExpected.Count))
                {
                    Assert.That(lExpected[i].Value, Is.EqualTo(lActual[i].Value));
                }
            }
        }

        class CreatePost : ConnectionFixture
        {
            RecordingService _service;
            Recording _ret;
            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
                var id = _service.Insert(_Record);
                _ret = _repos.FindBy<Recording>(r => r.Id == id).First();
            }

            CreateViewModel _Record = new CreateViewModel()
            {
                Title = "Sgt. Peppers Lonely Hearts Club Band",
                ReleaseDate = new DateTime(1967, 5, 26),
                TrackTitles = new List<string>()
            {
                "Sgt. Pepper's Lonely Hearts Club Band",
                "With a Little Help from My Friends",
                "Lucy in the Sky with Diamonds"
            },
                Durations = new List<int?>()
            {
                122,
                163,
                208
            },
                SelectedArtistId = 1,
                SelectedLabelId = 2
            };

            public void Recordingの登録が出来ること()
            {
                Assert.That(_Record.Title, Is.EqualTo(_ret.Title));
            }

            [TestCase]
            public void Trackの登録が出来ること()
            {
                Assert.That(_Record.TrackTitles.Count, Is.EqualTo(_ret.Tracks.Count()));
            }

            [TestCase]
            public void Artistの登録が出来ること()
            {
                Assert.That(_Record.SelectedArtistId, Is.EqualTo(_ret.Artist.Id));
            }

            [TestCase]
            public void Labelの登録が出来ること()
            {
                Assert.That(_Record.SelectedLabelId, Is.EqualTo(_ret.Label.Id));
            }
        }

        class EditGet : ConnectionFixture
        {
            Recording _initial;
            RecordingService _service;
            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
                _initial = new Recording()
                {
                    Title = "Are You Experienced",
                    ReleaseDate = new DateTime(1967, 5, 12),
                    Artist = new Artist() { Name = "Jimi Hendrix" },
                    Label = new Label() { Name = "Track Record" },
                    Tracks = new List<Track>(){
                    new Track()
                    {
                        Title = "Foxy Lady",
                        Duration = 199
                    },
                    new Track()
                    {
                        Title = "Manic Depression",
                        Duration = 210
                    },
                    new Track()
                    {
                        Title = "Red House",
                        Duration = 224
                    }
                }
                };
                _repos.Add(_initial);
                _repos.Save();
                _repos.Reload();
            }

            [Test]
            public void Idに応じたCreaeViewModelを取得する事()
            {
                var rec = _service.GetCreateViewModel(_initial.Id);

                Assert.That(_initial.Title, Is.EqualTo(rec.Title));
                Assert.That(_initial.ReleaseDate, Is.EqualTo(rec.ReleaseDate));
                Assert.That(_initial.Artist.Id, Is.EqualTo(rec.SelectedArtistId));
                Assert.That(_initial.Label.Id, Is.EqualTo(rec.SelectedLabelId));

                CollectionAssert.AreEqual(new string[] { "Foxy Lady", "Manic Depression", "Red House" }, rec.TrackTitles);
                CollectionAssert.AreEqual(new int[] { 199, 210, 224 }, rec.Durations);
            }

            [Test]
            public void Idに応じたデータがあればTrue無ければFalseを返す事()
            {
                Assert.That(true, Is.EqualTo( _service.IsExists(_initial.Id)));
                Assert.That(false, Is.EqualTo(_service.IsExists(0)));
            }
        }

        class EditPost : ConnectionFixture
        {
            Recording _initial;
            RecordingService _service;
            CreateViewModel _newEntry;
            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
                _initial = new Recording()
                {
                    Title = "Are You Experienced",
                    ReleaseDate = new DateTime(1967, 5, 12),
                    Artist = new Artist() { Name = "Jimi Hendrix" },
                    Label = new Label() { Name = "Track Record" },
                    Tracks = new List<Track>()
                    {
                        new Track()
                        {
                            Title = "Foxy Lady",
                            Duration = 199
                        },
                        new Track()
                        {
                            Title = "Manic Depression",
                            Duration = 210
                        },
                        new Track()
                        {
                            Title = "Red House",
                            Duration = 224
                        }
                    }
                };

                _repos.Add(_initial);
                _repos.Save();
                _repos.Reload();

                _newEntry = new CreateViewModel()
                {
                    Title = "Sgt. Peppers Lonely Hearts Club Band",
                    ReleaseDate = new DateTime(1967, 5, 26),
                    TrackTitles = new List<string>()
                    {
                        "Sgt. Pepper's Lonely Hearts Club Band",
                        "With a Little Help from My Friends",
                        "Lucy in the Sky with Diamonds"
                    },
                    Durations = new List<int?>()
                    {
                        122,
                        163,
                        208
                    },
                    SelectedArtistId = 1,
                    SelectedLabelId = 2
                };
            }

            [Test]
            public void データの更新が出来る事()
            {
                _newEntry.Id = _initial.Id;
                _service.Update(_newEntry);
                _repos.Reload();

                Assert.That("Sgt. Peppers Lonely Hearts Club Band", Is.EqualTo(_repos.FindBy<Recording>(r => r.Id == _initial.Id).First().Title));
                
            }
                
        }
    }
        
    
}
