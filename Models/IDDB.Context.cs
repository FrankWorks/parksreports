﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DashBoard.Web.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class ParksEmployeeEntities : DbContext
    {
        public ParksEmployeeEntities()
            : base("name=ParksEmployeeEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ADUser> ADUsers { get; set; }
    
        public virtual int uspDeleteADUser()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("uspDeleteADUser");
        }
    }
}
