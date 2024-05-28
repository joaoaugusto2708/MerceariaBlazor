using Mercearia.Model.Model;

namespace Mercearia.Model.DTO
{
    public class VendaDTO
    {
        public string NumVenda { get; set; }
        public decimal ValorVenda { get; set; }
        public DateTime DiaVenda { get; set; }
        public List<Produto>? Produtos { get; set; }
        public string? FormaPagamento { get; set; }
    }
}
