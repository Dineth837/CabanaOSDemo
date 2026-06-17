using System;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QRCoder;

namespace CabanaOSDemo.Utils
{
    public static class PdfInvoiceService
    {
        // ── Shared QR code generator ─────────────────────────────────────────────
        private static byte[] GenerateQrCode(string content)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            return qrCode.GetGraphic(6);
        }

        // ════════════════════════════════════════════════════════════════════════
        //  METHOD 1 — Restaurant Invoice
        // ════════════════════════════════════════════════════════════════════════
        public static void GenerateRestaurantInvoice(
            string filePath,
            string orderNumber,       // e.g. "STF87492004"
            string roomNumber,        // e.g. "ST C01"
            string[] orderLines,      // e.g. ["XXXXXXXXXXX", "XXXXXXXXXXX"]
            string invoiceDate,       // e.g. "2026.03.03"
            string totalDue)          // e.g. "Rs. xxx,xxx.xx"
        {
            QuestPDF.Settings.License = LicenseType.Community;
            byte[] qrBytes = GenerateQrCode(orderNumber);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(0);
                    page.DefaultTextStyle(x => x.FontFamily("Segoe UI"));

                    page.Content()
                        .Background("#F0F4F8")
                        .AlignMiddle()
                        .AlignCenter()
                        .Width(300)
                        .Column(card =>
                        {
                            card.Item()
                                .Background("#FFFFFF")
                                .Border(1).BorderColor("#E2E8F0")
                                .Column(inner =>
                                {
                                    // ── Title ───────────────────────────────
                                    inner.Item()
                                        .PaddingTop(24).PaddingBottom(16)
                                        .PaddingHorizontal(20)
                                        .Text("Cabana Invoice : Restaurant")
                                        .FontSize(18)
                                        .Bold()
                                        .FontColor("#1E293B");

                                    // ── Order Number + QR ───────────────────
                                    inner.Item()
                                        .PaddingHorizontal(20)
                                        .PaddingBottom(6)
                                        .Row(row =>
                                        {
                                            row.RelativeItem().Column(col =>
                                            {
                                                col.Item()
                                                    .Text("Order Number")
                                                    .FontSize(13).Bold()
                                                    .FontColor("#1E293B");
                                                col.Item().Height(4);
                                                col.Item()
                                                    .Text(orderNumber)
                                                    .FontSize(11)
                                                    .FontColor("#64748B");

                                                col.Item().Height(12);

                                                col.Item()
                                                    .Text("Room Number")
                                                    .FontSize(13).Bold()
                                                    .FontColor("#1E293B");
                                                col.Item().Height(4);
                                                col.Item()
                                                    .Text(roomNumber)
                                                    .FontSize(11)
                                                    .FontColor("#64748B");
                                            });

                                            // QR Code
                                            row.ConstantItem(90)
                                                .AlignRight()
                                                .AlignTop()
                                                .Image(qrBytes)
                                                .FitWidth();
                                        });

                                    // ── Invoice section ──────────────────────
                                    inner.Item()
                                        .PaddingTop(16)
                                        .PaddingHorizontal(20)
                                        .PaddingBottom(0)
                                        .Background("#F8FAFC")
                                        .Column(invoice =>
                                        {
                                            invoice.Item()
                                                .PaddingTop(12).PaddingBottom(8)
                                                .PaddingHorizontal(4)
                                                .Text("Invoice")
                                                .FontSize(12).Bold()
                                                .FontColor("#1E293B");

                                            // Order lines
                                            for (int i = 0; i < orderLines.Length; i++)
                                            {
                                                invoice.Item()
                                                    .PaddingHorizontal(4)
                                                    .PaddingBottom(6)
                                                    .Row(row =>
                                                    {
                                                        row.ConstantItem(90)
                                                            .Text($"Order  No  {(i + 1):D2}")
                                                            .FontSize(10)
                                                            .FontColor("#475569");
                                                        row.RelativeItem()
                                                            .Text(orderLines[i])
                                                            .FontSize(10)
                                                            .FontColor("#475569");
                                                    });
                                            }

                                            // Date
                                            invoice.Item()
                                                .PaddingHorizontal(4)
                                                .PaddingBottom(12)
                                                .Text($"Date :  {invoiceDate}")
                                                .FontSize(10)
                                                .FontColor("#475569");
                                        });

                                    // ── Total Due ────────────────────────────
                                    inner.Item()
                                        .BorderTop(1).BorderColor("#E2E8F0")
                                        .PaddingHorizontal(20)
                                        .PaddingVertical(14)
                                        .Row(row =>
                                        {
                                            row.RelativeItem()
                                                .Text("Total Statement Due")
                                                .FontSize(11).Bold()
                                                .FontColor("#1E293B");
                                            row.AutoItem()
                                                .Text(totalDue)
                                                .FontSize(11).Bold()
                                                .FontColor("#1E293B");
                                        });

                                    // ── Bottom padding ───────────────────────
                                    inner.Item().Height(30);
                                });
                        });
                });
            })
            .GeneratePdf(filePath);
        }

        // ════════════════════════════════════════════════════════════════════════
        //  METHOD 2 — Suites Invoice
        // ════════════════════════════════════════════════════════════════════════
        public static void GenerateSuitesInvoice(
            string filePath,
            string bookingNumber,     // e.g. "STF87492004"
            string roomNumber,        // e.g. "Fam ST07"
            string suiteCharge,       // e.g. "Rs. XXX,XXX.XX"
            string exCharge,          // e.g. "Rs. XXX,XXX.XX"
            string invoiceDate,       // e.g. "2026.03.03"
            string totalCharge)       // e.g. "Rs. xxx,xxx.xx"
        {
            QuestPDF.Settings.License = LicenseType.Community;
            byte[] qrBytes = GenerateQrCode(bookingNumber);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(0);
                    page.DefaultTextStyle(x => x.FontFamily("Segoe UI"));

                    page.Content()
                        .Background("#F0F4F8")
                        .AlignMiddle()
                        .AlignCenter()
                        .Width(300)
                        .Column(card =>
                        {
                            card.Item()
                                .Background("#FFFFFF")
                                .Border(1).BorderColor("#E2E8F0")
                                .Column(inner =>
                                {
                                    // ── Title ───────────────────────────────
                                    inner.Item()
                                        .PaddingTop(24).PaddingBottom(16)
                                        .PaddingHorizontal(20)
                                        .Text("Cabana Invoice : Suites")
                                        .FontSize(18)
                                        .Bold()
                                        .FontColor("#1E293B");

                                    // ── Booking Number + QR ─────────────────
                                    inner.Item()
                                        .PaddingHorizontal(20)
                                        .PaddingBottom(6)
                                        .Row(row =>
                                        {
                                            row.RelativeItem().Column(col =>
                                            {
                                                col.Item()
                                                    .Text("Booking Number")
                                                    .FontSize(13).Bold()
                                                    .FontColor("#1E293B");
                                                col.Item().Height(4);
                                                col.Item()
                                                    .Text(bookingNumber)
                                                    .FontSize(11)
                                                    .FontColor("#64748B");

                                                col.Item().Height(12);

                                                col.Item()
                                                    .Text("Room Number")
                                                    .FontSize(13).Bold()
                                                    .FontColor("#1E293B");
                                                col.Item().Height(4);
                                                col.Item()
                                                    .Text(roomNumber)
                                                    .FontSize(11)
                                                    .FontColor("#64748B");
                                            });

                                            // QR Code
                                            row.ConstantItem(90)
                                                .AlignRight()
                                                .AlignTop()
                                                .Image(qrBytes)
                                                .FitWidth();
                                        });

                                    // ── Invoice section ──────────────────────
                                    inner.Item()
                                        .PaddingTop(16)
                                        .PaddingHorizontal(20)
                                        .Background("#F8FAFC")
                                        .Column(invoice =>
                                        {
                                            invoice.Item()
                                                .PaddingTop(12).PaddingBottom(8)
                                                .PaddingHorizontal(4)
                                                .Text("Invoice")
                                                .FontSize(12).Bold()
                                                .FontColor("#1E293B");

                                            // Suite charge
                                            invoice.Item()
                                                .PaddingHorizontal(4)
                                                .PaddingBottom(6)
                                                .Row(row =>
                                                {
                                                    row.ConstantItem(90)
                                                        .Text("Suite  charge")
                                                        .FontSize(10)
                                                        .FontColor("#475569");
                                                    row.RelativeItem()
                                                        .Text(suiteCharge)
                                                        .FontSize(10)
                                                        .FontColor("#475569");
                                                });

                                            // EX charge
                                            invoice.Item()
                                                .PaddingHorizontal(4)
                                                .PaddingBottom(6)
                                                .Row(row =>
                                                {
                                                    row.ConstantItem(90)
                                                        .Text("EX  charge")
                                                        .FontSize(10)
                                                        .FontColor("#475569");
                                                    row.RelativeItem()
                                                        .Text(exCharge)
                                                        .FontSize(10)
                                                        .FontColor("#475569");
                                                });

                                            // Date
                                            invoice.Item()
                                                .PaddingHorizontal(4)
                                                .PaddingBottom(12)
                                                .Text($"Date :  {invoiceDate}")
                                                .FontSize(10)
                                                .FontColor("#475569");
                                        });

                                    // ── Total Charge ─────────────────────────
                                    inner.Item()
                                        .BorderTop(1).BorderColor("#E2E8F0")
                                        .PaddingHorizontal(20)
                                        .PaddingVertical(14)
                                        .Row(row =>
                                        {
                                            row.RelativeItem()
                                                .Text("Total Statement Charge")
                                                .FontSize(11).Bold()
                                                .FontColor("#1E293B");
                                            row.AutoItem()
                                                .Text(totalCharge)
                                                .FontSize(11).Bold()
                                                .FontColor("#1E293B");
                                        });

                                    // ── Bottom padding ───────────────────────
                                    inner.Item().Height(30);
                                });
                        });
                });
            })
            .GeneratePdf(filePath);
        }
    }
}