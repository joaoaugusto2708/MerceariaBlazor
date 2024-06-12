using Mercearia.Infra.DAOs;
using Mercearia.Model;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercearia.API.Services
{
    public class VendaService
    {
        public bool RealizarVenda(Venda venda) 
        {
            List<Produto> produtos = new List<Produto>();
            foreach (var p in venda.Produtos)
            {
                if(p.Disponivel && p.Quantidade != 0) 
                {
                    p.Quantidade--;
                    if(p.Quantidade == 0)
                        p.Disponivel = false;
                    venda.ValorVenda += p.Preco;
                    produtos.Add(p);
                }else 
                {
                    return false;
                }
            }
            if (produtos.Count() == venda.Produtos.Count()) {
                new VendaDAO().InsertAsync(venda);
                
                    foreach (var produto in produtos)
                    {
                        new ProdutoDAO().UpdateAsync(produto);
                    }
                    return true;
            }
            return false;

        }
    }
}
