namespace JobProcessor.Service.Configurations
{
    public class BackgroundJobConfig
    {
        public int JobCheckingFrequencyInSeconds { get; set; }
        public bool IsFakeLongRunningTask { get; set; }
    }
}
