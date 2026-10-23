using System.Globalization;
using Travel.Web.Entities.Tour;

namespace Travel.Web.Helpers
{
    /// <summary>
    /// MongoDB'de çok dilli saklanan tur içeriklerini aktif kültüre göre seçer.
    /// İngilizce alan boşsa Türkçe içerik gösterilir.
    /// </summary>
    public static class TourLocalizer
    {
        public static bool IsEnglish =>
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "en";

        public static Tour Localize(this Tour tour)
        {
            if (!IsEnglish || tour == null)
                return tour!;

            if (!string.IsNullOrWhiteSpace(tour.NameEn))
                tour.Name = tour.NameEn;

            if (!string.IsNullOrWhiteSpace(tour.ShortDescriptionEn))
                tour.ShortDescription = tour.ShortDescriptionEn;

            if (!string.IsNullOrWhiteSpace(tour.DescriptionEn))
                tour.Description = tour.DescriptionEn;

            return tour;
        }

        public static List<Tour> Localize(this List<Tour> tours)
        {
            tours.ForEach(x => x.Localize());
            return tours;
        }
    }
}
