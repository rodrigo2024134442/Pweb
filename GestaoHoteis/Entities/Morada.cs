using System.ComponentModel.DataAnnotations;

namespace GestaoHoteis.Entities;

// Uma morada. Numa tabela própria, a mesma estrutura serve o utilizador
// (cliente ou funcionário) e o hotel, com as mesmas regras escritas uma só vez.
// Cada objeto desta classe corresponde a uma linha da tabela Moradas.
public class Morada
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Rua, número e andar.
    [StringLength(100)]
    public string? Rua { get; set; }

    [StringLength(100)]
    public string? Localidade { get; set; }

    [StringLength(100)]
    public string? Cidade { get; set; }

    // Só aceita o formato 1234-567: quatro dígitos, hífen, três dígitos.
    [StringLength(8)]
    [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "O código postal deve estar no formato '1234-567'.")]
    public string? CodigoPostal { get; set; }

    [StringLength(100)]
    public string? Pais { get; set; }
}