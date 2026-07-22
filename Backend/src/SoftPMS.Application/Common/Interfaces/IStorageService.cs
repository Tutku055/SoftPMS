
namespace SoftPMS.Application.Common.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(Guid documentId, Stream fileStream, string fileName, CancellationToken cancellationToken = default);
    Task AppendChunkAsync(string uploadId, Stream chunkStream, CancellationToken cancellationToken = default);
    Task<string> CommitChunksAsync(string uploadId, Guid documentId, string fileName, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string relativePath, CancellationToken cancellationToken = default);
    Task DeleteAsync(string relativePath, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string relativePath, CancellationToken cancellationToken = default);
    Task<long> GetFileSizeAsync(string relativePath, CancellationToken cancellationToken = default);
}
