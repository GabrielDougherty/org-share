using Backend.Db;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Backend.Db
{
    public class OrgFilesRepository : IOrgFilesRepository
    {
        private readonly OrgFilesContext _orgContext;

        public OrgFilesRepository(OrgFilesContext context)
        {
            _orgContext = context;
        }

        public async Task<OrgFile> GetOrgFileById(int id)
        {
            var file =  _orgContext.OrgFiles
            .Where(f => f.Id == id).FirstOrDefaultAsync();

            return await file;
        }
    }
}