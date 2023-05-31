using System.Linq;
using API.DTO;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    src.DateOfBirth.CalculateAge()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => 
                    src.UserName));
            CreateMap<MemberDTO, MemberWithDetailsDTO>();

            CreateMap<Photo, PhotoDTO>();

            CreateMap<MemberUpdateDTO, AppUser>();

            CreateMap<RegisterDTO, AppUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => 
                    src.Name));

            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src => 
                    src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src => 
                    src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}