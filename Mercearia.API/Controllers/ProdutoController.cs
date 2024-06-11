using Mercearia.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Mercearia.Model;
using Mercearia.Infra.DAOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercearia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        public ProdutoController()
        {
            dao = new ProdutoDAO();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAsync()
        {
            var objetos = await dao.ReadAllAsync();

            if (objetos == null)
                return NotFound();

            return Ok(objetos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> GetId(string id)
        {

            var obj = await dao.ReadAsync(id);

            if (obj == null)
                return NotFound();

            return obj;
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> PostAsync([FromBody]Produto obj)
        {
            obj.NumProduto = "PD" + new Random().Next(100000, 999999);
            await dao.InsertAsync(obj);

            return CreatedAtAction(
                nameof(GetId),
                new { id = obj.Id },
                obj
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id, [FromBody]Produto obj)
        {
            if (id != obj.Id)
                return BadRequest();

            Produto produto = await dao.ReadAsync(id);

            if (produto == null)
                return NotFound();

            produto.TipoProduto = obj.TipoProduto;
            produto.Disponivel = obj.Disponivel;
            produto.Quantidade = obj.Quantidade;
            produto.Descricao = obj.Descricao;
            produto.Nome = obj.Nome;
            produto.Preco = obj.Preco;
            produto.Validade = obj.Validade;

            await dao.UpdateAsync(produto);

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

        private readonly ProdutoDAO dao;
    }
}
