﻿using Finix.Domain.Entity.SystemManage;
using System.Data.Entity.ModelConfiguration;

namespace Finix.Mapping.SystemManage
{
    public class ModuleMap : EntityTypeConfiguration<ModuleEntity>
    {
        public ModuleMap()
        {
            this.ToTable("Sys_Module");
            this.HasKey(t => t.F_Id);
        }
    }
}
