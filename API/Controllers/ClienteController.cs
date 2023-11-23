using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class ClienteController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ClienteController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> Get()
        {
            var Cliente = await _unitOfWork.Clientes.GetAllAsync();
            return mapper.Map<List<ClienteDto>>(Cliente);
        }

        [HttpGet("Total-Pedidos-Por-Cliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetPedidosPorCliente()
        {
            var Cliente = await _unitOfWork.Clientes.TotalPedidosPorCliente();
            return Ok(Cliente);
        }

        [HttpGet("Pedidos-Tardios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetPedidosTardios()
        {
            var Cliente = await _unitOfWork.Clientes.PedidoTardio();
            return Ok(Cliente);
        }

        
        [HttpGet("gamasCliente")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetGamas()
        {
            var Cliente = await _unitOfWork.Clientes.GamaPorCliente();
            return Ok(Cliente);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Cliente>> Post(ClienteDto ClienteDto)
        {
            var Cliente = this.mapper.Map<Cliente>(ClienteDto);
            _unitOfWork.Clientes.Add(Cliente);
            await _unitOfWork.SaveAsync();

            if (Cliente == null)
            {
                return BadRequest();
            }
            ClienteDto.CodigoCliente = Cliente.CodigoCliente;
            return CreatedAtAction(nameof(Post), new { CodigoCliente = ClienteDto.CodigoCliente },Cliente);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Get(int id)
        {
            var Cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            return mapper.Map<ClienteDto>(Cliente);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClienteDto>> Put(int id, [FromBody] ClienteDto ClienteDto)
        {
            if (ClienteDto == null)
                return NotFound();

            var Cliente = this.mapper.Map<Cliente>(ClienteDto);
            _unitOfWork.Clientes.Update(Cliente);
            await _unitOfWork.SaveAsync();
            return ClienteDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Cliente = await _unitOfWork.Clientes.GetByIdAsync(id);
            if (Cliente == null)
                return NotFound();

            _unitOfWork.Clientes.Remove(Cliente);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}