using Microsoft.Extensions.Configuration;
using SoftPMS.Application.Common.Interfaces;

namespace SoftPMS.Infrastructure.Services;

public class LocalFileStorageService : IStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(IConfiguration configuration)
    {
        _basePath = configuration.GetValue<string>("VaultStorage:BasePath") ?? @"C:\SoftPMS_Storage";
    }

    public async Task<string> UploadAsync(Guid documentId, Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("File stream is empty", nameof(fileStream));

        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{documentId}{extension}";
        
        var now = DateTime.UtcNow;
        var relativeDirectory = Path.Combine(now.ToString("yyyy"), now.ToString("MM"));
        var physicalDirectory = Path.Combine(_basePath, relativeDirectory);

        if (!Directory.Exists(physicalDirectory))
        {
            Directory.CreateDirectory(physicalDirectory);
        }

        var relativePath = Path.Combine(relativeDirectory, uniqueFileName).Replace("\\", "/");
        var physicalPath = Path.Combine(physicalDirectory, uniqueFileName);

        using var stream = new FileStream(physicalPath, FileMode.Create);
        await fileStream.CopyToAsync(stream, cancellationToken);

        return relativePath;
    }

    public async Task AppendChunkAsync(string uploadId, Stream chunkStream, CancellationToken cancellationToken = default)
    {
        if (chunkStream == null || chunkStream.Length == 0)
            throw new ArgumentException("Chunk stream is empty", nameof(chunkStream));

        var tempDirectory = Path.Combine(_basePath, "TempUploads");
        if (!Directory.Exists(tempDirectory))
        {
            Directory.CreateDirectory(tempDirectory);
        }

        var tempFilePath = Path.Combine(tempDirectory, uploadId);

        var mode = File.Exists(tempFilePath) ? FileMode.Append : FileMode.Create;
        using var stream = new FileStream(tempFilePath, mode, FileAccess.Write, FileShare.None);
        await chunkStream.CopyToAsync(stream, cancellationToken);
    }

    public Task<string> CommitChunksAsync(string uploadId, Guid documentId, string fileName, CancellationToken cancellationToken = default)
    {
        var tempDirectory = Path.Combine(_basePath, "TempUploads");
        var tempFilePath = Path.Combine(tempDirectory, uploadId);

        if (!File.Exists(tempFilePath))
            throw new FileNotFoundException("Temporary upload file not found", tempFilePath);

        var extension = Path.GetExtension(fileName);
        var uniqueFileName = $"{documentId}{extension}";
        
        var now = DateTime.UtcNow;
        var relativeDirectory = Path.Combine(now.ToString("yyyy"), now.ToString("MM"));
        var physicalDirectory = Path.Combine(_basePath, relativeDirectory);

        if (!Directory.Exists(physicalDirectory))
        {
            Directory.CreateDirectory(physicalDirectory);
        }

        var relativePath = Path.Combine(relativeDirectory, uniqueFileName).Replace("\\", "/");
        var physicalPath = Path.Combine(physicalDirectory, uniqueFileName);

        File.Move(tempFilePath, physicalPath);

        return Task.FromResult(relativePath);
    }

    public Task<Stream> DownloadAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        var physicalPath = Path.Combine(_basePath, relativePath.Replace("/", "\\"));
        
        if (!File.Exists(physicalPath))
            throw new FileNotFoundException("Document not found", physicalPath);

        Stream stream = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult(stream);
    }

    public Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        var physicalPath = Path.Combine(_basePath, relativePath.Replace("/", "\\"));
        
        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
        
        return Task.CompletedTask;
    }

    public Task<long> GetFileSizeAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        var physicalPath = Path.Combine(_basePath, relativePath.Replace("/", "\\"));
        if (!File.Exists(physicalPath)) return Task.FromResult(0L);
        return Task.FromResult(new FileInfo(physicalPath).Length);
    }

    public Task<bool> FileExistsAsync(string relativePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return Task.FromResult(false);
        var physicalPath = Path.Combine(_basePath, relativePath.Replace("/", "\\"));
        return Task.FromResult(File.Exists(physicalPath));
    }
}
