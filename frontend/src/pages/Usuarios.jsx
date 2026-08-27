import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

export default function Usuarios() {
  const [usuarios, setUsuarios] = useState([])
  const [erro, setErro] = useState('')
  const [mensagem, setMensagem] = useState('')

  const [form, setForm] = useState({
    nome: '',
    email: '',
    senha: '',
    perfil: 'Recepcionista',
  })

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    try {
      const dados = await apiRequest(
        '/api/Usuario?Pagina=1&TamanhoPagina=100'
      )

      setUsuarios(dados?.itens || [])
    } catch (error) {
      setErro(error.message)
    }
  }

  async function cadastrar(event) {
    event.preventDefault()

    setErro('')
    setMensagem('')

    try {
      await apiRequest('/api/Usuario', {
        method: 'POST',
        body: JSON.stringify(form),
      })

      setMensagem('Usuário cadastrado com sucesso!')

      setForm({
        nome: '',
        email: '',
        senha: '',
        perfil: 'Recepcionista',
      })

      carregar()
    } catch (error) {
      setErro(error.message)
    }
  }

  return (
    <div>
      <h1>Usuários</h1>

      <form onSubmit={cadastrar} style={styles.form}>
        <h2>Novo usuário</h2>

        <label>Nome</label>

        <input
          value={form.nome}
          onChange={(e) =>
            setForm({
              ...form,
              nome: e.target.value,
            })
          }
          required
        />

        <label>E-mail</label>

        <input
          type="email"
          value={form.email}
          onChange={(e) =>
            setForm({
              ...form,
              email: e.target.value,
            })
          }
          required
        />

        <label>Senha</label>

        <input
          type="password"
          minLength={6}
          value={form.senha}
          onChange={(e) =>
            setForm({
              ...form,
              senha: e.target.value,
            })
          }
          required
        />

        <label>Perfil</label>

        <select
          value={form.perfil}
          onChange={(e) =>
            setForm({
              ...form,
              perfil: e.target.value,
            })
          }
        >
          <option value="Recepcionista">
            Recepcionista
          </option>

          <option value="Enfermeiro">
            Enfermeiro
          </option>

          <option value="Medico">
            Médico
          </option>
        </select>

        <button type="submit">
          Cadastrar usuário
        </button>
      </form>

      {mensagem && (
        <p style={{ color: 'green' }}>{mensagem}</p>
      )}

      {erro && (
        <p style={{ color: 'red' }}>{erro}</p>
      )}

      <h2>Usuários cadastrados</h2>

      <table style={styles.tabela}>
        <thead>
          <tr>
            <th>Nome</th>
            <th>E-mail</th>
            <th>Perfil</th>
            <th>Ativo</th>
          </tr>
        </thead>

        <tbody>
          {usuarios.map((usuario) => (
            <tr key={usuario.id}>
              <td>{usuario.nome}</td>
              <td>{usuario.email}</td>
              <td>{usuario.perfil}</td>
              <td>
                {usuario.ativo ? 'Sim' : 'Não'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

const styles = {
  form: {
    maxWidth: 500,
    background: '#fff',
    padding: 25,
    borderRadius: 10,
    display: 'flex',
    flexDirection: 'column',
    gap: 10,
  },

  tabela: {
    width: '100%',
    background: '#fff',
    borderCollapse: 'collapse',
  },
}