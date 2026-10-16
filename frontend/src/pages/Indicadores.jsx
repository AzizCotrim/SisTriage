import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

export default function Indicadores() {
  const [dados, setDados] = useState(null)
  const [erro, setErro] = useState('')

  const coresClassificacao = {
  Vermelho: '#e53935',
  Laranja: '#fb8c00',
  Amarelo: '#fdd835',
  Verde: '#43a047',
  Azul: '#1e88e5',
}


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
          titulo="Total de registros: "
          valor={dados.totalRegistros}
        />

        <Card
          titulo="Atendidos: "
          valor={dados.atendidos}
        />

        <Card
          titulo="Não comparecimentos: "
          valor={dados.naoComparecimentos}
        />

        <Card
          titulo="Ainda na fila: "
          valor={dados.aindaNaFila}
        />

        <Card
          titulo="Tempo médio: "
          valor={`${dados.tempoMedioEsperaMinutos} min`}
        />

        <Card
          titulo="Dentro do tempo: "
          valor={dados.dentroDoTempo}
        />

        <Card
          titulo="Fora do tempo: "
          valor={dados.foraDoTempo}
        />

        <Card
          titulo="% dentro do tempo: "
          valor={`${dados.percentualDentroDoTempo}%`}
        />
      </div>

      <h2>Por classificação</h2>

      <table style={styles.tabela}>
        <thead>
          <tr>
            <th style={styles.th}>Classificação</th>
            <th style={styles.th}>Quantidade</th>
            <th style={styles.th}>Tempo máximo</th>
            <th style={styles.th}>Tempo médio</th>
            <th style={styles.th}>Dentro</th>
            <th style={styles.th}>Fora</th>
          </tr>
        </thead>

        <tbody>
          {dados.porClassificacao?.map((item) => (
            <tr key={item.classificacao}>
              <td style={styles.td}>
  <span style={styles.linhaClassificacao}>
    <Bolinha cor={coresClassificacao[item.classificacao]} />
    {item.classificacao}
  </span>
</td>
              <td style={styles.td}>{item.quantidade}</td>
              <td style={styles.td}>
                {item.tempoMaximoEsperaMinutos} min
              </td>
              <td style={styles.td}>
                {item.tempoMedioEsperaMinutos} min
              </td>
              <td style={styles.td}>{item.dentroDoTempo}</td>
              <td style={styles.td}>{item.foraDoTempo}</td>
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