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
using Backend.Utils;

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
        public async Task<ActionResult<byte[]>> Get(int id)
        {
            var file = await _orgRepository.GetOrgFileById(id);
            string name = $"Result-{file.Id}.html";
            return File(file.FileData,"text/html");
            // return "test";
        }

        // POST api/orgFiles
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] List<IFormFile> files)
        {
            // Investigate just taking a single IFormFile as argument instead
            if (files.Count > 1 || files.Count == 0)
            {
                return BadRequest();
            }

            var inputFile = files[0];

            Console.WriteLine(inputFile.ContentType);

            if (inputFile.Length == 0)
            {
                return BadRequest();
            }

            string tmpInputPath = Path.GetTempFileName();
            string tmpOutputPath = Path.GetTempFileName();

            using (FileStream tmpInputFile = System.IO.File.Create(tmpInputPath))       
            //using (var htmlMs = new MemoryStream())
            {
                await inputFile.CopyToAsync(tmpInputFile);
            }
            string panCmd = $"pandoc -s -f org -t html -o {tmpOutputPath} {tmpInputPath}";
            panCmd.RunCommand();

            
            byte[] htmlFileBytes;

            // convert to in-memory bytes
            using (FileStream tmpOutputFile = System.IO.File.Open(tmpOutputPath, FileMode.Open))
            using (var ms = new MemoryStream())
            {
                await tmpOutputFile.CopyToAsync(ms);
                htmlFileBytes = ms.ToArray();
            }

            using (var context = new OrgFilesContext())
            {
                var uploadFile = new OrgFile {FileData = htmlFileBytes};
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
