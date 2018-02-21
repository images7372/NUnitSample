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
        RecordingService _service;
        Recording _initial;
        CreateViewModel _vm;
        [SetUp]
        public void ParentSetUp()
        {
            //サービスの呼び出しに使用
            _service = RecordingService.GetInstance(_repos);
            _service.DeleteAll();

            //参照テストに使用
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
            _repos.Add(initial);
            _repos.Save();
            _repos.Reload();
            _initial = initial;

            //登録テストに使用
            _vm = new CreateViewModel()
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

        #region FillById
        [TestFixture]
        class FillByIdTest : RecordingServiceTest
        {

            [Test]
            public void IDを指定して特定の行を取得できる事()
            {
                var rec = _service.FillById(_initial.Id);

                Assert.That(rec.Title, Is.EqualTo(_initial.Title));
            }

            [Test]
            public void 指定したIDが存在しない場合に例外を発生する事()
            {
                Assert.That(() => _service.FillById(0), Throws.TypeOf<InvalidOperationException>());
            }
        }
        #endregion

        #region CreateCreateViewModel
        [TestFixture]
        class CreateCreateViewModelTest : RecordingServiceTest
        {

            CreateViewModel _newVm;
            [SetUp]
            public void SetUp()
            {
                _newVm = _service.CreateCreateViewModel();
            }

            [Test]
            public void CreateViewModel表示用のTitleとReleaseDateがnullであること()
            {
                Assert.IsNull(_newVm.Title);
                Assert.IsNull(_newVm.ReleaseDate);
            }

            [TestCase]
            public void Recording表示用のArtistsに登録された値があること()
            {
                SelectListItemAssert(
                    _repos.GetAll<Artist>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                    _newVm.Artists);
            }

            [TestCase]
            public void Recording表示用のLabelsに登録された値があること()
            {
                SelectListItemAssert(
                    _repos.GetAll<Label>().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.Name }),
                     _newVm.Labels);
            }

            [Test]
            public void Tracksに表示用の初期表示用の空データが1件存在する事()
            {
                Assert.That(_newVm.TrackTitles.Count, Is.EqualTo(1));
                Assert.That(_newVm.TrackTitles[0], Is.EqualTo(""));
                Assert.That(_newVm.Durations.Count, Is.EqualTo(1));
                Assert.IsNull(_newVm.Durations[0]);
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
        class Insert : RecordingServiceTest
        {
            Recording _ret;

            [SetUp]
            public void SetUp()
            {
                var id = _service.Insert(_vm);
                _ret = _repos.FindBy<Recording>(r => r.Id == id).First();
            }

            public void Recordingの登録が出来ること()
            {
                Assert.That(_ret.Title, Is.EqualTo(_vm.Title));
            }

            [TestCase]
            public void Trackの登録が出来ること()
            {
                Assert.That(_ret.Tracks.Count, Is.EqualTo(_vm.TrackTitles.Count()));
            }

            [TestCase]
            public void Artistの登録が出来ること()
            {
                Assert.That(_ret.Artist.Id, Is.EqualTo(_vm.SelectedArtistId));
            }

            [TestCase]
            public void Labelの登録が出来ること()
            {
                Assert.That(_ret.Label.Id, Is.EqualTo(_vm.SelectedLabelId));
            }
        }
        #endregion

        #region GetCreateViewModel
        class GetCreateViewModelTest : RecordingServiceTest
        {
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
        class IsExistsTest : RecordingServiceTest
        {
            [Test]
            public void Idに応じたデータがあればTrue無ければFalseを返す事()
            {
                Assert.That(_service.IsExists(_initial.Id), Is.EqualTo(true));
                Assert.That(_service.IsExists(0), Is.EqualTo(false));

            }
        }
        #endregion

        #region Update
        class UpdateTest : RecordingServiceTest
        {

            [Test]
            public void データの更新が出来る事()
            {
                _vm.Id = _initial.Id;
                _service.Update(_vm);
                _repos.Reload();

                Assert.That(_repos.FindBy<Recording>(r => r.Id == _initial.Id).First().Title, Is.EqualTo("Sgt. Peppers Lonely Hearts Club Band"));

            }
        }
        #endregion

        #region DeleteAll
        class DeleteAllTest : RecordingServiceTest
        {
            [Test]
            public void 全件削除出来る事()
            {
                _service.DeleteAll();
                _repos.Reload();

                Assert.That(_repos.GetAll<Recording>().Count(), Is.EqualTo(0));
            }
        }
        #endregion

        #region SetListItemSources
        class SetListItemSources : RecordingServiceTest
        {

            [Test]
            public void Artists及びLabelsに値がある事()
            {
                _service.SetListItemSources(_vm);

                Assert.IsNotNull(_vm.Artists);
                Assert.IsNotNull(_vm.Labels);
            }
        }
        #endregion

        #region GetAll
        class GetAllTest : RecordingServiceTest
        {
            [Test]
            public void 初期登録データと後に登録したデータが共に取得出来る事()
            {
                _service.Insert(_vm);
                _repos.Reload();

                var cnt = _service.GetAll().Count;
                Assert.That(cnt, Is.EqualTo(2));
            }
        }

        #endregion

    }
        
    
}
