using Dapper;
using Mercearia.Infra.DAO;
using Mercearia.Model;
using MySql.Data.MySqlClient;
using Mysqlx;

namespace Mercearia.Infra.DAOs
{
    public class VendaDAO : BaseDAO<Venda>
    {
        public override string NomeTabela => "Venda";

        public override Mapa[] Mapas => new Mapa[] {
            new() {Propriedade = "Id", Campo = "Id"},
            new() {Propriedade = "NumVenda", Campo = "NumVenda"},
            new() {Propriedade = "ValorVenda", Campo = "ValorVenda"},
            new() {Propriedade = "DiaVenda", Campo = "DiaVenda"},
            new() {Propriedade = "FormaPagamento", Campo = "FormaPagamento"},
        };

        public async Task InsertAsync(Venda venda)
        {
            await base.InsertAsync(venda);
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                foreach (var produto in venda.Produtos)
                {
                    string sql = @$"INSERT INTO venda_produto (idVenda, idProduto) VALUES (@idVenda, @idProduto)";
                    await conexao.ExecuteAsync(sql, new { idVenda = venda.Id, idProduto = produto.Id });
                }
            }
        }

        public async Task<Venda> ReadWithProdutosAsync(string id)
        {
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                string sql = $"SELECT * FROM {NomeTabela} WHERE Id = @Id";
                var venda = await conexao.QuerySingleAsync<Venda>(sql, new { Id = id });

                string produtosSql = @$"SELECT p.* FROM Produto p
                                        INNER JOIN venda_produto vp ON p.Id = vp.idProduto
                                        WHERE vp.idVenda = @idVenda";
                var produtos = await conexao.QueryAsync<Produto>(produtosSql, new { idVenda = id });
                venda.Produtos = produtos.ToList();

                return venda;
            }
        }

        public async Task UpdateAsync(Venda venda)
        {
            await base.UpdateAsync(venda);
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();

                string sql = "DELETE FROM venda_produto WHERE idVenda = @idVenda";
                await conexao.ExecuteAsync(sql, new { idVenda = venda.Id });

                foreach (var produto in venda.Produtos)
                {
                    string sqlProduto = "INSERT INTO venda_produto (idVenda, idProduto) VALUES (@idVenda, @idProduto)";
                    await conexao.ExecuteAsync(sqlProduto, new { idVenda = venda.Id, idProduto = produto.Id });
                }
            }
        }

        public async Task DeleteAsync(string id)
        {
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();

                string sql = "DELETE FROM venda_produto WHERE idVenda = @idVenda";
                await conexao.ExecuteAsync(sql, new { idVenda = id });

                await base.DeleteAsync(id);
            }
        }

        public async Task<IEnumerable<Venda>> ReadAllWithProdutosAsync()
        {
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                string sql = @"
                    SELECT 
                        v.*, p.*
                    FROM
                        venda v
                        INNER JOIN venda_produto vp ON v.Id = vp.idVenda
                        INNER JOIN produto p ON p.Id = vp.idProduto";

                var vendaDictionary = new Dictionary<string, Venda>();

                var result = await conexao.QueryAsync<Venda, Produto, Venda>(
                    sql,
                    (venda, produto) =>
                    {
                        if (!vendaDictionary.TryGetValue(venda.Id, out var vendaEntry))
                        {
                            vendaEntry = venda;
                            vendaEntry.Produtos = new List<Produto>();
                            vendaDictionary.Add(vendaEntry.Id, vendaEntry);
                        }
                        vendaEntry.Produtos.Add(produto);
                        return vendaEntry;
                    },
                    splitOn: "Id");

                return result.Distinct().ToList();
            }
        }
    }
}
