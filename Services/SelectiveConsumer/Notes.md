RE: Azure

dotnet add package Azure.Identity
dotnet add package Azure.Messaging.ServiceBus
dotnet add package Microsoft.Extensions.Azure
dotnet add package Microsoft.Extensions.Http
dotnet add package Microsoft.Extensions.Http.Polly 

RE: Docker

docker build -t <ACR-NAME>.azurecr.io/swstravel04-selectiveconsumer:latest -f Dockerfile .

RE: Container Registry

az acr login --name <ACR-NAME>.azurecr.io
docker push <ACR-NAME>.azurecr.io/swstravel04-selectiveconsumer:latest