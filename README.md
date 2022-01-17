# Sistema PDI

Sistema de gerenciamento de projetos de pesquisa e desenvolvimentos

## Prerequisitos

[.NET Core SKD](https://dotnet.microsoft.com/download) instalado\
[ASP.NET Core Runtime 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) instalado\
[SqlServer](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) instalado ou com acesso a um servidor
sqlserver

## Instalação

```bash
git clone caminho_do_repositório
cd ./caminho-do-repositorio
```

- Duplique o arquivo appsettings.example.json com o nome appsettings.json\
`cp ./PeD/appsettings.example.json ./PeD/appsettings.json`
- Copie os arquivos já transpilado do fronted para uma pasta de sua preferência



![image](readme/appsettings.json.png)

> Configurações necessárias

1) `ConnectionStrings.BaseGestor` string de conexão com o banco de dados.
2) `SpaPath` é o caminho onde o client(SPA) do sistema foi compilado.
3) `StoragePath` é a pasta onde os arquivos gerados pelo sistema deverão ser salvos  (Obs.: Somente caminho absoluto).
4) `ApiKey` do Sendgrid para o disparo de emails.
5) `Url` do sistema.
6) `CorsOrigin` Urls que podem fazer requisições
7) `AllowedExtensionFiles` Extensões de arquivos pertimidas para upload
8) `MaxFailedAccessAttempts` Máximo de tentativas de login
9) `LockoutTimeSpan` Minutos que um usuário fica impedido de fazer login caso erre a senha varias vezes seguidas

---

## Rodando a aplicação em desenvolvimento

```bash
dotnet run --project PeD/PeD.csproj
```
ou para que a aplicação atualize sempre que haja uma atualização nos arquivos use o seguinte comando:
```bash
dotnet watch --project PeD/PeD.csproj run
```


> Acesse a http://localhost:5000 e crie o usuário administrador

> Acesse http://locahost:4200 somente para o desenvolvimento do frontend, 
> é possível usar a aplicação somente com o servidor do backend corretamente configurado


![image](readme/criar-admin.png)

---

## Publicação

### Simples

```bash
dotnet publish -c Release
```
Após o build da aplicação os arquivos estarão por padrão na pasta `./PeD/bin/Release/netcoreapp3.1/publish`.
Abra o arquivo appsettings.json nesta página e preencha ou corrija com os dados descritos na seção de configurações acima
após isso é possível executar a aplicação como o comando `dotnet ./PeD.dll`

### Servidor
É possível realizar a publicação em alguns tipos diferente de servidores como apache, nginx, IIS
https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/?view=aspnetcore-3.1

## Dificuldades

### A aplicação não compila
- Verifique se versão do dotnet instalada é igual ou superior a 3.1, 
após corrigida a versão apague as pasta `bin` e `obj` na pasta do projeto e rode novamente o comando.
- Com a ajuda de um IDE, verifique se não houve alterações dos códigos que impença a compilação, como, por exemplo, 
a ausência de ; e } .

### Erros ao iniciar
> **"provider: TCP Provider, error: 40 - Could not open a connection to SQL Server"**
  - Verifique se o servidor de banco de dados está ativo ou se a string de conexão está correta, como Url, usuário ou senha incorretos.
  - Verifique se a máquina consegue acessar o servidor de banco de dados
 
> **Failed to bind to address _url:porta_ address already in use.**
  - Alguma outra aplicação na máquina já está usando o endereço desejado,
  mude para uma porta que não esteja em uso ( _Adicione `--urls=http://localhost:5003` ao final da linha de comando que inicial a aplicação_ ) 
ou desative a aplicação que está usando a porta. 

> Sendgrid não configurado!
- Preencha no appsettings.json um chave de api válida do Sendgrid para o funcionamento correto dos emails.



## Authors

- **Cristiano Chermont** - _Initial work_ - [xerminada](https://github.com/xerminada)
- **Diego França** - [diegointerativa](https://github.com/diegointerativa) [diihveloper](https://github.com/diihveloper)
