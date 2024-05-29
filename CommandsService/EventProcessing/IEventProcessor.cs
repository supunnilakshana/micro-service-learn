namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(String message);


    }
}


