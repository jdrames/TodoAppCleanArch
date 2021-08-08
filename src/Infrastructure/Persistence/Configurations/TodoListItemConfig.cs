using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class TodoListItemConfig : IEntityTypeConfiguration<TodoListItem>
    {
        public void Configure(EntityTypeBuilder<TodoListItem> builder)
        {
            builder.Ignore(li => li.DomainEvents);

            builder.Property(li => li.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(li => li.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
