namespace VROT.Services
{
    internal interface ITenorService
    {
        Task<string?> GetRandomGifUrlAsync(string search);
    }
}
