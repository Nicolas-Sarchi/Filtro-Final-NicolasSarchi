using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class GamaProductoController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public GamaProductoController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GamaProductoDto>>> Get()
        {
            var GamaProducto = await _unitOfWork.GamaProductos.GetAllAsync();
            return mapper.Map<List<GamaProductoDto>>(GamaProducto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GamaProducto>> Post(GamaProductoDto GamaProductoDto)
        {
            var GamaProducto = this.mapper.Map<GamaProducto>(GamaProductoDto);
            _unitOfWork.GamaProductos.Add(GamaProducto);
            await _unitOfWork.SaveAsync();

            if (GamaProducto == null)
            {
                return BadRequest();
            }
            GamaProductoDto.Gama = GamaProducto.Gama;
            return CreatedAtAction(nameof(Post), new { Gama = GamaProductoDto.Gama },GamaProducto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GamaProductoDto>> Get(int id)
        {
            var GamaProducto = await _unitOfWork.GamaProductos.GetByIdAsync(id);
            return mapper.Map<GamaProductoDto>(GamaProducto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GamaProductoDto>> Put(int id, [FromBody] GamaProductoDto GamaProductoDto)
        {
            if (GamaProductoDto == null)
                return NotFound();

            var GamaProducto = this.mapper.Map<GamaProducto>(GamaProductoDto);
            _unitOfWork.GamaProductos.Update(GamaProducto);
            await _unitOfWork.SaveAsync();
            return GamaProductoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var GamaProducto = await _unitOfWork.GamaProductos.GetByIdAsync(id);
            if (GamaProducto == null)
                return NotFound();

            _unitOfWork.GamaProductos.Remove(GamaProducto);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}