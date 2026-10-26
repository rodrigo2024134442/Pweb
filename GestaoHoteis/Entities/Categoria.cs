using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestaoHoteis.Entities;

// Uma categoria de alojamentos (por exemplo, "Suite" ou "Apartamento T1").
// Cada objeto desta classe corresponde a uma linha da tabela Categorias.
public class Categoria
{
    // Chave primária. Por se chamar Id, o EF Core reconhece-a sozinho,
    // e é a base de dados que atribui o valor (1, 2, 3...).
    public int Id { get; set; }

    // Nome da categoria: obrigatório, com 100 caracteres no máximo.
    // O tamanho é preciso para o índice único do nome (ApplicationDbContext):
    // o SQL Server não indexa colunas nvarchar(max).
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome tem no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    // Posição da categoria nas listagens: serve para as ordenar. Não pode ser negativa.
    [Range(0, 1000, ErrorMessage = "A ordem tem de estar entre 0 e 1000.")]
    public int Ordem { get; set; }

    // Nome do ficheiro da imagem, e a imagem em si (bytes, coluna varbinary).
    [StringLength(200)]
    public string? UrlImagem { get; set; }

    public byte[]? Imagem { get; set; }

    // Navegação de coleção: os alojamentos desta categoria.
    [JsonIgnore]
    public ICollection<Alojamento>? Alojamentos { get; set; }
}