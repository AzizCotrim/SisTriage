import { useEffect, useState } from 'react'
import { apiRequest } from '../services/api'

const classificacoes = [
  'Vermelho',
  'Laranja',
  'Amarelo',
  'Verde',
  'Azul',
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

        <select
          value={form.classificacao}
          onChange={(e) =>
            alterar('classificacao', e.target.value)
          }
        >
          {classificacoes.map((item) => (
            <option key={item} value={item}>
              {item}
            </option>
          ))}
        </select>

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
          label="Pressão sistólica"
          value={form.pressaoArterialSistolica}
          onChange={(v) =>
            alterar('pressaoArterialSistolica', v)
          }
        />

        <Campo
          label="Pressão diastólica"
          value={form.pressaoArterialDiastolica}
          onChange={(v) =>
            alterar('pressaoArterialDiastolica', v)
          }
        />

        <Campo
          label="Frequência cardíaca"
          value={form.frequenciaCardiaca}
          onChange={(v) =>
            alterar('frequenciaCardiaca', v)
          }
        />

        <Campo
          label="Frequência respiratória"
          value={form.frequenciaRespiratoria}
          onChange={(v) =>
            alterar('frequenciaRespiratoria', v)
          }
        />

        <Campo
          label="Temperatura"
          value={form.temperatura}
          onChange={(v) => alterar('temperatura', v)}
        />

        <Campo
          label="SpO2"
          value={form.spO2}
          onChange={(v) => alterar('spO2', v)}
        />

        <Campo
          label="Escala de dor"
          value={form.escalaDor}
          onChange={(v) => alterar('escalaDor', v)}
        />

        <Campo
          label="Glicemia"
          value={form.glicemia}
          onChange={(v) => alterar('glicemia', v)}
        />

        <Campo
          label="Peso"
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
}