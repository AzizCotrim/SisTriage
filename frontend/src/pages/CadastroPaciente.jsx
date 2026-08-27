import { useState } from 'react'
import { apiRequest } from '../services/api'

export default function CadastroPaciente({ voltar }) {
  const [form, setForm] = useState({
    nome: '',
    cpf: '',
    dataNascimento: '',
    sexo: 'Masculino',
    telefone: '',
    alergias: '',
  })

  const [mensagem, setMensagem] = useState('')
  const [erro, setErro] = useState('')

  function alterar(campo, valor) {
    setForm({
      ...form,
      [campo]: valor,
    })
  }

  async function cadastrar(event) {
    event.preventDefault()

    setErro('')
    setMensagem('')

    try {
      await apiRequest('/api/Paciente', {
        method: 'POST',
        body: JSON.stringify({
          ...form,
          dataNascimento: new Date(form.dataNascimento).toISOString(),
        }),
      })

      setMensagem('Paciente cadastrado com sucesso!')

      setTimeout(voltar, 1000)
    } catch (error) {
      setErro(error.message)
    }
  }

  return (
    <div>
      <h1>Novo paciente</h1>

      <form onSubmit={cadastrar} style={styles.form}>
        <label>Nome</label>
        <input
          value={form.nome}
          onChange={(e) => alterar('nome', e.target.value)}
          required
        />

        <label>CPF</label>
        <input
          value={form.cpf}
          maxLength={11}
          onChange={(e) =>
            alterar('cpf', e.target.value.replace(/\D/g, ''))
          }
          required
        />

        <label>Data de nascimento</label>
        <input
          type="date"
          value={form.dataNascimento}
          onChange={(e) =>
            alterar('dataNascimento', e.target.value)
          }
          required
        />

        <label>Sexo</label>
        <select
          value={form.sexo}
          onChange={(e) => alterar('sexo', e.target.value)}
        >
          <option value="Masculino">Masculino</option>
          <option value="Feminino">Feminino</option>
        </select>

        <label>Telefone</label>
        <input
          value={form.telefone}
          onChange={(e) => alterar('telefone', e.target.value)}
        />

        <label>Alergias</label>
        <textarea
          value={form.alergias}
          onChange={(e) => alterar('alergias', e.target.value)}
        />

        <div>
          <button type="submit">Cadastrar</button>

          <button type="button" onClick={voltar}>
            Voltar
          </button>
        </div>
      </form>

      {mensagem && <p style={{ color: 'green' }}>{mensagem}</p>}
      {erro && <p style={{ color: 'red' }}>{erro}</p>}
    </div>
  )
}

const styles = {
  form: {
    maxWidth: 600,
    display: 'flex',
    flexDirection: 'column',
    gap: 10,
    background: '#fff',
    padding: 25,
    borderRadius: 10,
  },
}