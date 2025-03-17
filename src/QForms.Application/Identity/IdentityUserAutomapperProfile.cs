using AutoMapper;

namespace QForms.Identity;

public class IdentityUserAutomapperProfile: Profile
{
    public IdentityUserAutomapperProfile()
    {
        CreateMap<ApplicationUser, IdentityUserDto>();
        CreateMap<ApplicationUser, IdentityUserLookUpDto>();
        CreateMap<IdentityUserFilterInputDto, IdentityUserFilter>();
    }   
}