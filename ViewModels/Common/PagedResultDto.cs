namespace CRM_Management_Student.Backend.ViewModels.Common

{
    public class PagedResultDto<T>
    {
        public List<T>? Items { set; get; }
        public int? TotalCount { set; get; }
    }
}