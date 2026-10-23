using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private static readonly string[] Headers =
        {
            "Ad Soyad", "E-posta", "Telefon", "Tur", "Tur Tarihi", "Yetişkin",
            "Çocuk", "Toplam Kişi", "Rezervasyon Tarihi", "Toplam Ücret", "Durum"
        };

        public static string StatusText(string status) => status switch
        {
            "Approved" => "Onaylandı",
            "Cancelled" => "İptal Edildi",
            _ => "Bekliyor"
        };

        public byte[] CreateParticipantExcel(string title, List<ReservationListDto> rows)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Katılımcılar");

            ws.Cell(1, 1).Value = title;
            ws.Range(1, 1, 1, Headers.Length).Merge().Style
                .Font.SetBold().Font.SetFontSize(14);

            ws.Cell(2, 1).Value = $"Oluşturulma: {DateTime.Now:dd.MM.yyyy HH:mm}";
            ws.Range(2, 1, 2, Headers.Length).Merge().Style.Font.SetItalic();

            for (var i = 0; i < Headers.Length; i++)
                ws.Cell(4, i + 1).Value = Headers[i];

            var headerRange = ws.Range(4, 1, 4, Headers.Length);
            headerRange.Style.Font.SetBold()
                .Font.SetFontColor(XLColor.White)
                .Fill.SetBackgroundColor(XLColor.FromHtml("#0e2a3b"));

            var row = 5;
            foreach (var r in rows)
            {
                ws.Cell(row, 1).Value = r.FullName;
                ws.Cell(row, 2).Value = r.Email;
                ws.Cell(row, 3).Value = r.Phone;
                ws.Cell(row, 4).Value = r.TourName;
                ws.Cell(row, 5).Value = r.TourStartDate;
                ws.Cell(row, 5).Style.DateFormat.Format = "dd.MM.yyyy";
                ws.Cell(row, 6).Value = r.AdultCount;
                ws.Cell(row, 7).Value = r.ChildCount;
                ws.Cell(row, 8).Value = r.TotalPerson;
                ws.Cell(row, 9).Value = r.CreatedAt?.ToLocalTime();
                ws.Cell(row, 9).Style.DateFormat.Format = "dd.MM.yyyy HH:mm";
                ws.Cell(row, 10).Value = r.TotalPrice;
                ws.Cell(row, 10).Style.NumberFormat.Format = "#,##0 \"TL\"";
                ws.Cell(row, 11).Value = StatusText(r.Status);
                row++;
            }

            // Toplam satırı
            ws.Cell(row, 7).Value = "Toplam";
            ws.Cell(row, 8).Value = rows.Sum(x => x.TotalPerson);
            ws.Cell(row, 10).Value = rows.Sum(x => x.TotalPrice);
            ws.Cell(row, 10).Style.NumberFormat.Format = "#,##0 \"TL\"";
            ws.Range(row, 1, row, Headers.Length).Style.Font.SetBold();

            ws.Range(4, 1, row, Headers.Length).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
                .Border.SetInsideBorder(XLBorderStyleValues.Thin);
            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] CreateParticipantPdf(string title, List<ReservationListDto> rows)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(24);
                    page.DefaultTextStyle(x => x.FontSize(8));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Travelio · Tur Katılımcı Raporu").FontSize(9).FontColor(Colors.Teal.Darken2).SemiBold();
                        col.Item().Text(title).FontSize(15).Bold().FontColor("#0e2a3b");
                        col.Item().Text($"Oluşturulma: {DateTime.Now:dd.MM.yyyy HH:mm} · Kayıt: {rows.Count}").FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingVertical(6).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                    });

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(2.2f); // Ad Soyad
                            c.RelativeColumn(2.6f); // E-posta
                            c.RelativeColumn(1.8f); // Telefon
                            c.RelativeColumn(2.6f); // Tur
                            c.RelativeColumn(1.4f); // Tur Tarihi
                            c.RelativeColumn(0.9f); // Yetişkin
                            c.RelativeColumn(0.8f); // Çocuk
                            c.RelativeColumn(0.9f); // Toplam
                            c.RelativeColumn(1.7f); // Rez. Tarihi
                            c.RelativeColumn(1.5f); // Ücret
                            c.RelativeColumn(1.3f); // Durum
                        });

                        table.Header(h =>
                        {
                            foreach (var header in Headers)
                            {
                                h.Cell().Background("#0e2a3b").Padding(4)
                                    .Text(header).FontColor(Colors.White).SemiBold();
                            }
                        });

                        var index = 0;
                        foreach (var r in rows)
                        {
                            var bg = index++ % 2 == 0 ? "#ffffff" : "#f7f5f0";

                            void Cell(string text) =>
                                table.Cell().Background(bg).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(4).Text(text);

                            Cell(r.FullName);
                            Cell(r.Email);
                            Cell(r.Phone);
                            Cell(r.TourName);
                            Cell(r.TourStartDate.ToString("dd.MM.yyyy"));
                            Cell(r.AdultCount.ToString());
                            Cell(r.ChildCount.ToString());
                            Cell(r.TotalPerson.ToString());
                            Cell(r.CreatedAt?.ToLocalTime().ToString("dd.MM.yyyy HH:mm") ?? "-");
                            Cell($"{r.TotalPrice:N0} TL");
                            Cell(StatusText(r.Status));
                        }
                    });

                    page.Footer().Row(row =>
                    {
                        row.RelativeItem().Text(
                            $"Toplam kişi: {rows.Sum(x => x.TotalPerson)} · Toplam ücret: {rows.Sum(x => x.TotalPrice):N0} TL")
                            .SemiBold();

                        row.ConstantItem(80).AlignRight().Text(t =>
                        {
                            t.CurrentPageNumber();
                            t.Span(" / ");
                            t.TotalPages();
                        });
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}
