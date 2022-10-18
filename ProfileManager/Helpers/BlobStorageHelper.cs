using Azure.Storage.Blobs;

namespace ProfileManager.Helpers
{
    public class BlobStorageHelper
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public BlobStorageHelper(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Azure:Storage:ConnectionString");
            var containerName = configuration.GetValue<string>("Azure:Storage:Container");

            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
            _containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        }

        public async Task<string> UploadProfileAsync(string blobName, Stream content)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content,overwrite: true);
            await blobClient.SetMetadataAsync(new Dictionary<string, string>
            {
                { "Owner", "Sonu"},
                { "Application", "Profile Manager" }
            });
            return blobClient.Uri.AbsoluteUri;
        }
    }
}
