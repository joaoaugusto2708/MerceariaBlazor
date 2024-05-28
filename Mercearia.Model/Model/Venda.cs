using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercearia.Model.Model
{
    public class Venda
    {
        public string Id { get; set; }
        public string NumVenda { get; set; }
        public decimal ValorVenda { get; set; }
        public DateTime DiaVenda { get; set; }
        public List<Produto>? Produtos { get; set; }
        public string? FormaPagamento { get; set; }
    }
}
