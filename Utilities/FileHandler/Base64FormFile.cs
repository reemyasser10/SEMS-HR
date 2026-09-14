using Microsoft.AspNetCore.Http;

namespace Utilities.FileHandler
{
    public class Base64FormFile : IFormFile
    {
        private readonly MemoryStream _stream;
        public string ContentType { get; }
        public string FileName { get; }
        public long Length => _stream.Length;
        public string Name { get; }
        public IHeaderDictionary Headers => new HeaderDictionary();
        public string ContentDisposition => $"form-data; name=\"{Name}\"; filename=\"{FileName}\"";

        public Base64FormFile(string base64String, string name, string contentType, string fileName)
        {
            var data = Convert.FromBase64String(base64String);
            _stream = new MemoryStream(data);
            Name = name;
            ContentType = contentType;
            FileName = fileName;
        }

        public Stream OpenReadStream() => _stream;
        public void CopyTo(Stream target) => _stream.CopyTo(target);
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => _stream.CopyToAsync(target, cancellationToken);
    }
}
