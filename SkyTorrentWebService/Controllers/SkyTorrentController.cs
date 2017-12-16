using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SkyTorrentWebService.Helpers;
using SkyTorrentWebService.Models;

namespace SkyTorrentWebService.Controllers
{
    [Route("api/[controller]")]
    public class SkyTorrentController : Controller
    {

        private readonly SkyTorrentSettings _skyTorrentSettings;

        public SkyTorrentController(IOptions<SkyTorrentSettings> skyTorrentSettings)
        {
            _skyTorrentSettings = skyTorrentSettings.Value;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }



        // GET api/values/5
        [HttpGet("{searchEntry}")]
        public string Get(string searchEntry)
        {

            var manager = new SkyTorrentProcessor();

            var result = manager.Process(_skyTorrentSettings.Url, searchEntry);

            return result;

        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
