using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class OficinaController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public OficinaController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OficinaDto>>> Get()
        {
            var Oficina = await _unitOfWork.Oficinas.GetAllAsync();
            return mapper.Map<List<OficinaDto>>(Oficina);
        }

        [HttpGet("sin-Empleados-Ventas-Frutales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OficinaDto>>> GetSinEmpleadosVentasFrutales()
        {
            var Oficina = await _unitOfWork.Oficinas.GetSinEmpleadosVentasFrutales();
            return mapper.Map<List<OficinaDto>>(Oficina);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Oficina>> Post(OficinaDto OficinaDto)
        {
            var Oficina = this.mapper.Map<Oficina>(OficinaDto);
            _unitOfWork.Oficinas.Add(Oficina);
            await _unitOfWork.SaveAsync();

            if (Oficina == null)
            {
                return BadRequest();
            }
            OficinaDto.CodigoOficina = Oficina.CodigoOficina;
            return CreatedAtAction(nameof(Post), new { id = OficinaDto.CodigoOficina },Oficina);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OficinaDto>> Get(int id)
        {
            var Oficina = await _unitOfWork.Oficinas.GetByIdAsync(id);
            return mapper.Map<OficinaDto>(Oficina);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OficinaDto>> Put(int id, [FromBody] OficinaDto OficinaDto)
        {
            if (OficinaDto == null)
                return NotFound();

            var Oficina = this.mapper.Map<Oficina>(OficinaDto);
            _unitOfWork.Oficinas.Update(Oficina);
            await _unitOfWork.SaveAsync();
            return OficinaDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Oficina = await _unitOfWork.Oficinas.GetByIdAsync(id);
            if (Oficina == null)
                return NotFound();

            _unitOfWork.Oficinas.Remove(Oficina);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}