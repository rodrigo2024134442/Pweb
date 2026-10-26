# Sessão 01: enunciado, guia da Gestão de Hotéis e criação do projeto

| | |
|---|---|
| **Data** | 2026-10-25 |
| **Quem** | Marco, com o Claude |
| **Passos do guia no fim da sessão** | 1.a e 1.b feitos; 1.c em curso |

---

## 1. Análise do enunciado

- Enunciado: `TrabalhoPratico_PWeb20262027_TemaUnico_V1.pdf` (MyHotels, entrega a 28/12/2026).
- Três aplicações sobre **uma só base de dados**:
  - **Frontend público**: Blazor Web + Blazor Hybrid + RCL, um só código e um só CSS para Web, Android, iOS, macOS e Tizen;
  - **API RESTful**: Swagger, Identity + JWT, Dev Tunnel em desenvolvimento;
  - **Gestão de Hotéis** (backoffice): Blazor Web, fala diretamente com a base de dados.
- Entregáveis: MER, relatório (com a secção de regras de negócio), base de dados populada, código, PDF com os dados dos alunos e as credenciais.
- Notado: o enunciado tem restos de um tema anterior ("suportes media", "Editora", "Fornecedor", "Gestão de Loja").

## 2. Guia da Gestão de Hotéis

- Produzido o guia **`MyHotels_Gestao_Guia.html`**, no formato dos guias das fichas 3 a 5. Base: Ficha 2 (RCL), Fichas 3, 4 e 5 (Gestão de Loja). Onze secções, desde a criação do projeto até aos entregáveis.
- O ficheiro está na pasta `Desktop\PWEB\TP\img` (convém pô-lo também em `docs/` para o colega ter acesso).
- Antes de o escrever, todo o código foi compilado e testado: projeto .NET 8, migration, dados de exemplo e cada página testada no browser.
- Erros encontrados e corrigidos durante os testes, que ficaram explicados no guia:
  - mudar um filtro numa listagem com QuickGrid fazia duas consultas ao mesmo tempo no mesmo contexto. A solução é a ordem em `AplicaFiltros`: primeiro voltar à página 1, depois montar a consulta;
  - a morada do hotel com `= new()` inseria moradas vazias na base de dados; passou a `= null!`;
  - a coluna calculada `PrecoFinal` ficava com 8 casas decimais; passou a ter `CAST(... AS decimal(10,2))`.

### Decisões tomadas (ver secção 0.2 do guia)

| Decisão | Porquê |
|---|---|
| Perfis `Admin`, `Funcionario` e `Cliente` | O enunciado usa "Funcionário" |
| Sem perfil Fornecedor | Uma cadeia dona dos seus hotéis não tem fornecedores de alojamentos |
| Produtos = alojamentos; stock = `Unidades`; compras = reservas | Adaptação ao tema MyHotels |
| Preço final = preço base + percentagem, calculado pela base de dados | Regra do enunciado |
| "Atualizar stocks" = unidades livres calculadas pelas reservas confirmadas | Uma reserva só ocupa unidades nas suas datas |
| Pagamentos e envio do documento de reserva simulados | O enunciado pede que sejam simulados |
| Só entram contas com estado Activo (alteração ao `Login.razor`) | Quem se regista fica Pendente até ser aprovado |
| **Um pagamento por reserva** (relação 1 : 0..1) | Pedido pelo grupo. O guia foi atualizado, testado, e ganhou a regra RN26 |

## 3. Dúvidas esclarecidas

- **A Gestão também tem de ser Web + Android + iOS + macOS + Tizen?** Não. O enunciado só o exige ao frontend (pontos 1, 6 e 9). A Gestão é só Web e fala diretamente com o SQL Server, o que uma aplicação móvel não pode fazer.
- **`Unidades`, `Ativo` e `ParaReserva` na tabela Alojamentos:**
  - `Unidades`: quantos alojamentos iguais o hotel tem (o stock);
  - `Ativo`: se o alojamento é visível aos clientes (desativar esconde-o sem o apagar);
  - `ParaReserva`: se se pode reservar ou se só aparece na listagem.
- **Imagens em Alojamentos e em Categorias:** a da categoria é a do friso de categorias do frontend; a do alojamento é a fotografia daquele alojamento concreto. Decidido manter as duas.

## 4. Imagens (Claude Design)

- Criado no Claude Design o canvas **"MyHotels Imagens"**, com 18 imagens:
  - 5 hotéis e 6 categorias, a 220×120, para `wwwroot/img/seed`;
  - 7 ícones (`novo`, `editicon`, `detailsicon`, `deleteicon`, `saveicon`, `backicon`, `noproductstrans`), para `wwwroot/img`.
- **Por fazer:** exportar cada artboard como PNG, com o nome do título do artboard.

## 5. Projeto no Visual Studio (feito pelo Marco)

- Solução `MyHotels` com o projeto `GestaoHoteis`, em `Desktop\PWEB\TP\TP` (passo 1.a). Criado para SQL Server LocalDB.
- Apagadas as páginas de exemplo (passo 1.b).
- `appsettings.json` com `Database=MyHotelsPWeb` (passo 1.c, em parte).
- Repositório Git criado, com um primeiro commit ("firt commit"). Esse commit incluiu as pastas `obj/` por não haver `.gitignore`; corrigido na sessão 02.

## Por fazer no fim da sessão

- 1.c: instalar a QuickGrid.
- 1.d: exportar as imagens do canvas e copiar os ícones.
- Secção 2 do guia em diante.
