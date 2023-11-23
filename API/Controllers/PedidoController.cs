using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class PedidoController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public PedidoController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> Get()
        {
            var Pedido = await _unitOfWork.Pedidos.GetAllAsync();
            return mapper.Map<List<PedidoDto>>(Pedido);
        }

        [HttpGet("Tardios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetTardios()
        {
            var Pedido = await _unitOfWork.Pedidos.PedidosTardios();
            return Ok(Pedido);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pedido>> Post(PedidoDto PedidoDto)
        {
            var Pedido = this.mapper.Map<Pedido>(PedidoDto);
            _unitOfWork.Pedidos.Add(Pedido);
            await _unitOfWork.SaveAsync();

            if (Pedido == null)
            {
                return BadRequest();
            }
            PedidoDto.CodigoPedido = Pedido.CodigoPedido;
            return CreatedAtAction(nameof(Post), new { id = PedidoDto.CodigoPedido },Pedido);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PedidoDto>> Get(int id)
        {
            var Pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
            return mapper.Map<PedidoDto>(Pedido);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PedidoDto>> Put(int id, [FromBody] PedidoDto PedidoDto)
        {
            if (PedidoDto == null)
                return NotFound();

            var Pedido = this.mapper.Map<Pedido>(PedidoDto);
            _unitOfWork.Pedidos.Update(Pedido);
            await _unitOfWork.SaveAsync();
            return PedidoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Pedido = await _unitOfWork.Pedidos.GetByIdAsync(id);
            if (Pedido == null)
                return NotFound();

            _unitOfWork.Pedidos.Remove(Pedido);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}