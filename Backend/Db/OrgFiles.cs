using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Backend.Db
{
    public class OrgFile
    {
        [Key]
        public int Id { get; set; }
        public byte[] FileData { get; set; }
    }
}