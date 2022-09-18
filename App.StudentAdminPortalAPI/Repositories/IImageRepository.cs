using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace App.StudentAdminPortalAPI.Repositories
{
    public interface IImageRepository
    {
        Task<string> Upload(IFormFile file, string fileName);
    }
}