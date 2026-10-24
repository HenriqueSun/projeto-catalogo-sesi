CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;
CREATE TABLE "Categories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "IconKey" TEXT NOT NULL
);

CREATE TABLE "CourseStatuses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_CourseStatuses" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL
);

CREATE TABLE "Modalities" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Modalities" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL
);

CREATE TABLE "Courses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Courses" PRIMARY KEY AUTOINCREMENT,
    "FullName" TEXT NOT NULL,
    "ShortName" TEXT NOT NULL,
    "Summary" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "CategoryId" INTEGER NOT NULL,
    "ModalityId" INTEGER NOT NULL,
    "StatusId" INTEGER NOT NULL,
    "WorkloadHours" INTEGER NOT NULL,
    "TargetAudience" TEXT NOT NULL,
    "ImagePath" TEXT NOT NULL,
    "ImageAlt" TEXT NOT NULL,
    "AvailableFrom" TEXT NULL,
    "SearchText" TEXT NOT NULL,
    CONSTRAINT "CK_Courses_WorkloadHours" CHECK ("WorkloadHours" > 0),
    CONSTRAINT "FK_Courses_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Courses_CourseStatuses_StatusId" FOREIGN KEY ("StatusId") REFERENCES "CourseStatuses" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Courses_Modalities_ModalityId" FOREIGN KEY ("ModalityId") REFERENCES "Modalities" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "CourseModules" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_CourseModules" PRIMARY KEY AUTOINCREMENT,
    "CourseId" INTEGER NOT NULL,
    "Position" INTEGER NOT NULL,
    "Title" TEXT NOT NULL,
    CONSTRAINT "FK_CourseModules_Courses_CourseId" FOREIGN KEY ("CourseId") REFERENCES "Courses" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Lessons" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Lessons" PRIMARY KEY AUTOINCREMENT,
    "ModuleId" INTEGER NOT NULL,
    "Position" INTEGER NOT NULL,
    "Description" TEXT NOT NULL,
    CONSTRAINT "FK_Lessons_CourseModules_ModuleId" FOREIGN KEY ("ModuleId") REFERENCES "CourseModules" ("Id") ON DELETE CASCADE
);

INSERT INTO "Categories" ("Id", "IconKey", "Name")
VALUES (1, 'matematica', 'Matemática');
SELECT changes();

INSERT INTO "Categories" ("Id", "IconKey", "Name")
VALUES (2, 'ciencias-natureza', 'Ciências da Natureza');
SELECT changes();

INSERT INTO "Categories" ("Id", "IconKey", "Name")
VALUES (3, 'linguagens', 'Linguagens');
SELECT changes();


INSERT INTO "CourseStatuses" ("Id", "Name")
VALUES (1, 'Inscrições abertas');
SELECT changes();

INSERT INTO "CourseStatuses" ("Id", "Name")
VALUES (2, 'Em breve');
SELECT changes();

INSERT INTO "CourseStatuses" ("Id", "Name")
VALUES (3, 'Encerrado');
SELECT changes();


INSERT INTO "Modalities" ("Id", "Name")
VALUES (1, 'Online');
SELECT changes();

INSERT INTO "Modalities" ("Id", "Name")
VALUES (2, 'Presencial');
SELECT changes();


INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (1, '2027-01-05', 1, 'Explore nosso curso de Matemática para a Precisão: Medidas e Escalas na Metalurgia, perfeito para quem deseja se destacar nesse campo! Este programa abrangente foi desenvolvido para capacitar profissionais com as habilidades e conhecimentos fundamentais para garantir medições precisas e eficientes no ambiente metalúrgico.', 'Matemática para a Precisão: Medidas e Escalas na Metalurgia', 'Mãos medindo uma peça metálica com paquímetro.', '/assets/img/curso-1.jpg', 1, 'matematica para a precisao: medidas e escalas na metalurgia mat-precisao', 'MAT-PRECISAO', 1, 'Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.', 'Operadores, técnicos e supervisores da indústria metalúrgica que realizam ou conferem medições.', 60);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (2, '2027-03-08', 1, 'Descubra como a geometria está presente em cada tecido e estampa. O curso apresenta simetrias, padrões de repetição e modelagem plana, conectando conceitos matemáticos ao dia a dia da produção têxtil e do desenvolvimento de coleções.', 'Geometria e Modelagem no Design Têxtil', 'Máquina têxtil com fios brancos tensionados em uma fábrica.', '/assets/img/curso-2.jpg', 2, 'geometria e modelagem no design textil mat-geotextil', 'MAT-GEOTEXTIL', 2, 'Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.', 'Modelistas, estilistas e profissionais de desenvolvimento de produto da indústria têxtil.', 40);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (3, '2026-03-02', 1, 'Aprenda a transformar dados de produção em decisões. O curso aborda estatística descritiva, gráficos de controle e indicadores de qualidade, com exercícios baseados em situações reais de linhas de produção.', 'Estatística e Controle de Qualidade na Produção', 'Profissional com capacete apontando para um painel de controle industrial.', '/assets/img/curso-3.jpg', 1, 'estatistica e controle de qualidade na producao mat-estqualidade', 'MAT-ESTQUALIDADE', 3, 'Leitura e interpretação de dados de controle de qualidade e produtividade.', 'Analistas de qualidade, líderes de produção e técnicos de processos industriais.', 80);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (4, '2026-11-23', 2, 'Entenda os princípios físicos que fazem máquinas, fornos e prensas funcionarem. O curso relaciona força, movimento, atrito e transferência de calor às rotinas da metalurgia, com foco em eficiência energética e segurança.', 'Física do Movimento e da Energia na Metalurgia', 'Operador ajustando uma peça em um torno mecânico.', '/assets/img/curso-4.jpg', 2, 'fisica do movimento e da energia na metalurgia cn-fismetalurgia', 'CN-FISMETALURGIA', 1, 'Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.', 'Operadores de máquinas, mantenedores e técnicos de processos metalúrgicos.', 48);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (5, '2026-11-09', 2, 'Conheça a química por trás do tingimento e do acabamento de tecidos e descubra como reduzir impactos ambientais no processo. O curso combina fundamentos de reações químicas com práticas de uso racional de água e tratamento de efluentes.', 'Química e Sustentabilidade', 'Fios e bobinas de uma máquina têxtil em operação.', '/assets/img/curso-5.jpg', 1, 'quimica e sustentabilidade cn-quisustentavel', 'CN-QUISUSTENTAVEL', 1, 'Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.', 'Profissionais de tinturaria, acabamento têxtil e gestão ambiental.', 30);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (6, '2026-05-04', 2, 'Aprofunde-se na estrutura dos metais e em como ela determina resistência, dureza e durabilidade. O curso apresenta ligas, ensaios mecânicos e tratamentos térmicos, ajudando a escolher o material certo para cada aplicação.', 'Ciência dos Materiais na Metalurgia', 'Peça cilíndrica de metal sendo medida com paquímetro.', '/assets/img/curso-6.jpg', 2, 'ciencia dos materiais na metalurgia cn-cienmateriais', 'CN-CIENMATERIAIS', 3, 'Conceitos básicos de estrutura, resistência e propriedades dos metais.', 'Técnicos de metalurgia, inspetores de qualidade e estudantes da área industrial.', 72);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (7, NULL, 3, 'Desenvolva a comunicação escrita e oral no ambiente industrial. O curso trabalha a leitura de procedimentos e manuais, a redação de relatórios objetivos e a comunicação clara entre equipes e turnos.', 'Comunicação Técnica e Leitura de Documentos Industriais', 'Dois profissionais com equipamentos de proteção conversando em um ambiente industrial.', '/assets/img/curso-7.jpg', 1, 'comunicacao tecnica e leitura de documentos industriais lin-comtecnica', 'LIN-COMTECNICA', 2, 'Leitura, interpretação e produção de procedimentos, relatórios e instruções de trabalho.', 'Profissionais da indústria que redigem ou utilizam procedimentos, relatórios e instruções de trabalho.', 24);
SELECT changes();

INSERT INTO "Courses" ("Id", "AvailableFrom", "CategoryId", "Description", "FullName", "ImageAlt", "ImagePath", "ModalityId", "SearchText", "ShortName", "StatusId", "Summary", "TargetAudience", "WorkloadHours")
VALUES (8, '2026-12-01', 3, 'Ganhe autonomia para ler manuais, telas de equipamentos e normas internacionais em inglês. O curso foca no vocabulário técnico mais usado na indústria e em estratégias de leitura instrumental.', 'Inglês Técnico para a Indústria', 'Profissional sorridente operando uma máquina têxtil.', '/assets/img/curso-8.jpg', 1, 'ingles tecnico para a industria lin-ingtecnico', 'LIN-INGTECNICO', 1, 'Vocabulário técnico em inglês para manuais, painéis de máquinas e normas internacionais.', 'Profissionais da indústria com inglês básico que precisam ler documentação técnica.', 36);
SELECT changes();


INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (1, 1, 1, 'Introdução à Matemática na Metalurgia');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (2, 1, 2, 'Identificação e Avaliação de Medidas');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (3, 1, 3, 'Normas e Regulamentações');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (4, 1, 4, 'Instrumentos de Medição');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (5, 1, 5, 'Prevenção de Erros de Medição');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (6, 1, 6, 'Ergonomia e Eficiência na Medição');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (7, 1, 7, 'Análise de Resultados de Medição');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (8, 1, 8, 'Cultura de Precisão e Comportamento Seguro');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (9, 1, 9, 'Tecnologias Aplicadas à Medição');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (10, 1, 10, 'Avaliação Final e Certificação');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (11, 2, 1, 'Formas e Medidas no Tecido');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (12, 2, 2, 'Simetria e Padrões de Repetição');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (13, 2, 3, 'Modelagem Plana');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (14, 2, 4, 'Projeto Aplicado');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (15, 3, 1, 'Fundamentos de Estatística');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (16, 3, 2, 'Amostragem e Inspeção');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (17, 3, 3, 'Controle Estatístico de Processo');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (18, 3, 4, 'Indicadores de Qualidade e Produtividade');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (19, 3, 5, 'Estudo de Caso Final');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (20, 4, 1, 'Forças e Movimento');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (21, 4, 2, 'Atrito e Desgaste');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (22, 4, 3, 'Calor e Temperatura');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (23, 4, 4, 'Energia e Eficiência');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (24, 5, 1, 'Reações Químicas na Tinturaria');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (25, 5, 2, 'Acabamentos Têxteis');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (26, 5, 3, 'Produção Sustentável');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (27, 6, 1, 'Estrutura dos Metais');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (28, 6, 2, 'Ligas Metálicas');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (29, 6, 3, 'Propriedades Mecânicas');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (30, 6, 4, 'Ensaios de Materiais');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (31, 6, 5, 'Tratamentos Térmicos');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (32, 7, 1, 'Leitura de Documentos Técnicos');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (33, 7, 2, 'Redação de Relatórios');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (34, 7, 3, 'Comunicação entre Equipes');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (35, 8, 1, 'Leitura Instrumental');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (36, 8, 2, 'Vocabulário de Máquinas e Painéis');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (37, 8, 3, 'Manuais e Normas Internacionais');
SELECT changes();

INSERT INTO "CourseModules" ("Id", "CourseId", "Position", "Title")
VALUES (38, 8, 4, 'Segurança do Trabalho em Inglês');
SELECT changes();


INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (1, 'Conceitos fundamentais de medidas e escalas aplicados à metalurgia.', 1, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (2, 'Importância da precisão nas medições e controle de qualidade.', 1, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (3, 'Panorama das normas e regulamentos no Brasil relacionados à metalurgia.', 1, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (4, 'Tipos de medidas utilizadas: comprimento, massa, volume e temperatura.', 2, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (5, 'Técnicas de análise de precisão nas medições.', 2, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (6, 'Elaboração de gráficos e tabelas de medidas.', 2, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (7, 'Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras).', 3, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (8, 'Interpretação e aplicação prática das normas de medição.', 3, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (9, 'Atualizações e impactos legais nas práticas de medição.', 3, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (10, 'Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças.', 4, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (11, 'Critérios de seleção, uso e calibração.', 4, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (12, 'Responsabilidades legais de operadores e supervisores.', 4, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (13, 'Tipos de erros e incertezas nas medições.', 5, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (14, 'Procedimentos para garantir a precisão e exatidão.', 5, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (15, 'Treinamento prático em técnicas de medição.', 5, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (16, 'Princípios ergonômicos aplicados ao uso de instrumentos de medição.', 6, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (17, 'Prevenção de lesões por uso inadequado de ferramentas.', 6, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (18, 'Qualidade de vida no ambiente de trabalho.', 6, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (19, 'Técnicas de análise de dados de medições.', 7, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (20, 'Elaboração de relatórios e planos de ação para melhorias.', 7, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (21, 'Indicadores de precisão e melhoria contínua.', 7, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (22, 'Desenvolvimento de uma cultura organizacional voltada à precisão nas medições.', 8, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (23, 'Comunicação eficaz e liderança em práticas de medição.', 8, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (24, 'Programas de conscientização e engajamento dos colaboradores.', 8, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (25, 'Uso de plataformas digitais, sensores e automação na medição de precisão.', 9, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (26, 'Sistemas de gestão de qualidade.', 9, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (27, 'Tendências e inovações no setor metalúrgico.', 9, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (28, 'Teste de conhecimentos adquiridos sobre medidas e escalas.', 10, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (29, 'Atividades práticas e estudos de caso.', 10, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (30, 'Emissão de certificado digital de conclusão.', 10, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (31, 'Figuras planas e suas propriedades aplicadas ao corte.', 11, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (32, 'Cálculo de áreas e aproveitamento de matéria-prima.', 11, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (33, 'Tipos de simetria: reflexão, rotação e translação.', 12, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (34, 'Construção de rapport para estamparia contínua.', 12, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (35, 'Escalas e graduação de moldes.', 13, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (36, 'Encaixe de moldes para reduzir desperdício.', 13, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (37, 'Desenvolvimento de uma estampa geométrica autoral.', 14, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (38, 'Apresentação e avaliação do projeto.', 14, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (39, 'Média, mediana, moda e dispersão.', 15, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (40, 'Coleta e organização de dados de produção.', 15, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (41, 'Tipos de gráficos e quando utilizá-los.', 15, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (42, 'Planos de amostragem para inspeção de lotes.', 16, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (43, 'Erros de amostragem e como evitá-los.', 16, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (44, 'Construção e leitura de cartas de controle.', 17, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (45, 'Identificação de causas comuns e especiais de variação.', 17, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (46, 'Capacidade de processo (Cp e Cpk).', 17, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (47, 'Definição de indicadores e metas.', 18, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (48, 'Painéis de acompanhamento para a liderança.', 18, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (49, 'Análise de um conjunto de dados de produção.', 19, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (50, 'Elaboração de plano de ação e relatório final.', 19, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (51, 'Leis de Newton aplicadas a máquinas industriais.', 20, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (52, 'Torque, alavancas e transmissão de movimento.', 20, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (53, 'Tipos de atrito e seus efeitos nos equipamentos.', 21, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (54, 'Lubrificação e manutenção preventiva.', 21, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (55, 'Transferência de calor em fornos e tratamentos térmicos.', 22, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (56, 'Dilatação térmica dos metais.', 22, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (57, 'Medição de temperatura no processo.', 22, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (58, 'Conservação de energia e rendimento de máquinas.', 23, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (59, 'Boas práticas para reduzir o consumo energético.', 23, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (60, 'Corantes, fixadores e auxiliares químicos.', 24, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (61, 'Influência de pH e temperatura no tingimento.', 24, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (62, 'Acabamentos químicos e suas finalidades.', 25, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (63, 'Segurança no manuseio de produtos químicos.', 25, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (64, 'Uso racional de água e energia.', 26, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (65, 'Tratamento de efluentes e legislação ambiental.', 26, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (66, 'Indicadores de sustentabilidade na indústria.', 26, 3);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (67, 'Arranjos cristalinos e defeitos.', 27, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (68, 'Relação entre estrutura e propriedades.', 27, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (69, 'Aços, ferros fundidos e ligas não ferrosas.', 28, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (70, 'Diagrama ferro-carbono.', 28, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (71, 'Resistência, dureza, ductilidade e tenacidade.', 29, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (72, 'Fadiga e fluência.', 29, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (73, 'Ensaios de tração, dureza e impacto.', 30, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (74, 'Interpretação de laudos técnicos.', 30, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (75, 'Têmpera, revenimento e recozimento.', 31, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (76, 'Seleção de materiais para aplicações industriais.', 31, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (77, 'Estrutura de procedimentos e instruções de trabalho.', 32, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (78, 'Estratégias de leitura de manuais de equipamentos.', 32, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (79, 'Clareza, objetividade e organização das informações.', 33, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (80, 'Registro de ocorrências e não conformidades.', 33, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (81, 'Passagem de turno e comunicação de riscos.', 34, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (82, 'Apresentação de resultados para a liderança.', 34, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (83, 'Estratégias de skimming e scanning.', 35, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (84, 'Cognatos e falsos cognatos no contexto industrial.', 35, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (85, 'Termos comuns em interfaces de equipamentos.', 36, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (86, 'Alarmes, mensagens de erro e comandos.', 36, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (87, 'Estrutura de manuais técnicos em inglês.', 37, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (88, 'Leitura orientada de trechos de normas ISO.', 37, 2);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (89, 'Sinalização e avisos de segurança.', 38, 1);
SELECT changes();

INSERT INTO "Lessons" ("Id", "Description", "ModuleId", "Position")
VALUES (90, 'Fichas de dados de segurança (SDS).', 38, 2);
SELECT changes();


CREATE UNIQUE INDEX "IX_Categories_Name" ON "Categories" ("Name");

CREATE UNIQUE INDEX "IX_CourseModules_CourseId_Position" ON "CourseModules" ("CourseId", "Position");

CREATE INDEX "IX_Courses_CategoryId" ON "Courses" ("CategoryId");

CREATE INDEX "IX_Courses_ModalityId" ON "Courses" ("ModalityId");

CREATE UNIQUE INDEX "IX_Courses_ShortName" ON "Courses" ("ShortName");

CREATE INDEX "IX_Courses_StatusId" ON "Courses" ("StatusId");

CREATE UNIQUE INDEX "IX_CourseStatuses_Name" ON "CourseStatuses" ("Name");

CREATE UNIQUE INDEX "IX_Lessons_ModuleId_Position" ON "Lessons" ("ModuleId", "Position");

CREATE UNIQUE INDEX "IX_Modalities_Name" ON "Modalities" ("Name");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20261024152239_InitialCatalog', '10.0.13');

COMMIT;

