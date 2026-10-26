using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoHoteis.Entities;

// Um alojamento (quarto ou apartamento) de um hotel: é o "produto" da aplicação.
// Cada objeto desta classe corresponde a uma linha da tabela Alojamentos.
// IValidatableObject: a classe tem um método Validate com as regras que envolvem
// mais de um campo. O formulário e a API chamam-no ao validar.
public class Alojamento : IValidatableObject
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Nome do alojamento: obrigatório, até 100 caracteres.
    // É único dentro do hotel (índice HotelId + Nome no ApplicationDbContext).
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome tem no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    // Descrição do alojamento: obrigatória, até 500 caracteres.
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [StringLength(500, ErrorMessage = "A descrição tem no máximo 500 caracteres.")]
    public string? Descricao { get; set; }

    // Nome do ficheiro da imagem, e a imagem em si (bytes, coluna varbinary).
    [StringLength(200)]
    public string? UrlImagem { get; set; }

    public byte[]? Imagem { get; set; }

    // Preço base, por noite e por unidade: o preço definido pelo hotel.
    // [Column] fixa o tipo da coluna: 10 dígitos no total, 2 decimais.
    // Para dinheiro usa-se decimal, e não double, para não haver erros de arredondamento.
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 99999999.99, ErrorMessage = "O preço base tem de ser maior do que zero.")]
    public decimal PrecoBase { get; set; }

    // Percentagem somada ao preço base para obter o preço de venda.
    // 20 quer dizer: preço final = preço base + 20 %.
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 200, ErrorMessage = "A percentagem tem de estar entre 0 e 200.")]
    public decimal Margem { get; set; }

    // Preço final, o que os clientes veem: preço base + percentagem.
    // É uma coluna calculada pela base de dados (ApplicationDbContext): o SQL Server
    // recalcula-a sempre que o preço base ou a percentagem mudam. Por isso o set é private:
    // ninguém a escreve no código, só o EF Core a lê.
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoFinal { get; private set; }

    // Fórmula do preço final, escrita num só sítio. A coluna calculada da base de dados
    // usa a mesma conta (ver ApplicationDbContext); aqui serve para calcular antes de gravar
    // (pré-visualização nos formulários, dados de exemplo).
    public static decimal CalculaPrecoFinal(decimal precoBase, decimal margem)
        => Math.Round(precoBase * (1 + margem / 100m), 2);

    // Número de hóspedes que cada unidade comporta.
    [Range(1, 20, ErrorMessage = "A capacidade tem de estar entre 1 e 20 hóspedes.")]
    public int Capacidade { get; set; } = 2;

    // Stock: quantas unidades iguais a esta o hotel tem (por exemplo, 12 quartos duplos
    // standard). Uma reserva pode pedir mais de uma unidade do mesmo alojamento.
    [Range(0, 999, ErrorMessage = "As unidades têm de estar entre 0 e 999.")]
    public int Unidades { get; set; }

    // Ativo: o alojamento é visível aos clientes. Inativo: fica escondido,
    // sem se perderem os dados nem as reservas antigas.
    public bool Ativo { get; set; } = true;

    // ParaReserva: true = pode ser reservado; false = só aparece em listagem
    // (por exemplo, um quarto em obras ou que o hotel só quer mostrar).
    public bool ParaReserva { get; set; } = true;

    // Relação com Hotel (obrigatória): todo o alojamento pertence a um hotel.
    // É obrigatória porque a chave estrangeira é int, que não aceita NULL.
    public int HotelId { get; set; }
    // Navegação de referência: o objeto Hotel a que o alojamento pertence.
    public Hotel hotel { get; set; } = null!;

    // Relação com Categoria (obrigatória).
    public int CategoriaId { get; set; }
    public Categoria categoria { get; set; } = null!;

    // Relação com ModoDisponibilizacao (opcional).
    // É opcional porque a chave estrangeira é int?, que aceita NULL:
    // um alojamento pode ser gravado sem modo e recebê-lo mais tarde.
    public int? ModoDisponibilizacaoId { get; set; }
    public ModoDisponibilizacao? modoDisponibilizacao { get; set; }

    // Navegação de coleção: as linhas de reserva que pedem este alojamento.
    // Serve para saber se se pode apagar o alojamento (RN: só sem reservas).
    [JsonIgnore]
    public ICollection<ReservaItem>? ReservaItens { get; set; }

    // Regras que envolvem mais de um campo.
    // Corre depois das regras de cada campo ([Required], [Range], ...), se essas passarem.
    // Cada yield return é um erro: o texto e o campo onde o formulário o mostra.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Não faz sentido um alojamento reservável e visível sem nenhuma unidade.
        if (Ativo && ParaReserva && Unidades <= 0)
        {
            yield return new ValidationResult(
                "Um alojamento sem unidades não pode estar ativo e disponível para reserva.",
                [nameof(Ativo)]);
        }
    }
}