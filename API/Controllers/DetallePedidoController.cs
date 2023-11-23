using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class DetallePedidoController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public DetallePedidoController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> Get()
        {
            var DetallePedido = await _unitOfWork.DetallePedidos.GetAllAsync();
            return mapper.Map<List<DetallePedidoDto>>(DetallePedido);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetallePedido>> Post(DetallePedidoDto DetallePedidoDto)
        {
            var DetallePedido = this.mapper.Map<DetallePedido>(DetallePedidoDto);
            _unitOfWork.DetallePedidos.Add(DetallePedido);
            await _unitOfWork.SaveAsync();

            if (DetallePedido == null)
            {
                return BadRequest();
            }
            DetallePedidoDto.CodigoPedido = DetallePedido.CodigoPedido;
            return CreatedAtAction(nameof(Post), new { id = DetallePedidoDto.CodigoPedido },DetallePedido);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetallePedidoDto>> Get(int id)
        {
            var DetallePedido = await _unitOfWork.DetallePedidos.GetByIdAsync(id);
            return mapper.Map<DetallePedidoDto>(DetallePedido);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DetallePedidoDto>> Put(int id, [FromBody] DetallePedidoDto DetallePedidoDto)
        {
            if (DetallePedidoDto == null)
                return NotFound();

            var DetallePedido = this.mapper.Map<DetallePedido>(DetallePedidoDto);
            _unitOfWork.DetallePedidos.Update(DetallePedido);
            await _unitOfWork.SaveAsync();
            return DetallePedidoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var DetallePedido = await _unitOfWork.DetallePedidos.GetByIdAsync(id);
            if (DetallePedido == null)
                return NotFound();

            _unitOfWork.DetallePedidos.Remove(DetallePedido);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}