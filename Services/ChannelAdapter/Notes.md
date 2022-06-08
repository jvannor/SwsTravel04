RE: EFCore

dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.Data.SqlClient --version 4.1.0

dotnet ef dbcontext scaffold "Server=<SERVER-NAME>;Database=<DATABASE-NAME>;Authentication=Active Directory Default;" Microsoft.EntityFrameworkCore.SqlServer --context EventDbContext --context-dir Data --output-dir Models --no-onconfiguring

RE: Azure

dotnet add package Azure.Identity
dotnet add package Azure.Messaging.ServiceBus
dotnet add package Microsoft.Extensions.Azure

RE: Docker

docker build -t <ACR-NAME>.azurecr.io/swstravel04-channeladapter:latest -f Dockerfile .

RE: Container Registry

az acr login --name <ACR-NAME>.azurecr.io
docker push <ACR-NAME>.azurecr.io/swstravel04-channeladapter:latest

RE: AKS

kubectl apply -f ./deploy.yml