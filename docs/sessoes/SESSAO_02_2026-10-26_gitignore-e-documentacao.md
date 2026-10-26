# Sessão 02: .gitignore e documentação do grupo

| | |
|---|---|
| **Data** | 2026-10-26 |
| **Quem** | Marco, com o Claude |
| **Passos do guia no fim da sessão** | Secções 1 e 2 feitas; 2.2 e 2.3 revistas pelo Claude |

---

## 1. Integração do Claude com o Visual Studio 2022

- Não há extensão oficial do Claude Code para o Visual Studio 2022 (há para VS Code e JetBrains).
- Decidido: o Claude (app desktop) trabalha diretamente na pasta da solução (`Desktop\PWEB\TP\TP`). O Visual Studio fica para compilar, correr (F5), depurar e usar o Console do Gerenciador de Pacotes, e recarrega os ficheiros alterados fora dele.
- Forma de trabalhar: o grupo faz os passos do guia à mão no Visual Studio e, no fim de cada um, pede ao Claude para os rever.

## 2. `.gitignore`

- Criado o `.gitignore` do .NET (`dotnet new gitignore`) na pasta da solução. Ignora `bin/`, `obj/`, `.vs/` e `*.user`.
- Retirados do Git (`git rm --cached`) 16 ficheiros que tinham entrado no primeiro commit: a pasta `GestaoHoteis/obj/`, `GestaoHoteis.csproj.user` e `Properties/serviceDependencies.local.json.user`. **Continuam no disco**: só deixaram de ser seguidos pelo Git.
- **Por fazer:** commit destas alterações (o Claude não fez commit).

## 3. Documentação criada

| Ficheiro | Para quê |
|---|---|
| `docs/PROGRESSO_GUIA_GESTAO_HOTEIS.md` | Todos os passos do guia, com estado, quem, data e notas, e uma caixa "Onde estamos" no topo. Inclui as regras para trabalhar a dois (migrations, base de dados, versões) |
| `docs/sessoes/SESSAO_01_2026-10-25_enunciado-guia-e-criacao-do-projeto.md` | O que foi feito na sessão 01 (antes desta) |
| `docs/sessoes/SESSAO_02_2026-10-26_gitignore-e-documentacao.md` | Este ficheiro |

### Convenção para os próximos registos

- Um ficheiro por sessão de trabalho, em `docs/sessoes/`.
- Nome: `SESSAO_<número com 2 dígitos>_<data AAAA-MM-DD>_<assunto-curto>.md`, por exemplo `SESSAO_03_2026-10-28_entidades-e-contexto.md`.
- Conteúdo: data, quem, passos feitos, decisões, problemas encontrados e o que ficou por fazer.
- No fim de cada sessão, atualizar também o `PROGRESSO_GUIA_GESTAO_HOTEIS.md`.

## 4. Dúvidas esclarecidas

- **A pasta `GestaoHoteis` pode ser a raiz do GitHub?** Não convém. `GestaoHoteis` é a pasta do **projeto**; a da **solução** é a que tem o `MyHotels.sln`, e é ela que já é a raiz do repositório (`rodrigo2024134442/Pweb`). Com o projeto como raiz, o `.sln` e os futuros projetos da API e do frontend ficariam de fora.
- **Como abrir os `docs` no Visual Studio?** No Gerenciador de Soluções, Alternar Exibições > Exibição de Pasta. Em alternativa, criar uma Pasta de Solução `docs` e acrescentar os ficheiros com Item Existente.
- **A QuickGrid só é precisa em projetos criados para SQLite?** Não, é precisa sempre: o `Program.cs` e as listagens usam-na. Só a troca para o SQL Server é que depende de o projeto ter vindo com SQLite.

## 5. Imagens (passo 1.d)

- As 18 imagens do canvas "MyHotels Imagens" foram convertidas de SVG para PNG, com fundo transparente, pelo Chrome em modo headless. Os nomes foram confirmados pelo título de cada artboard.
- **`GestaoHoteis/wwwroot/img/`:** `novo.png`, `editicon.png`, `detailsicon.png`, `deleteicon.png`, `saveicon.png`, `backicon.png`, `noproductstrans.png` (128×128; o `noproductstrans` tem 220×120).
- **`GestaoHoteis/wwwroot/img/seed/`:** `hotel-lisboa`, `hotel-porto`, `hotel-coimbra`, `hotel-algarve`, `hotel-madeira`, `quarto-standard`, `quarto-superior`, `suite`, `apartamento-t0`, `apartamento-t1` e `apartamento-t2` (todos `.png`, 220×120).

## 6. Secção 2 do guia (modelo de dados), feita pelo Marco e revista pelo Claude

- **Dúvida esclarecida: para que serve o `ModoDisponibilizacao`?** É o regime em que o alojamento é vendido (só alojamento, pequeno-almoço, meia pensão, pensão completa, pack). O enunciado pede "Gerir os modos de disponibilização" e é o equivalente ao `ModoEntrega` das fichas. A relação é opcional, não entra no preço e serve de filtro no frontend.
- **Dúvida esclarecida: o enunciado pede o NIF?** Não. O enunciado só diz que os dados do registo de um cliente "devem ser específicos deste tipo de cliente". O NIF vem do `ApplicationUser` da ficha 3 (com o atributo `[validarNIF]`). Proposta: manter, justificando-o no relatório como dado para a faturação da estadia. Se se decidir retirar, é antes do passo 4.a (a migration): basta apagar a propriedade `NIF`, a classe `validarNIF` e o argumento do NIF nas chamadas a `CriaUtilizador` da `Inicializacao`.

> **🔔 Lembretes**
> - [ ] **Debater com o professor e com o colega** se o NIF fica no `ApplicationUser`. Decidir antes do passo 4.a; depois da migration, retirá-lo obriga a uma segunda migration.
> - [ ] **Reler o porquê de cada entidade** ser criada desta maneira: o MER e a tabela das relações (secção 2.1 do guia) e os blocos "Porquê" da secção 2.2 (Hotel, Alojamento, Reserva e ReservaItem, Pagamento) e da 2.3 (ApplicationUser, Perfis, ApplicationDbContext). A defesa vale 35 % e pode incluir explicar estas escolhas.
- **Primeira revisão** (não compilava, 4 erros):
  - `Estados.cs` ainda tinha a classe vazia criada pelo Visual Studio;
  - a pasta `Entities` estava dentro de `Data` (devia estar na raiz do projeto);
  - `ApplicationUser.cs` e `ApplicationDbContext.cs` estavam gravados em ANSI (Windows-1252) e não em UTF-8, o que estragava os acentos das mensagens de erro.
- **Segunda revisão**: tudo corrigido. Os 12 ficheiros têm o código igual ao guia e estão em UTF-8; o projeto compila com 0 erros e 0 avisos. A QuickGrid (passo 1.c) também já está instalada.
- **Lição para o grupo**: um ficheiro que já existia no template e em que se colam acentos deve ser gravado com "Salvar com Codificação... > Unicode (UTF-8 com assinatura)".

## Por fazer no fim da sessão

- Commit do `.gitignore`, da remoção das pastas `obj/` do Git e da pasta `docs/`.
- Copiar `MyHotels_Gestao_Guia.html` para `docs/`, para o colega também o ter.
- Começar a secção 3 do guia (3.a AppConfig).
