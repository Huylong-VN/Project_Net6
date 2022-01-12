namespace CRM_Management_Student.Backend.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {
        public string[] ValidationErrors { set; get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApiErrorResult()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApiErrorResult(string message)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            IsSuccessed = false;
            Message = message;
        }

        public ApiErrorResult(string[] validationErrors)
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors;
        }
    }
}