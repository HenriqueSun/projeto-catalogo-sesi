// Builds elements with textContent only, so API data is never parsed as HTML.
export function el(tag, options = {}, children = []) {
  const element = document.createElement(tag);
  const { className, text, attrs } = options;

  if (className) element.className = className;
  if (text !== undefined) element.textContent = text;
  if (attrs) {
    for (const [name, value] of Object.entries(attrs)) {
      if (value !== undefined && value !== null && value !== false) element.setAttribute(name, value);
    }
  }
  for (const child of children) {
    if (child) element.append(child);
  }
  return element;
}

export function formatDate(isoDate) {
  if (!isoDate) return null;
  const [year, month, day] = isoDate.split("-");
  return `${day}/${month}/${year}`;
}

export function formatWorkload(hours) {
  return `${hours} ${hours === 1 ? "hora" : "horas"}`;
}

export function pluralizeCourses(total) {
  return total === 1 ? "1 curso encontrado" : `${total} cursos encontrados`;
}

export function prefersReducedMotion() {
  return window.matchMedia("(prefers-reduced-motion: reduce)").matches;
}
