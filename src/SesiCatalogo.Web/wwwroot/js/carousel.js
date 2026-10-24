import { prefersReducedMotion } from "./dom.js";

export function setupCarousel(root) {
  const track = root.querySelector("[data-carousel-track]");
  const previous = root.querySelector("[data-carousel-prev]");
  const next = root.querySelector("[data-carousel-next]");

  const step = () => {
    const item = track.firstElementChild;
    if (!item) return track.clientWidth;
    const gap = parseFloat(getComputedStyle(track).columnGap) || 0;
    return item.getBoundingClientRect().width + gap;
  };

  const scroll = (direction) =>
    track.scrollBy({ left: direction * step(), behavior: prefersReducedMotion() ? "auto" : "smooth" });

  const update = () => {
    previous.disabled = track.scrollLeft <= 1;
    next.disabled = track.scrollLeft + track.clientWidth >= track.scrollWidth - 1;
  };

  previous.addEventListener("click", () => scroll(-1));
  next.addEventListener("click", () => scroll(1));
  track.addEventListener("scroll", update, { passive: true });
  window.addEventListener("resize", update);
  update();

  return update;
}
