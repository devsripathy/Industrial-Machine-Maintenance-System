import type { AuthResponse, MachineDto } from '../types';

export const API_BASE_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5050';

async function request<T>(
  endpoint: string,
  options: RequestInit = {},
  token?: string,
): Promise<T> {
  const response = await fetch(`${API_BASE_URL}${endpoint}`, {
    ...options,
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers ?? {}),
    },
  });

  if (!response.ok) {
    const contentType = response.headers.get('content-type') ?? '';
    let errorMessage = response.statusText;

    if (contentType.includes('application/json')) {
      const payload = (await response.json()) as { message?: string; title?: string; errors?: Record<string, string[]> };
      errorMessage = payload.message ?? payload.title ?? Object.values(payload.errors ?? {}).flat().join(', ') ?? errorMessage;
    } else {
      errorMessage = await response.text();
    }

    throw new Error(errorMessage || 'Request failed.');
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}

export async function loginUser(username: string, password: string): Promise<AuthResponse> {
  return request<AuthResponse>('/api/auth/login', {
    method: 'POST',
    body: JSON.stringify({ username, password }),
  });
}

export async function fetchMachines(token: string): Promise<MachineDto[]> {
  const payload = await request<{ value?: MachineDto[]; Count?: number }>('/api/machines', { method: 'GET' }, token);
  return payload.value ?? [];
}

export async function fetchMachineById(token: string, id: number): Promise<MachineDto> {
  return request<MachineDto>(`/api/machines/${id}`, { method: 'GET' }, token);
}
