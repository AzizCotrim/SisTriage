import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

const coresClassificacao = {
  Vermelho: '#e53935',
  Laranja: '#fb8c00',
  Amarelo: '#fdd835',
  Verde: '#43a047',
  Azul: '#1e88e5',
}

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
          titulo="Total: "
          valor={indicadores?.totalRegistros ?? 0}
        />

        <Card
          titulo="Atendidos: "
          valor={indicadores?.atendidos ?? 0}
        />

        <Card
          titulo="Na fila: "
          valor={indicadores?.aindaNaFila ?? fila.length}
        />

        <Card
          titulo="Não compareceram: "
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
              <th style={styles.th}>Paciente</th>
              <th style={styles.th}>Classificação</th>
              <th style={styles.th}>Status</th>
              <th style={styles.th}>Tempo de espera</th>
            </tr>
          </thead>

          <tbody>
            {fila.map((item) => (
              <tr key={item.id}>
                <td style={styles.td}>{item.pacienteNome}</td>
                <td style={styles.td}>
                  <span style={styles.linhaClassificacao}>
                    <Bolinha cor={coresClassificacao[item.classificacao]} />
                    {item.classificacao}
                  </span>
                </td>
                <td style={styles.td}>{item.status}</td>
                <td style={styles.td}>
                  {item.tempoEsperaMinutos} minutos
                </td>
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

function Bolinha({ cor }) {
  return (
    <span
      style={{
        display: 'inline-block',
        width: 10,
        height: 10,
        borderRadius: '50%',
        backgroundColor: cor || '#bbb',
        marginRight: 8,
        flexShrink: 0,
      }}
    />
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
    borderRadius: 10,
    overflow: 'hidden',
  },

  th: {
    textAlign: 'left',
    padding: '12px',
    borderBottom: '1px solid #ddd',
    fontWeight: 'bold',
  },

  td: {
    textAlign: 'left',
    padding: '12px',
    borderBottom: '1px solid #eee',
  },

  linhaClassificacao: {
    display: 'inline-flex',
    alignItems: 'center',
  },
}
