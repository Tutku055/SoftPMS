using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SoftlarePMS.Application.Common.Interfaces;
using SoftlarePMS.Domain.Entities;
using SoftlarePMS.Domain.Enums;

namespace SoftlarePMS.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;

    public ApplicationDbContextInitialiser(IApplicationDbContext context, ILogger<ApplicationDbContextInitialiser> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Eğer veritabanında halihazırda herhangi bir Employee varsa işlemi tamamen atlasın (Tek seferlik kuralı)
            if (await _context.Employees.AnyAsync())
            {
                return;
            }

            var creatorUserId = Guid.Parse("983cbe48-fffa-4236-ad2e-90dac4516b4b");

            // Foreign Key hatası almamak için bu ID'ye sahip bir User veritabanında var mı kontrol ediyoruz
            var creatorUserExists = await _context.Users.AnyAsync(u => u.Id == creatorUserId);

            if (!creatorUserExists)
            {
                _logger.LogWarning("Seeding iptal edildi: '983cbe48-fffa-4236-ad2e-90dac4516b4b' ID'li bir User veritabanında bulunamadı! Önce user eklemelisiniz.");
                return;
            }

            _logger.LogInformation("Bogus ile tek seferlik sahte personel verileri üretiliyor...");

            // Bogus Kuralları Tanımlama
            var employeeFaker = new Faker<Employee>("tr") // Türkçe isimler ve veriler için
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.EmployeeNo, f => $"EMP-{f.Random.Number(1000, 9999)}")
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Gender, f => f.PickRandom<Gender>())
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past(40, DateTime.Now.AddYears(-20))) // 20-60 yaş arası
                .RuleFor(e => e.Nationality, f => "TR")
                .RuleFor(e => e.Profession, f => f.Name.JobTitle())
                .RuleFor(e => e.EmploymentStatus, f => f.PickRandom<EmploymentStatus>())
                .RuleFor(e => e.HireDate, f => f.Date.Past(5)) // Son 5 yıl içinde
                .RuleFor(e => e.TerminationDate, (f, e) => e.EmploymentStatus == EmploymentStatus.Terminated ? f.Date.Recent() : null)
                .RuleFor(e => e.ProbationEndDate, (f, e) => e.HireDate.AddMonths(2)) // Standart 2 ay deneme süresi
                .RuleFor(e => e.WorkingHoursPerWeek, f => f.PickRandom(new decimal[] { 40, 45, 22.5m }))
                .RuleFor(e => e.VacationDaysTotal, f => f.Random.Number(14, 26))
                .RuleFor(e => e.IsDeleted, false)
                .RuleFor(e => e.CreatedByUserId, creatorUserId); // Sabit user ID'n

            // 50 adet gerçekçi veri üret
            var fakeEmployees = employeeFaker.Generate(50);

            await _context.Employees.AddRangeAsync(fakeEmployees);
            await _context.SaveChangesAsync();

            _logger.LogInformation("50 adet sahte personel veritabanına tek seferlik başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Veritabanı seed edilirken beklenmedik bir hata oluştu.");
            throw;
        }
    }
}