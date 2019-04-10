using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrgFilesController : ControllerBase
    {
        private readonly IOrgFilesRepository _orgRepository;

        public OrgFilesController(IOrgFilesRepository orgRepository)
        {
            _orgRepository = orgRepository;
        }

        // This should really just be for debug... don't want to be layer 7 DOSed
        // GET api/orgFiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<byte[]>>> Get()
        {
            using (var cxt = new OrgFilesContext())
            {
                return await cxt.OrgFiles
                .Select(f => f.FileData)
                .ToArrayAsync();
            }
        }

        // GET api/orgFiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            // await _orgRepository.GetOrgFileById(id);
            return "test";
        }

        // POST api/orgFiles
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] List<IFormFile> files)
        {
            if (files.Count > 1 || files.Count == 0)
            {
                return BadRequest();
            }

            var file = files[0];

            Console.WriteLine(file.ContentType);

            if (file.Length == 0)
            {
                return BadRequest();
            }

            byte[] fileBytes;

            // convert to bytes
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                fileBytes = ms.ToArray();
            }

            using (var context = new OrgFilesContext())
            {
                var uploadFile = new OrgFile {FileData = fileBytes};
                context.OrgFiles.Add(uploadFile);
                await context.SaveChangesAsync();
                return Ok(uploadFile.Id);
            }
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
