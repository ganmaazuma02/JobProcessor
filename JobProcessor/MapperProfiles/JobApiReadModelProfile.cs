using AutoMapper;
using JobProcessor.API.ApiModels;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.Enums;

namespace JobProcessor.API.MapperProfiles
{
    public class JobApiReadModelProfile : Profile
    {
        public JobApiReadModelProfile()
        {
            CreateMap<Job, JobApiReadModel>()
                .ForMember(
                    dest => dest.JobInput,
                    opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.JobInput) ? 
                    null : 
                    src.JobInput.Split(',', System.StringSplitOptions.None).Select(int.Parse))
                )
                .ForMember(
                    dest => dest.JobOutput,
                    opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.JobOutput) ?
                    null :
                    src.JobOutput.Split(',', System.StringSplitOptions.None).Select(int.Parse))
                )
                .ForMember(
                    dest => dest.JobStatus,
                    opt => opt.MapFrom(src => src.JobStatus.ToString())
                );
        }
    }
}
