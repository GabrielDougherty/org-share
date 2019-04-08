using Microsoft.EntityFrameworkCore.Design;
using System.ComponentModel.DataAnnotations;

namespace Backend.Db
{
    public class OrgFile
    {
        [Key]
        public int Id { get; set; }
        public byte[] FileData { get; set; }
    }
}