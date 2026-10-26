namespace GestaoHoteis.Entities;

// Os valores possíveis dos estados e tipos da aplicação, com nome.
// São classes static: não se criam objetos delas, usam-se diretamente, como em
// EstadoReserva.Pendente. Assim cada texto fica escrito num só sítio: um engano
// no nome dá erro de compilação, em vez de um erro escondido nos dados.

// Hotel ou aparthotel: o tipo de estabelecimento.
public static class TipoEstabelecimento
{
    public const string Hotel = "Hotel";
    public const string Aparthotel = "Aparthotel";

    public static readonly string[] Todos = [Hotel, Aparthotel];
}

// O ciclo de vida de uma reserva:
// Pendente -> Confirmada ou Rejeitada; Confirmada -> Cancelada.
public static class EstadoReserva
{
    public const string Pendente = "Pendente";
    public const string Confirmada = "Confirmada";
    public const string Rejeitada = "Rejeitada";
    public const string Cancelada = "Cancelada";

    public static readonly string[] Todos = [Pendente, Confirmada, Rejeitada, Cancelada];
}

// O estado de um pagamento (simulado).
public static class EstadoPagamento
{
    public const string Pendente = "Pendente";
    public const string Pago = "Pago";
    public const string Falhado = "Falhado";
    public const string Reembolsado = "Reembolsado";

    public static readonly string[] Todos = [Pendente, Pago, Falhado, Reembolsado];
}

// Os métodos de pagamento aceites (todos simulados).
public static class MetodoPagamento
{
    public const string Cartao = "Cartão";
    public const string MbWay = "MB Way";
    public const string Transferencia = "Transferência";

    public static readonly string[] Todos = [Cartao, MbWay, Transferencia];
}