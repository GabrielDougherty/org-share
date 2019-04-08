using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgFilesController : ControllerBase
    {
        // GET api/orgFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<byte[]>>> Get()
        {
            using (var cxt = new OrgFilesContext()) {
                return await cxt.OrgFiles
                .Select(f => f.FileData)
                .ToArrayAsync();
            }
        }

        // GET api/orgFiles/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/orgFiles
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IFormFile file)
        {
            // using (var context = new OrgFilesContext()) {
            //     var uploadFile = new OrgFile {FileData = file};
            //     context.OrgFiles.Add(uploadFile);
            //     await context.SaveChangesAsync();
            //     return Ok(uploadFile.Id);
            // }
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
