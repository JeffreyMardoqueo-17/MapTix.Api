using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthService.Models;
using AuthService.Repositories;
using AutoMapper;
using AuthService.Models.DTOs.Company;
using AuthService.Models.DTOs.Role;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }
        //Get : api/role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleReadDTO>>> GetAllRolesAsync()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (!result.Success)
                return NotFound(result.Message);
            var rolesDTO = _mapper.Map<IEnumerable<RoleReadDTO>>(result.Data);
            return Ok(rolesDTO);
        }
        // GET: api/role/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RoleReadDTO>> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (!role.Success)
                return NotFound(role.Message);
            var roleDTO = _mapper.Map<RoleReadDTO>(role.Data);
            return Ok(roleDTO);
        }
        //get: api/role/{name}
        [HttpGet("byname/{roleName}")]
        public async Task<ActionResult<RoleReadDTO>> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleService.GetRoleByNameAsync(roleName);
            if (!role.Success)
                return NotFound(role.Message);
            var roleDTO = _mapper.Map<RoleReadDTO>(role.Data);
            return Ok(roleDTO);
        }
         
        // POST: api/role
        [HttpPost]
        public async Task<ActionResult<RoleReadDTO>> CreateRoleAsync([FromBody] RoleCreateDTO createDto)
        {
            var role = _mapper.Map<Role>(createDto);
            var result = await _roleService.CreateRoleAsync(role);
            if (!result.Success)
                return BadRequest(result.Message);
            var roleDTO = _mapper.Map<RoleReadDTO>(result.Data);
            // return CreatedAtAction(nameof(GetRoleByIdAsync), new { id = roleDTO.Id }, roleDTO);
            return Ok(roleDTO);
        }
        // PUT: api/company/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRoleAsync(Guid id, [FromBody] RoleUpdateDTO updateDto)
        {
            if (updateDto is null)
                return BadRequest("Invalid data.");

            // if (id)
            //     return BadRequest("ID mismatch.");
            if (id == Guid.Empty)
                return BadRequest("Invalid ID.");

            var result = await _roleService.UpdateRoleAsync(_mapper.Map<Role>(updateDto));

            if (!result.Success)
                return BadRequest(result.Message);
            var roleDTO = _mapper.Map<RoleReadDTO>(result.Data);
            return Ok(roleDTO);
        }
        // DELETE: api/company/{id}
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteRoleAsync(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result.Success)
                return NotFound(result.Message);
            return NoContent();
        }

    }
}