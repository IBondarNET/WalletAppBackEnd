using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WalletApp.Backend.Models;

namespace WalletApp.Backend.Data;

public class WalletContext : DbContext 
{
    public WalletContext()
    {
        
    }
    public WalletContext(DbContextOptions<WalletContext> options) : base(options)
    {
        
    }
    
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionType> Types { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<AuthorizedUser> AuthorizedUsers { get; set; }
    public DbSet<TransactionImage> Images { get; set; }
    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("NpgConnection"));
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.TransactionType)
                .WithMany()
                .HasForeignKey(t => t.TypeId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.AuthorizedUser)
                .WithMany()
                .HasForeignKey(t => t.AuthorizedUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.TransactionImage)
                .WithMany()
                .HasForeignKey(t => t.ImageId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

        });
        modelBuilder.Entity<TransactionType>(builder =>
        {
            builder.HasKey(t => t.Id);
        });
        modelBuilder.Entity<Status>(builder =>
        {
            builder.HasKey(t => t.Id);
        });
        modelBuilder.Entity<AuthorizedUser>(builder =>
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<TransactionImage>(builder =>
        {
            builder.HasKey(t => t.Id);
        });
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(t => t.Id);
        });
    }
}
