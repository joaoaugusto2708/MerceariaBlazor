using Mercearia.API.DTO;
using Mercearia.Model.Model;
using Mercearia.Infra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Mercearia.Model.DTO;
using Mercearia.API.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mercearia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : ControllerBase
    {
        // GET: api/<ProdutoController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendaDTO>>> Get()
        {
            var vendas = new VendaDAO().BuscarTodos();

            if (vendas != null)
            {
                var vendasDTO = vendas.Select(p => new VendaDTO
                {
                   DiaVenda = p.DiaVenda,
                   FormaPagamento = p.FormaPagamento,
                   NumVenda = p.NumVenda,
                   Produtos = p.Produtos,
                   ValorVenda = p.ValorVenda
                });

                return Ok(vendasDTO);
            }
            else
            {
                return BadRequest();
            }
        }


        // GET api/<ProdutoController>/5
        [HttpGet("{numVenda}")]
        public ActionResult<ProdutoDTO> Get(string numVenda)
        {
            Venda venda = new VendaDAO().Buscar(numVenda);

            if (venda != null)
            {
                var vendaDTO = new VendaDTO
                {
                    DiaVenda = venda.DiaVenda,
                    ValorVenda = venda.ValorVenda,
                    FormaPagamento = venda.FormaPagamento,
                    NumVenda = venda.NumVenda,
                    Produtos = venda.Produtos
                };

                return Ok(vendaDTO);
            }

            return NotFound();
        }


        // POST api/<ProdutoController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VendaDTO vendaDTO)
        {
            try
            {
                List<Produto> produtos =  new List<Produto>();
                foreach (var produto in vendaDTO.Produtos)
                {
                    try
                    {
                        produtos.Add(new ProdutoDAO().Buscar(produto.NumProduto));
                    }
                    catch (Exception)
                    {

                        NotFound("Produto inexistente, Numero do produto: " + produto.NumProduto );
                    }
                    
                }
                if (vendaDTO != null)
                {
                    Random random = new Random();
                    Venda venda = new Venda
                    {
                        Id = Guid.NewGuid().ToString(),
                        NumVenda = random.Next(10000000, 99999999).ToString("D8"),
                        DiaVenda = vendaDTO.DiaVenda,
                        FormaPagamento = vendaDTO.FormaPagamento,
                        Produtos = produtos,                 
                        ValorVenda = vendaDTO.ValorVenda
                    };
                    

                    return new VendaService().RealizarVenda(venda) ? Ok(venda) : BadRequest("Produtos indisponiveis");

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
        [HttpPut("{numVenda}")]
        public async Task<ActionResult> Put(string numVenda, [FromBody] VendaDTO vendaDTO)
        {
            try
            {
                if (vendaDTO != null)
                {
                    Venda venda  = new VendaDAO().Buscar(numVenda);
                    if (venda != null)
                    {
                        venda.DiaVenda = vendaDTO.DiaVenda;
                        venda.ValorVenda = vendaDTO.ValorVenda;
                        venda.FormaPagamento = vendaDTO.FormaPagamento;
                        venda.Produtos = vendaDTO.Produtos;
                        new VendaDAO().Atualizar(venda);
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
        [HttpDelete("{numVenda}")]
        public async Task<ActionResult> Delete(string numVenda) 
        {
            try
            {
                Venda venda = new VendaDAO().Buscar(numVenda);
                if (venda != null)
                {
                    new VendaDAO().Deletar(venda);
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
