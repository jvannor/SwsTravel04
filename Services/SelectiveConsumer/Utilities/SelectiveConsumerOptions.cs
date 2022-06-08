namespace SelectiveConsumer.Utilities;

public class SelectiveConsumerOptions
{
    public string Topic { get; set; } = string.Empty;

    public string Subscription { get; set; } = string.Empty;

    public string Label { get; set; } = "SelectiveConsumer";

    public string MicroserviceUri { get; set; } = "http://httpbin.org/post";
}