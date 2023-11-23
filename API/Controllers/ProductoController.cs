using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class ProductoController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ProductoController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
        {
            var Producto = await _unitOfWork.Productos.GetAllAsync();
            return mapper.Map<List<ProductoDto>>(Producto);
        }

         [HttpGet("sin-Pedidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductoSinPedidosDto>>> GetsinPedidos()
        {
            var Producto = await _unitOfWork.Productos.GetSinPedidos();
            return mapper.Map<List<ProductoSinPedidosDto>>(Producto);
        }

        [HttpGet("Ventas-Mas-3000")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetVentasMas3000()
        {
            var Producto = await _unitOfWork.Productos.GetMas3000();
            return Ok(Producto);
        }

         [HttpGet("MasVendido")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetVentasMasVendido()
        {
            var Producto = await _unitOfWork.Productos.GetMasVendido();
            return Ok(Producto);
        }

        [HttpGet("20MasVendidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Get20MasVendidos()
        {
            var Producto = await _unitOfWork.Productos.Get20MasVendidos();
            return Ok(Producto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Producto>> Post(ProductoDto ProductoDto)
        {
            var Producto = this.mapper.Map<Producto>(ProductoDto);
            _unitOfWork.Productos.Add(Producto);
            await _unitOfWork.SaveAsync();

            if (Producto == null)
            {
                return BadRequest();
            }
            ProductoDto.CodigoProducto = Producto.CodigoProducto;
            return CreatedAtAction(nameof(Post), new { id = ProductoDto.CodigoProducto },Producto);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var Producto = await _unitOfWork.Productos.GetByIdAsync(id);
            return mapper.Map<ProductoDto>(Producto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductoDto>> Put(int id, [FromBody] ProductoDto ProductoDto)
        {
            if (ProductoDto == null)
                return NotFound();

            var Producto = this.mapper.Map<Producto>(ProductoDto);
            _unitOfWork.Productos.Update(Producto);
            await _unitOfWork.SaveAsync();
            return ProductoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Producto = await _unitOfWork.Productos.GetByIdAsync(id);
            if (Producto == null)
                return NotFound();

            _unitOfWork.Productos.Remove(Producto);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}