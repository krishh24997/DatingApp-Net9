using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContex(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }


}
