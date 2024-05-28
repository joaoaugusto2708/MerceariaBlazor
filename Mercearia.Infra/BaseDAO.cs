using Dapper;
using Mercearia.Model.Model;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;

namespace Mercearia.Infra
{
    public abstract class BaseDAO<T>
    {
        private static readonly IConfiguration Configuration;
        private readonly IDbConnection _connection;

        static BaseDAO()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = configBuilder.Build();
        }

        protected BaseDAO()
        {
            try
            {
                //string connectionString = Configuration.GetConnectionString("DefaultConnection");
                string connectionString = "Server=localhost;Port=3306;Database=mercearia;User=root;Password=bios1480;";
                _connection = new MySqlConnection(connectionString);
                _connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
                throw;
            }
        }


        protected BaseDAO(IDbConnection connection)
        {
            _connection = connection;
        }

        public abstract void Inserir(T entity);
        public abstract void Atualizar(T entity);
        public abstract void Deletar(T entity);
        public abstract IEnumerable<T> BuscarTodos();
        public abstract T Buscar(string entity);
        protected IDbConnection Connection => _connection;

        protected void ExecutarQuery(string sql, object param = null)
        {
            try
            {
                _connection.Execute(sql, param);
            }
            finally
            {
                _connection.Close();
            }
        }

        protected IEnumerable<T> ConsultarTodos(string sql, object parameters = null)
        {
            try
            {
                return _connection.Query<T>(sql, parameters);
            }
            finally
            {
                _connection.Close();
            }
        }

        protected T ConsultarPorParametro(string sql, object parameters = null)
        {
            try
            {
                return _connection.QueryFirstOrDefault<T>(sql, parameters);
            }
            finally
            {
                _connection.Close();
            }
        }
        protected IEnumerable<Venda> ConsultarVendas(string sql, object parameters = null)
        {
            try
            {
                var vendaDict = new Dictionary<string, Venda>();

                using (var reader = _connection.ExecuteReader(sql, parameters))
                {
                    while (reader.Read())
                    {
                        var vendaId = reader.GetString(reader.GetOrdinal("idVenda"));

                        if (!vendaDict.TryGetValue(vendaId, out Venda venda))
                        {
                            venda = new Venda
                            {
                                Id = vendaId,
                                DiaVenda = reader.GetDateTime(reader.GetOrdinal("DiaVenda")),
                                FormaPagamento = reader.GetString(reader.GetOrdinal("FormaPagamento")),
                                NumVenda = reader.GetString(reader.GetOrdinal("NumVenda")),
                                ValorVenda = reader.GetDecimal(reader.GetOrdinal("ValorVenda")),
                                Produtos = new List<Produto>()
                            };
                            vendaDict.Add(vendaId, venda);
                        }

                        var produto = new Produto
                        {
                            Id = reader.GetString(reader.GetOrdinal("idProduto")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
                            Descricao = reader.GetString(reader.GetOrdinal("Descricao")),
                            Disponivel = reader.GetBoolean(reader.GetOrdinal("Disponivel")),
                            NumProduto = reader.GetString(reader.GetOrdinal("NumProduto")),
                            Quantidade = reader.GetInt32(reader.GetOrdinal("Quantidade")),
                            TipoProduto = reader.GetString(reader.GetOrdinal("TipoProduto")),
                            Validade = reader.GetDateTime(reader.GetOrdinal("Validade"))

                        };

                        venda.Produtos.Add(produto);
                    }
                }

                return vendaDict.Values;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao executar a consulta: {ex.Message}");
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
