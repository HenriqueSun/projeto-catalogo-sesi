using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SesiCatalogo.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    IconKey = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ModalityId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkloadHours = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetAudience = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ImageAlt = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AvailableFrom = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    SearchText = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.CheckConstraint("CK_Courses_WorkloadHours", "\"WorkloadHours\" > 0");
                    table.ForeignKey(
                        name: "FK_Courses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_CourseStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "CourseStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Modalities_ModalityId",
                        column: x => x.ModalityId,
                        principalTable: "Modalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseModules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_CourseModules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "CourseModules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IconKey", "Name" },
                values: new object[,]
                {
                    { 1, "matematica", "Matemática" },
                    { 2, "ciencias-natureza", "Ciências da Natureza" },
                    { 3, "linguagens", "Linguagens" }
                });

            migrationBuilder.InsertData(
                table: "CourseStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Inscrições abertas" },
                    { 2, "Em breve" },
                    { 3, "Encerrado" }
                });

            migrationBuilder.InsertData(
                table: "Modalities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "Presencial" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours" },
                values: new object[,]
                {
                    { 1, new DateOnly(2027, 1, 5), 1, "Explore nosso curso de Matemática para a Precisão: Medidas e Escalas na Metalurgia, perfeito para quem deseja se destacar nesse campo! Este programa abrangente foi desenvolvido para capacitar profissionais com as habilidades e conhecimentos fundamentais para garantir medições precisas e eficientes no ambiente metalúrgico.", "Matemática para a Precisão: Medidas e Escalas na Metalurgia", "Mãos medindo uma peça metálica com paquímetro.", "/assets/img/curso-1.jpg", 1, "matematica para a precisao: medidas e escalas na metalurgia mat-precisao", "MAT-PRECISAO", 1, "Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.", "Operadores, técnicos e supervisores da indústria metalúrgica que realizam ou conferem medições.", 60 },
                    { 2, new DateOnly(2027, 3, 8), 1, "Descubra como a geometria está presente em cada tecido e estampa. O curso apresenta simetrias, padrões de repetição e modelagem plana, conectando conceitos matemáticos ao dia a dia da produção têxtil e do desenvolvimento de coleções.", "Geometria e Modelagem no Design Têxtil", "Máquina têxtil com fios brancos tensionados em uma fábrica.", "/assets/img/curso-2.jpg", 2, "geometria e modelagem no design textil mat-geotextil", "MAT-GEOTEXTIL", 2, "Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.", "Modelistas, estilistas e profissionais de desenvolvimento de produto da indústria têxtil.", 40 },
                    { 3, new DateOnly(2026, 3, 2), 1, "Aprenda a transformar dados de produção em decisões. O curso aborda estatística descritiva, gráficos de controle e indicadores de qualidade, com exercícios baseados em situações reais de linhas de produção.", "Estatística e Controle de Qualidade na Produção", "Profissional com capacete apontando para um painel de controle industrial.", "/assets/img/curso-3.jpg", 1, "estatistica e controle de qualidade na producao mat-estqualidade", "MAT-ESTQUALIDADE", 3, "Leitura e interpretação de dados de controle de qualidade e produtividade.", "Analistas de qualidade, líderes de produção e técnicos de processos industriais.", 80 },
                    { 4, new DateOnly(2026, 11, 23), 2, "Entenda os princípios físicos que fazem máquinas, fornos e prensas funcionarem. O curso relaciona força, movimento, atrito e transferência de calor às rotinas da metalurgia, com foco em eficiência energética e segurança.", "Física do Movimento e da Energia na Metalurgia", "Operador ajustando uma peça em um torno mecânico.", "/assets/img/curso-4.jpg", 2, "fisica do movimento e da energia na metalurgia cn-fismetalurgia", "CN-FISMETALURGIA", 1, "Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.", "Operadores de máquinas, mantenedores e técnicos de processos metalúrgicos.", 48 },
                    { 5, new DateOnly(2026, 11, 9), 2, "Conheça a química por trás do tingimento e do acabamento de tecidos e descubra como reduzir impactos ambientais no processo. O curso combina fundamentos de reações químicas com práticas de uso racional de água e tratamento de efluentes.", "Química e Sustentabilidade", "Fios e bobinas de uma máquina têxtil em operação.", "/assets/img/curso-5.jpg", 1, "quimica e sustentabilidade cn-quisustentavel", "CN-QUISUSTENTAVEL", 1, "Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.", "Profissionais de tinturaria, acabamento têxtil e gestão ambiental.", 30 },
                    { 6, new DateOnly(2026, 5, 4), 2, "Aprofunde-se na estrutura dos metais e em como ela determina resistência, dureza e durabilidade. O curso apresenta ligas, ensaios mecânicos e tratamentos térmicos, ajudando a escolher o material certo para cada aplicação.", "Ciência dos Materiais na Metalurgia", "Peça cilíndrica de metal sendo medida com paquímetro.", "/assets/img/curso-6.jpg", 2, "ciencia dos materiais na metalurgia cn-cienmateriais", "CN-CIENMATERIAIS", 3, "Conceitos básicos de estrutura, resistência e propriedades dos metais.", "Técnicos de metalurgia, inspetores de qualidade e estudantes da área industrial.", 72 },
                    { 7, null, 3, "Desenvolva a comunicação escrita e oral no ambiente industrial. O curso trabalha a leitura de procedimentos e manuais, a redação de relatórios objetivos e a comunicação clara entre equipes e turnos.", "Comunicação Técnica e Leitura de Documentos Industriais", "Dois profissionais com equipamentos de proteção conversando em um ambiente industrial.", "/assets/img/curso-7.jpg", 1, "comunicacao tecnica e leitura de documentos industriais lin-comtecnica", "LIN-COMTECNICA", 2, "Leitura, interpretação e produção de procedimentos, relatórios e instruções de trabalho.", "Profissionais da indústria que redigem ou utilizam procedimentos, relatórios e instruções de trabalho.", 24 },
                    { 8, new DateOnly(2026, 12, 1), 3, "Ganhe autonomia para ler manuais, telas de equipamentos e normas internacionais em inglês. O curso foca no vocabulário técnico mais usado na indústria e em estratégias de leitura instrumental.", "Inglês Técnico para a Indústria", "Profissional sorridente operando uma máquina têxtil.", "/assets/img/curso-8.jpg", 1, "ingles tecnico para a industria lin-ingtecnico", "LIN-INGTECNICO", 1, "Vocabulário técnico em inglês para manuais, painéis de máquinas e normas internacionais.", "Profissionais da indústria com inglês básico que precisam ler documentação técnica.", 36 }
                });

            migrationBuilder.InsertData(
                table: "CourseModules",
                columns: new[] { "Id", "CourseId", "Position", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "Introdução à Matemática na Metalurgia" },
                    { 2, 1, 2, "Identificação e Avaliação de Medidas" },
                    { 3, 1, 3, "Normas e Regulamentações" },
                    { 4, 1, 4, "Instrumentos de Medição" },
                    { 5, 1, 5, "Prevenção de Erros de Medição" },
                    { 6, 1, 6, "Ergonomia e Eficiência na Medição" },
                    { 7, 1, 7, "Análise de Resultados de Medição" },
                    { 8, 1, 8, "Cultura de Precisão e Comportamento Seguro" },
                    { 9, 1, 9, "Tecnologias Aplicadas à Medição" },
                    { 10, 1, 10, "Avaliação Final e Certificação" },
                    { 11, 2, 1, "Formas e Medidas no Tecido" },
                    { 12, 2, 2, "Simetria e Padrões de Repetição" },
                    { 13, 2, 3, "Modelagem Plana" },
                    { 14, 2, 4, "Projeto Aplicado" },
                    { 15, 3, 1, "Fundamentos de Estatística" },
                    { 16, 3, 2, "Amostragem e Inspeção" },
                    { 17, 3, 3, "Controle Estatístico de Processo" },
                    { 18, 3, 4, "Indicadores de Qualidade e Produtividade" },
                    { 19, 3, 5, "Estudo de Caso Final" },
                    { 20, 4, 1, "Forças e Movimento" },
                    { 21, 4, 2, "Atrito e Desgaste" },
                    { 22, 4, 3, "Calor e Temperatura" },
                    { 23, 4, 4, "Energia e Eficiência" },
                    { 24, 5, 1, "Reações Químicas na Tinturaria" },
                    { 25, 5, 2, "Acabamentos Têxteis" },
                    { 26, 5, 3, "Produção Sustentável" },
                    { 27, 6, 1, "Estrutura dos Metais" },
                    { 28, 6, 2, "Ligas Metálicas" },
                    { 29, 6, 3, "Propriedades Mecânicas" },
                    { 30, 6, 4, "Ensaios de Materiais" },
                    { 31, 6, 5, "Tratamentos Térmicos" },
                    { 32, 7, 1, "Leitura de Documentos Técnicos" },
                    { 33, 7, 2, "Redação de Relatórios" },
                    { 34, 7, 3, "Comunicação entre Equipes" },
                    { 35, 8, 1, "Leitura Instrumental" },
                    { 36, 8, 2, "Vocabulário de Máquinas e Painéis" },
                    { 37, 8, 3, "Manuais e Normas Internacionais" },
                    { 38, 8, 4, "Segurança do Trabalho em Inglês" }
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "Description", "ModuleId", "Position" },
                values: new object[,]
                {
                    { 1, "Conceitos fundamentais de medidas e escalas aplicados à metalurgia.", 1, 1 },
                    { 2, "Importância da precisão nas medições e controle de qualidade.", 1, 2 },
                    { 3, "Panorama das normas e regulamentos no Brasil relacionados à metalurgia.", 1, 3 },
                    { 4, "Tipos de medidas utilizadas: comprimento, massa, volume e temperatura.", 2, 1 },
                    { 5, "Técnicas de análise de precisão nas medições.", 2, 2 },
                    { 6, "Elaboração de gráficos e tabelas de medidas.", 2, 3 },
                    { 7, "Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras).", 3, 1 },
                    { 8, "Interpretação e aplicação prática das normas de medição.", 3, 2 },
                    { 9, "Atualizações e impactos legais nas práticas de medição.", 3, 3 },
                    { 10, "Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças.", 4, 1 },
                    { 11, "Critérios de seleção, uso e calibração.", 4, 2 },
                    { 12, "Responsabilidades legais de operadores e supervisores.", 4, 3 },
                    { 13, "Tipos de erros e incertezas nas medições.", 5, 1 },
                    { 14, "Procedimentos para garantir a precisão e exatidão.", 5, 2 },
                    { 15, "Treinamento prático em técnicas de medição.", 5, 3 },
                    { 16, "Princípios ergonômicos aplicados ao uso de instrumentos de medição.", 6, 1 },
                    { 17, "Prevenção de lesões por uso inadequado de ferramentas.", 6, 2 },
                    { 18, "Qualidade de vida no ambiente de trabalho.", 6, 3 },
                    { 19, "Técnicas de análise de dados de medições.", 7, 1 },
                    { 20, "Elaboração de relatórios e planos de ação para melhorias.", 7, 2 },
                    { 21, "Indicadores de precisão e melhoria contínua.", 7, 3 },
                    { 22, "Desenvolvimento de uma cultura organizacional voltada à precisão nas medições.", 8, 1 },
                    { 23, "Comunicação eficaz e liderança em práticas de medição.", 8, 2 },
                    { 24, "Programas de conscientização e engajamento dos colaboradores.", 8, 3 },
                    { 25, "Uso de plataformas digitais, sensores e automação na medição de precisão.", 9, 1 },
                    { 26, "Sistemas de gestão de qualidade.", 9, 2 },
                    { 27, "Tendências e inovações no setor metalúrgico.", 9, 3 },
                    { 28, "Teste de conhecimentos adquiridos sobre medidas e escalas.", 10, 1 },
                    { 29, "Atividades práticas e estudos de caso.", 10, 2 },
                    { 30, "Emissão de certificado digital de conclusão.", 10, 3 },
                    { 31, "Figuras planas e suas propriedades aplicadas ao corte.", 11, 1 },
                    { 32, "Cálculo de áreas e aproveitamento de matéria-prima.", 11, 2 },
                    { 33, "Tipos de simetria: reflexão, rotação e translação.", 12, 1 },
                    { 34, "Construção de rapport para estamparia contínua.", 12, 2 },
                    { 35, "Escalas e graduação de moldes.", 13, 1 },
                    { 36, "Encaixe de moldes para reduzir desperdício.", 13, 2 },
                    { 37, "Desenvolvimento de uma estampa geométrica autoral.", 14, 1 },
                    { 38, "Apresentação e avaliação do projeto.", 14, 2 },
                    { 39, "Média, mediana, moda e dispersão.", 15, 1 },
                    { 40, "Coleta e organização de dados de produção.", 15, 2 },
                    { 41, "Tipos de gráficos e quando utilizá-los.", 15, 3 },
                    { 42, "Planos de amostragem para inspeção de lotes.", 16, 1 },
                    { 43, "Erros de amostragem e como evitá-los.", 16, 2 },
                    { 44, "Construção e leitura de cartas de controle.", 17, 1 },
                    { 45, "Identificação de causas comuns e especiais de variação.", 17, 2 },
                    { 46, "Capacidade de processo (Cp e Cpk).", 17, 3 },
                    { 47, "Definição de indicadores e metas.", 18, 1 },
                    { 48, "Painéis de acompanhamento para a liderança.", 18, 2 },
                    { 49, "Análise de um conjunto de dados de produção.", 19, 1 },
                    { 50, "Elaboração de plano de ação e relatório final.", 19, 2 },
                    { 51, "Leis de Newton aplicadas a máquinas industriais.", 20, 1 },
                    { 52, "Torque, alavancas e transmissão de movimento.", 20, 2 },
                    { 53, "Tipos de atrito e seus efeitos nos equipamentos.", 21, 1 },
                    { 54, "Lubrificação e manutenção preventiva.", 21, 2 },
                    { 55, "Transferência de calor em fornos e tratamentos térmicos.", 22, 1 },
                    { 56, "Dilatação térmica dos metais.", 22, 2 },
                    { 57, "Medição de temperatura no processo.", 22, 3 },
                    { 58, "Conservação de energia e rendimento de máquinas.", 23, 1 },
                    { 59, "Boas práticas para reduzir o consumo energético.", 23, 2 },
                    { 60, "Corantes, fixadores e auxiliares químicos.", 24, 1 },
                    { 61, "Influência de pH e temperatura no tingimento.", 24, 2 },
                    { 62, "Acabamentos químicos e suas finalidades.", 25, 1 },
                    { 63, "Segurança no manuseio de produtos químicos.", 25, 2 },
                    { 64, "Uso racional de água e energia.", 26, 1 },
                    { 65, "Tratamento de efluentes e legislação ambiental.", 26, 2 },
                    { 66, "Indicadores de sustentabilidade na indústria.", 26, 3 },
                    { 67, "Arranjos cristalinos e defeitos.", 27, 1 },
                    { 68, "Relação entre estrutura e propriedades.", 27, 2 },
                    { 69, "Aços, ferros fundidos e ligas não ferrosas.", 28, 1 },
                    { 70, "Diagrama ferro-carbono.", 28, 2 },
                    { 71, "Resistência, dureza, ductilidade e tenacidade.", 29, 1 },
                    { 72, "Fadiga e fluência.", 29, 2 },
                    { 73, "Ensaios de tração, dureza e impacto.", 30, 1 },
                    { 74, "Interpretação de laudos técnicos.", 30, 2 },
                    { 75, "Têmpera, revenimento e recozimento.", 31, 1 },
                    { 76, "Seleção de materiais para aplicações industriais.", 31, 2 },
                    { 77, "Estrutura de procedimentos e instruções de trabalho.", 32, 1 },
                    { 78, "Estratégias de leitura de manuais de equipamentos.", 32, 2 },
                    { 79, "Clareza, objetividade e organização das informações.", 33, 1 },
                    { 80, "Registro de ocorrências e não conformidades.", 33, 2 },
                    { 81, "Passagem de turno e comunicação de riscos.", 34, 1 },
                    { 82, "Apresentação de resultados para a liderança.", 34, 2 },
                    { 83, "Estratégias de skimming e scanning.", 35, 1 },
                    { 84, "Cognatos e falsos cognatos no contexto industrial.", 35, 2 },
                    { 85, "Termos comuns em interfaces de equipamentos.", 36, 1 },
                    { 86, "Alarmes, mensagens de erro e comandos.", 36, 2 },
                    { 87, "Estrutura de manuais técnicos em inglês.", 37, 1 },
                    { 88, "Leitura orientada de trechos de normas ISO.", 37, 2 },
                    { 89, "Sinalização e avisos de segurança.", 38, 1 },
                    { 90, "Fichas de dados de segurança (SDS).", 38, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseModules_CourseId_Position",
                table: "CourseModules",
                columns: new[] { "CourseId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ModalityId",
                table: "Courses",
                column: "ModalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ShortName",
                table: "Courses",
                column: "ShortName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_StatusId",
                table: "Courses",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStatuses_Name",
                table: "CourseStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ModuleId_Position",
                table: "Lessons",
                columns: new[] { "ModuleId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modalities_Name",
                table: "Modalities",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "CourseModules");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CourseStatuses");

            migrationBuilder.DropTable(
                name: "Modalities");
        }
    }
}
