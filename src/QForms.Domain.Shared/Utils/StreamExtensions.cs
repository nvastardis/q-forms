namespace QForms.Utils;

public static class StreamExtensions
{
    private static async Task<Stream?> CopyToMemoryStreamAsync(
        this Stream stream,
        CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    public static async Task<byte[]> GetAllBytesAsync(
        this Stream stream,
        CancellationToken cancellationToken = default)
    {
        var buffer = new byte[stream.Length];
        using var ms = new MemoryStream();

        int read;
        while (( read = await stream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await ms.WriteAsync(buffer.AsMemory(0, read), cancellationToken);
        }
        return ms.ToArray();
    }
}