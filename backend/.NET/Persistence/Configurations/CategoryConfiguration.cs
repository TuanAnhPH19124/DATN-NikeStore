using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(nameof(Category));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.ModifiedDate).IsRequired(false);
            builder.Property(p => p.Name).IsRequired();
            builder.HasIndex(p => p.Name).IsUnique();

            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.ChildCategories)
                   .HasForeignKey(c => c.ParentCategoriesId)
                   .IsRequired(false);

            builder.HasMany(p => p.CategoryProducts)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
