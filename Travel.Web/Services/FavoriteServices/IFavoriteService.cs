namespace Travel.Web.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        /// <summary>Favoriye ekler ya da çıkarır. Sonuç: favoride mi?</summary>
        Task<bool> ToggleAsync(string userId, string tourId);

        Task<bool> IsFavoriteAsync(string userId, string tourId);

        Task<List<string>> GetTourIdsByUserAsync(string userId);
    }
}
