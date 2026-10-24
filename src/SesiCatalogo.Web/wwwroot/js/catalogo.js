import { getCourses, getFilterOptions } from "./api.js";
import { courseCard, stateMessage } from "./components.js";
import { el, pluralizeCourses } from "./dom.js";
import { initLayout } from "./layout.js";

const SEARCH_DEBOUNCE_MS = 300;
const SKELETON_COUNT = 4;
const FILTER_KEYS = ["search", "categoryId", "modalityId", "statusId"];

const form = document.querySelector("[data-filters]");
const countOutput = document.querySelector("[data-results-count]");
const stateRegion = document.querySelector("[data-results-state]");
const resultsList = document.querySelector("[data-results]");
const clearButton = document.querySelector("[data-clear-filters]");

let activeRequest;
let debounceTimer;

function readFilters() {
  const data = new FormData(form);
  return Object.fromEntries(FILTER_KEYS.map((key) => [key, String(data.get(key) ?? "").trim()]));
}

function hasActiveFilters(filters) {
  return FILTER_KEYS.some((key) => filters[key]);
}

function syncUrl(filters) {
  const params = new URLSearchParams();
  for (const key of FILTER_KEYS) {
    if (filters[key]) params.set(key, filters[key]);
  }
  const query = params.toString();
  history.replaceState(null, "", query ? `?${query}` : location.pathname);
}

function fillSelect(select, options) {
  select.append(...options.map((option) => el("option", { text: option.name, attrs: { value: String(option.id) } })));
}

function retryButton() {
  const button = el("button", { className: "button button--dark", text: "Tentar novamente", attrs: { type: "button" } });
  button.addEventListener("click", () => init());
  return button;
}

function showLoading() {
  resultsList.setAttribute("aria-busy", "true");
  countOutput.textContent = "Carregando cursos…";
  stateRegion.replaceChildren();
  resultsList.replaceChildren(
    ...Array.from({ length: SKELETON_COUNT }, () => el("li", { className: "skeleton-card", attrs: { "aria-hidden": "true" } })),
  );
}

function showError() {
  resultsList.removeAttribute("aria-busy");
  resultsList.replaceChildren();
  countOutput.textContent = "Erro ao carregar os cursos.";
  stateRegion.replaceChildren(
    stateMessage({
      title: "Não foi possível carregar os cursos",
      text: "Ocorreu um problema na comunicação com o servidor. Verifique sua conexão e tente novamente.",
      variant: "error",
      action: retryButton(),
    }),
  );
}

function showResults({ total, items }) {
  resultsList.removeAttribute("aria-busy");
  countOutput.textContent = pluralizeCourses(total);

  if (total === 0) {
    const clear = el("button", { className: "button button--dark", text: "Limpar filtros", attrs: { type: "button" } });
    clear.addEventListener("click", () => form.reset());
    resultsList.replaceChildren();
    stateRegion.replaceChildren(
      stateMessage({
        title: "Nenhum curso encontrado",
        text: "Não encontramos cursos com os critérios informados. Tente outro termo de pesquisa ou remova alguns filtros.",
        action: clear,
      }),
    );
    return;
  }

  stateRegion.replaceChildren();
  resultsList.replaceChildren(...items.map((course) => el("li", {}, [courseCard(course, "h2")])));
}

async function search() {
  clearTimeout(debounceTimer);
  activeRequest?.abort();
  const controller = new AbortController();
  activeRequest = controller;

  const filters = readFilters();
  syncUrl(filters);
  clearButton.disabled = !hasActiveFilters(filters);
  showLoading();

  try {
    const result = await getCourses(filters, controller.signal);
    if (activeRequest === controller) showResults(result);
  } catch (error) {
    if (controller.signal.aborted) return;
    console.error(error);
    showError();
  }
}

function applyUrlFilters() {
  const params = new URLSearchParams(location.search);
  for (const key of FILTER_KEYS) {
    const field = form.elements.namedItem(key);
    const value = params.get(key);
    if (!field || !value) continue;
    if (field instanceof HTMLSelectElement && ![...field.options].some((option) => option.value === value)) continue;
    field.value = value.slice(0, 100);
  }
}

let filtersLoaded = false;

async function init() {
  if (!filtersLoaded) {
    showLoading();
    try {
      const options = await getFilterOptions();
      fillSelect(form.elements.namedItem("categoryId"), options.categories);
      fillSelect(form.elements.namedItem("modalityId"), options.modalities);
      fillSelect(form.elements.namedItem("statusId"), options.statuses);
      filtersLoaded = true;
      applyUrlFilters();
    } catch (error) {
      console.error(error);
      showError();
      return;
    }
  }
  await search();
}

form.addEventListener("submit", (event) => {
  event.preventDefault();
  search();
});

form.addEventListener("input", (event) => {
  if (event.target.name !== "search") return;
  clearTimeout(debounceTimer);
  debounceTimer = setTimeout(search, SEARCH_DEBOUNCE_MS);
});

form.addEventListener("change", (event) => {
  if (event.target instanceof HTMLSelectElement) search();
});

form.addEventListener("reset", () => {
  // The reset event fires before the fields are cleared.
  setTimeout(() => {
    search();
    form.elements.namedItem("search").focus();
  });
});

initLayout();
init();
