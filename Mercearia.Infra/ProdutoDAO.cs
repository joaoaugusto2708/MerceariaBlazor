using Mercearia.API.DTO;
using Mercearia.Model.Model;
using System;

namespace Mercearia.Infra
{
    public class ProdutoDAO : BaseDAO<Produto>
    {
        public override void Atualizar(Produto produto)
        {
            const string sql = @"UPDATE `mercearia`.`produto`
                                SET `Preco` = @Preco,
                                    `Quantidade` = @Quantidade,
                                    `Disponivel` = @Disponivel,
                                    `TipoProduto` = @TipoProduto,
                                    `Validade` = @Validade,
                                    `Descricao` = @Descricao,
                                    `Nome` = @Nome
                                    
                                WHERE `NumProduto` = @NumProduto;";
            ExecutarQuery(sql, produto);
        }

        public override Produto Buscar(string numProduto)
        {
            Produto produto = new Produto{NumProduto = numProduto };
            const string sql = @"SELECT * FROM mercearia.produto where produto.NumProduto = @numProduto;";
            return ConsultarPorParametro(sql, produto);
        }

        public override IEnumerable<Produto> BuscarTodos()
        {
            const string sql = @"SELECT * FROM mercearia.produto;";
            return ConsultarTodos(sql);
        }

        public override void Deletar(Produto produto)
        {
            const string sql = "DELETE FROM Produto WHERE NumProduto = @NumProduto";
            ExecutarQuery(sql, produto);
        }

        public override void Inserir(Produto produto)
        {

                const string sql = @"INSERT INTO `mercearia`.`produto`
                                    (`Id`,
                                    `NumProduto`,
                                    `Preco`,
                                    `Quantidade`,
                                    `Disponivel`,
                                    `TipoProduto`,
                                    `Validade`,
                                    `Nome`,
                                    `Descricao`)
                                    VALUES
                                    (@Id, @NumProduto, @Preco, @Quantidade, @Disponivel, @TipoProduto, @Validade, @Nome, @Descricao);";
                ExecutarQuery(sql, produto);
        }
    }
}
