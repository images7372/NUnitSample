using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MediaLibrary.Models;
using System.ComponentModel.DataAnnotations;
using MediaLibrary.Services;

namespace MediaLibrary.Tests
{
    class ConnectionFixture
    {
        //TODO:IRepositoryに修正要
        protected Repository _repos;

        [SetUp]
        public void BaseSetUp()
        {
            _repos = new Repository();
        }

        [TearDown]
        public void BaseTearDown()
        {
            if (_repos != null) { _repos.Dispose(); }
        }

        protected bool TryModelStateValidate<T>(T model, out List<ValidationResult> result ) where T : class
        {
            result = new List<ValidationResult>();
            var context = new ValidationContext(model, null, null);
            return Validator.TryValidateObject(model, context, result, true);
        }
    }
}
