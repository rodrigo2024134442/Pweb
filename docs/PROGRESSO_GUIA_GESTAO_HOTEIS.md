# Progresso do guia: Gestão de Hotéis (backoffice)

Lista de todos os passos do guia **`MyHotels_Gestao_Guia.html`** ("MyHotels: Gestão de Hotéis, passo a passo"), para o grupo saber, a qualquer momento, em que passo está o projeto e quem está a fazer o quê.

---

## Onde estamos

| | |
|---|---|
| **Passo atual** | Secção 2 concluída e revista |
| **Próximo passo** | 3.a AppConfig |
| **Última atualização** | 2026-10-26 |
| **Base de dados** | `MyHotelsPWeb` (SQL Server LocalDB), ainda não criada |
| **Projeto criado para** | SQL Server LocalDB (a pasta `Data/Migrations` do template fica) |

> Atualizar esta caixa sempre que um passo muda de estado.

### 🔔 Lembretes em aberto

- [ ] Debater com o professor e com o colega se o **NIF** fica no `ApplicationUser` (decidir antes do passo 4.a). Ver a sessão 02.
- [ ] **Reler o porquê de cada entidade** (guia: secção 2.1 e blocos "Porquê" das secções 2.2 e 2.3), a pensar na defesa.

> Quando um lembrete estiver resolvido, marcar `[x]` e escrever a decisão no registo da sessão em que foi tomada.

---

## Como usar este ficheiro

1. Antes de começar um passo, puxar as alterações do colega (`git pull`) e ver aqui se alguém já o tem **Em curso**.
2. Pôr o passo **Em curso**, com o nome de quem o faz, e fazer commit só deste ficheiro (assim o colega vê logo).
3. Fazer o passo seguindo o guia, até ao bloco **Verificar** desse passo.
4. Pôr o passo **Feito**, com a data, e fazer commit com a mensagem `Passo X.y: <descrição>` (por exemplo `Passo 2.2: entidades`).
5. Quando o Claude rever o passo, marcar **Revisto** e escrever na coluna Notas o que foi corrigido.

### Estados

| Símbolo | Estado | Significado |
|---|---|---|
| ⬜ | Por fazer | Ninguém começou |
| 🟡 | Em curso | Alguém está a fazer (ver coluna Quem) |
| ✅ | Feito | Feito e verificado pelo bloco "Verificar" do guia |
| 🔍 | Revisto | Feito e revisto pelo Claude |
| ⚠️ | Com problemas | Feito mas não compila ou não passa no "Verificar"; ver Notas |

---

## Regras para trabalhar a dois

Resumo da ficha 5, secção E.11, aplicado a este projeto:

- **Um passo de cada vez por pessoa.** Dois passos que mexem no mesmo ficheiro (por exemplo o `Program.cs` ou o `ApplicationDbContext.cs`) nunca ficam **Em curso** ao mesmo tempo.
- **Migrations: só uma pessoa as cria.** A migration `BDInicial` (passo 4.a) é criada por uma pessoa e vai para o Git. O colega faz `git pull` e corre só `Update-Database`; nunca cria a mesma migration outra vez.
- **A base de dados não vai para o Git.** Cada um tem a sua `MyHotelsPWeb` no seu LocalDB, recriada com `Update-Database` + F5 (os dados de exemplo são criados pela `Inicializacao`).
- **Nunca alterar uma migration que já foi para o Git.** Para corrigir, cria-se uma migration nova.
- **Versões iguais.** O mesmo Visual Studio 2022, o mesmo SDK .NET 8 e os pacotes NuGet todos na versão **8.0.31**. Ninguém atualiza pacotes sozinho.
- **Pasta curta, fora do OneDrive.** Caminhos muito longos dão erro no SQL Server ("O nome de ficheiro ou a extensão é demasiado longo").

---

## Passos do guia

### Secção 0: Antes de começar (leitura)

| Passo | O que se faz | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|
| 0.1 a 0.5 | Ler: o que se vai construir, decisões sobre o enunciado, ponto de partida | ✅ | Marco | 2026-10-25 | |

### Secção 1: Criar o projeto

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 1.a | Solução `MyHotels` + projeto `GestaoHoteis` (Aplicativo Web Blazor, .NET 8, Contas Individuais, Servidor, Global) | `MyHotels.sln`, `GestaoHoteis.csproj` | ✅ | Marco | 2026-10-25 | Criado para LocalDB |
| 1.b | Apagar `Counter`, `Weather`, `Error` e `Auth` | `Components/Pages/` | ✅ | Marco | 2026-10-25 | Só ficou `Home.razor` |
| 1.c | Instalar a QuickGrid (`Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter` 8.0.31); connection string `Database=MyHotelsPWeb` | `GestaoHoteis.csproj`, `appsettings.json` | ✅ | Marco | 2026-10-26 | QuickGrid 8.0.31 instalada |
| 1.d | Ícones em `wwwroot/img`; imagens de exemplo em `wwwroot/img/seed` | `wwwroot/img/`, `wwwroot/img/seed/` | ✅ | Claude | 2026-10-26 | 7 ícones + 11 imagens (5 hotéis, 6 categorias), em PNG com fundo transparente, convertidos do canvas "MyHotels Imagens" |

### Secção 2: Modelo de dados

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 2.1 | Ler o MER e a tabela das relações (entregável do enunciado) | (nenhum) | ✅ | Marco | 2026-10-26 | |
| 2.2 | Pasta `Entities` com as 9 entidades | `Entities/Estados.cs`, `Morada.cs`, `Hotel.cs`, `Categoria.cs`, `ModoDisponibilizacao.cs`, `Alojamento.cs`, `Reserva.cs`, `ReservaItem.cs`, `Pagamento.cs` | 🔍 | Marco | 2026-10-26 | Revisto: o `Estados.cs` estava por preencher e a pasta `Entities` estava dentro de `Data`; ambos corrigidos |
| 2.3 | Utilizador, perfis e contexto | `Data/ApplicationUser.cs`, `Data/Perfis.cs` (novo), `Data/ApplicationDbContext.cs` | 🔍 | Marco | 2026-10-26 | Revisto: `ApplicationUser.cs` e `ApplicationDbContext.cs` estavam em ANSI; convertidos para UTF-8. Compila com 0 erros e 0 avisos |

### Secção 3: Serviços, dados iniciais e arranque

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 3.a | `AppConfig` | `AppConfig.cs` | ⬜ | | | |
| 3.b | Serviços: 5 interfaces + 5 classes | `Interfaces/I*Service.cs`, `Services/*Service.cs` (Imagens, Mensagens, Reservas, Pagamentos, Utilizadores) | ⬜ | | | |
| 3.c | Dados iniciais e de demonstração | `Data/Inicializacao.cs` | ⬜ | | | |
| 3.d | `Program.cs` completo (gravar em UTF-8) | `Program.cs` | ⬜ | | | Verificar: compila com zero erros |

### Secção 4: Base de dados

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 4.a | `Add-Migration BDInicial` + `Update-Database` | `Data/Migrations/<data>_BDInicial.cs` | ⬜ | | | **Só uma pessoa** cria a migration. Verificar: 16 tabelas |
| 4.b | Primeiro arranque (F5): dados de exemplo | (base de dados) | ⬜ | | | Verificar: 5 hotéis, 23 alojamentos, 9 reservas, 10 linhas, 9 pagamentos, 6 utilizadores |

### Secção 5: Layout, menu e componentes partilhados

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 5.a | `Routes.razor` (Acesso negado vs. login) | `Components/Routes.razor` | ⬜ | | | |
| 5.b | Componentes partilhados | `Components/Shared/Aviso.razor`, `Confirmacao.razor` | ⬜ | | | |
| 5.c | `_Imports.razor` | `Components/_Imports.razor` | ⬜ | | | |
| 5.d | Layout: MainLayout, TopBar (novo) e NavMenu, com os `.razor.css` | `Components/Layout/*` | ⬜ | | | Limpar e recompilar a solução depois de mexer em CSS |
| 5.e | Painel (Home) e Sobre | `Components/Pages/Home.razor`, `Sobre.razor` | ⬜ | | | |
| 5.f | Login: só entram contas ativas | `Components/Account/Pages/Login.razor` (2 excertos) | ⬜ | | | Verificar: `cliente3` não entra |

### Secção 6: Páginas CRUD

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 6.0 | Ler como se usa o scaffolding (gerar, substituir logo os 5 ficheiros, compilar) | (nenhum) | ⬜ | | | Depois do 1.º scaffolding, comparar o `Program.cs` com o da secção 3.d |
| 6.1 | Categorias | `Components/Pages/CategoriaPages/` (Index, Create, Edit, Details, Delete) | ⬜ | | | |
| 6.2 | Modos de disponibilização | `Components/Pages/ModoDisponibilizacaoPages/` (5 ficheiros) | ⬜ | | | |
| 6.3 | Hotéis | `Components/Pages/HotelPages/` (5 ficheiros) | ⬜ | | | |
| 6.4 | Alojamentos | `Components/Pages/AlojamentoPages/` (5 ficheiros) | ⬜ | | | Atenção à ordem em `AplicaFiltros` |
| 6.5 | Preços e stocks | `AlojamentoPages/Precos.razor` | ⬜ | | | |
| 6.6 | Só listagem | `AlojamentoPages/Listagens.razor` | ⬜ | | | |

### Secção 7: Reservas e pagamentos

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 7.a | Listagem de reservas | `Components/Pages/ReservaPages/Index.razor` | ⬜ | | | |
| 7.b | Detalhe e decisões sobre uma reserva | `Components/Pages/ReservaPages/Details.razor` | ⬜ | | | Verificar: reservas 8, 7, 1 e 3 (secção 7 do guia) |
| 7.c | Pagamentos | `Components/Pages/PagamentoPages/Index.razor` | ⬜ | | | Um pagamento por reserva |

### Secção 8: Utilizadores e perfis

| Passo | O que se faz | Ficheiros | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|---|
| 8 | Gestão de utilizadores | `Components/Pages/UtilizadorPages/Index.razor` | ⬜ | | | Testar como funcionário e como administrador |

### Secções 9 a 11: Relatório, testes e entrega

| Passo | O que se faz | Estado | Quem | Data | Notas |
|---|---|---|---|---|---|
| 9 | Copiar as regras de negócio (RN01 a RN26) para o relatório | ⬜ | | | |
| 10 | Fazer o percurso de testes completo e preencher a tabela de credenciais do PDF | ⬜ | | | |
| 11 | Lista de verificação, erros frequentes e entregáveis (MER, base de dados, relatório, código, PDF) | ⬜ | | | |

---

## Depois do backoffice

O enunciado pede mais duas aplicações, na mesma solução `MyHotels`. Não fazem parte deste guia:

| Aplicação | Estado |
|---|---|
| API RESTful (Swagger, Identity, JWT) | ⬜ |
| Frontend (Blazor Web + MAUI Blazor Hybrid + RCL) | ⬜ |
