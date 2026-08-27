import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

export default function Pacientes({ abrirCadastro }) {
  const [pacientes, setPacientes] = useState([])
  const [erro, setErro] = useState('')

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    try {
      const dados = await apiRequest(
        '/api/Paciente?Pagina=1&TamanhoPagina=100'
      )

      setPacientes(dados?.itens || [])
    } catch (error) {
      setErro(error.message)
    }
  }

  return (
    <div>
      <div style={styles.header}>
        <div>
          <h1>Pacientes</h1>
          <p>Cadastro e consulta de pacientes.</p>
        </div>

        <button onClick={abrirCadastro}>
          + Novo paciente
        </button>
      </div>

      {erro && <p style={{ color: 'red' }}>{erro}</p>}

      <table style={styles.tabela}>
        <thead>
          <tr>
            <th>Nome</th>
            <th>CPF</th>
            <th>Sexo</th>
          </tr>
        </thead>

        <tbody>
          {pacientes.map((paciente) => (
            <tr key={paciente.id}>
              <td>{paciente.nome}</td>
              <td>{paciente.cpf}</td>
              <td>{paciente.sexo}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}

const styles = {
  header: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 25,
  },

  tabela: {
    width: '100%',
    background: '#fff',
    borderCollapse: 'collapse',
  },
}