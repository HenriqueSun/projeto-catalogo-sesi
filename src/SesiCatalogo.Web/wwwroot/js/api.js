const REQUEST_TIMEOUT_MS = 10000;

export class ApiError extends Error {
  constructor(status) {
    super(`A API respondeu com o status ${status}.`);
    this.name = "ApiError";
    this.status = status;
  }
}

async function getJson(path, signal) {
  const signals = [AbortSignal.timeout(REQUEST_TIMEOUT_MS)];
  if (signal) signals.push(signal);

  const response = await fetch(path, {
    headers: { Accept: "application/json" },
    signal: AbortSignal.any(signals),
  });

  if (!response.ok) throw new ApiError(response.status);
  return response.json();
}

export function getCourses(filters = {}, signal) {
  const params = new URLSearchParams();
  for (const [key, value] of Object.entries(filters)) {
    if (value) params.set(key, value);
  }
  const query = params.toString();
  return getJson(`/api/courses${query ? `?${query}` : ""}`, signal);
}

export function getCourse(id, signal) {
  return getJson(`/api/courses/${encodeURIComponent(id)}`, signal);
}

export async function getFilterOptions(signal) {
  const [categories, modalities, statuses] = await Promise.all([
    getJson("/api/categories", signal),
    getJson("/api/modalities", signal),
    getJson("/api/statuses", signal),
  ]);
  return { categories, modalities, statuses };
}
