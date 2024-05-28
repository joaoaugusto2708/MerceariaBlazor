using Mercearia.API.DTO;
using Mercearia.Model.Model;
using Mercearia.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercearia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        // GET: api/<ProdutoController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> Get()
        {
            var produtos = new ProdutoDAO().BuscarTodos();

            if (produtos != null)
            {
                var produtosDTO = produtos.Select(p => new ProdutoDTO
                {
                    NumProduto = p.NumProduto,
                    Preco = p.Preco,
                    Quantidade = p.Quantidade,
                    Disponivel = p.Disponivel,
                    TipoProduto = p.TipoProduto,
                    Validade = p.Validade,
                    Descricao = p.Descricao,
                    Nome = p.Nome
                });

                return Ok(produtosDTO);
            }
            else
            {
                return BadRequest();
            }
        }


        // GET api/<ProdutoController>/5
        [HttpGet("{numProduto}")]
        public ActionResult<ProdutoDTO> Get(string numProduto)
        {
            Produto produto = new ProdutoDAO().Buscar(numProduto);

            if (produto != null)
            {
                var produtoDTO = new ProdutoDTO
                {
                    NumProduto = produto.NumProduto,
                    Preco = produto.Preco,
                    Quantidade = produto.Quantidade,
                    Disponivel = produto.Disponivel,
                    TipoProduto = produto.TipoProduto,
                    Validade = produto.Validade,
                    Descricao = produto.Descricao,
                    Nome = produto.Nome
                };

                return Ok(produtoDTO);
            }

            return NotFound();
        }


        // POST api/<ProdutoController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ProdutoDTO produtoDTO)
        {
            try
            {
                if (produtoDTO != null)
                {
                    Random random = new Random();
                    Produto produto = new Produto
                    {
                        Id = Guid.NewGuid().ToString(),
                        NumProduto = random.Next(10000000, 99999999).ToString("D8"),
                        Preco = produtoDTO.Preco,
                        Quantidade = produtoDTO.Quantidade,
                        Disponivel = produtoDTO.Disponivel,
                        TipoProduto = produtoDTO.TipoProduto == null ? "Indefinido" : produtoDTO.TipoProduto,
                        Validade = produtoDTO.Validade,
                        Descricao = produtoDTO.Descricao,
                        Nome = produtoDTO.Nome
                    };
                    new ProdutoDAO().Inserir(produto);
                    return Ok("Produto Salvo");

                }
                else
                { 
                    return BadRequest("Falta Informação");
                }

                }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // PUT api/<ProdutoController>/5
        [HttpPut("{numProduto}")]
        public async Task<ActionResult> Put(string numProduto, [FromBody] ProdutoDTO produtoDTO)
        {
            try
            {
                if (produtoDTO != null)
                {
                    Produto produto  = new ProdutoDAO().Buscar(numProduto);
                    if (produto != null)
                    {
                        produto.Preco = produtoDTO.Preco;
                        produto.Quantidade = produtoDTO.Quantidade;
                        produto.Disponivel = produtoDTO.Disponivel;
                        produto.TipoProduto = produtoDTO.TipoProduto;
                        produto.Validade = produtoDTO.Validade;
                        produto.Descricao = produtoDTO.Descricao;
                        produto.Nome = produtoDTO.Nome;
                        new ProdutoDAO().Atualizar(produto);
                        return Ok("Produto Alterado");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("O objeto ProdutoDTO recebido é nulo.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // DELETE api/<ProdutoController>/5
        [HttpDelete("{numProduto}")]
        public async Task<ActionResult> Delete(string numProduto)
        {
            try
            {
                Produto produto = new ProdutoDAO().Buscar(numProduto);
                if (produto != null)
                {
                    new ProdutoDAO().Deletar(produto);
                    return NoContent();
                }
                else
                {
                    return NotFound("Não existe este produto");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
