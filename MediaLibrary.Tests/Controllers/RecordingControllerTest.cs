using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using MediaLibrary.ViewModels.Recording;
using System.ComponentModel.DataAnnotations;
using MediaLibrary.Controllers;
using System.Web.Mvc;
using MediaLibrary.Services;
using MediaLibrary.Models;
using System.Net;

namespace MediaLibrary.Tests.Controllers
{
    class RecordingControllerTest : ConnectionFixture
    {
        public static Recording InsertInitialRecord()
        {
            var newrec = new Recording()
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

            var repos = new Repository();
            repos.Add(newrec);
            repos.Save();
            repos.Reload();
            return newrec;
        }

        #region Create Get
        class CreateGet : ConnectionFixture
        {
            RecordingsController _controller;
            [SetUp]
            public void SetUp()
            {
                _controller = new RecordingsController(_repos);
            }

            [Test]
            public void CreateGetのViewNameが空白である事()
            {
                var vr = _controller.Create() as ViewResult;

                Assert.That("", Is.EqualTo(vr.ViewName));
            }

            [Test]
            public void CreateGetのViewModelが入力前状態である事()
            {
                var vr = _controller.Create() as ViewResult;
                var vm = vr.Model as MediaLibrary.ViewModels.Recording.CreateViewModel;

                Assert.IsNull(vm.Title);
                Assert.IsNull(vm.ReleaseDate);
                Assert.IsNull(vm.SelectedArtistId);
                Assert.IsNull(vm.SelectedLabelId);
                Assert.IsNotNull(vm.TrackTitles);   //初期表示の為に1件存在
                Assert.IsNotNull(vm.Durations); //初期表示の為に1件存在

                Assert.IsNotNull(vm.Labels);
                Assert.IsNotNull(vm.Artists);
            }


        }
        #endregion

        #region Create Post
        class CreatePostTest : ConnectionFixture
        {
            CreateViewModel _vm;
            RecordingsController _controller;
            [SetUp]
            public void SetUp()
            {
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
                    SelectedLabelId = 2,
                };
                _controller = new RecordingsController(_repos);
            }

            [Test]
            public void Titleに値があってDurationにない時はエラーとする事()
            {
                _vm.TrackTitles.Add("New Song");
                _vm.Durations.Add(null);

                var err = new List<ValidationResult>();
                var context = new ValidationContext(_vm, null, null);
                Assert.IsFalse(Validator.TryValidateObject(_vm, context, err, true));
            }

            [Test]
            public void Durationに値があってTitleにない時はエラーとする事()
            {
                _vm.TrackTitles.Add("");
                _vm.Durations.Add(300);

                var err = new List<ValidationResult>();
                var context = new ValidationContext(_vm, null, null);
                Assert.IsFalse(Validator.TryValidateObject(_vm, context, err, true));
            }

            [Test]
            public void TitleとDurationの数が一致しない場合に例外を発生する事()
            {
                _vm.TrackTitles.Add("New Song");
                var err = new List<ValidationResult>();
                var context = new ValidationContext(_vm, null, null);
                Assert.That(() => Validator.TryValidateObject(_vm, context, err, true), Throws.TypeOf<InvalidOperationException>());
            }

            [TestCase("")]
            [TestCase(null)]
            public void Titleに入力が無い場合はエラーとすること(string title)
            {
                _vm.Title = title;

                var err = new List<ValidationResult>();
                var isValid = TryModelStateValidate(_vm, out err);

                Assert.IsFalse(isValid);
                Assert.That(err.First().ToString(), Is.EqualTo("タイトルに入力が必要です"));
            }

            [Test]
            public void ViewModelにエラーが無い場合のViewNameがIndexである事()
            {
                var ret = _controller.Create(_vm) as RedirectToRouteResult;
                Assert.That(ret.RouteValues["action"].ToString(), Is.EqualTo("Index"));
            }

            [Test]
            public void ViewModelにエラーがある場合ViewNameが空白か()
            {
                _controller.ModelState.AddModelError("test", "test error");

                var vr = _controller.Create(_vm) as ViewResult;
                Assert.That(vr.ViewName, Is.EqualTo(""));
            }

            [Test]
            public void ViewModelにエラーがある場合SelectListItemsに値があるか()
            {
                _controller.ModelState.AddModelError("test", "test error");

                var vr = _controller.Create(_vm) as ViewResult;
                Assert.IsNotNull(_vm.Artists);
                Assert.IsNotNull(_vm.Labels);
            }
        }
        #endregion

        #region Edit Get
        [TestFixture]
        class EditGetTest : ConnectionFixture
        {
            RecordingsController _controller;
            Recording _initial;
            [SetUp]
            public void SetUp()
            {
                _controller = new RecordingsController(_repos);
                _initial = InsertInitialRecord();
            }

            [Test]
            public void IdそのものがなければBadRequestを返す事()
            {
                int? id = null;
                var ret = _controller.Edit(id) as HttpStatusCodeResult;
                Assert.That(ret.StatusCode, Is.EqualTo(new HttpStatusCodeResult(HttpStatusCode.BadRequest).StatusCode));
            }

            [Test]
            public void Idに該当するデータが無ければNotFoundを返す事()
            {
                var ret = _controller.Edit(0) as HttpNotFoundResult;
                Assert.That(ret.StatusCode, Is.EqualTo(new HttpStatusCodeResult(HttpStatusCode.NotFound).StatusCode));
            }

            [Test]
            public void 初期表示するデータを取得出来る事()
            {
                var vr = _controller.Edit(_initial.Id) as ViewResult;
                var vm = vr.Model as CreateViewModel;

                Assert.That(vm.Title, Is.EqualTo("Are You Experienced"));
            }
        }
        #endregion

        #region Edit Post

        class EditPostTest : ConnectionFixture
        {
            CreateViewModel _vm;
            RecordingsController _controller;
            Recording _initial;
            [SetUp]
            public void Setup()
            {
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

                _controller = new RecordingsController(_repos);
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

            [Test]
            public void 更新に成功した場合のViewNameがIndexである事()
            {
                _vm.Id = _initial.Id;
                var vr = _controller.Edit(_vm) as RedirectToRouteResult;
                Assert.That(vr.RouteValues["action"].ToString(), Is.EqualTo("Index"));
            }

            [Test]
            public void ViewModelにエラーがある場合ViewNameが空白か()
            {
                _controller.ModelState.AddModelError("test", "test error");

                var vr = _controller.Edit(_vm) as ViewResult;
                Assert.That(vr.ViewName, Is.EqualTo(""));
            }

            [Test]
            public void ViewModelにエラーがある場合SelectListItemsに値があるか()
            {
                _controller.ModelState.AddModelError("test", "test error");

                var vr = _controller.Edit(_vm) as ViewResult;
                Assert.IsNotNull(_vm.Artists);
                Assert.IsNotNull(_vm.Labels);
            }

        }
        #endregion

    }
}
