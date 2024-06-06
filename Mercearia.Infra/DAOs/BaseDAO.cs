using Dapper;
using Mercearia.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercearia.Infra.DAO
{
    public abstract class BaseDAO<T> where T : IModel
    {
        public abstract string NomeTabela { get; }
        public abstract Mapa[] Mapas { get; }
        protected static string GetStringConnection() => "Server=localhost;Port=3306;Database=mercearia;User=root;Password=bios1480;";

        public async Task InsertAsync(T obj)
        {
            if (string.IsNullOrWhiteSpace(obj.Id))
                obj.Id = Guid.NewGuid().ToString();

            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                string sql = @$"INSERT INTO {NomeTabela}
                                ({GetInsertNomes()})
                                VALUES ({GetInsertValores()})";
                await conexao.ExecuteAsync(sql, obj);
            }
        }

        public async Task<IList<T>> ReadAllAsync()
        {
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                string sql = @$"SELECT * FROM {NomeTabela}";
                var objetos = await conexao.QueryAsync<T>(sql);
                return objetos.ToList();
            }
        }
        public async Task<T> ReadAsync(string id)
        {
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                string sql = $"SELECT * FROM {NomeTabela} WHERE Id = @Id";
                var obj = await conexao.QuerySingleAsync<T>(sql, new { Id = id});
                return obj;
            }
        }

        public async Task UpdateAsync(T obj)
        {
            using (var conexao = new MySqlConnection(GetStringConnection()))
            {
                conexao.Open();
                string sql = @$"UPDATE {NomeTabela}
                                SET {GetUpdate()}
                                WHERE Id = @Id;";
                await conexao.ExecuteAsync(sql, obj);
            }
        }
        public async Task DeleteAsync(string id)
        {
            using (var conexao = new MySqlConnection(GetStringConnection())) 
            {
                conexao.Open();
                string sql = @$"DELETE FROM {NomeTabela} WHERE Id = @Id";
                await conexao.ExecuteAsync(sql, new { Id = id});
            }
        }

        private string GetInsertValores()
        {
            var sb = new StringBuilder();
            foreach (var m in Mapas)
            {
                sb.Append($", @{m.Propriedade}");
            }
            return sb.ToString().Substring(1);
        }

        private string GetUpdate() 
        { 
            var sb = new StringBuilder();
            foreach (var m in Mapas)
            {
                sb.Append($", {m.Campo}=@{m.Propriedade}");
            }
            return sb.ToString().Substring(1);
        }

        private string GetInsertNomes() 
        {
            var sb = new StringBuilder();
            foreach (var m in Mapas)
            {
                sb.Append($", {m.Campo}");
            }
            return sb.ToString().Substring(1);
        }
    }
}
