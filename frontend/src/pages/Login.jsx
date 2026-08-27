import { useState } from 'react'
import { apiRequest, setToken } from '../services/api'

export default function Login({ onLogin }) {
  const [email, setEmail] = useState('')
  const [senha, setSenha] = useState('')
  const [erro, setErro] = useState('')
  const [carregando, setCarregando] = useState(false)

  async function fazerLogin(event) {
    event.preventDefault()

    setErro('')
    setCarregando(true)

    try {
      const dados = await apiRequest('/api/Usuario/login', {
        method: 'POST',
        body: JSON.stringify({
          email,
          senha,
        }),
      })

      if (!dados?.token) {
        throw new Error('A API não retornou o token.')
      }

      setToken(dados.token)

      onLogin()
    } catch (error) {
      setErro(error.message || 'E-mail ou senha inválidos.')
    } finally {
      setCarregando(false)
    }
  }

  return (
    <main style={estilos.container}>
      <div style={estilos.card}>
        <div style={estilos.logo}>S</div>

        <h1 style={estilos.titulo}>SisTriage</h1>
        <p style={estilos.subtitulo}>Sistema de Triagem e Gestão de Filas</p>

        <form onSubmit={fazerLogin}>
          <label style={estilos.label}>E-mail</label>
          <input
            style={estilos.input}
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Digite seu e-mail"
            required
          />

          <label style={estilos.label}>Senha</label>
          <input
            style={estilos.input}
            type="password"
            value={senha}
            onChange={(e) => setSenha(e.target.value)}
            placeholder="Digite sua senha"
            required
          />

          <button style={estilos.botao} type="submit" disabled={carregando}>
            {carregando ? 'Entrando...' : 'Entrar'}
          </button>
        </form>

        {erro && <p style={estilos.erro}>{erro}</p>}

        <div style={estilos.dica}>
          <strong>Usuário inicial</strong>
          <br />
          recepcao@sistriage.local
          <br />
          Sistriage@123
        </div>
      </div>
    </main>
  )
}

const estilos = {
  container: {
    minHeight: '100vh',
    display: 'flex',
    justifyContent: 'center',
    alignItems: 'center',
    background: 'linear-gradient(135deg, #0E7C86, #0F1B2D)',
    fontFamily: 'Arial, sans-serif',
    padding: 20,
  },

  card: {
    width: '100%',
    maxWidth: 380,
    padding: 36,
    background: '#fff',
    borderRadius: 14,
    boxShadow: '0 10px 30px rgba(0,0,0,0.2)',
    textAlign: 'center',
  },

  logo: {
    width: 44,
    height: 44,
    margin: '0 auto 16px',
    borderRadius: 10,
    background: '#0E7C86',
    color: '#fff',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    fontWeight: 'bold',
    fontSize: 20,
  },

  titulo: {
    margin: '0 0 4px',
    color: '#16232E',
  },

  subtitulo: {
    color: '#6B7885',
    fontSize: 13,
    marginBottom: 24,
  },

  label: {
    display: 'block',
    textAlign: 'left',
    fontSize: 13,
    fontWeight: 600,
    color: '#33424F',
    marginBottom: 6,
  },

  input: {
    width: '100%',
    padding: '10px 12px',
    marginBottom: 16,
    borderRadius: 8,
    border: '1.5px solid #DCE3E8',
    fontSize: 14,
    boxSizing: 'border-box',
  },

  botao: {
    width: '100%',
    padding: 12,
    border: 'none',
    borderRadius: 8,
    background: '#0E7C86',
    color: '#fff',
    fontWeight: 600,
    fontSize: 14,
    cursor: 'pointer',
  },

  erro: {
    color: '#B3261E',
    marginTop: 14,
    fontSize: 13,
  },

  dica: {
    marginTop: 22,
    padding: 12,
    background: '#F2F6F5',
    borderRadius: 8,
    fontSize: 12,
    color: '#4B5A62',
    lineHeight: 1.6,
  },
}