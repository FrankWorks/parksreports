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
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects.DataClasses;
    using System.Linq;
    
    public partial class AWorksEntities : DbContext
    {
        public AWorksEntities()
            : base("name=AWorksEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<f_uspGetTotalYearlySales_Result> f_uspGetTotalYearlySales()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<f_uspGetTotalYearlySales_Result>("f_uspGetTotalYearlySales");
        }
    }
}
