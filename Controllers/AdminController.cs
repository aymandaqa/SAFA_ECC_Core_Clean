using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SAFA_ECC_Core_Clean.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Helper to get UserName from Session (simulated)
        private string GetUserName()
        {
            // In a real application, this would come from HttpContext.Session or Identity
            // For now, we'll simulate it. You might need to set this in a login process.
            return HttpContext.Session.GetString("UserName") ?? "SimulatedUser";
        }

        // Helper to get ApplicationID (simulated)
        private int GetApplicationID()
        {
            // Simulate Application ID. Adjust as per your application's actual ID.
            return 1; 
        }

        // GET: Admin
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Accept_Reject_user_Permission(string id, string Status, string app, string page)
        {
            var jsonResult = new JsonResult(new { success = false, message = "" });
            int userId = 0;
            int appId = 0;
            int pageId = 0;

            if (!int.TryParse(id, out userId) || !int.TryParse(app, out appId) || !int.TryParse(page, out pageId))
            {
                jsonResult = new JsonResult(new { success = false, message = "Invalid input parameters." });
                return jsonResult;
            }

            try
            {
                _logger.LogInformation($"Start with Accept_Reject_user_Permission for UserID: {userId}, Status: {Status}");

                // Find the authorization record
                var userPermissionAuth = await _context.Users_Permissions_Auth
                    .SingleOrDefaultAsync(x => x.Application_ID == appId && x.UserID == userId && x.Page_Id == pageId);

                if (userPermissionAuth != null)
                {
                    // Find the actual permission record
                    var userPermission = await _context.Users_Permissions
                        .SingleOrDefaultAsync(x => x.Application_ID == appId && x.UserID == userId && x.Page_Id == pageId);

                    if (Status == "1") // Accept
                    {
                        if (userPermission == null)
                        {
                            // Create new permission if it doesn't exist
                            userPermission = new Users_Permissions
                            {
                                UserID = userPermissionAuth.UserID,
                                Application_ID = userPermissionAuth.Application_ID,
                                Page_Id = userPermissionAuth.Page_Id,
                                Add = userPermissionAuth.Add,
                                Reverse = userPermissionAuth.Reverse,
                                Post = userPermissionAuth.Post,
                                Delete = userPermissionAuth.Delete,
                                Update = userPermissionAuth.Update,
                                Access = userPermissionAuth.Access,
                                Value = userPermissionAuth.Value // Assuming 'Value' is the equivalent of 'Reverse' in VB for user permissions
                            };
                            _context.Users_Permissions.Add(userPermission);
                        }
                        else
                        {
                            // Update existing permission
                            userPermission.Add = userPermissionAuth.Add;
                            userPermission.Reverse = userPermissionAuth.Reverse;
                            userPermission.Post = userPermissionAuth.Post;
                            userPermission.Delete = userPermissionAuth.Delete;
                            userPermission.Update = userPermissionAuth.Update;
                            userPermission.Access = userPermissionAuth.Access;
                            userPermission.Value = userPermissionAuth.Value;
                        }
                        userPermissionAuth.status = "Accept";
                    }
                    else // Reject
                    {
                        userPermissionAuth.status = "Reject";
                    }

                    await _context.SaveChangesAsync();

                    // Log History
                    var history = new Users_Permissions_History
                    {
                        UserID = userPermissionAuth.UserID,
                        Application_ID = userPermissionAuth.Application_ID,
                        Page_Id = userPermissionAuth.Page_Id,
                        Add = userPermissionAuth.Add,
                        Reverse = userPermissionAuth.Reverse,
                        Post = userPermissionAuth.Post,
                        Delete = userPermissionAuth.Delete,
                        Update = userPermissionAuth.Update,
                        Access = userPermissionAuth.Access,
                        Value = userPermissionAuth.Value,
                        UserName = GetUserName(),
                        Updatedate = DateTime.Now
                    };
                    _context.Users_Permissions_History.Add(history);
                    await _context.SaveChangesAsync();

                    jsonResult = new JsonResult(new { success = true, message = "Operation successful." });
                }
                else
                {
                    jsonResult = new JsonResult(new { success = false, message = "User permission authorization record not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Accept_Reject_user_Permission for UserID: {userId}. Error: {ex.Message}");
                // In a real app, you'd integrate with your _LogSystem here
                jsonResult = new JsonResult(new { success = false, message = "An error occurred during the operation." });
            }
            return jsonResult;
        }

        [HttpPost]
        public async Task<JsonResult> Accept_Reject_Finanical_Site(string id, string Status)
        {
            var jsonResult = new JsonResult(new { success = false, message = "" });
            int financialSiteId = 0;

            if (!int.TryParse(id, out financialSiteId))
            {
                jsonResult = new JsonResult(new { success = false, message = "Invalid input parameter." });
                return jsonResult;
            }

            try
            {
                _logger.LogInformation($"Start with Accept_Reject_Finanical_Site for FinancialSiteID: {financialSiteId}, Status: {Status}");

                var financialSiteAuth = await _context.Financial_Sites_Auth
                    .SingleOrDefaultAsync(x => x.Financial_Site_ID == financialSiteId);

                if (financialSiteAuth != null)
                {
                    var financialSite = await _context.Financial_Sites
                        .SingleOrDefaultAsync(x => x.Financial_Site_ID == financialSiteId);

                    if (Status == "1") // Accept
                    {
                        if (financialSite == null)
                        {
                            financialSite = new Financial_Sites
                            {
                                Financial_Site_ID = financialSiteAuth.Financial_Site_ID,
                                Site_Name = financialSiteAuth.Site_Name,
                                // Copy other relevant properties from financialSiteAuth to financialSite
                            };
                            _context.Financial_Sites.Add(financialSite);
                        }
                        else
                        {
                            financialSite.Site_Name = financialSiteAuth.Site_Name;
                            // Update other relevant properties
                        }
                        financialSiteAuth.status = "Accept";
                    }
                    else // Reject
                    {
                        financialSiteAuth.status = "Reject";
                    }

                    await _context.SaveChangesAsync();

                    // Log History
                    var history = new Financial_Sites_History
                    {
                        Financial_Site_ID = financialSiteAuth.Financial_Site_ID,
                        Site_Name = financialSiteAuth.Site_Name,
                        UserName = GetUserName(),
                        Updatedate = DateTime.Now
                    };
                    _context.Financial_Sites_History.Add(history);
                    await _context.SaveChangesAsync();

                    jsonResult = new JsonResult(new { success = true, message = "Operation successful." });
                }
                else
                {
                    jsonResult = new JsonResult(new { success = false, message = "Financial site authorization record not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Accept_Reject_Finanical_Site for FinancialSiteID: {financialSiteId}. Error: {ex.Message}");
                jsonResult = new JsonResult(new { success = false, message = "An error occurred during the operation." });
            }
            return jsonResult;
        }

        [HttpPost]
        public async Task<JsonResult> Accept_Reject_Group_Permission(string id, string Status, string app, string page)
        {
            var jsonResult = new JsonResult(new { success = false, message = "" });
            int groupId = 0;
            int appId = 0;
            int pageId = 0;

            if (!int.TryParse(id, out groupId) || !int.TryParse(app, out appId) || !int.TryParse(page, out pageId))
            {
                jsonResult = new JsonResult(new { success = false, message = "Invalid input parameters." });
                return jsonResult;
            }

            try
            {
                _logger.LogInformation($"Start with Accept_Reject_Group_Permission for GroupID: {groupId}, Status: {Status}");

                var groupPermissionAuth = await _context.Groups_Permissions_Auth
                    .SingleOrDefaultAsync(x => x.Application_ID == appId && x.Group_Id == groupId && x.Page_Id == pageId);

                if (groupPermissionAuth != null)
                {
                    var groupPermission = await _context.Groups_Permissions
                        .SingleOrDefaultAsync(x => x.Application_ID == appId && x.Group_Id == groupId && x.Page_Id == pageId);

                    if (Status == "1") // Accept
                    {
                        if (groupPermission == null)
                        {
                            groupPermission = new Groups_Permissions
                            {
                                Group_Id = groupPermissionAuth.Group_Id,
                                Application_ID = groupPermissionAuth.Application_ID,
                                Page_Id = groupPermissionAuth.Page_Id,
                                Add = groupPermissionAuth.Add,
                                Reverse = groupPermissionAuth.Reverse,
                                Post = groupPermissionAuth.Post,
                                Delete = groupPermissionAuth.Delete,
                                Update = groupPermissionAuth.Update,
                                Access = groupPermissionAuth.Access,
                                // Assuming 'Reverse' in VB was meant for a boolean flag, mapping to 'Reverse' in C# model
                                Reverse = groupPermissionAuth.Reverse 
                            };
                            _context.Groups_Permissions.Add(groupPermission);
                        }
                        else
                        {
                            groupPermission.Add = groupPermissionAuth.Add;
                            groupPermission.Reverse = groupPermissionAuth.Reverse;
                            groupPermission.Post = groupPermissionAuth.Post;
                            groupPermission.Delete = groupPermissionAuth.Delete;
                            groupPermission.Update = groupPermissionAuth.Update;
                            groupPermission.Access = groupPermissionAuth.Access;
                            groupPermission.Reverse = groupPermissionAuth.Reverse;
                        }
                        groupPermissionAuth.status = "Accept";
                    }
                    else // Reject
                    {
                        groupPermissionAuth.status = "Reject";
                    }

                    await _context.SaveChangesAsync();

                    // Log History
                    var history = new Groups_Permissions_History
                    {
                        Group_Id = groupPermissionAuth.Group_Id,
                        Application_ID = groupPermissionAuth.Application_ID,
                        Page_Id = groupPermissionAuth.Page_Id,
                        Add = groupPermissionAuth.Add,
                        Reverse = groupPermissionAuth.Reverse,
                        Post = groupPermissionAuth.Post,
                        Delete = groupPermissionAuth.Delete,
                        Update = groupPermissionAuth.Update,
                        Access = groupPermissionAuth.Access,
                        Reverse = groupPermissionAuth.Reverse,
                        UserName = GetUserName(),
                        Updatedate = DateTime.Now
                    };
                    _context.Groups_Permissions_History.Add(history);
                    await _context.SaveChangesAsync();

                    jsonResult = new JsonResult(new { success = true, message = "Operation successful." });
                }
                else
                {
                    jsonResult = new JsonResult(new { success = false, message = "Group permission authorization record not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Accept_Reject_Group_Permission for GroupID: {groupId}. Error: {ex.Message}");
                jsonResult = new JsonResult(new { success = false, message = "An error occurred during the operation." });
            }
            return jsonResult;
        }
    }
}

