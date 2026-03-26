namespace Order.Application.Interfaces
{
    public interface IMessageProducer
    {
        Task ProducerAsync(string topic, object message);
    }
}
