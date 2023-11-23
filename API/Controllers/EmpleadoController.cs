using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class EmpleadoController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public EmpleadoController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<EmpleadoDto>>> Get()
        {
            var Empleado = await _unitOfWork.Empleados.GetAllAsync();
            return mapper.Map<List<EmpleadoDto>>(Empleado);
        }

        [HttpGet("Sin-Clientes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetSinclientes()
        {
            var Empleado = await _unitOfWork.Empleados.GetSinClientes();
            return Ok(Empleado);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Empleado>> Post(EmpleadoDto EmpleadoDto)
        {
            var Empleado = this.mapper.Map<Empleado>(EmpleadoDto);
            _unitOfWork.Empleados.Add(Empleado);
            await _unitOfWork.SaveAsync();

            if (Empleado == null)
            {
                return BadRequest();
            }
            EmpleadoDto.CodigoEmpleado = Empleado.CodigoEmpleado;
            return CreatedAtAction(nameof(Post), new { CodigoEmpleado = EmpleadoDto.CodigoEmpleado },Empleado);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmpleadoDto>> Get(int id)
        {
            var Empleado = await _unitOfWork.Empleados.GetByIdAsync(id);
            return mapper.Map<EmpleadoDto>(Empleado);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmpleadoDto>> Put(int id, [FromBody] EmpleadoDto EmpleadoDto)
        {
            if (EmpleadoDto == null)
                return NotFound();

            var Empleado = this.mapper.Map<Empleado>(EmpleadoDto);
            _unitOfWork.Empleados.Update(Empleado);
            await _unitOfWork.SaveAsync();
            return EmpleadoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Empleado = await _unitOfWork.Empleados.GetByIdAsync(id);
            if (Empleado == null)
                return NotFound();

            _unitOfWork.Empleados.Remove(Empleado);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}