using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configurations
{
    public class TodoListConfig : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Ignore(l => l.DomainEvents);

            builder.Property(l => l.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(l => l.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
