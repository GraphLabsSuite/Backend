using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;

namespace GraphLabs.Backend.Api.Infrastructure
{
    public class ModuleStore : IDisposable
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        
        public ModuleStore(IHostingEnvironment hostingEnvironment, IContentTypeProvider contentTypeProvider)
        {
            _hostingEnvironment = hostingEnvironment;
            _contentTypeProvider = contentTypeProvider;
        }
        
        private string GetModulePath(int key)
            => Path.Combine("modules", key.ToString(CultureInfo.InvariantCulture));
        
        public void Upload(int key, Stream module)
        {
            _lock.EnterWriteLock();

            try
            {
                var targetPath = Path.Combine(
                    _hostingEnvironment.WebRootPath,
                    GetModulePath(key));

                var targetDirectory = new DirectoryInfo(targetPath);
                if (targetDirectory.Exists)
                    targetDirectory.Delete(true);

                using (var archive = new ZipArchive(module, ZipArchiveMode.Read))
                {
                    archive.ExtractToDirectory(targetPath);
                }

                var buildDirectory = new DirectoryInfo(Path.Combine(targetPath, "build"));
                var tempPath = targetPath.TrimEnd('/', '\\') + Guid.NewGuid().ToString("N");
                if (buildDirectory.Exists && targetDirectory.GetDirectories().Length == 1)
                {
                    buildDirectory.MoveTo(tempPath);
                    targetDirectory.Delete(true);
                    Directory.Move(tempPath, targetPath);
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }

        }
        
        public (Stream file, string contentType) Download(int key, string path)
        {
            _lock.EnterReadLock();
            
            try
            {
                path = path
                    .TrimStart('/')
                    .Replace('/', Path.DirectorySeparatorChar)
                    .Replace('\\', Path.DirectorySeparatorChar);

                var targetPath = Path.Combine(
                    GetModulePath(key),
                    path);

                var file = _hostingEnvironment.WebRootFileProvider.GetFileInfo(targetPath);

                if (file.Exists
                    && !file.IsDirectory
                    && _contentTypeProvider.TryGetContentType(targetPath, out var contentType))
                {
                    return (file.CreateReadStream(), contentType);
                }
                else
                {
                    return (null, null);
                }
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Dispose()
        {
            _lock?.Dispose();
        }
    }
}