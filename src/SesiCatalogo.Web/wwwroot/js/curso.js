import { ApiError, getCourse } from "./api.js";
import { categoryTag, loadingMessage, stateMessage } from "./components.js";
import { el, formatDate, formatWorkload } from "./dom.js";
import { setupCarousel } from "./carousel.js";
import { initLayout, showNotice } from "./layout.js";

const stateRegion = document.querySelector("[data-course-state]");
const content = document.querySelector("[data-course-content]");

const ENROLL_NOTICE = "As matrículas não fazem parte desta versão do catálogo. Fale com a gente para mais informações.";

function setText(selector, value) {
  for (const element of document.querySelectorAll(selector)) element.textContent = value;
}

function catalogLink() {
  return el("a", { className: "button button--dark", text: "Ver catálogo de cursos", attrs: { href: "/cursos.html" } });
}

function showState(options) {
  content.hidden = true;
  stateRegion.replaceChildren(stateMessage(options));
  stateRegion.querySelector(".state__title")?.focus();
}

function renderModules(modules) {
  return modules.map((module) =>
    el("li", {}, [
      el("h4", { text: `${module.position}. ${module.title}` }),
      el("ul", {}, module.lessons.map((lesson) => el("li", { text: lesson.description }))),
    ]),
  );
}

function renderCourse(course) {
  document.title = `${course.fullName} | SESI + Indústria`;

  document.querySelector("[data-course-category]").replaceChildren(categoryTag(course.category));
  setText("[data-course-name]", course.fullName);
  setText("[data-course-name-bar]", course.fullName);
  setText("[data-course-start]", formatDate(course.availableFrom) ?? "A definir");
  setText("[data-course-workload]", formatWorkload(course.workloadHours));
  setText("[data-course-modality]", course.modality.name);
  setText("[data-course-status]", course.status.name);
  setText("[data-course-shortname]", course.shortName);
  setText("[data-course-description]", course.description);
  setText("[data-course-audience]", course.targetAudience);

  const image = document.querySelector("[data-course-image]");
  image.src = course.imageUrl;
  image.alt = course.imageAlt;

  document.querySelector("[data-course-modules]").replaceChildren(...renderModules(course.modules));

  stateRegion.replaceChildren();
  content.hidden = false;
  updateTestimonialControls();
}

async function load() {
  const id = new URLSearchParams(location.search).get("id");

  if (!id || !/^\d{1,9}$/.test(id)) {
    showState({ title: "Curso não encontrado", text: "O endereço acessado não corresponde a nenhum curso do catálogo.", action: catalogLink() });
    return;
  }

  content.hidden = true;
  stateRegion.replaceChildren(loadingMessage("Carregando informações do curso…"));

  try {
    renderCourse(await getCourse(id));
  } catch (error) {
    if (error instanceof ApiError && error.status === 404) {
      showState({ title: "Curso não encontrado", text: "Este curso não existe ou não está mais disponível no catálogo.", action: catalogLink() });
      return;
    }
    console.error(error);
    const retry = el("button", { className: "button button--dark", text: "Tentar novamente", attrs: { type: "button" } });
    retry.addEventListener("click", load);
    showState({
      title: "Não foi possível carregar o curso",
      text: "Ocorreu um problema na comunicação com o servidor. Verifique sua conexão e tente novamente.",
      variant: "error",
      action: retry,
    });
  }
}

initLayout();
const updateTestimonialControls = setupCarousel(document.querySelector('[data-carousel="depoimentos"]'));
for (const button of document.querySelectorAll("[data-enroll]")) {
  button.addEventListener("click", () => showNotice(ENROLL_NOTICE));
}
load();
