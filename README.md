# SisTriage

Sistema informatizado de triagem e gestão de filas para Unidades Básicas de Saúde,
baseado no Protocolo de Manchester.

## Como rodar

Único pré-requisito: [Docker Desktop](https://www.docker.com/products/docker-desktop/).

```bash
docker compose up
```

Isso sobe três containers: o PostgreSQL, a API e o servidor de desenvolvimento do
frontend. As migrations do banco são aplicadas automaticamente e o primeiro usuário
é criado na primeira execução.

| Serviço | Endereço |
| --- | --- |
| Frontend | http://localhost:5173 |
| API | http://localhost:5285 |
| Documentação da API (Scalar) | http://localhost:5285/scalar |
| PostgreSQL | localhost:5432 |

Para parar: `Ctrl+C`, ou `docker compose down` em outro terminal.

## Usuário inicial

```
recepcao@sistriage.local
Sistriage@123
```

Apenas o perfil Recepcionista cadastra novos usuários, então é por ele que se começa.

## Desenvolvendo o frontend

O código fica em `frontend/src`. O container já roda o Vite com recarga automática:
salvar um arquivo atualiza o navegador, sem reiniciar nada.

As chamadas à API saem como caminhos relativos (`fetch('/api/Usuario/login')`).
O Vite encaminha para o backend, o que evita CORS e certificado autoassinado.

Ao instalar uma biblioteca nova, reinicie o container para que o `npm install` rode:

```bash
docker compose restart frontend
```

## Comandos úteis

```bash
docker compose up --build      # reconstrói a API após mudanças no backend
docker compose down -v         # remove os containers e apaga o banco
docker compose logs -f api     # acompanha os logs da API
```

## Estrutura

```
backend/     API em ASP.NET Core, em camadas (Domain, Application, Infrastructure, Repository, API)
frontend/    Aplicação React + Vite
```
