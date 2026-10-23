using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Services.ReportServices
{
    public interface IReportService
    {
        /// <summary>Katılımcı raporunu ClosedXML ile Excel olarak üretir (MemoryStream, diske yazılmaz).</summary>
        byte[] CreateParticipantExcel(string title, List<ReservationListDto> rows);

        /// <summary>Katılımcı raporunu QuestPDF ile PDF olarak üretir (MemoryStream, diske yazılmaz).</summary>
        byte[] CreateParticipantPdf(string title, List<ReservationListDto> rows);
    }
}
