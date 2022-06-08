using API.Enums;
using Microsoft.AspNetCore.Authorization;

namespace API.Helpers; 

public class AllowAuthorizedAttribute : AuthorizeAttribute{
    public AllowAuthorizedAttribute(params AccessRoles[] roles) : base() {
        Roles = String.Join(",", Enum.GetNames(typeof(AccessRoles)));
    }
}