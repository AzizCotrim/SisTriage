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

        <button
        onClick={abrirCadastro}
        style={styles.botaoNovoPaciente}>
        + Novo paciente
        </button>
      </div>

      {erro && <p style={{ color: 'red' }}>{erro}</p>}

      <table style={styles.tabela}>
        <thead>
          <tr>
            <th style={styles.th}>Nome</th>
            <th style={styles.th}>CPF</th>
            <th style={styles.th}>Sexo</th>
          </tr>
        </thead>

        <tbody>
          {pacientes.map((paciente) => (
            <tr key={paciente.id}>
              <td style={styles.td}>{paciente.nome}</td>
              <td style={styles.td}>{paciente.cpf}</td>
              <td style={styles.td}>{paciente.sexo}</td>
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
  th: { textAlign: 'left', 
    padding: '12px', 
    borderBottom: '1px solid #ddd', 
    fontWeight: 'bold', 
  },
  td: { textAlign: 'left', 
    padding: '12px', 
    borderBottom: '1px solid #eee', 
  },
  botaoNovoPaciente: {
  background: '#245b87',
  color: '#fff',
  border: 'none',
  padding: '12px 20px',
  borderRadius: 8,
  fontSize: 15,
  fontWeight: 'bold',
  cursor: 'pointer',
  boxShadow: '0 3px 8px rgba(0, 0, 0, 0.15)',
  transition: 'all 0.2s ease',
},
}