namespace SelectiveConsumer.Clients;

public interface IMicroserviceClient
{
    Task ProcessEvent(string data);
}