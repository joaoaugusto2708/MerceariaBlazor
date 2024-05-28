namespace Mercearia.API.DTO
{
    public class ProdutoDTO
    {
        public string? NumProduto { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public bool Disponivel { get; set; }
        public string? TipoProduto { get; set; }
        public DateTime Validade { get; set; }
        public string? Descricao { get; set; }
        public string? Nome { get; set; }
    }
}
