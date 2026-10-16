const TOKEN_KEY = 'sistriage_token'

export function getToken() {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token) {
  localStorage.setItem(TOKEN_KEY, token)
}

export function removeToken() {
  localStorage.removeItem(TOKEN_KEY)
}

export async function apiRequest(url, options = {}) {
  const token = getToken()

  const headers = {
    ...(options.body ? { 'Content-Type': 'application/json' } : {}),
    ...(options.headers || {}),
  }

  if (token) {
    headers.Authorization = `Bearer ${token}`
  }

  const resposta = await fetch(url, {
    ...options,
    headers,
  })

  if (resposta.status === 401) {
    removeToken()
  }

  const texto = await resposta.text()

  let dados = null

  try {
    dados = texto ? JSON.parse(texto) : null
  } catch {
    dados = texto
  }

  if (!resposta.ok) {
    throw new Error(
      dados?.mensagem ||
      dados?.message ||
      dados?.detail ||
      `Erro HTTP ${resposta.status}`
    )
  }

  return dados
}