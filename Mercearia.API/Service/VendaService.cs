using Mercearia.Infra;
using Mercearia.Model.Model;

namespace Mercearia.API.Service
{
    public class VendaService
    {
        public VendaService() { }
        public bool RealizarVenda(Venda venda)
        {
            bool validaVenda = true;
            foreach (Produto produto in venda.Produtos)
            {
                if (produto.Quantidade > 0 && produto.Disponivel)
                {
                    produto.Quantidade--;
                    venda.ValorVenda = CalcularVenda(venda.Produtos);
                    new ProdutoDAO().Atualizar(produto);
                }
                else
                {
                    produto.Disponivel = false;
                    new ProdutoDAO().Atualizar(produto);
                    return validaVenda = false;
                }
            }
            new VendaDAO().Inserir(venda);
            return validaVenda;
        }

        private decimal CalcularVenda(List<Produto> produtos)
        {
            decimal valorFinal = 0;
            foreach (var produto in produtos)
            {
                valorFinal += produto.Preco;
            }
            return valorFinal;
        }
    }
}
