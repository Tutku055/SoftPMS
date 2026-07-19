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

            _logger.LogInformation("Bogus ile ilişkili sahte personel verileri (100 adet) üretiliyor...");

            // 1. Address Faker
            var addressFaker = new Faker<EmployeeAddress>()
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.AddressLine, f => f.Address.StreetAddress())
                .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.Country, f => f.PickRandom("United Kingdom", "Germany"))
                .RuleFor(a => a.IsPrimary, f => true);

            // 2. Compensation Faker
            var compensationFaker = new Faker<EmployeeCompensation>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.PayGrade, f => f.Random.String2(1, "ABCDE") + f.Random.Number(1, 5))
                .RuleFor(c => c.BaseSalary, f => f.Finance.Amount(3000m, 12000m))
                .RuleFor(c => c.SalaryType, f => f.PickRandom<SalaryType>())
                .RuleFor(c => c.EffectiveDate, f => f.Date.Past(2))
                .RuleFor(c => c.EndDate, f => null)
                .RuleFor(c => c.CreatedByUserId, f => creatorUserId);

            // 3. Document Faker
            var documentFaker = new Faker<EmployeeDocument>()
                .RuleFor(d => d.Id, f => Guid.NewGuid())
                .RuleFor(d => d.DocumentType, f => f.PickRandom<DocumentType>())
                .RuleFor(d => d.FileName, (f, d) => $"{d.DocumentType}_{f.Random.AlphaNumeric(6)}.pdf")
                .RuleFor(d => d.FilePath, (f, d) => $@"C:\Users\user\Downloads\DocumentsFake\{d.FileName}")
                .RuleFor(d => d.IssueDate, f => f.Date.Past(1))
                .RuleFor(d => d.ExpiryDate, f => f.Date.Future(3))
                .RuleFor(d => d.ReminderDate, (f, d) => d.ExpiryDate?.AddDays(-30))
                .RuleFor(d => d.CreatedByUserId, f => creatorUserId);

            // 4. Note Faker
            var noteFaker = new Faker<EmployeeNote>()
                .RuleFor(n => n.Id, f => Guid.NewGuid())
                .RuleFor(n => n.Title, f => f.Lorem.Sentence(3))
                .RuleFor(n => n.Content, f => f.Lorem.Paragraphs(2))
                .RuleFor(n => n.CreatedByUserId, f => creatorUserId);

            // 5. Reference Faker
            var referenceFaker = new Faker<EmployeeReference>()
                .RuleFor(r => r.Id, f => Guid.NewGuid())
                .RuleFor(r => r.CompanyName, f => f.Company.CompanyName())
                .RuleFor(r => r.ContactPerson, f => f.Name.FullName())
                .RuleFor(r => r.Title, f => f.Name.JobTitle())
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .RuleFor(r => r.Notes, f => f.Lorem.Sentence());

            // 6. Main Employee Faker
            int employeeIdCounter = 1;
            var employeeFaker = new Faker<Employee>() // Dil opsiyonunu kaldırdık çünkü UK ve DE datası üretiyoruz
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.EmployeeNo, f => $"EMP-{employeeIdCounter++:D4}")
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Gender, f => f.PickRandom<Gender>())
                .RuleFor(e => e.DateOfBirth, f => f.Date.Past(40, DateTime.Now.AddYears(-18)))
                .RuleFor(e => e.Nationality, f => f.PickRandom("English", "German"))
                .RuleFor(e => e.Profession, f => f.Name.JobTitle())
                .RuleFor(e => e.EmploymentStatus, f => f.PickRandom<EmploymentStatus>())
                .RuleFor(e => e.HireDate, f => f.Date.Past(5))
                .RuleFor(e => e.TerminationDate, f => null)
                .RuleFor(e => e.ProbationEndDate, (f, e) => e.HireDate.AddMonths(3))
                .RuleFor(e => e.WorkingHoursPerWeek, f => f.Random.Decimal(20m, 40m))
                .RuleFor(e => e.VacationDaysTotal, f => f.Random.Int(20, 30))
                .RuleFor(e => e.IsDeleted, false)
                .RuleFor(e => e.CreatedByUserId, creatorUserId)

                // --- Alt Koleksiyonların Guid Bağlantılarıyla Oluşturulması ---
                .RuleFor(e => e.Addresses, (f, e) => {
                    var addresses = addressFaker.Generate(f.Random.Int(1, 2));
                    addresses.ForEach(a => a.EmployeeId = e.Id);
                    return addresses;
                })
                .RuleFor(e => e.Compensations, (f, e) => {
                    var comps = compensationFaker.Generate(1);
                    comps.ForEach(c => c.EmployeeId = e.Id);
                    return comps;
                })
                .RuleFor(e => e.Documents, (f, e) => {
                    var docs = documentFaker.Generate(f.Random.Int(2, 5));
                    docs.ForEach(d => d.EmployeeId = e.Id);
                    return docs;
                })
                .RuleFor(e => e.Notes, (f, e) => {
                    var notes = noteFaker.Generate(f.Random.Int(0, 3));
                    notes.ForEach(n => n.EmployeeId = e.Id);
                    return notes;
                })
                .RuleFor(e => e.References, (f, e) => {
                    var refs = referenceFaker.Generate(f.Random.Int(1, 3));
                    refs.ForEach(r => r.EmployeeId = e.Id);
                    return refs;
                });

            // 100 adet çalışan ve iç içe bağlı tüm verileri üret
            var fakeEmployees = employeeFaker.Generate(100);

            // AddRangeAsync içindeki koleksiyonları Entity Framework Core algılayıp toplu insert atar
            await _context.Employees.AddRangeAsync(fakeEmployees);
            await _context.SaveChangesAsync();

            _logger.LogInformation("100 adet English/German personel ve bunlara bağlı tüm ilişkili veriler başarıyla eklendi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Veritabanı seed edilirken beklenmedik bir hata oluştu.");
            throw;
        }
    }
}