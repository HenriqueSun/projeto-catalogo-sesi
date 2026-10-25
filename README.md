# SESI + Indústria — Catálogo de cursos

Atividade prática do processo seletivo 519/2026 (Desenvolvedor de Sistemas I). Todos os cursos e dados apresentados são fictícios.

## 1. Objetivo da aplicação

Aplicação web pública e somente leitura que apresenta o catálogo de cursos de um ambiente de ensino. O usuário pode:

- listar os cursos, obtidos pela API;
- pesquisar pelo nome, sem diferenciar acentos nem maiúsculas;
- filtrar por categoria, modalidade e status, e combinar a pesquisa com os filtros;
- ver a quantidade de resultados e limpar os filtros com uma ação;
- abrir os detalhes de um curso, com a ficha, os módulos e a descrição das aulas.

As mensagens de carregamento, de lista vazia e de erro (com a opção de tentar novamente) fazem parte da interface. Cadastro, edição e exclusão de cursos estão fora do escopo, conforme o enunciado.

## 2. Tecnologias utilizadas

| Camada | Tecnologia |
|---|---|
| Back-end e API | C# com .NET 10 (LTS) e ASP.NET Core Minimal APIs |
| Acesso a dados | Entity Framework Core 10 com migrations |
| Banco de dados | SQLite (código aberto, em arquivo) |
| Front-end | HTML, CSS e JavaScript (módulos ES), sem framework nem etapa de build |
| Testes | xUnit e `Microsoft.AspNetCore.Mvc.Testing` (testes de integração da API) |
| Publicação | Docker e Render (plano gratuito) |

## 3. Arquitetura da solução

Uma única aplicação ASP.NET Core serve o front-end estático (`wwwroot`) e a API (`/api`), na mesma origem. Com isso não é preciso configurar CORS, e a política de segurança de conteúdo pode se restringir a `'self'`.

```
Navegador ──HTTP──► ASP.NET Core
                     ├─ arquivos estáticos (HTML/CSS/JS)  ← wwwroot
                     ├─ /api/*  (Minimal APIs, só GET)    → EF Core → SQLite (somente leitura)
                     └─ /health
```

- **Front-end:** as páginas buscam os dados com `fetch`. Todo conteúdo vindo da API entra no DOM via `textContent`, sem `innerHTML`.
- **API:** `Catalog/CatalogEndpoints.cs`. Valida os parâmetros, devolve DTOs (`Catalog/CatalogContracts.cs`) e retorna erros no formato ProblemDetails (RFC 9457).
- **Dados:** `Data/CatalogDbContext.cs` define o modelo; `Data/CatalogSeed.cs` traz os dados fictícios, aplicados pela migration.
- **Segurança:**
  - Em execução, a aplicação abre o banco com `Mode=ReadOnly`. Uma conexão separada, de escrita, só aplica as migrations na inicialização.
  - As consultas são feitas pelo LINQ do EF Core, ou seja, parametrizadas.
  - A API tem rate limiting por IP (120 requisições por minuto).
  - Cabeçalhos de segurança: CSP, `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy` e `Permissions-Policy`.
  - Em produção, a aplicação envia HSTS e usa um tratamento de exceções que não expõe detalhes internos.

## 4. Estrutura das pastas

```
├── src/SesiCatalogo.Web/
│   ├── Catalog/          # Endpoints, contratos (DTOs) e normalização da pesquisa
│   ├── Data/             # DbContext, seed, inicialização do banco e Migrations/
│   ├── Domain/           # Entidades (Curso, Categoria, Modalidade, Status, Módulo, Aula)
│   ├── wwwroot/
│   │   ├── assets/       # Ícones, logos, fonte Open Sans (OFL) e imagens
│   │   ├── css/          # styles.css (tokens de design e componentes)
│   │   ├── js/           # api, dom, components, layout, carousel e um script por página
│   │   ├── index.html    # Página inicial
│   │   ├── cursos.html   # Catálogo com pesquisa e filtros
│   │   └── curso.html    # Detalhes do curso (?id=)
│   └── Program.cs        # Configuração, middlewares e cabeçalhos de segurança
├── tests/SesiCatalogo.Tests/   # Testes de integração da API e testes unitários
├── database/catalog.sql        # Script SQL gerado a partir das migrations
├── Dockerfile, .dockerignore, render.yaml
├── README.md
└── USO_DE_IA.md
```

## 5. Instruções para execução

**Pré-requisito:** [SDK do .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0). Não é preciso instalar nenhum banco de dados.

```bash
dotnet run --project src/SesiCatalogo.Web
```

Acesse http://localhost:5272. Na primeira execução, o arquivo `data/catalog.db` é criado e populado automaticamente.

Para rodar os testes:

```bash
dotnet test
```

Para rodar com Docker:

```bash
docker build -t sesi-catalogo .
docker run --rm -p 8080:8080 sesi-catalogo
```

Acesse http://localhost:8080.

## 6. Configuração do banco

- **Motor:** SQLite. O arquivo fica, por padrão, em `data/catalog.db`, relativo à pasta da aplicação. Para mudar o caminho, use a chave `Database:Path` (variável de ambiente `Database__Path`).
- **Criação:** a aplicação executa `Migrate()` ao iniciar. Isso cria o esquema e insere os dados fictícios, que estão em `HasData` na migration `InitialCatalog`.
- **Script SQL:** `database/catalog.sql` contém o mesmo esquema e os mesmos dados, para consulta ou criação manual (`sqlite3 catalog.db < database/catalog.sql`). Para gerá-lo de novo:

  ```bash
  dotnet tool restore
  dotnet ef migrations script -p src/SesiCatalogo.Web -o database/catalog.sql
  ```

- **Rate limiting:** o limite é configurável pela chave `RateLimiting:PermitsPerMinute` (padrão: 120).

## 7. Endpoints

Todos os endpoints são `GET` e respondem em JSON. Erros vêm como `application/problem+json`.

| Método e rota | Descrição | Respostas |
|---|---|---|
| `GET /api/courses` | Lista os cursos. Parâmetros opcionais: `search` (até 100 caracteres), `categoryId`, `modalityId` e `statusId` (inteiros ≥ 1), que podem ser combinados. | `200` com `{ total, items[] }`; `400` se algum parâmetro for inválido |
| `GET /api/courses/{id}` | Detalhes do curso, com módulos e aulas ordenados | `200`; `404` se o curso não existir |
| `GET /api/categories` | Categorias (id, nome e ícone) | `200` |
| `GET /api/modalities` | Modalidades | `200` |
| `GET /api/statuses` | Status | `200` |
| `GET /health` | Verificação de saúde, usada pela hospedagem | `200 Healthy` |

Excedido o limite de requisições, a API responde `429`.

Exemplo: `GET /api/courses?search=metalurgia&categoryId=2&modalityId=2`

## 8. Modelo de dados

```
Category (Id, Name, IconKey) ─┐
Modality (Id, Name) ──────────┼─< Course (Id, FullName, ShortName, Summary, Description,
CourseStatus (Id, Name) ──────┘          WorkloadHours, TargetAudience, ImagePath, ImageAlt,
                                         AvailableFrom?, SearchText)
                                           └─< CourseModule (Id, Position, Title)
                                                 └─< Lesson (Id, Position, Description)
```

- `ShortName` (nome breve) é único e serve como código do curso, por exemplo `MAT-PRECISAO`.
- `AvailableFrom` (data de disponibilização) é opcional. Quando vazio, a interface exibe "A definir".
- `SearchText` guarda o nome completo e o nome breve normalizados (minúsculas e sem acentos), o que permite pesquisar "matematica" e encontrar "Matemática".
- **Restrições:**
  - carga horária maior que zero;
  - nomes únicos;
  - posição única dos módulos dentro do curso e das aulas dentro do módulo;
  - categoria, modalidade e status com exclusão restrita;
  - módulos e aulas excluídos em cascata junto com o curso.
- **Dados fictícios:** 8 cursos, 3 categorias (Matemática, Ciências da Natureza e Linguagens), 2 modalidades (Online e Presencial) e 3 status (Inscrições abertas, Em breve e Encerrado). Os cursos variam em carga horária, módulos, aulas e público-alvo.

## 9. Estratégia de responsividade

- Abordagem fluida, com `clamp()` na tipografia e nos espaçamentos, CSS Grid e Flexbox. Os pontos de quebra ficam em 560, 720, 800 e 900 px.
- **Cabeçalho:** abaixo de 720 px, a navegação vira um menu expansível, com botão e `aria-expanded`; o botão "Área do aluno" continua visível.
- **Grade do catálogo:** usa `auto-fill`/`minmax`, passando de 1 coluna no celular para até 3 no desktop. Os filtros se empilham em telas estreitas.
- **Carrosséis (cursos e depoimentos):** rolagem horizontal nativa com `scroll-snap`, que funciona por toque. As setas são desativadas nas extremidades.
- **Verificação:** feita no navegador em 375 px e em 1920 px, sem rolagem horizontal da página.

## 10. Decisões de acessibilidade

- HTML semântico: `header`, `nav`, `main`, `footer`, títulos em ordem, listas e a ficha do curso em `dl`.
- Link "Pular para o conteúdo" e foco visível (`:focus-visible`) em todos os elementos interativos.
- Formulário de pesquisa com `role="search"` e rótulos associados a cada campo.
- A contagem de resultados e os estados de carregamento e erro ficam em regiões `aria-live`. Os avisos (por exemplo, ao clicar em "Matricule-se") usam uma região `role="status"` criada quando a página carrega.
- Ao limpar os filtros, o foco volta para o campo de pesquisa.
- Acordeões com `details`/`summary` nativos, operáveis por teclado sem JavaScript.
- Imagens informativas têm texto alternativo; imagens e ícones decorativos usam `alt=""` ou `aria-hidden`.
- Contraste do texto sobre fotos reforçado com degradê escuro.
- `prefers-reduced-motion` desativa animações e rolagem suave.
- Idioma da página declarado: `lang="pt-BR"`.

## 11. Processo de publicação

A hospedagem prevista é o [Render](https://render.com) no plano gratuito, usando o `Dockerfile` da raiz:

1. Publicar o repositório no GitHub.
2. No Render, criar um *Blueprint* apontando para o repositório. O arquivo `render.yaml` define o serviço web Docker, o plano gratuito, o health check em `/health` e a variável `ASPNETCORE_FORWARDEDHEADERS_ENABLED=true`. Essa variável faz a aplicação reconhecer o IP real do cliente e o HTTPS por trás do proxy do Render.
3. A cada push na branch principal, o Render reconstrói a imagem e publica de novo.

O banco SQLite é recriado a partir das migrations a cada inicialização do contêiner. Isso basta porque os dados são somente leitura.

**Alternativa: Vercel (plano Hobby, gratuito).** O Vercel não tem runtime .NET próprio, mas executa a mesma aplicação ASP.NET Core como contêiner, a partir do `Dockerfile.vercel` e do `vercel.json` da raiz:

1. Importar o repositório no Vercel, mantendo a raiz do projeto como diretório raiz e o *Framework Preset* como "Other".
2. Em *Settings → Environment Variables*, criar `PORT` com o valor `8080`. O Vercel envia o tráfego para a porta 80 por padrão, mas o contêiner roda sem privilégios de root e escuta na 8080 (`ASPNETCORE_HTTP_PORTS`).
3. Fazer um novo deploy.

No Vercel, o disco do contêiner é somente leitura, exceto `/tmp`. Por isso o `Dockerfile.vercel` grava o banco em `/tmp/catalog.db` (variável `Database__Path`). Como no Render, ele é recriado a cada inicialização.

**ZIP de publicação:** gerado com:

```bash
dotnet publish src/SesiCatalogo.Web -c Release -o publish
```

O conteúdo da pasta `publish` é então compactado em `dist/SesiCatalogo-publish.zip`. Para executar a partir do ZIP, com o runtime ASP.NET Core 10 instalado, rode `dotnet SesiCatalogo.Web.dll` dentro da pasta extraída.

## 12. Limitações conhecidas

**Itens não concluídos ou fora do escopo:**

- **Tela do AVA (simulação do Moodle):** não foi implementada. Exige login, progresso por aluno e integração real com o Moodle, o que está fora do escopo de um catálogo público. O enunciado trata a integração com o Moodle como diferencial opcional.
- **Matrícula e vídeo de apresentação:** não existem nesta versão. Os botões "Matricule-se" e de play exibem um aviso acessível, em vez de levar a um destino falso.
- **Hospedagem gratuita:** o serviço hiberna após um período sem acesso, e a primeira requisição depois disso pode levar alguns segundos.

**Diferenças em relação ao Figma, com justificativa:**

| Diferença | Justificativa |
|---|---|
| Página de catálogo (`cursos.html`) com pesquisa, filtros, contagem e estados | O Figma não tem essa tela, mas as funcionalidades são obrigatórias. Ela segue a mesma identidade visual. |
| Ficha do curso com carga horária, modalidade, status, código e público-alvo | São campos obrigatórios do enunciado que não aparecem no Figma |
| Estados de carregamento, vazio e erro; layout mobile | O Figma não tem esses estados nem quadros mobile; foram desenhados com os tokens existentes |
| Categoria "Linguagens" e cursos adicionais | O enunciado exige no mínimo 3 categorias e 6 cursos. O Figma mostra 2 categorias. |
| Datas de início ajustadas para datas futuras | As datas do Figma já passaram, o que conflitaria com o status "Inscrições abertas" |
| Imagens dos cartões recortadas das 3 fotos fornecidas | As fotos originais dos cartões não estavam entre os arquivos fornecidos |
| Links sem destino real omitidos (redes sociais, blog, termos) | Evita links quebrados; "Sobre" aponta para a seção de impacto da página inicial |
| "Área do aluno" como botão estático no cabeçalho | Presente como elemento visual; a área do aluno exige login e está fora do escopo |
| Visual modernizado: superfícies claras no lugar dos blocos vermelho-escuro e pêssego, cartões brancos com imagem no topo, cantos arredondados e cabeçalho translúcido | Mesma estrutura, conteúdo e identidade (vermelho SESI como cor de destaque), com aparência mais leve |
| "Cursos populares" no rodapé gerado a partir da API | Garante que os links levem a cursos que existem |
| Nome do 5º depoimento inferido | O cartão aparece cortado no Figma |
| Medidas, cores e espaçamentos aproximados | A API do Figma exige permissão de edição; os valores foram obtidos por inspeção visual do protótipo público |
