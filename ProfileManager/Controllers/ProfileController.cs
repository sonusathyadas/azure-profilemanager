using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProfileManager.Helpers;
using ProfileManager.Models;
using ProfileManager.Models.ViewModels;

namespace ProfileManager.Controllers
{
    public class ProfileController : Controller
    {
        private readonly BlobStorageHelper _blobHelper;
        private readonly CosmosHelper _cosmosHelper;
        private readonly IDistributedCache _cache;
        public ProfileController(BlobStorageHelper blobHelper, CosmosHelper cosmosHelper, IDistributedCache cache)
        {
            _blobHelper = blobHelper;
            _cosmosHelper = cosmosHelper;
            _cache = cache;
        }

        public IActionResult Index()
        {
            List<UserProfile> profiles;
            var cachedData = _cache.GetString("Profiles");
            if (string.IsNullOrEmpty(cachedData))
            {
                profiles = _cosmosHelper.GetAllProfiles();
                _cache.SetString("Profiles", JsonConvert.SerializeObject(profiles));
                ViewBag.Message = "Loaded from database";
            }
            else
            {
                profiles = JsonConvert.DeserializeObject<List<UserProfile>>(cachedData);
                ViewBag.Message = "Loaded from cache";
            }

            return View(profiles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Do file upload here
                string? fileName = model.Profile?.FileName;
                var content = model.Profile?.OpenReadStream();
                if (fileName != null && content != null)
                {
                    var blobUri = await _blobHelper.UploadProfileAsync(fileName, content);
                    //Store data to CosmosDB
                    UserProfile doc = new UserProfile()
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        Department = model.Department,
                        Designation = model.Designation,
                        JoinDate = model.JoinDate,
                        ProfileUrl = blobUri
                    };
                    await _cosmosHelper.InsertProfileAsync(doc);

                    return RedirectToAction("Index");
                }else{
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
