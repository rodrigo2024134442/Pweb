using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestaoHoteis.Entities;

// Um hotel ou aparthotel da cadeia.
// Cada objeto desta classe corresponde a uma linha da tabela Hoteis.
public class Hotel
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Nome do estabelecimento: obrigatório, único (índice no ApplicationDbContext).
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome tem no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    // "Hotel" ou "Aparthotel" (valores em TipoEstabelecimento).
    [Required]
    [StringLength(20)]
    public string Tipo { get; set; } = TipoEstabelecimento.Hotel;

    // Número de estrelas, de 1 a 5. É uma das formas de categorizar os hotéis.
    [Range(1, 5, ErrorMessage = "As estrelas têm de estar entre 1 e 5.")]
    public int Estrelas { get; set; } = 3;

    // Texto de apresentação, opcional.
    [StringLength(500, ErrorMessage = "A descrição tem no máximo 500 caracteres.")]
    public string? Descricao { get; set; }

    // Nome do ficheiro da imagem, e a imagem em si (bytes, coluna varbinary).
    [StringLength(200)]
    public string? UrlImagem { get; set; }

    public byte[]? Imagem { get; set; }

    // Um hotel inativo deixa de aparecer aos clientes (e de aceitar reservas).
    public bool Ativo { get; set; } = true;

    // Relação com Morada (obrigatória): todo o hotel tem morada. A cidade da
    // morada é a "localização" usada para categorizar os hotéis.
    // = null!: começa vazia; o EF Core preenche-a quando se pede Include(h => h.morada).
    // Não se usa = new(): um hotel lido SEM Include ficava com uma morada nova (Id 0),
    // e o EF Core, ao ligar esse hotel a um contexto, tentava gravá-la (INSERT).
    public int MoradaId { get; set; }
    public Morada morada { get; set; } = null!;

    // Navegação de coleção: os alojamentos deste hotel. Não cria coluna nesta tabela.
    // [JsonIgnore]: fica de fora quando o objeto é convertido em JSON (API).
    [JsonIgnore]
    public ICollection<Alojamento>? Alojamentos { get; set; }

    // Valida a morada do hotel. O <DataAnnotationsValidator /> de um formulário só valida
    // o objeto principal (o Hotel), e não os objetos que estão dentro dele (a morada):
    // por isso as páginas chamam este método antes de gravar.
    // Devolve null se estiver tudo bem, ou a primeira mensagem de erro.
    public string? ValidaMorada()
    {
        // Num hotel, a cidade é obrigatória: é a "localização" mostrada aos clientes.
        if (string.IsNullOrWhiteSpace(morada.Cidade))
        {
            return "A cidade do hotel é obrigatória.";
        }

        // As regras escritas na classe Morada ([StringLength], código postal 1234-567).
        var erros = new List<ValidationResult>();
        if (!Validator.TryValidateObject(morada, new ValidationContext(morada), erros, validateAllProperties: true))
        {
            return erros[0].ErrorMessage;
        }

        return null;
    }
}