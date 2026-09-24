import { useEffect } from 'react'
import { removeToken } from '../services/api'

export default function Layout({ pagina, setPagina, children }) {
  useEffect(() => {
    document.body.style.margin = '0'

    return () => {
      document.body.style.margin = ''
    }
  }, [])
  function sair() {
    removeToken()
    window.location.reload()
  }

  const menus = [
    ['dashboard', 'Dashboard'],
    ['pacientes', 'Pacientes'],
    ['triagem', 'Nova Triagem'],
    ['fila', 'Fila de Atendimento'],
    ['indicadores', 'Indicadores'],
    ['usuarios', 'Usuários'],
  ]

  return (
    <div style={styles.container}>
      <aside style={styles.menu}>
        <h2>SisTriage</h2>

        {menus.map(([id, nome]) => (
          <button
            key={id}
            onClick={() => setPagina(id)}
            style={{
              ...styles.menuButton,
              ...(pagina === id ? styles.menuAtivo : {}),
            }}
          >
            {nome}
          </button>
        ))}

        <button onClick={sair} style={styles.sair}>
          Sair
        </button>
      </aside>

      <main style={styles.conteudo}>
        {children}
      </main>
    </div>
  )
}

const styles = {
  container: {
    display: 'flex',
    minHeight: '100vh',
    fontFamily: 'Arial, sans-serif',
    background: '#f5f7fa',
  },

  menu: {
    width: 230,
    background: '#17324d',
    color: '#fff',
    padding: 20,
    display: 'flex',
    flexDirection: 'column',
    gap: 8,
  },

  menuButton: {
    border: 0,
    padding: 12,
    textAlign: 'left',
    background: 'transparent',
    color: '#fff',
    cursor: 'pointer',
    borderRadius: 6,
  },

  menuAtivo: {
    background: '#245b87',
  },

  sair: {
    marginTop: 'auto',
    padding: 12,
    border: 0,
    borderRadius: 6,
    cursor: 'pointer',
    background: '#c62828',
    color: '#fff',
  },

  conteudo: {
    flex: 1,
    padding: 30,
    overflow: 'auto',
  },
}

