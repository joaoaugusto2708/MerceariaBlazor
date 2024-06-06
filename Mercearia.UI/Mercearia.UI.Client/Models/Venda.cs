namespace Mercearia.UI.Client.Models
{
    public class Venda
    {
        public string? Id { get; set; }
        public string? NumVenda { get; set; }
        public decimal ValorVenda { get; set; }
        public DateTime DiaVenda { get; set; }
        public List<Produto>? Produtos { get; set; }
        public string? FormaPagamento { get; set; }
    }
}
