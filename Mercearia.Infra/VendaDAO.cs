using Mercearia.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercearia.Infra
{
    public class VendaDAO : BaseDAO<Venda>
    {
        public override void Atualizar(Venda entity)
        {
            const string sql = @"UPDATE `mercearia`.`venda`
                                SET 
                               `ValorVenda` = @ValorVenda, `DiaVenda` = @DiaVenda, `FormaPagamento` = @FormaPagamento 
                                WHERE `Id` = @Id;";
            ExecutarQuery(sql, entity);
            foreach (var produto in entity.Produtos)
            {
                new ProdutoDAO().Atualizar(produto);
            }
        }

        public override Venda Buscar(string entity)
        {

            const string sql = @"SELECT * FROM mercearia.venda v 
                                inner join mercearia.venda_produto vp on v.Id = vp.idVenda
                                inner join mercearia.produto p on p.Id = vp.idProduto
                                WHERE v.NumVenda = @NumVenda;";
            return ConsultarVendas(sql, new { NumVenda = entity }).FirstOrDefault();
        }

        public override IEnumerable<Venda> BuscarTodos()
        {
            const string sql = @"SELECT * FROM mercearia.venda v 
                                inner join mercearia.venda_produto vp on v.Id = vp.idVenda
                                inner join mercearia.produto p on p.Id = vp.idProduto;";
            return ConsultarVendas(sql);
        }

        public override void Deletar(Venda entity)
        {
            const string sql = @"DELETE FROM `mercearia`.`venda`
                                 WHERE Id = @Id;";
            ExecutarQuery(sql, entity);
        }

        public override void Inserir(Venda entity)
        {
            const string sql = @"INSERT INTO `mercearia`.`venda`
                                (`Id`, `NumVenda`, `ValorVenda`, `DiaVenda`, `FormaPagamento`)
                                VALUES (@Id, @NumVenda, @ValorVenda, @DiaVenda, @FormaPagamento);";
            ExecutarQuery(sql, entity);
            InserirComFK(entity);
        }
        private void InserirComFK(Venda venda)
        {
            const string sql = @"INSERT INTO `mercearia`.`venda_produto` (`idProduto`, `idVenda`)
                                VALUES (@idProduto, @idVenda);";
            foreach (var produto in venda.Produtos)
            {
                string idProduto = produto.Id, idVenda = venda.Id;
                ExecutarQuery(sql, new { idVenda, idProduto });
            }
            
        }
    }
}
