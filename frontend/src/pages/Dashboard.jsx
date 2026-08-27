import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

export default function Dashboard() {
  const [fila, setFila] = useState([])
  const [indicadores, setIndicadores] = useState(null)
  const [erro, setErro] = useState('')

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    try {
      const [filaData, indicadoresData] = await Promise.all([
        apiRequest('/api/FilaAtendimento'),
        apiRequest('/api/Indicador'),
      ])

      setFila(filaData || [])
      setIndicadores(indicadoresData)
    } catch (error) {
      setErro(error.message)
    }
  }

  return (
    <div>
      <h1>Dashboard</h1>

      {erro && <p style={{ color: 'red' }}>{erro}</p>}

      <div style={styles.cards}>
        <Card
          titulo="Total"
          valor={indicadores?.totalRegistros ?? 0}
        />

        <Card
          titulo="Atendidos"
          valor={indicadores?.atendidos ?? 0}
        />

        <Card
          titulo="Na fila"
          valor={indicadores?.aindaNaFila ?? fila.length}
        />

        <Card
          titulo="Não compareceram"
          valor={indicadores?.naoComparecimentos ?? 0}
        />
      </div>

      <h2>Fila atual</h2>

      {fila.length === 0 ? (
        <p>Nenhum paciente aguardando.</p>
      ) : (
        <table style={styles.tabela}>
          <thead>
            <tr>
              <th>Paciente</th>
              <th>Classificação</th>
              <th>Status</th>
              <th>Tempo de espera</th>
            </tr>
          </thead>

          <tbody>
            {fila.map((item) => (
              <tr key={item.id}>
                <td>{item.pacienteNome}</td>
                <td>{item.classificacao}</td>
                <td>{item.status}</td>
                <td>{item.tempoEsperaMinutos}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  )
}

function Card({ titulo, valor }) {
  return (
    <div style={styles.card}>
      <span>{titulo}</span>
      <strong>{valor}</strong>
    </div>
  )
}

const styles = {
  cards: {
    display: 'grid',
    gridTemplateColumns: 'repeat(4, 1fr)',
    gap: 20,
    margin: '25px 0',
  },

  card: {
    background: '#fff',
    padding: 25,
    borderRadius: 10,
    boxShadow: '0 2px 10px rgba(0,0,0,.08)',
  },

  tabela: {
    width: '100%',
    background: '#fff',
    borderCollapse: 'collapse',
  },
}