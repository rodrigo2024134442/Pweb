using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoHoteis.Entities;

// Uma linha de uma reserva: um alojamento e quantas unidades dele se reservam.
// É a tabela que resolve a relação muitos-para-muitos entre Reserva e Alojamento
// (uma reserva tem vários alojamentos, e um alojamento está em várias reservas).
// Cada objeto desta classe corresponde a uma linha da tabela ReservaItens.
public class ReservaItem
{
    // Chave primária, atribuída pela base de dados.
    public int Id { get; set; }

    // Relação com Reserva (obrigatória). Apagar a reserva apaga as suas linhas.
    public int ReservaId { get; set; }
    [JsonIgnore]
    public Reserva reserva { get; set; } = null!;

    // Relação com Alojamento (obrigatória). Sem cascata: um alojamento com reservas
    // não pode ser apagado (RN).
    public int AlojamentoId { get; set; }
    public Alojamento alojamento { get; set; } = null!;

    // Quantas unidades deste alojamento se reservam (por exemplo, 2 quartos duplos).
    [Range(1, 99, ErrorMessage = "A quantidade tem de estar entre 1 e 99.")]
    public int Quantidade { get; set; } = 1;

    // Preço por noite e por unidade NO MOMENTO DA RESERVA (uma cópia do preço final).
    // Guarda-se aqui para que, se o hotel mudar o preço depois, as reservas antigas
    // continuem a mostrar o que o cliente combinou.
    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoNoite { get; set; }

    // Subtotal desta linha, para o número de noites da reserva.
    public decimal Subtotal(int noites) => PrecoNoite * Quantidade * noites;
}