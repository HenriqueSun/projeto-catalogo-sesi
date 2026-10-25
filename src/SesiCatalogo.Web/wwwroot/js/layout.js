import { getCourses } from "./api.js";
import { el } from "./dom.js";

const NAV_LINKS = [
  { href: "/", label: "Início", page: "inicio" },
  { href: "/cursos.html", label: "Cursos", page: "cursos" },
  { href: "/#impacto", label: "Sobre", page: "sobre" },
];

const CONTACT_EMAIL = "contato@sesindustria.com.br";

function renderHeader(container, activePage) {
  const list = el(
    "ul",
    { className: "site-nav__list", attrs: { id: "menu-principal" } },
    NAV_LINKS.map((link) =>
      el("li", {}, [
        el("a", {
          className: "site-nav__link",
          text: link.label,
          attrs: { href: link.href, "aria-current": link.page === activePage ? "page" : null },
        }),
      ]),
    ),
  );

  const toggle = el("button", {
    className: "icon-button site-nav__toggle",
    attrs: { type: "button", "aria-label": "Menu", "aria-expanded": "false", "aria-controls": "menu-principal" },
  });

  toggle.addEventListener("click", () => {
    const isOpen = list.classList.toggle("is-open");
    toggle.setAttribute("aria-expanded", String(isOpen));
  });

  container.className = "site-header";
  container.replaceChildren(
    el("div", { className: "container site-header__inner" }, [
      el("a", { className: "site-header__brand", attrs: { href: "/" } }, [
        el("img", {
          attrs: {
            src: "/assets/icons/logo-sesi-industria-primary.svg",
            alt: "SESI + Indústria — página inicial",
            width: 188,
            height: 19,
          },
        }),
      ]),
      el("nav", { className: "site-nav", attrs: { "aria-label": "Principal" } }, [toggle, list]),
      el("button", {
        className: "button button--dark button--sm site-header__student",
        text: "Área do aluno",
        attrs: { type: "button" },
      }),
    ]),
  );
}

function footerColumn(title, items) {
  return el("div", {}, [
    el("h2", { className: "site-footer__title", text: title }),
    el("ul", { className: "site-footer__list" }, items),
  ]);
}

function linkItem(href, label) {
  return el("li", {}, [el("a", { text: label, attrs: { href } })]);
}

async function renderFooter(container) {
  const popularList = el("ul", { className: "site-footer__list" }, [el("li", { text: "Carregando…" })]);

  container.className = "site-footer";
  container.replaceChildren(
    el("div", { className: "container" }, [
      el("div", { className: "site-footer__grid" }, [
        el("div", { className: "site-footer__brand" }, [
          el("img", {
            attrs: { src: "/assets/icons/logo-sesi-industria-white.svg", alt: "SESI + Indústria", width: 188, height: 19 },
          }),
          el("p", {}, [el("a", { text: CONTACT_EMAIL, attrs: { href: `mailto:${CONTACT_EMAIL}` } })]),
          el("p", { text: "Av. Paulista, 1313" }),
        ]),
        footerColumn("Navegação", NAV_LINKS.map((link) => linkItem(link.href, link.label))),
        el("div", {}, [el("h2", { className: "site-footer__title", text: "Cursos populares" }), popularList]),
        footerColumn("Suporte", [linkItem(`mailto:${CONTACT_EMAIL}`, "Fale conosco")]),
      ]),
      el("p", {
        className: "site-footer__legal",
        text: "© 2026 SESI + Indústria. Catálogo demonstrativo: todos os cursos e dados apresentados são fictícios.",
      }),
    ]),
  );

  try {
    const { items } = await getCourses();
    popularList.replaceChildren(...items.slice(0, 5).map((course) => linkItem(`/curso.html?id=${course.id}`, course.fullName)));
  } catch {
    popularList.replaceChildren(linkItem("/cursos.html", "Ver catálogo de cursos"));
  }
}

let toastTimer;

function ensureToast() {
  let toast = document.getElementById("aviso");
  if (!toast) {
    toast = el("div", { className: "toast", attrs: { id: "aviso", role: "status", "aria-live": "polite" } });
    document.body.append(toast);
  }
  return toast;
}

export function showNotice(message) {
  const toast = ensureToast();
  toast.textContent = message;
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => {
    toast.textContent = "";
  }, 6000);
}

export function initLayout() {
  const header = document.querySelector("[data-site-header]");
  const footer = document.querySelector("[data-site-footer]");
  if (header) renderHeader(header, document.body.dataset.page);
  if (footer) renderFooter(footer);

  // Live regions must exist before their content changes, or screen readers may skip the first announcement.
  ensureToast();

  for (const trigger of document.querySelectorAll("[data-notice]")) {
    trigger.addEventListener("click", () => showNotice(trigger.dataset.notice));
  }
}
