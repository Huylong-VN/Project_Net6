using CRM_Management_Student.Backend.Application;
using CRM_Management_Student.Backend.ViewModels.Common;
using CRM_Management_Student.Backend.ViewModels.Subjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM_Management_Student.Backend.Controllers
{
    public class SubjectsController : BasesController
    {
        private readonly ISubjectService _subjectService;
        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PagedAndSortedResultRequestDto request)
        {
            return Ok(await _subjectService.GetListAsync(request));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await _subjectService.GetClassById(Id));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubjectCreate request)
        {
            request.UserCreated = User?.Identity?.Name;
            return Ok(await _subjectService.CreateAsync(request));
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            return Ok(await _subjectService.DeleteAsync(Id));
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(Guid Id, SubjectUpdate request)
        {
            request.UserModified = User?.Identity?.Name;
            return Ok(await _subjectService.UpdateAsync(Id, request));
        }
    }
}
