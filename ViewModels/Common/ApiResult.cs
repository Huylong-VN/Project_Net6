﻿namespace CRM_Management_Student.Backend.ViewModels.Common
{
    public class ApiResult<T>
    {
        public bool? IsSuccessed { get; set; }

        public string? Message { get; set; }

        public T? ResultObj { get; set; }
    }
}