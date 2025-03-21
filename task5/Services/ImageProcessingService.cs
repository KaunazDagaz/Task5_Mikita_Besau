using DotNetEnv;
using SkiaSharp;
using task5.Models;
using task5.Services.IServices;

public class ImageProcessingService : IImageProcessingService
{
    private readonly HttpClient httpClient;

    private static readonly SKColor overlayColor = new SKColor(0, 0, 0, 180);
    private static readonly SKColor textColor = SKColors.White;
    private static readonly SKTypeface typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);
    private static readonly string imageGeneratorUrl = Environment.GetEnvironmentVariable("PIC_SUM_URL") ?? Env.GetString("PIC_SUM_URL");

    public ImageProcessingService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<string> GenerateBookCoverImage(BookViewModel book)
    {
        var imageUrl = $"{imageGeneratorUrl}/{book.ISBN}/400/600";
        using var memoryStream = new MemoryStream();
        using var httpStream = await httpClient.GetStreamAsync(imageUrl).ConfigureAwait(false);
        await httpStream.CopyToAsync(memoryStream).ConfigureAwait(false);
        memoryStream.Position = 0;
        using var originalBitmap = SKBitmap.Decode(memoryStream);
        return ProcessImageWithEfficiency(originalBitmap, book.Title, book.Author);
    }

    private string ProcessImageWithEfficiency(SKBitmap bitmap, string title, string author)
    {
        int bitmapWidth = bitmap.Width;
        int bitmapHeight = bitmap.Height;
        using var font = new SKFont
        {
            Typeface = typeface,
            Size = 30
        };
        using var paint = new SKPaint
        {
            Color = textColor,
            IsAntialias = true
        };
        using var backgroundPaint = new SKPaint { Color = overlayColor };
        float textHeight = font.Size;
        float spacing = 10;
        float rectPadding = 20;
        float rectHeight = textHeight * 2 + spacing + rectPadding * 2;
        float rectY = (bitmapHeight - rectHeight) / 2;
        float textX = bitmapWidth / 2;
        float titleY = rectY + rectPadding + textHeight;
        float authorY = titleY + textHeight + spacing;
        using var surface = SKSurface.Create(new SKImageInfo(bitmapWidth, bitmapHeight));
        using var canvas = surface.Canvas;
        canvas.DrawBitmap(bitmap, 0, 0);
        canvas.DrawRect(0, rectY, bitmapWidth, rectHeight, backgroundPaint);
        canvas.DrawText(title, textX, titleY, SKTextAlign.Center, font, paint);
        canvas.DrawText(author, textX, authorY, SKTextAlign.Center, font, paint);
        using var snapshot = surface.Snapshot();
        using var data = snapshot.Encode(SKEncodedImageFormat.Jpeg, 85);
        return $"data:image/jpeg;base64,{Convert.ToBase64String(data.ToArray())}";
    }
}