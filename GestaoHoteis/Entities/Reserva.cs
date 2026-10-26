using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GestaoHoteis.Data;

namespace GestaoHoteis.Entities;

// Uma reserva feita por um cliente: um ou mais alojamentos, todos do mesmo hotel,
// para as mesmas datas. É criada pela aplicação de frontend; a Gestão de Hotéis
// só a consulta e decide (confirmar, rejeitar, cancelar).
// Cada objeto desta classe corresponde a uma linha da tabela Reservas.
public class Reserva : IValidatableObject
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Relação com o cliente que reservou (obrigatória). O Id do utilizador é uma string,
    // porque o IdentityUser usa strings como chave.
    public string ClienteId { get; set; } = null!;
    public ApplicationUser cliente { get; set; } = null!;

    // Relação com Hotel (obrigatória): uma reserva é de um só hotel.
    public int HotelId { get; set; }
    public Hotel hotel { get; set; } = null!;

    // Quando a reserva foi feita (hora UTC).
    public DateTime DataReserva { get; set; } = DateTime.UtcNow;

    // Datas da estadia. DateOnly: só a data, sem hora (coluna date).
    // A noite de saída não conta: entrar a 10 e sair a 12 são duas noites.
    public DateOnly DataEntrada { get; set; }
    public DateOnly DataSaida { get; set; }

    // Número de hóspedes, para o hotel preparar a estadia.
    [Range(1, 100, ErrorMessage = "O número de hóspedes tem de estar entre 1 e 100.")]
    public int NumHospedes { get; set; } = 1;

    // Estado da reserva (valores em EstadoReserva). Começa Pendente.
    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = EstadoReserva.Pendente;

    // Total da reserva, calculado quando a reserva é criada:
    // soma, de cada linha, do preço por noite x unidades x noites.
    [Column(TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    // Pedidos do cliente (opcional) e motivo da rejeição (preenchido pelo funcionário).
    [StringLength(500)]
    public string? Observacoes { get; set; }

    [StringLength(300)]
    public string? MotivoRejeicao { get; set; }

    // Quando o documento de reserva foi "enviado" ao cliente (simulado). null = ainda não.
    public DateTime? DocumentoEnviadoEm { get; set; }

    // Navegação de coleção: as linhas da reserva (que alojamentos, quantas unidades).
    public ICollection<ReservaItem> Itens { get; set; } = [];

    // Navegação de referência: o pagamento da reserva. Uma reserva tem no máximo um
    // pagamento (relação 1 : 0..1); null enquanto o cliente ainda não pagou.
    public Pagamento? pagamento { get; set; }

    // Número de noites. [NotMapped]: calcula-se das datas, não tem coluna na tabela.
    [NotMapped]
    public int Noites => DataSaida.DayNumber - DataEntrada.DayNumber;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataSaida <= DataEntrada)
        {
            yield return new ValidationResult(
                "A data de saída tem de ser posterior à data de entrada.",
                [nameof(DataSaida)]);
        }
    }
}