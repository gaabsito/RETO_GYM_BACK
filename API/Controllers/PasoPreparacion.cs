using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecetasAPI.Models;
using RecetasAPI.Services;

namespace RecetasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasoPreparacionController : ControllerBase
    {
        private readonly IPasoPreparacionService _pasoPreparacionService;

        public PasoPreparacionController(IPasoPreparacionService pasoPreparacionService)
        {
            _pasoPreparacionService = pasoPreparacionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PasoPreparacion>>> GetAll()
        {
            return Ok(await _pasoPreparacionService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PasoPreparacion>> GetById(int id)
        {
            var paso = await _pasoPreparacionService.GetByIdAsync(id);
            if (paso == null) return NotFound();
            return Ok(paso);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] PasoPreparacion paso)
        {
            await _pasoPreparacionService.AddAsync(paso);
            return CreatedAtAction(nameof(GetById), new { id = paso.PasoID }, paso);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] PasoPreparacion paso)
        {
            if (id != paso.PasoID) return BadRequest();
            await _pasoPreparacionService.UpdateAsync(paso);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _pasoPreparacionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
