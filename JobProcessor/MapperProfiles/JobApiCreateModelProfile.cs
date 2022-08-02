using AutoMapper;
using JobProcessor.API.ApiModels;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;

namespace JobProcessor.API.MapperProfiles
{
    public class JobApiCreateModelProfile : Profile
    {
        public JobApiCreateModelProfile()
        {
            CreateMap<JobApiCreateModel, Job>()
                .ForMember(
                    dest => dest.JobId,
                    opt => opt.MapFrom(src => Guid.NewGuid())
                )
                .ForMember(
                    dest => dest.JobInput,
                    opt => opt.MapFrom(src => string.Join(",", src.NumberListInput.Select(number => number.ToString())))
                )
                .ForMember(
                    dest => dest.JobOutput,
                    opt => opt.MapFrom(src => string.Empty)
                )
                .ForMember(
                    dest => dest.JobEnqueuedDateTimeUtc,
                    opt => opt.MapFrom(src => DateTime.UtcNow)
                )
                .ForMember(
                    dest => dest.JobProcessingDurationMiliseconds,
                    opt => opt.MapFrom(src => 0)
                )
                .ForMember(
                    dest => dest.JobStatus,
                    opt => opt.MapFrom(src => JobStatus.Queued)
                );

        }
    }
}
