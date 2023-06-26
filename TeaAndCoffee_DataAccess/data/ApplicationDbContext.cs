﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TeaAndCoffee_Models;

namespace TeaAndCoffee_DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<InquiryHeader> InquiryHeader { get; set; }
        public DbSet<InquiryDetails> InquiryDetails { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
    }
}
