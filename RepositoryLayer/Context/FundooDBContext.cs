using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Context
{
    public class FundooDBContext : DbContext
    {
        public FundooDBContext(DbContextOptions options) : base(options)
        {
        }

        //table creation
        public DbSet<UserEntity> Users { get; set; }
    }
}
