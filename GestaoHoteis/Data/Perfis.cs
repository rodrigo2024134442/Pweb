namespace GestaoHoteis.Data;

// Os nomes dos três perfis (roles) da aplicação, tal como estão na tabela AspNetRoles.
// São const para poderem ser usados em atributos: [Authorize(Roles = Perfis.Gestao)].
// Escritos aqui, num só sítio, um engano no nome dá erro de compilação, em vez de
// uma página que "deixa de abrir" sem se perceber porquê.
public static class Perfis
{
    public const string Admin = "Admin";
    public const string Funcionario = "Funcionario";
    public const string Cliente = "Cliente";

    // Os perfis que gerem os hotéis: a vírgula quer dizer "ou" (basta ter um deles).
    // O Admin e o Funcionário fazem o mesmo, exceto gerir os funcionários (só o Admin).
    public const string Gestao = Admin + "," + Funcionario;

    // Todos os perfis, pela ordem em que são criados no arranque.
    public static readonly string[] Todos = [Admin, Funcionario, Cliente];
}