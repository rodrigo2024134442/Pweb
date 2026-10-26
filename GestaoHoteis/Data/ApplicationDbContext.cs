using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// O namespace onde estão as nossas entidades: Hotel, Alojamento, Reserva, ...
using GestaoHoteis.Entities;

namespace GestaoHoteis.Data;

// O contexto representa a base de dados no código C#.
// Herda de IdentityDbContext, por isso já traz as tabelas dos utilizadores e dos roles.
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // Registo das nossas entidades no contexto. Cada DbSet passa a ser uma tabela
    // da base de dados, com o nome da propriedade: Moradas, Hoteis, Categorias, ...
    // É também por aqui que se leem e gravam os dados, por exemplo context.Alojamentos.
    public DbSet<Morada> Moradas { get; set; }
    public DbSet<Hotel> Hoteis { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<ModoDisponibilizacao> ModosDisponibilizacao { get; set; }
    public DbSet<Alojamento> Alojamentos { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<ReservaItem> ReservaItens { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    // Regras da base de dados. São a última garantia: valem para qualquer código que
    // grave nestas tabelas (as páginas, a API, um script). Ficam aqui, e não nas
    // entidades, porque são regras de armazenamento (Fluent API).
    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Obrigatório: configura as tabelas do Identity (AspNetUsers, AspNetRoles, ...).
        base.OnModelCreating(builder);

        // ---------- Nomes únicos ----------
        // Um índice único não deixa gravar dois registos com o mesmo nome.
        // O SQL Server não distingue maiúsculas: "Suite" e "suite" contam como iguais.
        builder.Entity<Hotel>().HasIndex(h => h.Nome).IsUnique();
        builder.Entity<Categoria>().HasIndex(c => c.Nome).IsUnique();
        builder.Entity<ModoDisponibilizacao>().HasIndex(m => m.Nome).IsUnique();
        // O nome de um alojamento é único dentro do hotel (dois hotéis podem ter
        // um "Quarto Duplo Standard"), por isso o índice tem duas colunas.
        builder.Entity<Alojamento>().HasIndex(a => new { a.HotelId, a.Nome }).IsUnique();

        // ---------- Relações e o que acontece ao apagar ----------
        // Restrict: a base de dados recusa apagar o registo principal enquanto houver
        // registos dependentes. Protege os dados: não se apaga um hotel, uma categoria
        // ou um alojamento que ainda tenham alojamentos ou reservas.
        builder.Entity<Alojamento>()
            .HasOne(a => a.hotel).WithMany(h => h.Alojamentos)
            .HasForeignKey(a => a.HotelId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Alojamento>()
            .HasOne(a => a.categoria).WithMany(c => c.Alojamentos)
            .HasForeignKey(a => a.CategoriaId).OnDelete(DeleteBehavior.Restrict);

        // SetNull: ao apagar um modo de disponibilização, os alojamentos que o usavam
        // ficam sem modo (a chave estrangeira passa a NULL) em vez de serem apagados.
        builder.Entity<Alojamento>()
            .HasOne(a => a.modoDisponibilizacao).WithMany(m => m.Alojamentos)
            .HasForeignKey(a => a.ModoDisponibilizacaoId).OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Hotel>()
            .HasOne(h => h.morada).WithMany()
            .HasForeignKey(h => h.MoradaId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reserva>()
            .HasOne(r => r.cliente).WithMany()
            .HasForeignKey(r => r.ClienteId).OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reserva>()
            .HasOne(r => r.hotel).WithMany()
            .HasForeignKey(r => r.HotelId).OnDelete(DeleteBehavior.Restrict);

        // Cascade: apagar uma reserva apaga as suas linhas e o seu pagamento.
        builder.Entity<ReservaItem>()
            .HasOne(i => i.reserva).WithMany(r => r.Itens)
            .HasForeignKey(i => i.ReservaId).OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ReservaItem>()
            .HasOne(i => i.alojamento).WithMany(a => a.ReservaItens)
            .HasForeignKey(i => i.AlojamentoId).OnDelete(DeleteBehavior.Restrict);

        // Um para um: WithOne em vez de WithMany. HasForeignKey<Pagamento> diz em que lado
        // fica a chave estrangeira (no pagamento), e o EF Core cria-lhe um índice único:
        // não pode haver dois pagamentos com o mesmo ReservaId.
        builder.Entity<Pagamento>()
            .HasOne(p => p.reserva).WithOne(r => r.pagamento)
            .HasForeignKey<Pagamento>(p => p.ReservaId).OnDelete(DeleteBehavior.Cascade);

        // ---------- Preço final: coluna calculada ----------
        // O SQL Server calcula PrecoFinal = preço base + percentagem, e guarda o resultado
        // (stored: true), por isso as listagens podem filtrar e ordenar por ele na base de dados.
        // É a mesma conta de Alojamento.CalculaPrecoFinal. O CAST fixa o tipo do resultado
        // em decimal(10,2); sem ele, o SQL Server guardava-o com 8 casas decimais.
        builder.Entity<Alojamento>()
            .Property(a => a.PrecoFinal)
            .HasComputedColumnSql("CAST(ROUND([PrecoBase] * (1 + [Margem] / 100), 2) AS decimal(10,2))", stored: true);

        // ---------- Restrições CHECK ----------
        // A base de dados recusa linhas que não cumpram estas condições.
        builder.Entity<Alojamento>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Alojamentos_PrecoBase", "[PrecoBase] > 0");
            t.HasCheckConstraint("CK_Alojamentos_Margem", "[Margem] >= 0");
            t.HasCheckConstraint("CK_Alojamentos_Unidades", "[Unidades] >= 0");
            // Ativo e reservável implica ter unidades. [Ativo] e [ParaReserva] são bit (0 ou 1).
            t.HasCheckConstraint("CK_Alojamentos_AtivoComUnidades",
                "[Ativo] = 0 OR [ParaReserva] = 0 OR [Unidades] > 0");
        });

        builder.Entity<Reserva>().ToTable(t =>
            t.HasCheckConstraint("CK_Reservas_Datas", "[DataSaida] > [DataEntrada]"));

        builder.Entity<ReservaItem>().ToTable(t =>
            t.HasCheckConstraint("CK_ReservaItens_Quantidade", "[Quantidade] > 0"));

        builder.Entity<Pagamento>().ToTable(t =>
            t.HasCheckConstraint("CK_Pagamentos_Valor", "[Valor] > 0"));
    }
}