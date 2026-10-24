import { el, formatWorkload } from "./dom.js";

const CATEGORY_ICONS = {
  matematica: "/assets/icons/icon-matematica-20px.svg",
  "ciencias-natureza": "/assets/icons/icon-ciencias-natureza-20px.svg",
  linguagens: "/assets/icons/icon-linguagens-20px.svg",
};

export function categoryTag(category) {
  const icon = CATEGORY_ICONS[category.iconKey];
  return el("span", { className: `tag tag--${category.iconKey}` }, [
    icon ? el("img", { attrs: { src: icon, alt: "", width: 20, height: 20 } }) : null,
    el("span", { text: category.name }),
  ]);
}

export function statusBadge(status) {
  return el("span", { className: `badge badge--status-${status.id}`, text: status.name });
}

export function courseCard(course, headingLevel = "h3") {
  return el("article", { className: "course-card" }, [
    el("img", {
      className: "course-card__image",
      attrs: { src: course.imageUrl, alt: "", width: 640, height: 480, loading: "lazy" },
    }),
    el("div", { className: "course-card__top" }, [categoryTag(course.category), statusBadge(course.status)]),
    el("div", { className: "course-card__body" }, [
      el(headingLevel, { className: "course-card__title" }, [
        el("a", {
          className: "course-card__link",
          text: course.fullName,
          attrs: { href: `/curso.html?id=${course.id}` },
        }),
      ]),
      el("p", { className: "course-card__summary", text: course.summary }),
      el("p", {
        className: "course-card__meta",
        text: `${course.modality.name} · ${formatWorkload(course.workloadHours)}`,
      }),
    ]),
  ]);
}

export function stateMessage({ title, text, variant, action }) {
  return el("div", { className: `state${variant ? ` state--${variant}` : ""}` }, [
    el("h2", { className: "state__title", text: title, attrs: { tabindex: "-1" } }),
    text ? el("p", { className: "state__text", text }) : null,
    action ?? null,
  ]);
}

export function loadingMessage(text) {
  return el("div", { className: "state", attrs: { role: "status" } }, [
    el("div", { className: "spinner", attrs: { "aria-hidden": "true" } }),
    el("p", { className: "state__text", text }),
  ]);
}
