using Microsoft.Azure.Cosmos;
using ProfileManager.Models;

namespace ProfileManager.Helpers
{
    public class CosmosHelper
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public CosmosHelper(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Azure:Cosmos:ConnectionString");
            _client = new CosmosClient(connectionString);
            _container = GetContainer(configuration).Result;
        }

        private async Task<Container> GetContainer(IConfiguration configuration)
        {
            var dbName = configuration.GetValue<string>("Azure:Cosmos:Database");
            var containerName = configuration.GetValue<string>("Azure:Cosmos:Container");
            var partitionKey = configuration.GetValue<string>("Azure:Cosmos:PartitionKey");

            var database = _client.GetDatabase(dbName);
            try
            {
                var response = await database.CreateContainerAsync(containerName, partitionKey);
                return response.Container;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return database.GetContainer(containerName); //Container exists, returns the reference of existing container
            }   
  
        }

        public async Task<UserProfile> InsertProfileAsync(UserProfile item)
        {
            var response = await _container.CreateItemAsync(item);
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return response.Resource;
            }
            else
            {
                throw new Exception("Failed to insert the item");
            }
        }

        public List<UserProfile> GetAllProfiles()
        {
            var result = _container.GetItemLinqQueryable<UserProfile>(allowSynchronousQueryExecution: true);
            return result.ToList();
        }
    }
}
