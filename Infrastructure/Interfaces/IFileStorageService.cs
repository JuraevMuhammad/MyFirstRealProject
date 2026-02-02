using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file, string relativeFolder);
    Task DeleteFileAsync(string relativePath);
}