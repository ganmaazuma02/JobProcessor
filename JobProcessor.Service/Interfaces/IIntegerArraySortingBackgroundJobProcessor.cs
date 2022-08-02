using JobProcessor.Data.EntityModels;

namespace JobProcessor.Service.Interfaces
{
    public interface IIntegerArraySortingBackgroundJobProcessor
    {
        Job SortArrayFromJob(Job job);
    }
}
