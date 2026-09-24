import { useEffect, useRef, useState } from 'react'
import { apiRequest } from '../services/api'

const classificacoes = [
  { valor: 'Vermelho', cor: '#e53935' },
  { valor: 'Laranja', cor: '#fb8c00' },
  { valor: 'Amarelo', cor: '#fdd835' },
  { valor: 'Verde', cor: '#43a047' },
  { valor: 'Azul', cor: '#1e88e5' },
]

export default function Triagem() {
  const [pacientes, setPacientes] = useState([])
  const [mensagem, setMensagem] = useState('')
  const [erro, setErro] = useState('')

  const [form, setForm] = useState({
    pacienteId: '',
    classificacao: 'Verde',
    queixaPrincipal: '',
    pressaoArterialSistolica: '',
    pressaoArterialDiastolica: '',
    frequenciaCardiaca: '',
    frequenciaRespiratoria: '',
    temperatura: '',
    spO2: '',
    escalaDor: '',
    glicemia: '',
    peso: '',
  })

  useEffect(() => {
    carregarPacientes()
  }, [])

  async function carregarPacientes() {
    try {
      const dados = await apiRequest(
        '/api/Paciente?Pagina=1&TamanhoPagina=100'
      )

      setPacientes(dados?.itens || [])
    } catch (error) {
      setErro(error.message)
    }
  }

  function alterar(campo, valor) {
    setForm({
      ...form,
      [campo]: valor,
    })
  }

  async function enviar(event) {
    event.preventDefault()

    setMensagem('')
    setErro('')

    try {
      const usuario = await apiRequest('/api/Usuario/me')

      await apiRequest('/api/Triagem', {
        method: 'POST',
        body: JSON.stringify({
          pacienteId: form.pacienteId,
          usuarioId: usuario.id,
          classificacao: form.classificacao,
          queixaPrincipal: form.queixaPrincipal,

          sinaisVitais: {
            pressaoArterialSistolica:
              Number(form.pressaoArterialSistolica),

            pressaoArterialDiastolica:
              Number(form.pressaoArterialDiastolica),

            frequenciaCardiaca:
              Number(form.frequenciaCardiaca),

            frequenciaRespiratoria:
              Number(form.frequenciaRespiratoria),

            temperatura: Number(form.temperatura),

            spO2: Number(form.spO2),

            escalaDor: Number(form.escalaDor),

            glicemia: Number(form.glicemia),

            peso: Number(form.peso),
          },
        }),
      })

      setMensagem('Triagem realizada com sucesso!')
    } catch (error) {
      setErro(error.message)
    }
  }

  return (
    <div>
      <h1>Nova Triagem</h1>

      <form onSubmit={enviar} style={styles.form}>
        <label>Paciente</label>

        <select
          value={form.pacienteId}
          onChange={(e) =>
            alterar('pacienteId', e.target.value)
          }
          required
        >
          <option value="">Selecione um paciente</option>

          {pacientes.map((paciente) => (
            <option key={paciente.id} value={paciente.id}>
              {paciente.nome} - {paciente.cpf}
            </option>
          ))}
        </select>

        <label>Classificação de risco</label>

        <SeletorClassificacao
          valor={form.classificacao}
          onChange={(v) => alterar('classificacao', v)}
        />

        <label>Queixa principal</label>

        <textarea
          value={form.queixaPrincipal}
          onChange={(e) =>
            alterar('queixaPrincipal', e.target.value)
          }
          required
        />

        <h3>Sinais vitais</h3>

        <Campo
          label="Pressão sistólica "
          value={form.pressaoArterialSistolica}
          onChange={(v) =>
            alterar('pressaoArterialSistolica', v)
          }
        />

        <Campo
          label="Pressão diastólica "
          value={form.pressaoArterialDiastolica}
          onChange={(v) =>
            alterar('pressaoArterialDiastolica', v)
          }
        />

        <Campo
          label="Frequência cardíaca "
          value={form.frequenciaCardiaca}
          onChange={(v) =>
            alterar('frequenciaCardiaca', v)
          }
        />

        <Campo
          label="Frequência respiratória "
          value={form.frequenciaRespiratoria}
          onChange={(v) =>
            alterar('frequenciaRespiratoria', v)
          }
        />

        <Campo
          label="Temperatura "
          value={form.temperatura}
          onChange={(v) => alterar('temperatura', v)}
        />

        <Campo
          label="SpO2 "
          value={form.spO2}
          onChange={(v) => alterar('spO2', v)}
        />

        <Campo
          label="Escala de dor "
          value={form.escalaDor}
          onChange={(v) => alterar('escalaDor', v)}
        />

        <Campo
          label="Glicemia "
          value={form.glicemia}
          onChange={(v) => alterar('glicemia', v)}
        />

        <Campo
          label="Peso "
          value={form.peso}
          onChange={(v) => alterar('peso', v)}
        />

        <button type="submit">
          Finalizar triagem
        </button>
      </form>

      {mensagem && (
        <p style={{ color: 'green' }}>{mensagem}</p>
      )}

      {erro && (
        <p style={{ color: 'red' }}>{erro}</p>
      )}
    </div>
  )
}

function SeletorClassificacao({ valor, onChange }) {
  const [aberto, setAberto] = useState(false)
  const containerRef = useRef(null)
  const atual = classificacoes.find((c) => c.valor === valor)

  useEffect(() => {
    function aoClicarFora(e) {
      if (
        containerRef.current &&
        !containerRef.current.contains(e.target)
      ) {
        setAberto(false)
      }
    }

    document.addEventListener('mousedown', aoClicarFora)
    return () =>
      document.removeEventListener('mousedown', aoClicarFora)
  }, [])

  return (
    <div ref={containerRef} style={styles.seletorContainer}>
      <button
        type="button"
        onClick={() => setAberto((v) => !v)}
        style={styles.seletorBotao}
      >
        <span style={styles.linha}>
          <Bolinha cor={atual?.cor} />
          {atual?.valor}
        </span>

        <span style={styles.seta}>{aberto ? '▲' : '▼'}</span>
      </button>

      {aberto && (
        <ul style={styles.listaOpcoes}>
          {classificacoes.map((item) => (
            <li key={item.valor}>
              <button
                type="button"
                onClick={() => {
                  onChange(item.valor)
                  setAberto(false)
                }}
                style={{
                  ...styles.opcaoBotao,
                  background:
                    item.valor === valor
                      ? '#f0f0f0'
                      : 'transparent',
                }}
              >
                <Bolinha cor={item.cor} />
                {item.valor}
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  )
}

function Bolinha({ cor }) {
  return (
    <span
      style={{
        display: 'inline-block',
        width: 12,
        height: 12,
        borderRadius: '50%',
        backgroundColor: cor,
        marginRight: 8,
        flexShrink: 0,
      }}
    />
  )
}

function Campo({ label, value, onChange }) {
  return (
    <div>
      <label>{label}</label>

      <input
        type="number"
        step="any"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        required
      />
    </div>
  )
}

const styles = {
  form: {
    maxWidth: 700,
    display: 'flex',
    flexDirection: 'column',
    gap: 10,
    background: '#fff',
    padding: 25,
    borderRadius: 10,
  },
  seletorContainer: {
    position: 'relative',
  },
  seletorBotao: {
    width: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    padding: '8px 12px',
    borderRadius: 6,
    border: '1px solid #ccc',
    background: '#fff',
    cursor: 'pointer',
  },
  linha: {
    display: 'flex',
    alignItems: 'center',
  },
  seta: {
    fontSize: 10,
    color: '#666',
  },
  listaOpcoes: {
    position: 'absolute',
    top: '100%',
    left: 0,
    right: 0,
    marginTop: 4,
    listStyle: 'none',
    padding: 4,
    background: '#fff',
    border: '1px solid #ccc',
    borderRadius: 6,
    boxShadow: '0 4px 10px rgba(0,0,0,0.1)',
    zIndex: 10,
  },
  opcaoBotao: {
    width: '100%',
    display: 'flex',
    alignItems: 'center',
    padding: '8px 10px',
    border: 'none',
    borderRadius: 4,
    cursor: 'pointer',
    textAlign: 'left',
  },
}