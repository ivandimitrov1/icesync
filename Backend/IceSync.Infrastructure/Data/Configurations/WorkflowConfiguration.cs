using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IceSync.Domain;

namespace IceSync.Infrastructure.Data.Configurations;

public class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder.ToTable("Workflows");
        
        builder.Property(c => c.Id).HasColumnName("Id").UseIdentityColumn().IsRequired();
        builder.HasKey(c => c.Id).HasName("PK_Workflow_Id");

        // TO:DO add column restrictions and indexes
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.WorkflowId).IsRequired();
        builder.Property(t => t.WorkflowName).IsRequired();
        builder.Property(t => t.IsActive).IsRequired();
        builder.Property(t => t.MultiExecBehavior).IsRequired();
        builder.Property(t => t.WorkflowName).IsRequired();
    }
}
