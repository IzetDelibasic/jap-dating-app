using AutoMapper;
using DatingApp.Application.Contracts.Requests;
using DatingApp.Application.Contracts.Responses;
using DatingApp.Entities;
using DatingApp.Extensions;

namespace DatingApp.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberResponse>()
            .ForMember(d => d.Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
            .ForMember(d => d.PhotoUrl, o =>
                o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<Photo, PhotoResponse>();
        CreateMap<MemberResponse, AppUser>();
        CreateMap<UpdateMemberRequest, AppUser>();
        CreateMap<RegisterUserRequest, AppUser>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.DateOfBirth) ? default : DateOnly.Parse(src.DateOfBirth)));
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        CreateMap<Message, MessageResponse>()
            .ForMember(d => d.SenderPhotoUrl,
                o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain)!.Url))
            .ForMember(d => d.RecipientPhotoUrl,
                o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
            DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}
