using System;
using System.Globalization;
using System.IO;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace GraphLabs.Backend.Api.Controllers
{
    public class TaskModulesDownloadController : ODataController
    {
        private readonly IHostingEnvironment _env;
        private readonly IContentTypeProvider _contentTypeProvider;
        
        public TaskModulesDownloadController( 
            IHostingEnvironment env,
            IContentTypeProvider contentTypeProvider)
        {
            _env = env;
            _contentTypeProvider = contentTypeProvider;
        }

        [HttpGet]
        [ODataRoute("TaskModules({key})/GraphLabs.Download(path={path})")]
        public IActionResult Download(int key, string path)
        {
            var targetPath = Path.Combine(
                "modules",
                key.ToString(CultureInfo.InvariantCulture),
                path);
            
            var file = _env.WebRootFileProvider.GetFileInfo(targetPath);

            if (file.Exists
                && !file.IsDirectory
                && _contentTypeProvider.TryGetContentType(targetPath, out var contentType))
            {
                return File(file.CreateReadStream(), contentType);
            }
            else
            {
                return NotFound();
            }
        }
    }
}