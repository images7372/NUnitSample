using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MediaLibrary.Models;

namespace MediaLibrary.Services
{
    public class Repository : IRepository
    {
        MediaLibraryContext _db;
        public Repository()
        {
            _db = new MediaLibraryContext();
        }

        public void Add<T>(T entry) where T : class
        {
            _db.Set<T>().Add(entry);
        }

        public void DeleteAll<T>() where T : class
        {
            var entries = GetAll<T>();
            _db.Set<T>().RemoveRange(entries);
        }

        public void DeleteBy<T>(Expression<Func<T, bool>> expression) where T : class
        {
            var entries = FindBy<T>(expression);
            _db.Set<T>().RemoveRange(entries);
        }

        public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _db.Set<T>().Where(expression);
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _db.Set<T>();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update<T>(T entry) where T : class
        {
            _db.Set<T>().Add(entry);
        }

        public bool Any<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return _db.Set<T>().Any(expression);
        }

        public void Dispose()
        {
            if(_db != null) { _db.Dispose(); }
        }

        public void Reload()
        {
            foreach (var e in _db.ChangeTracker.Entries())
            {
                e.Reload();
            }
        }

    }
}