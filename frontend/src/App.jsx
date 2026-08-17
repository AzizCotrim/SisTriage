import { useEffect, useState } from 'react'

/**
 * Ponto de partida. A chamada abaixo existe só para confirmar que o
 * frontend enxerga a API — pode ser removida assim que a primeira tela
 * de verdade for escrita.
 */
export default function App() {
  const [estado, setEstado] = useState('verificando')

  useEffect(() => {
    // Endpoint protegido: sem token, a API responde 401.
    // Qualquer resposta HTTP já prova que a conexão funciona.
    fetch('/api/Usuario/me')
      .then(() => setEstado('conectado'))
      .catch(() => setEstado('sem conexao'))
  }, [])

  return (
    <main style={{ fontFamily: 'system-ui, sans-serif', padding: 40 }}>
      <h1>SisTriage</h1>
      <p>Triagem e gestão de filas para Unidades Básicas de Saúde.</p>

      <p>
        API:{' '}
        <strong style={{ color: estado === 'conectado' ? '#1e8e5a' : '#c0392b' }}>
          {estado}
        </strong>
      </p>

      <p>Comece a escrever as telas em <code>src/</code>.</p>
    </main>
  )
}
