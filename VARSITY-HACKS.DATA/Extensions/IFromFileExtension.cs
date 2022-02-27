using Microsoft.AspNetCore.Http;

namespace VARSITY_HACKS.DATA;

public static class FormFileExtension
{
    public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
    {
        await using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        return memoryStream.ToArray();
    }
}