using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace Ticketing_Screen_Designer.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string ConnectionString;
        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        protected static void ValidateModel<T>(T model) where T : class
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(model, context, results, true))
            {
                var errors = string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
                throw new ValidationException(errors);
            }
        }
    }
}