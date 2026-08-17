import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  // process.env vem do Docker; loadEnv lê o arquivo .env local.
  const alvo = process.env.VITE_API_TARGET || env.VITE_API_TARGET || 'http://localhost:5285'

  return {
    plugins: [react()],
    server: {
      host: true,
      port: 5173,
      strictPort: true,
      proxy: {
        // O navegador fala só com o Vite (mesma origem), e o Vite repassa
        // para a API. Isso evita CORS e certificado autoassinado.
        '/api': {
          target: alvo,
          changeOrigin: true,
          secure: false
        }
      }
    }
  }
})
