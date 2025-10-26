using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthService.Repositories;
using AuthService.Models.DTOs.Company;
using AutoMapper;
using AuthService.Models;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }
        // GET: api/company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyReadDto>>> GetAllCompanies()
        {
            var result = await _companyService.GetAllCompaniesAsync();

            if (!result.Success)
                return NotFound(result.Message);

            var companiesDto = _mapper.Map<IEnumerable<CompanyReadDto>>(result.Data);
            return Ok(companiesDto);
        }
        // GET: api/company/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CompanyReadDto>> GetCompanyById(Guid id)
        {
            var result = await _companyService.GetCompanyByIdAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            var companyDto = _mapper.Map<CompanyReadDto>(result.Data);
            return Ok(companyDto);
        }
        // POST: api/company
        [HttpPost]
        public async Task<ActionResult<CompanyReadDto>> CreateCompany([FromBody] CompanyCreateDto createDto)
        {
            var company = _mapper.Map<Company>(createDto);
            var result = await _companyService.CreateCompanyAsync(company);

            if (!result.Success)
                return BadRequest(result.Message);

            var companyDto = _mapper.Map<CompanyReadDto>(result.Data);
            return CreatedAtAction(nameof(GetCompanyById), new { id = companyDto.Id }, companyDto);
        }
        // PUT: api/company/{id}
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CompanyReadDto>> UpdateCompany(Guid id, [FromBody] CompanyUpdateDto updateDto)
        {
            if (id != updateDto.Id)
                return BadRequest("ID mismatch.");

            var company = _mapper.Map<Company>(updateDto);
            var result = await _companyService.UpdateCompanyAsync(updateDto.Id, company);

            if (!result.Success)
                return BadRequest(result.Message);

            var companyDto = _mapper.Map<CompanyReadDto>(result.Data);
            return Ok(companyDto);
        }
        // DELETE: api/company/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCompany(Guid id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            return NoContent();
        }
    }
}