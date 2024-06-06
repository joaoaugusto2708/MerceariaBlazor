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

                    produtos.Add(p);
                }else 
                {
                    return false;
                }
            }
            if(produtos.Count() == venda.Produtos.Count()) {
                foreach (var produto in produtos)
                {
                    new ProdutoDAO().UpdateAsync(produto);
                }
                new VendaDAO().InsertAsync(venda);
                return true;
            }
            return false;

        }
    }
}
