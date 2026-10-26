using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoHoteis.Entities;

// Um pagamento de uma reserva. O pagamento é SIMULADO: não há banco nem cartões a sério.
// O cliente "paga" na aplicação de frontend, e a Gestão de Hotéis pode simular o resultado.
// Cada objeto desta classe corresponde a uma linha da tabela Pagamentos.
public class Pagamento
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Relação com Reserva (obrigatória e 1 : 0..1): cada reserva tem no máximo um pagamento.
    // A chave estrangeira ReservaId tem um índice único (ApplicationDbContext), por isso
    // a base de dados recusa um segundo pagamento para a mesma reserva.
    // Apagar a reserva apaga o seu pagamento.
    public int ReservaId { get; set; }
    [JsonIgnore]
    public Reserva reserva { get; set; } = null!;

    // Valor pago. Tem de ser maior do que zero (CHECK na base de dados).
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 99999999.99, ErrorMessage = "O valor tem de ser maior do que zero.")]
    public decimal Valor { get; set; }

    // Cartão, MB Way ou Transferência (valores em MetodoPagamento).
    [Required]
    [StringLength(30)]
    public string Metodo { get; set; } = MetodoPagamento.Cartao;

    // Estado do pagamento (valores em EstadoPagamento). Começa Pendente.
    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = EstadoPagamento.Pendente;

    // Quando foi pago (hora UTC). null enquanto não for pago.
    public DateTime? DataPagamento { get; set; }

    // Referência do pagamento (inventada pela simulação).
    [StringLength(50)]
    public string? Referencia { get; set; }
}