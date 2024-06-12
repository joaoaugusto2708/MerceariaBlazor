using Microsoft.AspNetCore.Mvc;
using Mercearia.Model;
using Mercearia.Infra.DAOs;
using Mercearia.API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercearia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        // GET: api/<ProdutoController>
        public VendaController()
        {
            dao = new VendaDAO();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Venda>>> GetAsync()
        {
            var objetos = await dao.ReadAllWithProdutosAsync();

            if (objetos == null)
                return NotFound();

            return Ok(objetos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Venda>> GetId(string id)
        {

            var obj = await dao.ReadWithProdutosAsync(id);

            if (obj == null)
                return NotFound();

            return obj;
        }

        [HttpPost]
        public async Task<ActionResult<Venda>> PostAsync([FromBody] Venda obj)
        {
            obj.DiaVenda = DateTime.Now;
            obj.NumVenda = "VEN" + new Random().Next(100000, 999999);
            bool validaVenda = new VendaService().RealizarVenda(obj);
            return validaVenda ? 
                CreatedAtAction(
                nameof(GetId),
                new { id = obj.Id },
                obj
            )  : BadRequest("Produtos indisponiveis");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody] Venda obj)
        {
            if (id != obj.Id)
                return BadRequest();

            Venda venda = await dao.ReadAsync(id);

            if (venda == null)
                return NotFound();

            venda.DiaVenda = obj.DiaVenda;
            venda.ValorVenda = obj.ValorVenda;
            venda.FormaPagamento = obj.FormaPagamento;
            venda.Produtos = obj.Produtos;

            await dao.UpdateAsync(venda);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var obj = await dao.ReadAsync(id);

            if (obj == null)
                return NotFound();

            await dao.DeleteAsync(id);

            return NoContent();
        }

        private readonly VendaDAO dao;
    }
}
