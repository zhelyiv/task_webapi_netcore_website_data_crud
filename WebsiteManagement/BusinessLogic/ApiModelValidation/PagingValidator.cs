using Shared.ApiModelValidation;
using Shared.Exceptions;
using System;
using System.Text;

namespace BusinessLogic.ApiModelValidation
{
    public class PagingValidator: IPagingValidator
    {
        public void Validate(int totalRecordsCount, int? page, int? pageSize) 
        {
            var pagingParametersValidationError = new StringBuilder();

            if (page < 0)
            {
                pagingParametersValidationError
                    .AppendLine("Page number must be greater than zero");
            }

            if (pageSize <= 0)
            {
                pagingParametersValidationError
                    .AppendLine("Page size must be greater than zero");
            }

            if (page.HasValue && pageSize.HasValue)
            {
                var totalPagesCount = (int)Math.Ceiling(totalRecordsCount / (decimal)pageSize);

                if (page > totalPagesCount)
                {
                    pagingParametersValidationError.AppendLine($"Page number cannot exceed {totalPagesCount}");
                } 
            }

            if (pagingParametersValidationError.Length > 0)
            {
                throw new BadRequestError(pagingParametersValidationError.ToString());
            }
        }  
    }
}
