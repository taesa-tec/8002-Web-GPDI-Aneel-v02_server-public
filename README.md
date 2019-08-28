# Sistema P&D

Sistema de gerenciamento de projetos de pesquisa e desenvolvimentos

## Iniciando

Essas Instruções são para que você possa rodar uma cópia local em sua máquina.

### Prerequisitos

[.NET Core 2.1 SKD](https://dotnet.microsoft.com/download) instalado

### Instalação

```bash
cd ./caminho-do-repositorio
```

- Duplique o arquivo appsettings.example.json com o nome appsettings.json

```bash
cp ./appsettings.example.json appsettings.json
```

- Atualize a _ConnectionString.BaseGestor_
- Defina o nome do aplicativo (*AppName*) e o email de contato (*ContactEmail*)
- Defina o login e a senha inicial do usuário administrativo (_AdminUser.Email_ e _AdminUser.Password_)
- Defina as credenciais do SendGrid para o disparo de emails (_SendGrid_.*)

```bash
dotnet restore
```

### Para instalar o banco de dados

Instale via linha de comando na máquina

```bash
dotnet ef database update
```

#### OU

Gere o script de instalação do banco de dados:

```bash
dotnet ef migrations script > install.sql
```

E importe o arquivo gerado para o banco de dados

---

## Configurações

## Rodando a aplicação

```bash
dotnet run
```

ou para atualizar a cada atualização de script

```bash
dotnet watch run
```

---

## Deployment

```bash
dotnet publish -c Relaease
```

## Built With

- [.NET Core](https://dotnet.microsoft.com/) - Plataforma de desenvolvimento usada

## Authors

- **Cristiano Chermont** - _Initial work_ - [xerminada](https://github.com/xerminada)
- **Diego França** - [diegointerativa](https://github.com/diegointerativa)
