import { getCourses } from "./api.js";
import { courseCard, loadingMessage, stateMessage } from "./components.js";
import { el } from "./dom.js";
import { setupCarousel } from "./carousel.js";
import { initLayout } from "./layout.js";

const FEATURED_LIMIT = 6;

function allCoursesCard() {
  return el("article", { className: "course-card course-card--all" }, [
    el("a", { className: "course-card__link", attrs: { href: "/cursos.html" } }, [
      el("span", { text: "Todos os cursos" }),
      el("img", { className: "button__arrow", attrs: { src: "/assets/icons/arrow-navigation-1.svg", alt: "", width: 24, height: 24 } }),
    ]),
  ]);
}

async function loadFeaturedCourses(carousel, updateControls) {
  const region = carousel.querySelector("[data-courses-region]");
  const track = carousel.querySelector("[data-carousel-track]");

  track.replaceChildren();
  region.replaceChildren(loadingMessage("Carregando cursos…"));

  try {
    const { items } = await getCourses();
    region.replaceChildren();
    track.replaceChildren(
      ...items.slice(0, FEATURED_LIMIT).map((course) => el("li", {}, [courseCard(course)])),
      el("li", {}, [allCoursesCard()]),
    );
  } catch {
    const retry = el("button", { className: "button button--dark", text: "Tentar novamente", attrs: { type: "button" } });
    retry.addEventListener("click", () => loadFeaturedCourses(carousel, updateControls));
    region.replaceChildren(
      stateMessage({
        title: "Não foi possível carregar os cursos",
        text: "Verifique sua conexão e tente novamente em instantes.",
        variant: "error",
        action: retry,
      }),
    );
  }

  updateControls();
}

initLayout();

const coursesCarousel = document.querySelector('[data-carousel="cursos"]');
const updateCoursesControls = setupCarousel(coursesCarousel);
setupCarousel(document.querySelector('[data-carousel="depoimentos"]'));
loadFeaturedCourses(coursesCarousel, updateCoursesControls);
