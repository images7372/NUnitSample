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
        #region ヘルパメソッド
        //テスト用のレコード挿入、検証用に挿入したレコードを返す
        public static Recording InsertInitialRecord()
        {
            var repos = new Repository();
            var initial = new Recording()
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
            repos.Add(initial);
            repos.Save();
            repos.Reload();
            return initial;
        }
        #endregion

        #region FillById
        [TestFixture]
        class FillByIdTest : ConnectionFixture
        {
            [Test, Ignore("テスト抜けてた")]
            public void test()
            {

            }
        }
        #endregion

        #region CreateCreateViewModel
        [TestFixture]
        class CreateCreateViewModelTest : ConnectionFixture
        {

            CreateViewModel _vm;
            [SetUp]
            public void SetUp()
            {
                var service = RecordingService.GetInstance(_repos);
                _vm = service.CreateCreateViewModel();
            }

            [Test]
            public void CreateViewModel表示用のTitleとReleaseDateがnullであること()
            {
                Assert.IsNull(_vm.Title);
                Assert.IsNull(_vm.ReleaseDate);
            }


            [TestCase]
            public void Recording表示用のArtistsに登録された値があること()
            {
                SelectListItemAssert(
                    _repos.GetAll<Artist>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                    _vm.Artists);
            }

            [TestCase]
            public void Recording表示用のLabelsに登録された値があること()
            {
                SelectListItemAssert(
                    _repos.GetAll<Label>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                     _vm.Labels);
            }

            [Test]
            public void Tracksに表示用の初期表示用の空データが1件存在する事()
            {
                Assert.That(_vm.TrackTitles.Count, Is.EqualTo(1));
                Assert.That(_vm.TrackTitles[0], Is.EqualTo(""));
                Assert.That(_vm.Durations.Count, Is.EqualTo(1));
                Assert.IsNull(_vm.Durations[0]);
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
        #endregion

        #region Insert
        class Insert : ConnectionFixture
        {
            RecordingService _service;
            Recording _ret;
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

            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
                var id = _service.Insert(_Record);
                _ret = _repos.FindBy<Recording>(r => r.Id == id).First();
            }

            public void Recordingの登録が出来ること()
            {
                Assert.That(_ret.Title, Is.EqualTo(_Record.Title));
            }

            [TestCase]
            public void Trackの登録が出来ること()
            {
                Assert.That(_ret.Tracks.Count, Is.EqualTo(_Record.TrackTitles.Count()));
            }

            [TestCase]
            public void Artistの登録が出来ること()
            {
                Assert.That(_ret.Artist.Id, Is.EqualTo(_Record.SelectedArtistId));
            }

            [TestCase]
            public void Labelの登録が出来ること()
            {
                Assert.That(_ret.Label.Id, Is.EqualTo(_Record.SelectedLabelId));
            }
        }
        #endregion

        #region GetCreateViewModel
        class GetCreateViewModelTest : ConnectionFixture
        {
            Recording _initial;
            RecordingService _service;
            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
                _initial = InsertInitialRecord();
            }

            [Test]
            public void Idに応じたCreaeViewModelを取得する事()
            {
                var rec = _service.GetCreateViewModel(_initial.Id);

                Assert.That(rec.Title, Is.EqualTo(_initial.Title));
                Assert.That(rec.ReleaseDate, Is.EqualTo(_initial.ReleaseDate));
                Assert.That(rec.SelectedArtistId, Is.EqualTo(_initial.Artist.Id));
                Assert.That(rec.SelectedLabelId, Is.EqualTo(_initial.Label.Id));

                CollectionAssert.AreEqual(new string[] { "Foxy Lady", "Manic Depression", "Red House" }, rec.TrackTitles);
                CollectionAssert.AreEqual(new int[] { 199, 210, 224 }, rec.Durations);

                Assert.IsNotNull(rec.Artists);
                Assert.IsNotNull(rec.Labels);
            }

        }
        #endregion

        #region IsExists
        class IsExistsTest : ConnectionFixture
        {
            [Test]
            public void Idに応じたデータがあればTrue無ければFalseを返す事()
            {
                var service = RecordingService.GetInstance(_repos);
                var initial = InsertInitialRecord();

                Assert.That(service.IsExists(initial.Id), Is.EqualTo(true));
                Assert.That(service.IsExists(0), Is.EqualTo(false));

            }
        }
        #endregion

        #region Update
        class UpdateTest : ConnectionFixture
        {
            Recording _initial;
            RecordingService _service;
            CreateViewModel _newEntry;
            [SetUp]
            public void SetUp()
            {
                _service = RecordingService.GetInstance(_repos);
                _initial = InsertInitialRecord();

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

                Assert.That(_repos.FindBy<Recording>(r => r.Id == _initial.Id).First().Title, Is.EqualTo("Sgt. Peppers Lonely Hearts Club Band"));

            }
        }
        #endregion

        #region SetValue
        [TestFixture]
        class SetValueTest : ConnectionFixture
        {
            [Test, Ignore("テスト抜けてた")]
            public void test()
            {

            }
        }
        #endregion

        #region DeleteAll
        class DeleteAllTest : ConnectionFixture
        {
            [Test]
            public void 全件削除出来る事()
            {
                InsertInitialRecord();

                var service = RecordingService.GetInstance(_repos);
                service.DeleteAll();
                _repos.Reload();

                Assert.That(_repos.GetAll<Recording>().Count(), Is.EqualTo(0));
            }
        }
        #endregion

    }
        
    
}
