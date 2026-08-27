import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

export default function Indicadores() {
  const [dados, setDados] = useState(null)
  const [erro, setErro] = useState('')

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    try {
      const resultado = await apiRequest('/api/Indicador')
      setDados(resultado)
    } catch (error) {
      setErro(error.message)
    }
  }

  if (erro) {
    return <p style={{ color: 'red' }}>{erro}</p>
  }

  if (!dados) {
    return <p>Carregando indicadores...</p>
  }

  return (
    <div>
      <h1>Indicadores</h1>

      <div style={styles.cards}>
        <Card
          titulo="Total de registros"
          valor={dados.totalRegistros}
        />

        <Card
          titulo="Atendidos"
          valor={dados.atendidos}
        />

        <Card
          titulo="Não comparecimentos"
          valor={dados.naoComparecimentos}
        />

        <Card
          titulo="Ainda na fila"
          valor={dados.aindaNaFila}
        />

        <Card
          titulo="Tempo médio"
          valor={`${dados.tempoMedioEsperaMinutos} min`}
        />

        <Card
          titulo="Dentro do tempo"
          valor={dados.dentroDoTempo}
        />

        <Card
          titulo="Fora do tempo"
          valor={dados.foraDoTempo}
        />

        <Card
          titulo="% dentro do tempo"
          valor={`${dados.percentualDentroDoTempo}%`}
        />
      </div>

      <h2>Por classificação</h2>

      <table style={styles.tabela}>
        <thead>
          <tr>
            <th>Classificação</th>
            <th>Quantidade</th>
            <th>Tempo máximo</th>
            <th>Tempo médio</th>
            <th>Dentro</th>
            <th>Fora</th>
          </tr>
        </thead>

        <tbody>
          {dados.porClassificacao?.map((item) => (
            <tr key={item.classificacao}>
              <td>{item.classificacao}</td>
              <td>{item.quantidade}</td>
              <td>
                {item.tempoMaximoEsperaMinutos} min
              </td>
              <td>
                {item.tempoMedioEsperaMinutos} min
              </td>
              <td>{item.dentroDoTempo}</td>
              <td>{item.foraDoTempo}</td>
            </tr>
          ))}
        </tbody>
      </table>
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
  },

  card: {
    background: '#fff',
    padding: 20,
    borderRadius: 10,
    boxShadow: '0 2px 10px rgba(0,0,0,.08)',
  },

  tabela: {
    width: '100%',
    marginTop: 25,
    background: '#fff',
    borderCollapse: 'collapse',
  },
}