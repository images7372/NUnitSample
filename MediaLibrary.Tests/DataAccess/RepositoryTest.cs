using System;
using System.Linq;
using NUnit.Framework;
using MediaLibrary.Models;
using System.Data.Entity.Validation;

namespace MediaLibrary.Tests.DataAccess
{
    class RepositoryTest
    {
        #region Recording
        class RecordingTest : ConnectionFixture
        {
            int _insId;
            Recording _entry;
            [SetUp]
            public void SetUp()
            {
                _entry = new Recording()
                {
                    Title = "Images And Words",
                    ReleaseDate = new DateTime(1992, 7, 7),
                    Artist = new Artist()
                    {
                        Name = "Dream Theater"
                    },
                    Label = new Label()
                    {
                        Name = "Atco Records"
                    }
                };

                _repos.Add<Recording>(_entry);
                _repos.Save();
                _insId = _entry.Id;
            }

            [Test]
            public void Titleを登録出来る事()
            {
                Assert.That("Images And Words", Is.EqualTo(FindById(_insId).Title));
            }

            [Test]
            public void ReleaseDateを登録出来る事()
            {
                Assert.That(new DateTime(1992, 7, 7), Is.EqualTo(FindById(_insId).ReleaseDate));
            }

            [Test]
            public void Artistを登録出来る事()
            {
                Assert.That("Dream Theater", Is.EqualTo(FindById(_insId).Artist.Name));
            }

            [Test]
            public void Artistがnullの場合に例外を発生する事()
            {
                _entry.Artist = null;
                _repos.Add(_entry);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [Test]
            public void Labelを登録出来る事()
            {
                Assert.That("Atco Records", Is.EqualTo(FindById(_insId).Label.Name));
            }

            [Test]
            public void Labelがnullの場合に例外を発生する事()
            {
                _entry.Label = null;
                _repos.Add(_entry);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [Test]
            public void Titleを更新出来る事()
            {
                FindById(_entry.Id).Title = "The Astonishing";
                _repos.Save();
                _repos.Reload();
                Assert.That("The Astonishing", Is.EqualTo(FindById(_entry.Id).Title));
            }

            [Test]
            public void ReleaseDatesを更新出来る事()
            {
                var release = new DateTime(2016, 1, 29);
                FindById(_entry.Id).ReleaseDate = release;
                _repos.Save();
                _repos.Reload();
                Assert.That(release, Is.EqualTo(FindById(_entry.Id).ReleaseDate));
            }

            [Test]
            public void 削除出来る事()
            {
                _repos.DeleteBy<Recording>(r => r.Id == _entry.Id);
                _repos.Save();

                Assert.False(_repos.Any<Recording>(r => r.Id == _entry.Id));
            }

            private Recording FindById(int id)
            {
                return _repos.FindBy<Recording>(r => r.Id == id).First();
            }
        }
        #endregion

        #region Artist
        class ArtistTest : ConnectionFixture
        {
            Artist _artist;
            [SetUp]
            public void SetUp()
            {
                _artist = new Artist()
                {
                    Name = "Bootsy Collins"
                };
                _repos.Add(_artist);
                _repos.Save();
                _repos.Reload();
            }

            [TestCase]
            public void Insert出来る事()
            {
                Assert.That("Bootsy Collins", Is.EqualTo(FindById(_artist.Id).Name));
            }

            [Test]
            public void Nameがnullの場合に例外を発生する事()
            {
                _artist.Name = null;
                _repos.Add(_artist);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [TestCase]
            public void 既登録データの更新を行う事()
            {
                var newArtist = FindById(_artist.Id);
                newArtist.Name = "T.M Stevens";
                _repos.Save();
                _repos.Reload();
                Assert.That("T.M Stevens", Is.EqualTo(FindById(_artist.Id).Name));
            }

            [Test]
            public void Delete出来る事()
            {
                _repos.DeleteBy<Artist>(r => r.Id == _artist.Id);
                _repos.Save();
                Assert.False(_repos.Any<Artist>(r => r.Id == _artist.Id));
            }

            private Artist FindById(int id)
            {
                return _repos.FindBy<Artist>(r => r.Id == id).First();
            }
        }
        #endregion

        #region Genre
        class GenreTest : ConnectionFixture
        {
            Genre _genre;
            [SetUp]
            public void SetUp()
            {
                _genre = new Genre()
                {
                    Name = "Progressive Rock"
                };
                _repos.Add<Genre>(_genre);
                _repos.Save();
                _repos.Reload();
            }

            [TestCase]
            public void Insert出来る事()
            {
                Assert.That("Progressive Rock", Is.EqualTo(FindById(_genre.Id).Name));
            }

            [Test]
            public void Nameがnullの場合に例外を発生する事()
            {
                _genre.Name = null;
                _repos.Add<Genre>(_genre);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [TestCase("initial", "expected")]
            public void 既登録データの更新を行う事(string initial, string expected)
            {
                _genre.Name = initial;
                _repos.Add<Genre>(_genre);
                _repos.Save();
                var registered = _repos.FindBy<Genre>(r => r.Id == _genre.Id).First();
                Assert.That(initial, Is.EqualTo(_repos.FindBy<Genre>(r => r.Id == _genre.Id).First().Name));

                registered.Name = expected;
                _repos.Save();
                Assert.That(expected, Is.EqualTo(_repos.FindBy<Genre>(r => r.Id == _genre.Id).First().Name));

            }

            private Genre FindById(int id)
            {
                return _repos.FindBy<Genre>(r => r.Id == _genre.Id).First();
            }
        }
        #endregion

        #region Review
        class ReviewTest : ConnectionFixture
        {
            private Review _review;
            private Review _ret;
            [SetUp]
            public void SetUp()
            {
                _review = new Review()
                {
                    Summary = "Review Summary",
                    Reviewer = new Reviewer()
                    {
                        Name = "Marty Friedman"
                    },
                    Recording = new Recording()
                    {
                        Title = "Sample",
                        Artist = new Artist() { Name = "Sample Artist" },
                        Label = new Label() { Name = "Sample Label" }
                    }
                };

                _repos.Add(_review);
                _repos.Save();
                _repos.Reload();

                _ret = _repos.FindBy<Review>(r => r.ID == _review.ID).First();
            }

            [Test]
            public void Insert出来る事()
            {
                Assert.That("Review Summary", Is.EqualTo(_ret.Summary));
            }

            [Test]
            public void Summaryの更新を行うこと()
            {
                const string NewSummary = "New Summary";
                _ret.Summary = NewSummary;
                _repos.Update(_ret);
                _repos.Save();

                Assert.That(NewSummary, Is.EqualTo(_repos.FindBy<Review>(r => r.ID == _ret.ID).First().Summary));
            }

            [Test]
            public void Summaryがnullの場合は例外を発生する事()
            {
                _review.Summary = null;
                _repos.Add<Review>(_review);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [Test]
            public void Reviewerがnullの場合は例外を発生する事()
            {
                _review.Reviewer = null;
                _repos.Add<Review>(_review);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [Test]
            public void Recordingがnullの場合は例外を発生する事()
            {
                _review.Recording = null;
                _repos.Add<Review>(_review);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }
        }
        #endregion

        #region Track
        class TrackTest : ConnectionFixture
        {
            int _insId;
            Track _entry;
            [SetUp]
            public void SetUp()
            {
                _entry = new Track()
                {
                    Title = "Within You Without You",
                    Duration = 305,
                    Genre = new Genre()
                    {
                        Name = "British Rock"
                    },
                    Recording = new Recording()
                    {
                        Artist = new Artist()
                        {
                            Name = "The Beatles"
                        },
                        Label = new Label()
                        {
                            Name = "EMI"
                        },
                        Title = "Sgt. Pepper's Lonely Hearts Club Band",
                        ReleaseDate = new DateTime(1967, 6, 1)
                    }
                };

                _repos.Add<Track>(_entry);
                _repos.Save();
                _insId = _entry.Id;
                _repos.Reload();
            }

            [Test]
            public void Titleの登録が出来る事()
            {
                Assert.That("Within You Without You", Is.EqualTo(FillById(_insId).Title));
            }

            [Test]
            public void Titleがnullの場合に例外を発生する事()
            {
                _entry.Title = null;
                _repos.Add(_entry);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [Test]
            public void Titleの更新が出来る事()
            {
                _entry.Title = "I Saw Her Standing There";
                _repos.Save();
                _repos.Reload();

                Assert.That("I Saw Her Standing There", Is.EqualTo(FillById(_insId).Title));
            }

            [Test]
            public void Durationの登録が出来る事()
            {
                Assert.That(305, Is.EqualTo(FillById(_insId).Duration));
            }

            [Test]
            public void Durationの更新が出来る事()
            {
                _entry.Duration = 177;
                _repos.Save();
                _repos.Reload();

                Assert.That(177, Is.EqualTo(FillById(_insId).Duration));
            }

            [Test]
            public void Genreの登録が出来る事()
            {
                Assert.That("British Rock", Is.EqualTo(FillById(_insId).Genre.Name));
            }

            [Test]
            public void Recordingの登録が出来る事()
            {
                Assert.That("The Beatles", Is.EqualTo(FillById(_insId).Recording.Artist.Name));
            }

            private Track FillById(int id)
            {
                return _repos.FindBy<Track>(r => r.Id == id).First();
            }
        }
        #endregion 

        #region Label
        class LabelTest : ConnectionFixture
        {
            Label _entry;
            [SetUp]
            public void SetUp()
            {
                _entry = new Label()
                {
                    Name = "Epic"
                };
                _repos.Add(_entry);
                _repos.Save();
                _repos.Reload();

            }

            [TestCase]
            public void Insert出来る事()
            {
                Assert.That("Epic", Is.EqualTo(FindById(_entry.Id).Name));
            }

            [Test]
            public void Nameがnullの場合に例外を発生する事()
            {
                _entry.Name = null;
                _repos.Add(_entry);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [TestCase]
            public void 既登録データの更新を行う事()
            {
                var newArtist = FindById(_entry.Id);
                newArtist.Name = "Sony Music Entertainment";
                _repos.Save();
                _repos.Reload();
                Assert.That("Sony Music Entertainment", Is.EqualTo(FindById(_entry.Id).Name));
            }

            [Test]
            public void Delete出来る事()
            {
                _repos.DeleteBy<Label>(r => r.Id == _entry.Id);
                _repos.Save();
                Assert.False(_repos.Any<Label>(r => r.Id == _entry.Id));
            }

            private Label FindById(int id)
            {
                return _repos.FindBy<Label>(r => r.Id == id).First();
            }
        }
        #endregion

        #region Reviewer
        class ReviewerTest : ConnectionFixture
        {
            Reviewer _entry;
            [SetUp]
            public void SetUp()
            {
                _entry = new Reviewer()
                {
                    Name = "Marty Friedman"
                };
                _repos.Add(_entry);
                _repos.Save();
                _repos.Reload();

            }

            [TestCase]
            public void Insert出来る事()
            {
                Assert.That("Marty Friedman", Is.EqualTo(FindById(_entry.Id).Name));
            }

            [Test]
            public void Nameがnullの場合に例外を発生する事()
            {
                _entry.Name = null;
                _repos.Add(_entry);
                Assert.That(() => _repos.Save(), Throws.TypeOf<DbEntityValidationException>());
            }

            [TestCase]
            public void 既登録データの更新を行う事()
            {
                var newArtist = FindById(_entry.Id);
                newArtist.Name = "近田春夫";
                _repos.Save();
                _repos.Reload();
                Assert.That("近田春夫", Is.EqualTo(FindById(_entry.Id).Name));
            }

            [Test]
            public void Delete出来る事()
            {
                _repos.DeleteBy<Reviewer>(r => r.Id == _entry.Id);
                _repos.Save();
                Assert.False(_repos.Any<Reviewer>(r => r.Id == _entry.Id));
            }

            private Reviewer FindById(int id)
            {
                return _repos.FindBy<Reviewer>(r => r.Id == id).First();
            }
        }
        #endregion
    }
}
