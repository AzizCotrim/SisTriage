import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

const coresClassificacao = {
  Vermelho: '#e53935',
  Laranja: '#fb8c00',
  Amarelo: '#fdd835',
  Verde: '#43a047',
  Azul: '#1e88e5',
}

export default function FilaAtendimento() {
  const [fila, setFila] = useState([])
  const [naoAtendidos, setNaoAtendidos] = useState([])
  const [erro, setErro] = useState('')
  const [mensagem, setMensagem] = useState('')

  useEffect(() => {
    carregar()
  }, [])

  async function carregar() {
    try {
      const dados = await apiRequest('/api/FilaAtendimento')

      setFila(dados || [])
    } catch (error) {
      setErro(error.message)
    }
  }

  async function chamarProximo() {
    try {
      const dados = await apiRequest(
        '/api/FilaAtendimento/chamar-proximo',
        {
          method: 'POST',
        }
      )

      setMensagem(`Paciente chamado: ${dados.pacienteNome}`)

      carregar()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function finalizar(id) {
    try {
      await apiRequest(
        `/api/FilaAtendimento/${id}/finalizar`,
        {
          method: 'POST',
        }
      )

      carregar()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function naoCompareceu(id) {
    try {
      await apiRequest(
        `/api/FilaAtendimento/${id}/nao-compareceu`,
        {
          method: 'POST',
        }
      )

      carregar()
    } catch (error) {
      setErro(error.message)
    }
  }

  async function carregarNaoAtendidos() {
    try {
      const dados = await apiRequest(
        '/api/FilaAtendimento/nao-atendidos?Pagina=1&TamanhoPagina=100'
      )

      setNaoAtendidos(dados?.itens || [])
    } catch (error) {
      setErro(error.message)
    }
  }

  return (
    <div>
      <div style={styles.header}>
        <h1>Fila de Atendimento</h1>

        <button
        onClick={chamarProximo}
        style={styles.botao}>
        Chamar próximo
        </button>
      </div>

      {mensagem && (
        <p style={{ color: 'green' }}>{mensagem}</p>
      )}

      {erro && (
        <p style={{ color: 'red' }}>{erro}</p>
      )}

      <table style={styles.tabela}>
        <thead>
          <tr>
            <th style={styles.th}>Paciente</th> 
            <th style={styles.th}>Classificação</th> 
            <th style={styles.th}>Status</th> 
            <th style={styles.th}>Espera</th> 
            <th style={styles.th}>Ações</th>
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
                {item.tempoEsperaMinutos}
              </td>

              <td style={styles.td}>
                {item.status === 'Chamado' && (
                  <>
                    <button
                      onClick={() =>
                        finalizar(item.id)
                      }
                    >
                      Finalizar
                    </button>

                    <button
                      onClick={() =>
                        naoCompareceu(item.id)
                      }
                    >
                      Não compareceu
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <hr />

      <button
        onClick={carregarNaoAtendidos}
        style={styles.botao}>
        Ver não atendidos
        </button>

      {naoAtendidos.length > 0 && (
        <>
          <h2>Não compareceram</h2>

          {naoAtendidos.map((item) => (
            <p key={item.id} style={styles.linhaClassificacao}>
              <Bolinha cor={coresClassificacao[item.classificacao]} />
              {item.pacienteNome} - {item.classificacao}
            </p>
          ))}
        </>
      )}
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
  header: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
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

  botao: {
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

  linhaClassificacao: {
    display: 'inline-flex',
    alignItems: 'center',
  },
  
}