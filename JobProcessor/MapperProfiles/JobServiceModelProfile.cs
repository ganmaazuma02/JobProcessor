using AutoMapper;
using JobProcessor.Data.EntityModels;
using JobProcessor.Data.ServiceModels;

namespace JobProcessor.API.MapperProfiles
{
    public class JobServiceModelProfile : Profile
    {
        public JobServiceModelProfile()
        {
            CreateMap<Job, JobServiceModel>()
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
               );

            CreateMap<JobServiceModel, Job>()
               .ForMember(
                    dest => dest.JobInput,
                    opt => opt.MapFrom(src => string.Join(",", src.JobInput.Select(number => number.ToString())))
                )
               .ForMember(
                    dest => dest.JobOutput,
                    opt => opt.MapFrom(src => string.Join(",", src.JobOutput.Select(number => number.ToString())))
                );
        }
    }
}
