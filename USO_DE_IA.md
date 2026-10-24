# Uso de Inteligência Artificial

## 1. Ferramenta utilizada

- **Cursor** (editor), no modo agente, com um modelo **Claude** da Anthropic.
- O agente leu os documentos da atividade e o protótipo público do Figma, rodou comandos (build, testes, migrations) e usou o navegador integrado para verificar as telas.

## 2. Objetivo do uso

- Analisar a documentação (PDF, e-mail e Figma) e transformá-la em requisitos, regras e um plano técnico.
- Acelerar a implementação: modelo de dados e seed fictício, API, front-end a partir do Figma e testes.
- Verificar o resultado no navegador (filtros, estados, responsividade) e redigir a documentação.

Todas as decisões de escopo foram revisadas por mim. O código foi validado por testes e por verificação manual antes de ser aceito.

## 3. Prompts relevantes: o que foi aproveitado ou descartado

| Prompt (resumo) | Aproveitado | Descartado |
|---|---|---|
| "Análise completa da documentação, planejamento técnico e preparação para desenvolvimento", com inventário, requisitos, regras, análise de Skills/Roles, arquitetura, segurança e plano em fases | Inventário dos arquivos, requisitos rastreados até o PDF, lacunas e plano de entrega | A parte de Skills, Roles e permissões: a documentação não menciona autenticação nem perfis, então essas premissas foram descartadas (ver o item 5) |
| "Criar tudo o que for possível agora que a análise terminou: protótipo e implementação das telas analisadas e o que mais couber no escopo do catálogo" | Página inicial, catálogo, detalhes do curso, API, banco, testes e documentação | A tela do AVA (simulação do Moodle): exigiria login e progresso por aluno, fora do escopo de um catálogo público somente leitura |
| Recomendação da própria IA de usar PostgreSQL (Neon) | A ideia de banco de código aberto com migrations versionadas | O PostgreSQL foi trocado por SQLite: o ambiente não tinha Docker nem Postgres, e um arquivo SQLite somente leitura atende a um catálogo sem escrita e roda localmente sem instalação |
| Sugestão de gerar imagens dos cursos com IA | — | Descartada; foram usados recortes das fotos fornecidas, para não introduzir conteúdo visual que não veio do material oficial |

## 4. Como o conteúdo foi validado

- **Testes automatizados:** 22 testes (xUnit), executados com `dotnet test`, todos passando.
  - Testes de integração da API com `WebApplicationFactory` e banco temporário, cobrindo:
    - pesquisa sem acento, sem diferenciar maiúsculas e pelo código;
    - cada filtro isolado e a combinação de pesquisa com filtros;
    - lista vazia;
    - `400` para parâmetros inválidos e `404` no formato ProblemDetails;
    - `405` para métodos de escrita;
    - cabeçalhos de segurança.
  - Testes unitários da normalização de texto.
- **Verificação no navegador:**
  - pesquisa, filtros combinados, URL sincronizada e "Limpar filtros";
  - lista vazia e erro de rede simulado, com "Tentar novamente";
  - página de detalhes, ids inexistente e malicioso (`<script>`) e acordeões;
  - aviso do "Matricule-se";
  - layout em 375 px e em 1920 px.
- **Fontes oficiais:**
  - documentação da Microsoft (.NET 10 LTS, Minimal APIs, ProblemDetails, rate limiting, EF Core com SQLite, cabeçalhos encaminhados);
  - documentação do Render (Docker, plano gratuito, Blueprint).
- **Revisão de requisitos:** cada funcionalidade obrigatória do PDF (seção 4c) foi conferida na aplicação, e a massa mínima de dados (seção 4b) foi conferida no seed.

## 5. Exemplos de erro ou inadequação identificados

1. **Premissa inexistente na análise:** o prompt inicial pedia análise de Skills, Roles e permissões. A leitura da documentação mostrou que esses conceitos não existem no escopo, que é um catálogo público e sem login. Seguir o prompt ao pé da letra levaria a construir autenticação desnecessária.
2. **Formato de erro incorreto na API:** no código gerado, o `404` do detalhe do curso era montado com um objeto `ProblemDetails` passado a `TypedResults.NotFound(...)`. O conteúdo parecia correto, mas a resposta saía como `application/json`, e não como `application/problem+json`. O teste automatizado de `404` falhou e revelou o problema.
3. **Defeitos visuais e de interface encontrados na verificação no navegador:**
   - seta dos botões claros invisível e apontando para o lado errado;
   - botão "anterior" do carrossel ativo já no início;
   - estrelas dos depoimentos com a cor errada;
   - botão "Limpar filtros" desativado no estado de erro mesmo com filtro ativo;
   - região de aviso criada só no primeiro clique, o que pode impedir leitores de tela de anunciar a primeira mensagem.
4. **Comando que não se aplica ao SQLite:** a IA tentou gerar um script SQL idempotente (`--idempotent`), recurso que o EF Core não suporta para SQLite. O comando falhou e foi refeito sem essa opção.

## 6. Correção realizada

1. O escopo foi limitado ao que o PDF exige: não há autenticação, perfis nem telas de administração, e a decisão está registrada no README.
2. O `404` passou a usar `TypedResults.Problem(title: ..., statusCode: 404)`, que gera `application/problem+json`. O teste agora passa e protege contra regressão.
3. Correções de interface:
   - troca do ícone da seta;
   - `scroll-padding` no carrossel;
   - ajuste do filtro de cor das estrelas;
   - o estado do botão "Limpar filtros" passou a ser calculado no início de cada pesquisa;
   - a região de aviso (`role="status"`) passou a ser criada quando a página carrega.

   Cada correção foi conferida de novo no navegador.
4. O script `database/catalog.sql` foi gerado sem `--idempotent`; como o banco é recriado do zero, a idempotência não é necessária.
