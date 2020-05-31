using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsiteManagement.WebAPI
{
    /// <summary>
    /// Exception Extentions
    /// </summary>
    public static class ExceptionExtentions
    {
        /// <summary>
        /// Get collection of inner exception
        /// </summary>
        /// <param name="ex">parent exception</param>
        /// <returns>collection of inner exception</returns>
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            if (ex == null)
                throw new ArgumentNullException(nameof(ex));

            var innerException = ex;
            do
            {
                yield return innerException;
                innerException = innerException.InnerException;
            }
            while (innerException != null);
        }

        /// <summary>
        /// Parse DbUpdateException and extract db errors
        /// </summary>
        /// <param name="ex">DbUpdateException</param>
        /// <returns></returns>
        public static string GetDbUpdateExceptionMessage(this DbUpdateException ex)
        {
            var lastInnerException = ex.GetInnerExceptions().ToList().LastOrDefault();

            var sbErrorText = new StringBuilder();
            if (lastInnerException != null)
                sbErrorText.AppendLine(lastInnerException.Message);

            foreach (var eve in ex.Entries)
            {
                var text = $"Entity of type {eve.Entity.GetType().Name} in state {eve.State} could not be updated";
                sbErrorText.AppendLine(text);
            }

            return sbErrorText.ToString();
        }
    } 
}
