using System.Threading.Tasks;

namespace Backend.Db
{
    public interface IOrgFilesRepository
    {
        Task<OrgFile> GetOrgFileById(int id);
    }
}