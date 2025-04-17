using System;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContex context){
        if(await context.Users.AnyAsync()) return;
        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var options =new JsonSerializerOptions{PropertyNameCaseInsensitive=true};
        var users=JsonSerializer.Deserialize<List<AppUser>>(userData,options);
        if(users==null) return;
        foreach (var user in users)
        {
            using var hmac =new HMACSHA512();
            user.UserName=user.UserName.ToLower();
            user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt =hmac.Key;

            context.Users.Add(user);
            

        }
        await context.SaveChangesAsync();
    }

}
