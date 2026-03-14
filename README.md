# Multi Gateway Payment API

API REST para gerenciamento de pagamentos utilizando múltiplos gateways.

Ao realizar uma compra, o sistema processa o pagamento utilizando os gateways ativos em ordem de prioridade. Caso o primeiro gateway falhe, o sistema tenta o próximo gateway disponível.

Quando um gateway retorna sucesso, a transação é registrada no banco de dados e o processo é finalizado.
O sistema também permite consultar clientes, transações e realizar reembolsos

## Requisitos
- .NET SDK
- MySQL
- Docker
- Git

## Ferramentas recomendadas

Visual Studio ou VS Code
Postman ou Insomnia (opcional)
Swagger (já incluído no projeto)

## Tecnologias
- .NET 8 SDK
- Docker
- Entity Framework Core
-  MySQL 8+
- Docker
- Swagger

## Requisitos
- .NET SDK
- MySQL
- Docker

# Como Instalar
## 1 - Clonar o repositório:
```bash
git clone <https://github.com/Lcpena98/PaymentGateway>
```
Entrar na pasta do projeto:
```bash
cd PaymentGateway
```
## 2 - Rodar os gateways mock:

Os gateways de pagamento são simulados utilizando a imagem docker fornecida no desafio.

Executar o comando:
```bash
docker run -p 3001:3001 -p 3002:3002 matheusprotzen/gateways-mock
```

Os gateways ficarão disponíveis em:

Gateway 1
http://localhost:3001

Gateway 2
http://localhost:3002

## 3 - Configurar banco de dados:
  
  Criar um banco MySQL e configurar a connection string no arquivo:
```bash
appsettings.json
Exemplo:
``````bash
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=paymentdb;user=root;password=senha"
}
```
## 4 -Executar migrations

Criar as tabelas do banco de dados:
```bash
dotnet ef database update
```
## 5 - Rodar a aplicação
Executar o projeto com:
```bash
dotnet run
```
## 6 - Acessar a documentação da API
Swagger estará disponível em:
```bash
https://localhost:7274/swagger
```

# Detalhamento das Rotas

## Compras:
### POST /api/purchase
Realiza uma compra informando produtos e dados do cliente.

Exemplo de request:
```JSON
{
  "products": [
    {
      "productId": 1,
      "quantity": 2
    }
  ],
  "name": "Cliente Teste",
  "email": "cliente@email.com",
  "cardNumber": "5569000000006063",
  "cvv": "010"
}
```
### Fluxo:

- O backend calcula o valor total da compra com base nos produtos.

- O PaymentService tenta processar o pagamento nos gateways ativos.

- Caso o primeiro gateway falhe, o próximo gateway é utilizado.

- Se o pagamento for aprovado, a transação é registrada.

## Transações
### GET /api/transactions

Lista todas as compras registradas no sistema.

### GET /api/transactions/{id}

Retorna os detalhes de uma compra específica.

### Inclui:

- cliente

- valor

- gateway utilizado

- status da transação

## Clientes
### GET /api/clients

Lista todos os clientes registrados.

### GET /api/clients/{id}

Retorna os dados de um cliente e seu histórico de compras.

## Reembolso
### POST /api/refund

Realiza o reembolso de uma transação utilizando o gateway responsável pela cobrança.

O sistema identifica automaticamente qual gateway foi utilizado na compra e envia a solicitação de reembolso.

# Estrutura do Banco de Dados

Principais tabelas do sistema:

### users
Armazena usuários do sistema e suas roles.

### clients
Clientes que realizam compras.

### products
Produtos disponíveis para compra.

### transactions
Registra as compras realizadas.

### transactionProducts
Relacionamento entre transações e produtos comprados.

### gateways
Configuração dos gateways disponíveis e sua prioridade de uso.

## Funcionamento do Multi-Gateway

O sistema utiliza uma estratégia de fallback entre gateways.

Fluxo:

- 1. O sistema consulta os gateways ativos no banco de dados.

- 2. Os gateways são ordenados pela prioridade configurada.

- 3. O PaymentService tenta processar o pagamento no primeiro gateway.

- 4. Caso ocorra erro, o sistema tenta o próximo gateway disponível.

- 5. Quando um gateway retorna sucesso, a transação é registrada e o processo é encerrado.

Esse modelo permite adicionar novos gateways futuramente com facilidade.

## Validação de dados

A API utiliza DataAnnotations para validar os dados recebidos nas requisições.

Exemplos:

- validação de email

- validação de cartão de crédito

- quantidade mínima de produtos

- campos obrigatórios

Caso os dados sejam inválidos, a API retorna automaticamente HTTP 400 Bad Request.

## Observações

- O projeto foi desenvolvido utilizando .NET e Entity Framework Core para persistência no banco de dados.

- A documentação da API pode ser explorada diretamente pelo Swagger.

- Devido ao prazo do teste técnico, testes automatizados (TDD) não foram implementados.
