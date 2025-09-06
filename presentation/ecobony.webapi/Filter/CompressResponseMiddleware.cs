using System.IO.Compression;

namespace ecobony.webapi.Filter
{
    public class CompressResponseMiddleware(RequestDelegate _next)
    {
       

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            var compressedStream = new MemoryStream();
            context.Response.Body = compressedStream;

            try
            {
                // Digər middleware-ləri icra et
                await _next(context);

                // Status code-a əsasən davranış seçimi
                switch (context.Response.StatusCode)
                {
                    case StatusCodes.Status200OK when compressedStream.Length > 0 &&
                                                        !string.IsNullOrEmpty(context.Response.ContentType) &&
                                                        (context.Response.ContentType.Contains("application/json") ||
                                                         context.Response.ContentType.Contains("text") ||
                                                         context.Response.ContentType.Contains("xml")):
                        // Yalnız 200 OK və gzip-ə uyğun content tipi
                        context.Response.Headers["Content-Encoding"] = "gzip";
                        compressedStream.Seek(0, SeekOrigin.Begin);
                        var gzipStream = new GZipStream(originalBodyStream, CompressionLevel.Fastest, leaveOpen: true);
                        await compressedStream.CopyToAsync(gzipStream);
                        await gzipStream.FlushAsync(); // Əmin ol ki, bütün data yazılıb
                        break;

                    case StatusCodes.Status401Unauthorized when compressedStream.Length > 0:
                        // 401 cavablar üçün sıxma etmədən göndər
                        compressedStream.Seek(0, SeekOrigin.Begin);
                        await compressedStream.CopyToAsync(originalBodyStream);
                        break;

                    default:
                        // Digər hallarda normal göndər
                        compressedStream.Seek(0, SeekOrigin.Begin);
                        await compressedStream.CopyToAsync(originalBodyStream);
                        break;
                }
            }
            finally
            {
                // Response stream-i bərpa et
                context.Response.Body = originalBodyStream;
                // compressedStream dispose etməyə ehtiyac yoxdur, GC bunu idarə edəcək
            }
        }
    }
}
