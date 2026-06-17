using CabanaOSDemo.Models;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Windows.Controls;
using Document = QuestPDF.Fluent.Document;

namespace CabanaOSDemo.Utils
{
    public static class RestaurantTokenGenerator
    {
        private const float CardW = 240f;

        public static byte[] GenerateTokenPdf(AccessCardData data)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var qrBytes = GenerateQrCode(data.BookingNumber);

            var doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.PageColor(Colors.White);

                    page.Content().AlignCenter().AlignMiddle().Width(CardW).Decoration(dec =>
                    {
                        dec.Before().Background("#F0F4FB").Border(1).BorderColor("#D6DFF0");

                        dec.Content().Padding(15).Column(col =>
                        {
                            col.Spacing(5);
                            // Title updated for Restaurant
                            col.Item().AlignCenter().Text("Restaurant Access Token").FontSize(16).Bold().FontColor("#1A1A2E");
                            col.Item().LineHorizontal(0.5f).LineColor("#C5CFE8");

                            // Order Number label instead of Billing Number
                            col.Item().PaddingTop(10).AlignCenter().Text("Order Number").FontSize(9).Bold();
                            col.Item().AlignCenter().Text(data.BookingNumber ?? "N/A").FontSize(8.5f).FontColor("#6B7FC4");

                            col.Item().AlignCenter().Width(100).Height(100).Image(qrBytes).FitArea();

                            // Guest Details Box
                            col.Item().Background(Colors.White).Border(0.5f).BorderColor("#D6DFF0").Padding(10).Column(inner =>
                            {
                                inner.Item().Text("Guest Details").FontSize(9).Bold();
                                inner.Item().PaddingTop(4).LineHorizontal(0.5f).LineColor("#E0E7F5");
                                inner.Item().PaddingTop(6).Text($"Guest Name : {data.GuestName ?? "N/A"}").FontSize(7.5f);
                                inner.Item().PaddingTop(6).Text($"Guest NIC  : {data.GuestNIC ?? "N/A"}").FontSize(7.5f);
                                inner.Item().PaddingTop(6).Text($"Date       : {data.Date ?? "N/A"}").FontSize(7.5f);
                            });
                        });
                    });
                });
            });
            return doc.GeneratePdf();
        }

        private static byte[] GenerateQrCode(string text)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(text ?? "0", QRCodeGenerator.ECCLevel.M);
            var qrCode = new BitmapByteQRCode(qrData);
            return qrCode.GetGraphic(4);
        }

        public static void SaveToken(AccessCardData data, string outputPath)
        {
            var bytes = GenerateTokenPdf(data);
            File.WriteAllBytes(outputPath, bytes);
        }
    }
}