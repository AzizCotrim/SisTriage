import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

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

        <button onClick={chamarProximo}>
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
            <th>Paciente</th>
            <th>Classificação</th>
            <th>Status</th>
            <th>Espera</th>
            <th>Ações</th>
          </tr>
        </thead>

        <tbody>
          {fila.map((item) => (
            <tr key={item.id}>
              <td>{item.pacienteNome}</td>

              <td>{item.classificacao}</td>

              <td>{item.status}</td>

              <td>
                {item.tempoEsperaMinutos}
              </td>

              <td>
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

      <button onClick={carregarNaoAtendidos}>
        Ver não atendidos
      </button>

      {naoAtendidos.length > 0 && (
        <>
          <h2>Não compareceram</h2>

          {naoAtendidos.map((item) => (
            <p key={item.id}>
              {item.pacienteNome} - {item.classificacao}
            </p>
          ))}
        </>
      )}
    </div>
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
}