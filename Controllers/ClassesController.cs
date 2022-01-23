using CRM_Management_Student.Backend.Application;
using CRM_Management_Student.Backend.ViewModels.Classes;
using CRM_Management_Student.Backend.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Management_Student.Backend.Controllers
{
    public class ClassesController : BasesController
    {
        private readonly IClassService _classService;
        public ClassesController(IClassService classService)
        {
            _classService = classService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]PagedAndSortedResultRequestDto request)
        {
            return Ok(await _classService.GetListAsync(request));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await _classService.GetClassById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ClassCreate request)
        {
            request.UserCreated=User?.Identity?.Name;
            return Ok(await _classService.CreateAsync(request));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            return Ok(await _classService.DeleteAsync(Id));
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(Guid Id, [FromForm]ClassUpdate request)
        {
            request.UserModified = User?.Identity?.Name;
            return Ok(await _classService.UpdateAsync(Id,request));
        }
        [HttpPut("addStudent/{Id}")]
        public async Task<IActionResult> AddStudent(Guid Id,ClassAssignRequest request)
        {
            return Ok(await _classService.AddNewStudent(Id,request));
        }
    }
}
