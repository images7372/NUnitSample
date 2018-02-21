using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MediaLibrary.Services
{
    public interface IRepository : IDisposable
    {
        IQueryable<T> GetAll<T>() where T : class;
        IQueryable<T> FindBy<T>(Expression<Func<T, bool>> expression) where T : class;
        void DeleteBy<T>(Expression<Func<T, bool>> expression) where T : class;
        void DeleteAll<T>() where T : class;
        void Add<T>(T entry) where T : class;
        void Update<T>(T entry) where T : class;
        void Save();
        bool Any<T>(Expression<Func<T, bool>> expression) where T : class;
    }

}
