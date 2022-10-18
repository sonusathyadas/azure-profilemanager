using Newtonsoft.Json;

namespace ProfileManager.Models
{
    public class UserProfile
    {
        [JsonProperty(PropertyName = "id")]
        public string? Id { get; set; }

        [JsonProperty(PropertyName = "firstName")]
        public string? FirstName { get; set; }

        [JsonProperty(PropertyName = "lastName")]
        public string? LastName { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string? Email { get; set; }

        [JsonProperty(PropertyName = "department")]
        public string? Department { get; set; }

        [JsonProperty(PropertyName = "designation")]
        public string? Designation { get; set; }

        [JsonProperty(PropertyName = "joinDate")]
        public DateTime JoinDate { get; set; }

        [JsonProperty(PropertyName = "profileUrl")]
        public string? ProfileUrl { get; set; }
    }
}
