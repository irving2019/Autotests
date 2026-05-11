using Xunit;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; } //количество
    public User User { get; set; }
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(200);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserId);
        });
    }
}

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public User Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User GetById(int id)
    {
        return _context.Users.Find(id);
    }

    public List<User> GetByName(string name)
    {
        return _context.Users.Where(u => u.Name == name).ToList();
    }

    public void Delete(int id)
    {
        var user = _context.Users.Find(id);

        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}

public class UserRepositoryIntegrationTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

        _context = new AppDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public void AddUser_ShouldPersistToDatabase()
    {
        var user = new User { Name = "Voldemar", Email = "vovapilip46@gmail.com", CreatedAt = DateTime.UtcNow };
        var savedUser = _repository.Add(user);

        savedUser.Id.Should().BeGreaterThan(0);

        var fromDb = _context.Users.Find(savedUser.Id);
        fromDb.Should().NotBeNull();
        fromDb.Name.Should().Be("Voldemar");
    }

    [Fact]
    public void GetById_ShouldReturnCorrectUser()
    {
        var user = new User { Name = "Bob", Email = "bob@example.ru" };
        _context.Users.Add(user);
        _context.SaveChanges();

        var retrieved = _repository.GetById(user.Id);

        retrieved.Should().NotBeNull();
        retrieved.Name.Should().Be("Bob");
    }

    [Fact]
    public void GetByName_ShouldReturnMatchingUsers()
    {
        _context.Users.AddRange(
            new User { Name = "Иван", Email = "IvanIve@example.com"},
            new User { Name = "Владимир", Email = "vovapilip46@gmail.com"},
            new User { Name = "Иван", Email = "devnull@example.com"}
            );
        _context.SaveChanges();

        var results = _repository.GetByName("Иван");

        results.Should().HaveCount(2);
        results.All(u => u.Name == "Иван").Should().BeTrue();
    }

    [Fact]
    public void Delete_ShouldRemoveUser()
    {
        var user = new User { Name = "Ева", Email = "eva@example.ru" };
        _context.Users.Add(user);
        _context.SaveChanges();

        _repository.Delete(user.Id);

        _context.Users.Find(user.Id).Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

}

