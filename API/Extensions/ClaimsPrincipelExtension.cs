using System;
using System.Security.Claims;

namespace API.Extensions;

static class ClaimsPrincipelExtension
{
    public static  string GetUsername(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier) 
        ?? throw new Exception("Cannot get the  username from token");
        return username;
    }

}
