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

        //Create table Users 
        public DbSet<UserEntity> Users { get; set; }

        //Create Table Notes
        public DbSet<NotesEntity> Notes { get; set; }
    }
}
