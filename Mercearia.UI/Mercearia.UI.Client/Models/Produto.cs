using System.ComponentModel.DataAnnotations;

namespace Mercearia.UI.Client.Models
{
    public class Produto
    {
        public string? Id { get; set; }
        public string? NumProduto { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantidade { get; set; }
        public bool Disponivel { get; set; }
        [Required(ErrorMessage = "O tipo de produto é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tipo de produto não pode exceder 50 caracteres.")]
        public string? TipoProduto { get; set; }
        [Required(ErrorMessage = "A validade é obrigatória.")]
        [DataType(DataType.Date, ErrorMessage = "Data de validade inválida.")]
        public DateTime Validade { get; set; }
        [StringLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome não pode exceder 200 caracteres.")]
        public string? Nome { get; set; }
    }
}
