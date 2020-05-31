namespace Shared.ApiModelValidation
{
    public interface IPagingValidator
    {
        void Validate(int totalRecordsCount, int? page, int? pageSize);
    }
}
