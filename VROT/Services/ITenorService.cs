namespace VROT.Services
{
    public interface ITenorService
    {
        Task<string?> GetRandomGifUrlAsync(string search);
    }
}
