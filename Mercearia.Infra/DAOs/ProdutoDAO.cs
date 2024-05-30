using Mercearia.Infra.DAO;
using Mercearia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercearia.Infra.DAOs
{
    public class ProdutoDAO : BaseDAO<Produto>
    {
        public override string NomeTabela => "Produto";

        public override Mapa[] Mapas => new Mapa[] {
            new() {Propriedade = "Id", Campo = "Id"},
            new() {Propriedade = "NumProduto", Campo = "NumProduto"},
            new() {Propriedade = "Preco", Campo = "Preco"},
            new() {Propriedade = "Quantidade", Campo = "Quantidade"},
            new() {Propriedade = "Disponivel", Campo = "Disponivel"},
            new() {Propriedade = "TipoProduto", Campo = "TipoProduto"},
            new() {Propriedade = "Validade", Campo = "Validade"},
            new() {Propriedade = "Descricao", Campo = "Descricao"},
            new() {Propriedade = "Nome", Campo = "Nome"},
        };
    }
}
