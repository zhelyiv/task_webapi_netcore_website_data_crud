using AutoMapper;
using DAL.Ef.Entities;
using Shared.ApiModel;

namespace DAL.Ef.EntityMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {  
            CreateMap<WebsiteProxy, Website>(MemberList.Source)
                .ReverseMap();

            CreateMap<WebsiteHomepageSnapshotProxy, WebsiteHomepageSnapshot>(MemberList.Source)
                .ReverseMap();

            CreateMap<WebsiteLoginProxy, WebsiteLogin>(MemberList.Source)
                .ReverseMap();

            CreateMap<WebsiteFieldProxy, WebsiteField>(MemberList.Source)
                .ReverseMap();

            CreateMap<Website, Website>(MemberList.None)
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.DateCreatedUtc, opt => opt.Ignore())
                .ForMember(x => x.DateUpdatedUtc, opt => opt.Ignore())
                .ForMember(x => x.HomepageSnapshotId, opt => opt.Ignore())
                .ForMember(x => x.HomepageSnapshot, opt => opt.Condition(src => src.HomepageSnapshot != null))
                .ForMember(x => x.Logins, opt => opt.Ignore())
                .ForMember(x => x.Fields, opt => opt.Ignore())
                .ForMember(x => x.StatusId, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Name)))
                .ForMember(x => x.Url, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Url)))
                ;

            CreateMap<WebsiteHomepageSnapshot, WebsiteHomepageSnapshot>(MemberList.None)
                .ForMember(x => x.Id, opt => opt.Ignore()) 
                .ForMember(x => x.DateCreatedUtc, opt => opt.Ignore())
                .ForMember(x => x.DateUpdatedUtc, opt => opt.Ignore())
                ;

            CreateMap<WebsiteField, WebsiteField>(MemberList.None) 
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Website, opt => opt.Ignore())
                .ForMember(x => x.WebsiteId, opt => opt.Ignore())
                .ForMember(x => x.DateCreatedUtc, opt => opt.Ignore())
                .ForMember(x => x.DateUpdatedUtc, opt => opt.Ignore())
                ;

            CreateMap<WebsiteLogin, WebsiteLogin>(MemberList.None)
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Website, opt => opt.Ignore())
                .ForMember(x => x.WebsiteId, opt => opt.Ignore()) 
                .ForMember(x => x.DateCreatedUtc, opt => opt.Ignore())
                .ForMember(x => x.DateUpdatedUtc, opt => opt.Ignore())
                .ForMember(x => x.Email, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Email)))
                .ForMember(x => x.Password, opt => opt.Condition(src => !string.IsNullOrWhiteSpace(src.Password)))
                ;
        }

        public static IMapper GetMapperInstance()
        {
            return new MapperConfiguration(x =>
            {
                x.AddProfile(new AutoMapperProfile());
            }).CreateMapper();
        }
    }
}
