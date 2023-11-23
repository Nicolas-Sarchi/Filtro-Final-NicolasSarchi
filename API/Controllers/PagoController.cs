using API.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;

namespace API.Controllers
{
    public class PagoController : BaseApiController
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public PagoController(IUnitOfWork UnitOfWork , IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PagoDto>>> Get()
        {
            var Pago = await _unitOfWork.Pagos.GetAllAsync();
            return mapper.Map<List<PagoDto>>(Pago);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pago>> Post(PagoDto PagoDto)
        {
            var Pago = this.mapper.Map<Pago>(PagoDto);
            _unitOfWork.Pagos.Add(Pago);
            await _unitOfWork.SaveAsync();

            if (Pago == null)
            {
                return BadRequest();
            }
            PagoDto.IdTransaccion = Pago.IdTransaccion;
            return CreatedAtAction(nameof(Post), new { IdTransaccion = PagoDto.IdTransaccion },Pago);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagoDto>> Get(int id)
        {
            var Pago = await _unitOfWork.Pagos.GetByIdAsync(id);
            return mapper.Map<PagoDto>(Pago);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagoDto>> Put(int id, [FromBody] PagoDto PagoDto)
        {
            if (PagoDto == null)
                return NotFound();

            var Pago = this.mapper.Map<Pago>(PagoDto);
            _unitOfWork.Pagos.Update(Pago);
            await _unitOfWork.SaveAsync();
            return PagoDto;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var Pago = await _unitOfWork.Pagos.GetByIdAsync(id);
            if (Pago == null)
                return NotFound();

            _unitOfWork.Pagos.Remove(Pago);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}