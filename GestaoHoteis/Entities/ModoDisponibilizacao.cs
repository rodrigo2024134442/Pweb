using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestaoHoteis.Entities;

// A forma como um alojamento é disponibilizado: só alojamento, com pequeno-almoço,
// meia pensão, pack de fim de semana...
// Cada objeto desta classe corresponde a uma linha da tabela ModosDisponibilizacao.
public class ModoDisponibilizacao
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Nome do modo.
    // [Required]: obrigatório no formulário e NOT NULL na tabela.
    // [StringLength(100)]: no máximo 100 caracteres, nvarchar(100) na tabela.
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome tem no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    // Descrição, opcional, até 200 caracteres.
    [StringLength(200, ErrorMessage = "O detalhe tem no máximo 200 caracteres.")]
    public string? Detalhe { get; set; }

    // Navegação de coleção: os alojamentos que usam este modo.
    // Não cria nenhuma coluna nesta tabela.
    [JsonIgnore]
    public ICollection<Alojamento>? Alojamentos { get; set; }
}