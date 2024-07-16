﻿namespace RegistrationService.Services.IServices;

public interface IBlobServices
{
    Task<string> UploadBlobAsync(IFormFile file);
    Task<string> UploadBlobWithContentTypeAsync(IFormFile file, string contentType);
    Task DeleteBlobAsync(string blobUrl);
}