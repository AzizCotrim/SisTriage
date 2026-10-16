import { useState } from 'react'

import Login from './pages/Login'
import Dashboard from './pages/Dashboard'
import Pacientes from './pages/Pacientes'
import CadastroPaciente from './pages/CadastroPaciente'
import Triagem from './pages/Triagem'
import FilaAtendimento from './pages/FilaAtendimento'
import Indicadores from './pages/Indicadores'
import Usuarios from './pages/Usuarios'

import Layout from './components/Layout'
import { getToken } from './services/api'

export default function App() {
  const [autenticado, setAutenticado] = useState(
    Boolean(getToken())
  )

  const [pagina, setPagina] = useState('dashboard')

  const [cadastroPaciente, setCadastroPaciente] =
    useState(false)

  if (!autenticado) {
    return (
      <Login
        onLogin={() => setAutenticado(true)}
      />
    )
  }

  function renderizarPagina() {
    if (cadastroPaciente) {
      return (
        <CadastroPaciente
          voltar={() => setCadastroPaciente(false)}
        />
      )
    }

    switch (pagina) {
      case 'dashboard':
        return <Dashboard />

      case 'pacientes':
        return (
          <Pacientes
            abrirCadastro={() =>
              setCadastroPaciente(true)
            }
          />
        )

      case 'triagem':
        return <Triagem />

      case 'fila':
        return <FilaAtendimento />

      case 'indicadores':
        return <Indicadores />

      case 'usuarios':
        return <Usuarios />

      default:
        return <Dashboard />
    }
  }

  return (
    <Layout
      pagina={pagina}
      setPagina={(novaPagina) => {
        setCadastroPaciente(false)
        setPagina(novaPagina)
      }}
    >
      {renderizarPagina()}
    </Layout>
  )
}