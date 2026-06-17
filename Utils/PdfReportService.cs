using System;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CabanaOSDemo.Utils
{
    public static class PdfReportService
    {
        public static void GenerateMonthlyRevenueReport(
            string filePath,
            string suiteRevenue,
            string restaurantRevenue,
            string totalRevenue)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.DefaultTextStyle(x => x.FontFamily("Segoe UI"));

                    page.Content().Background("#F0F4F8").AlignMiddle().AlignCenter()
                        .Width(320)
                        .Column(card =>
                        {
                            // ── White card ──────────────────────────────────────
                            card.Item().Background("#FFFFFF")
                                .Border(1).BorderColor("#E2E8F0")
                                .Column(inner =>
                                {
                                    // ── Title ───────────────────────────────────
                                    inner.Item()
                                        .PaddingTop(28).PaddingBottom(12)
                                        .PaddingHorizontal(24)
                                        .AlignCenter()
                                        .Text("Monthly Revenue Report")
                                        .FontSize(17)
                                        .Bold()
                                        .FontColor("#1E293B");

                                    // ── Divider ─────────────────────────────────
                                    inner.Item()
                                        .PaddingHorizontal(16)
                                        .BorderBottom(1)
                                        .BorderColor("#E2E8F0")
                                        .Height(1);

                                    // ── Revenue rows ────────────────────────────
                                    inner.Item()
                                        .PaddingHorizontal(16)
                                        .PaddingTop(24)
                                        .PaddingBottom(8)
                                        .Column(rows =>
                                        {
                                            RevenueRow(rows, "Suite Revenue :", suiteRevenue, highlighted: false);
                                            rows.Item().Height(10); // spacing
                                            RevenueRow(rows, "Restaurant Revenue :", restaurantRevenue, highlighted: false);
                                        });

                                    // ── Divider before total ─────────────────────
                                    inner.Item()
                                        .PaddingHorizontal(16)
                                        .BorderBottom(1)
                                        .BorderColor("#E2E8F0")
                                        .Height(1);

                                    // ── Total row ───────────────────────────────
                                    inner.Item()
                                        .PaddingHorizontal(16)
                                        .PaddingTop(8)
                                        .PaddingBottom(24)
                                        .Column(rows =>
                                        {
                                            RevenueRow(rows, "Total Revenue :", totalRevenue, highlighted: true);
                                        });

                                    // ── CabanaOS branding footer ─────────────────
                                    inner.Item()
                                        .Background("#EBF5FB")
                                        .BorderTop(1)
                                        .BorderColor("#D1E8F0")
                                        .PaddingVertical(22)
                                        .AlignCenter()
                                        .Text("CabanaOS")
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor("#1E293B");
                                });
                        });
                });
            })
            .GeneratePdf(filePath);
        }



        // Add this method to your PdfReportService class
        public static void GenerateDailyTransactionsReport(string filePath, List<(string Item, string Amount)> transactions, string grandTotal)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0);
                    page.DefaultTextStyle(x => x.FontFamily("Segoe UI"));

                    page.Content().Background("#F0F4F8").AlignMiddle().AlignCenter()
                        .Width(350) // Slightly wider for table content
                        .Column(card =>
                        {
                            card.Item().Background("#FFFFFF")
                                .Border(1).BorderColor("#E2E8F0")
                                .Column(inner =>
                                {
                                    // ── Title ───────────────────────────────────
                                    inner.Item().Padding(24).AlignCenter()
                                        .Text("Daily Transactions").FontSize(17).Bold().FontColor("#1E293B");

                                    // ── Transaction List ────────────────────────
                                    inner.Item().PaddingHorizontal(16).Column(rows =>
                                    {
                                        foreach (var transaction in transactions)
                                        {
                                            RevenueRow(rows, transaction.Item, transaction.Amount, highlighted: false);
                                            rows.Item().Height(4);
                                        }
                                    });

                                    // ── Divider ─────────────────────────────────
                                    inner.Item().Padding(16).BorderBottom(1).BorderColor("#E2E8F0");

                                    // ── Total Row ───────────────────────────────
                                    inner.Item().PaddingHorizontal(16).PaddingBottom(24)
                                        .Column(rows => RevenueRow(rows, "Total Daily :", grandTotal, highlighted: true));

                                    // ── Footer ──────────────────────────────────
                                    inner.Item().Background("#EBF5FB").BorderTop(1).BorderColor("#D1E8F0")
                                        .PaddingVertical(22).AlignCenter()
                                        .Text("CabanaOS").FontSize(20).Bold().FontColor("#1E293B");
                                });
                        });
                });
            })
            .GeneratePdf(filePath);
        }


        public static void GenerateCustomerReport(string filePath, List<(string Id, string Name, string NIC)> customers)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20); // Slightly more margin for the table
                    page.DefaultTextStyle(x => x.FontFamily("Segoe UI"));

                    page.Content().Background("#F0F4F8").AlignMiddle().AlignCenter()
                        .Width(500) // Increased width to fit 3 columns
                        .Column(card =>
                        {
                            card.Item().Background("#FFFFFF").Border(1).BorderColor("#E2E8F0")
                                .Column(inner =>
                                {
                                    // ── Title ───────────────────────────────────
                                    inner.Item().Padding(24).AlignCenter()
                                        .Text("Customer Report").FontSize(17).Bold().FontColor("#1E293B");

                                    // ── Customer Table ──────────────────────────
                                    inner.Item().PaddingHorizontal(16).PaddingBottom(24).Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(60);  // ID
                                            columns.RelativeColumn();    // Name
                                            columns.RelativeColumn();    // NIC
                                        });

                                        // Header
                                        table.Header(header =>
                                        {
                                            header.Cell().Text("ID").Bold();
                                            header.Cell().Text("Name").Bold();
                                            header.Cell().Text("NIC").Bold();
                                        });

                                        // Rows
                                        foreach (var customer in customers)
                                        {
                                            table.Cell().Text(customer.Id);
                                            table.Cell().Text(customer.Name);
                                            table.Cell().Text(customer.NIC);
                                        }
                                    });

                                    // ── Footer ──────────────────────────────────
                                    inner.Item().Background("#EBF5FB").BorderTop(1).BorderColor("#D1E8F0")
                                        .PaddingVertical(22).AlignCenter()
                                        .Text("CabanaOS").FontSize(20).Bold().FontColor("#1E293B");
                                });
                        });
                });
            })
            .GeneratePdf(filePath);
        }





        // ── Reusable row builder ─────────────────────────────────────────────────
        private static void RevenueRow(ColumnDescriptor rows, string label, string value, bool highlighted)
        {
            rows.Item()
                .Background(highlighted ? "#EFF6FF" : "#F8FAFC")
                .Padding(10)
                .Row(row =>
                {
                    row.RelativeItem()
                        .Text(label)
                        .FontSize(11)
                        .Bold()
                        .FontColor("#1E293B");

                    row.AutoItem()
                        .Text(value)
                        .FontSize(11)
                        .Bold()
                        .FontColor("#94A3B8");
                });
        }
    }
}