using Microsoft.EntityFrameworkCore;
using SesiCatalogo.Web.Catalog;
using SesiCatalogo.Web.Domain;

namespace SesiCatalogo.Web.Data;

// All catalog data is fictitious, as required by the activity instructions.
internal static class CatalogSeed
{
    private const int Matematica = 1, CienciasDaNatureza = 2, Linguagens = 3;
    private const int Online = 1, Presencial = 2;
    private const int InscricoesAbertas = 1, EmBreve = 2, Encerrado = 3;

    private sealed record ModuleSeed(string Title, params string[] Lessons);

    private sealed record CourseSeed(
        int Id,
        string FullName,
        string ShortName,
        string Summary,
        string Description,
        int CategoryId,
        int ModalityId,
        int StatusId,
        int WorkloadHours,
        string TargetAudience,
        string ImageAlt,
        DateOnly? AvailableFrom,
        ModuleSeed[] Modules);

    public static void Apply(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = Matematica, Name = "Matemática", IconKey = "matematica" },
            new Category { Id = CienciasDaNatureza, Name = "Ciências da Natureza", IconKey = "ciencias-natureza" },
            new Category { Id = Linguagens, Name = "Linguagens", IconKey = "linguagens" });

        modelBuilder.Entity<Modality>().HasData(
            new Modality { Id = Online, Name = "Online" },
            new Modality { Id = Presencial, Name = "Presencial" });

        modelBuilder.Entity<CourseStatus>().HasData(
            new CourseStatus { Id = InscricoesAbertas, Name = "Inscrições abertas" },
            new CourseStatus { Id = EmBreve, Name = "Em breve" },
            new CourseStatus { Id = Encerrado, Name = "Encerrado" });

        var courses = new List<Course>();
        var modules = new List<CourseModule>();
        var lessons = new List<Lesson>();
        var moduleId = 0;
        var lessonId = 0;

        foreach (var seed in Courses)
        {
            courses.Add(new Course
            {
                Id = seed.Id,
                FullName = seed.FullName,
                ShortName = seed.ShortName,
                Summary = seed.Summary,
                Description = seed.Description,
                CategoryId = seed.CategoryId,
                ModalityId = seed.ModalityId,
                StatusId = seed.StatusId,
                WorkloadHours = seed.WorkloadHours,
                TargetAudience = seed.TargetAudience,
                ImagePath = $"/assets/img/curso-{seed.Id}.jpg",
                ImageAlt = seed.ImageAlt,
                AvailableFrom = seed.AvailableFrom,
                SearchText = TextNormalizer.Normalize($"{seed.FullName} {seed.ShortName}"),
            });

            for (var m = 0; m < seed.Modules.Length; m++)
            {
                var module = seed.Modules[m];
                moduleId++;
                modules.Add(new CourseModule { Id = moduleId, CourseId = seed.Id, Position = m + 1, Title = module.Title });

                for (var l = 0; l < module.Lessons.Length; l++)
                {
                    lessons.Add(new Lesson { Id = ++lessonId, ModuleId = moduleId, Position = l + 1, Description = module.Lessons[l] });
                }
            }
        }

        modelBuilder.Entity<Course>().HasData(courses);
        modelBuilder.Entity<CourseModule>().HasData(modules);
        modelBuilder.Entity<Lesson>().HasData(lessons);
    }

    private static readonly CourseSeed[] Courses =
    [
        new(1,
            "Matemática para a Precisão: Medidas e Escalas na Metalurgia",
            "MAT-PRECISAO",
            "Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.",
            "Explore nosso curso de Matemática para a Precisão: Medidas e Escalas na Metalurgia, perfeito para quem deseja se destacar nesse campo! Este programa abrangente foi desenvolvido para capacitar profissionais com as habilidades e conhecimentos fundamentais para garantir medições precisas e eficientes no ambiente metalúrgico.",
            Matematica, Online, InscricoesAbertas, 60,
            "Operadores, técnicos e supervisores da indústria metalúrgica que realizam ou conferem medições.",
            "Mãos medindo uma peça metálica com paquímetro.",
            new DateOnly(2027, 1, 5),
            [
                new("Introdução à Matemática na Metalurgia",
                    "Conceitos fundamentais de medidas e escalas aplicados à metalurgia.",
                    "Importância da precisão nas medições e controle de qualidade.",
                    "Panorama das normas e regulamentos no Brasil relacionados à metalurgia."),
                new("Identificação e Avaliação de Medidas",
                    "Tipos de medidas utilizadas: comprimento, massa, volume e temperatura.",
                    "Técnicas de análise de precisão nas medições.",
                    "Elaboração de gráficos e tabelas de medidas."),
                new("Normas e Regulamentações",
                    "Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras).",
                    "Interpretação e aplicação prática das normas de medição.",
                    "Atualizações e impactos legais nas práticas de medição."),
                new("Instrumentos de Medição",
                    "Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças.",
                    "Critérios de seleção, uso e calibração.",
                    "Responsabilidades legais de operadores e supervisores."),
                new("Prevenção de Erros de Medição",
                    "Tipos de erros e incertezas nas medições.",
                    "Procedimentos para garantir a precisão e exatidão.",
                    "Treinamento prático em técnicas de medição."),
                new("Ergonomia e Eficiência na Medição",
                    "Princípios ergonômicos aplicados ao uso de instrumentos de medição.",
                    "Prevenção de lesões por uso inadequado de ferramentas.",
                    "Qualidade de vida no ambiente de trabalho."),
                new("Análise de Resultados de Medição",
                    "Técnicas de análise de dados de medições.",
                    "Elaboração de relatórios e planos de ação para melhorias.",
                    "Indicadores de precisão e melhoria contínua."),
                new("Cultura de Precisão e Comportamento Seguro",
                    "Desenvolvimento de uma cultura organizacional voltada à precisão nas medições.",
                    "Comunicação eficaz e liderança em práticas de medição.",
                    "Programas de conscientização e engajamento dos colaboradores."),
                new("Tecnologias Aplicadas à Medição",
                    "Uso de plataformas digitais, sensores e automação na medição de precisão.",
                    "Sistemas de gestão de qualidade.",
                    "Tendências e inovações no setor metalúrgico."),
                new("Avaliação Final e Certificação",
                    "Teste de conhecimentos adquiridos sobre medidas e escalas.",
                    "Atividades práticas e estudos de caso.",
                    "Emissão de certificado digital de conclusão."),
            ]),
        new(2,
            "Geometria e Modelagem no Design Têxtil",
            "MAT-GEOTEXTIL",
            "Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.",
            "Descubra como a geometria está presente em cada tecido e estampa. O curso apresenta simetrias, padrões de repetição e modelagem plana, conectando conceitos matemáticos ao dia a dia da produção têxtil e do desenvolvimento de coleções.",
            Matematica, Presencial, EmBreve, 40,
            "Modelistas, estilistas e profissionais de desenvolvimento de produto da indústria têxtil.",
            "Máquina têxtil com fios brancos tensionados em uma fábrica.",
            new DateOnly(2027, 3, 8),
            [
                new("Formas e Medidas no Tecido",
                    "Figuras planas e suas propriedades aplicadas ao corte.",
                    "Cálculo de áreas e aproveitamento de matéria-prima."),
                new("Simetria e Padrões de Repetição",
                    "Tipos de simetria: reflexão, rotação e translação.",
                    "Construção de rapport para estamparia contínua."),
                new("Modelagem Plana",
                    "Escalas e graduação de moldes.",
                    "Encaixe de moldes para reduzir desperdício."),
                new("Projeto Aplicado",
                    "Desenvolvimento de uma estampa geométrica autoral.",
                    "Apresentação e avaliação do projeto."),
            ]),
        new(3,
            "Estatística e Controle de Qualidade na Produção",
            "MAT-ESTQUALIDADE",
            "Leitura e interpretação de dados de controle de qualidade e produtividade.",
            "Aprenda a transformar dados de produção em decisões. O curso aborda estatística descritiva, gráficos de controle e indicadores de qualidade, com exercícios baseados em situações reais de linhas de produção.",
            Matematica, Online, Encerrado, 80,
            "Analistas de qualidade, líderes de produção e técnicos de processos industriais.",
            "Profissional com capacete apontando para um painel de controle industrial.",
            new DateOnly(2026, 3, 2),
            [
                new("Fundamentos de Estatística",
                    "Média, mediana, moda e dispersão.",
                    "Coleta e organização de dados de produção.",
                    "Tipos de gráficos e quando utilizá-los."),
                new("Amostragem e Inspeção",
                    "Planos de amostragem para inspeção de lotes.",
                    "Erros de amostragem e como evitá-los."),
                new("Controle Estatístico de Processo",
                    "Construção e leitura de cartas de controle.",
                    "Identificação de causas comuns e especiais de variação.",
                    "Capacidade de processo (Cp e Cpk)."),
                new("Indicadores de Qualidade e Produtividade",
                    "Definição de indicadores e metas.",
                    "Painéis de acompanhamento para a liderança."),
                new("Estudo de Caso Final",
                    "Análise de um conjunto de dados de produção.",
                    "Elaboração de plano de ação e relatório final."),
            ]),
        new(4,
            "Física do Movimento e da Energia na Metalurgia",
            "CN-FISMETALURGIA",
            "Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.",
            "Entenda os princípios físicos que fazem máquinas, fornos e prensas funcionarem. O curso relaciona força, movimento, atrito e transferência de calor às rotinas da metalurgia, com foco em eficiência energética e segurança.",
            CienciasDaNatureza, Presencial, InscricoesAbertas, 48,
            "Operadores de máquinas, mantenedores e técnicos de processos metalúrgicos.",
            "Operador ajustando uma peça em um torno mecânico.",
            new DateOnly(2026, 11, 23),
            [
                new("Forças e Movimento",
                    "Leis de Newton aplicadas a máquinas industriais.",
                    "Torque, alavancas e transmissão de movimento."),
                new("Atrito e Desgaste",
                    "Tipos de atrito e seus efeitos nos equipamentos.",
                    "Lubrificação e manutenção preventiva."),
                new("Calor e Temperatura",
                    "Transferência de calor em fornos e tratamentos térmicos.",
                    "Dilatação térmica dos metais.",
                    "Medição de temperatura no processo."),
                new("Energia e Eficiência",
                    "Conservação de energia e rendimento de máquinas.",
                    "Boas práticas para reduzir o consumo energético."),
            ]),
        new(5,
            "Química e Sustentabilidade",
            "CN-QUISUSTENTAVEL",
            "Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.",
            "Conheça a química por trás do tingimento e do acabamento de tecidos e descubra como reduzir impactos ambientais no processo. O curso combina fundamentos de reações químicas com práticas de uso racional de água e tratamento de efluentes.",
            CienciasDaNatureza, Online, InscricoesAbertas, 30,
            "Profissionais de tinturaria, acabamento têxtil e gestão ambiental.",
            "Fios e bobinas de uma máquina têxtil em operação.",
            new DateOnly(2026, 11, 9),
            [
                new("Reações Químicas na Tinturaria",
                    "Corantes, fixadores e auxiliares químicos.",
                    "Influência de pH e temperatura no tingimento."),
                new("Acabamentos Têxteis",
                    "Acabamentos químicos e suas finalidades.",
                    "Segurança no manuseio de produtos químicos."),
                new("Produção Sustentável",
                    "Uso racional de água e energia.",
                    "Tratamento de efluentes e legislação ambiental.",
                    "Indicadores de sustentabilidade na indústria."),
            ]),
        new(6,
            "Ciência dos Materiais na Metalurgia",
            "CN-CIENMATERIAIS",
            "Conceitos básicos de estrutura, resistência e propriedades dos metais.",
            "Aprofunde-se na estrutura dos metais e em como ela determina resistência, dureza e durabilidade. O curso apresenta ligas, ensaios mecânicos e tratamentos térmicos, ajudando a escolher o material certo para cada aplicação.",
            CienciasDaNatureza, Presencial, Encerrado, 72,
            "Técnicos de metalurgia, inspetores de qualidade e estudantes da área industrial.",
            "Peça cilíndrica de metal sendo medida com paquímetro.",
            new DateOnly(2026, 5, 4),
            [
                new("Estrutura dos Metais",
                    "Arranjos cristalinos e defeitos.",
                    "Relação entre estrutura e propriedades."),
                new("Ligas Metálicas",
                    "Aços, ferros fundidos e ligas não ferrosas.",
                    "Diagrama ferro-carbono."),
                new("Propriedades Mecânicas",
                    "Resistência, dureza, ductilidade e tenacidade.",
                    "Fadiga e fluência."),
                new("Ensaios de Materiais",
                    "Ensaios de tração, dureza e impacto.",
                    "Interpretação de laudos técnicos."),
                new("Tratamentos Térmicos",
                    "Têmpera, revenimento e recozimento.",
                    "Seleção de materiais para aplicações industriais."),
            ]),
        new(7,
            "Comunicação Técnica e Leitura de Documentos Industriais",
            "LIN-COMTECNICA",
            "Leitura, interpretação e produção de procedimentos, relatórios e instruções de trabalho.",
            "Desenvolva a comunicação escrita e oral no ambiente industrial. O curso trabalha a leitura de procedimentos e manuais, a redação de relatórios objetivos e a comunicação clara entre equipes e turnos.",
            Linguagens, Online, EmBreve, 24,
            "Profissionais da indústria que redigem ou utilizam procedimentos, relatórios e instruções de trabalho.",
            "Dois profissionais com equipamentos de proteção conversando em um ambiente industrial.",
            null,
            [
                new("Leitura de Documentos Técnicos",
                    "Estrutura de procedimentos e instruções de trabalho.",
                    "Estratégias de leitura de manuais de equipamentos."),
                new("Redação de Relatórios",
                    "Clareza, objetividade e organização das informações.",
                    "Registro de ocorrências e não conformidades."),
                new("Comunicação entre Equipes",
                    "Passagem de turno e comunicação de riscos.",
                    "Apresentação de resultados para a liderança."),
            ]),
        new(8,
            "Inglês Técnico para a Indústria",
            "LIN-INGTECNICO",
            "Vocabulário técnico em inglês para manuais, painéis de máquinas e normas internacionais.",
            "Ganhe autonomia para ler manuais, telas de equipamentos e normas internacionais em inglês. O curso foca no vocabulário técnico mais usado na indústria e em estratégias de leitura instrumental.",
            Linguagens, Online, InscricoesAbertas, 36,
            "Profissionais da indústria com inglês básico que precisam ler documentação técnica.",
            "Profissional sorridente operando uma máquina têxtil.",
            new DateOnly(2026, 12, 1),
            [
                new("Leitura Instrumental",
                    "Estratégias de skimming e scanning.",
                    "Cognatos e falsos cognatos no contexto industrial."),
                new("Vocabulário de Máquinas e Painéis",
                    "Termos comuns em interfaces de equipamentos.",
                    "Alarmes, mensagens de erro e comandos."),
                new("Manuais e Normas Internacionais",
                    "Estrutura de manuais técnicos em inglês.",
                    "Leitura orientada de trechos de normas ISO."),
                new("Segurança do Trabalho em Inglês",
                    "Sinalização e avisos de segurança.",
                    "Fichas de dados de segurança (SDS)."),
            ]),
    ];
}
