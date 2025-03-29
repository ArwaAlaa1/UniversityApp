using AutoMapper;
using Lab_1.Dtos;
using Lab_1.Filters;
using Lab_1.Interfaces;
using Lab_1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Lab_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly Interfaces.IUnitOfWork _unitOfWork;
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IMapper mapper,IUnitOfWork unitOfWork,IDepartmentRepository departmentRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _departmentRepository = departmentRepository;
        }

        // Endpoint for Get All Departments
        [HttpGet]
        public async Task<ActionResult<DepartmentDto>> GetAllDepartments()
        {
            var departments =await _departmentRepository.GetDepartmentsWithSudentsAsync();
        
            var departmentsDto = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
            return Ok(departmentsDto);

        }

        // Endpoint for Get Specific Department 
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSpecificDepartment(int id)
        {
            var department = await _departmentRepository.GetOneDeptWithSudentsAsync(id);
            if (department==null)
            {
                return NotFound();

            }
            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return Ok(departmentDto);

        }

        // Endpoint for Get Specific Department By Name
        [HttpGet("ByName/{name}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartmentByName(string name)
        {
            var department =await  _departmentRepository.GetDepartmentWithNameAsync(name);
            if (department == null)
            {
                return NotFound();

            }
            var departmentDto = _mapper.Map<DepartmentDto>(department);
            return Ok(departmentDto);

        }

        //Endpoint For Add Department 
        [HttpPost]
        public async Task<ActionResult> AddDepartment(DepartmentSendDto department)
        {
            var departmentAdded = _mapper.Map<Department>(department);
            _unitOfWork.Repository<Department>().Add(departmentAdded);
            await  _unitOfWork.CompleteAsync();
            
            return Ok(new { message = $"Department {department.Name} is Created Sucessfully" } );

        }

        //Endpoint For Add Department out source use [ValidateLocationFilter]
        [HttpPost("OutSourceLocation")]
        [ValidateLocationFilter]
        public async Task<ActionResult> AddDepartmentOutSourse(DepartmentSendDto department)
        {
            var departmentAdded = _mapper.Map<Department>(department);
            _unitOfWork.Repository<Department>().Add(departmentAdded);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = $"Department {department.Name} is Created Sucessfully" });

        }

        //Endpoint For Update Department
        [HttpPut]
        
        public async Task<ActionResult> UpdateDepartment(DepartmentSendDto departmentDto)
        {
            
            var department = _mapper.Map<Department>(departmentDto);
            _unitOfWork.Repository<Department>().Edit(department);
            await _unitOfWork.CompleteAsync();
           
            return Ok(new { message = $"Department {department.Name} is updated" });
        }

        //Endpoint For Delete Department
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
            var department =await  _unitOfWork.Repository<Department>().GetById(id);
            department.IsDeleted = true;
            _unitOfWork.Repository<Department>().Edit(department);
            await _unitOfWork.CompleteAsync();
            return Ok(new { message = $"Department {department.Name} is deleted" });
        }
    }
}
