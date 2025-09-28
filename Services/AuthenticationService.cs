using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.AuthenticationViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.SqlClient; // Added for SqlConnection and SqlDataAdapter
using System.Text; // Added for StringBuilder

namespace SAFA_ECC_Core_Clean.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;

        private readonly int _applicationID = 1; // Example Application ID
        private string _loggMessage = "";
        private string userName;
        private int userId;

        public AuthenticationService(ApplicationDbContext context, ILogger<AuthenticationService> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null)
        {
            // Implementation will go here, based on VB.NET logic
            return flatObjects.Where(x => x.Parent_ID == parentId)
                              .Select(item => new TreeNode
                              {
                                  SubMenu_Name_EN = item.SubMenu_Name_EN,
                                  SubMenu_ID = item.SubMenu_ID,
                                  Related_Page_ID = item.Related_Page_ID,
                                  Children = FillRecursive(flatObjects, item.SubMenu_ID)
                              }).ToList();
        }

        public async Task<UserPagePermissionsResultViewModel> getuser_group_permision(string pageid, string applicationid, string userid, string userName, int userId, string groupId)
        {
            _logger.LogInformation($"Executing getuser_group_permision for user: {userName}, pageid: {pageid}");
            this.userName = userName;
            this.userId = userId;

            var result = new UserPagePermissionsResultViewModel();
            result.AccessPage = "NoAccess"; // Default value

            try
            {
                var group_permission = new List<UserPagePermissionsResultViewModel>();

                string pagename = "";
                var menuItem = await _context.MenuItemsTbl.SingleOrDefaultAsync(c => c.Related_Page_ID == pageid);

                if (menuItem != null)
                {
                    pagename = menuItem.SubMenu_Name_EN;
                }
                result.Pagename = pagename;

                // This part requires a stored procedure or direct EF Core query equivalent
                // For now, we'll simulate or use a placeholder.
                // group_permission = await _context.USER_PAGE_PERMISSIONS(applicationid, userid, pageid).ToListAsync();
                // Placeholder for USER_PAGE_PERMISSIONS_Result
                // Assuming the stored procedure returns a list of UserPagePermissionsResultViewModel
                // For now, we'll return an empty list or mock data
                group_permission = new List<UserPagePermissionsResultViewModel>(); // Replace with actual call to stored procedure or EF Core query

                if (group_permission.Any())
                {
                    var user = await _context.UsersTbl.SingleOrDefaultAsync(x => x.User_ID == userid);

                    if (user != null)
                    {
                        foreach (var item in group_permission)
                        {
                            if (item.ACCESS == true)
                            {
                                if (int.Parse(pageid) >= 1300 && int.Parse(pageid) <= 1400 && groupId == GroupType.Group_Status.AdminAuthorized)
                                {
                                    result.AccessPage = "Access";
                                }
                                else if (int.Parse(pageid) >= 1 && int.Parse(pageid) <= 100 && groupId == GroupType.Group_Status.SystemAdmin)
                                {
                                    result.AccessPage = "Access";
                                }
                                else if (!(int.Parse(pageid) >= 1 && int.Parse(pageid) <= 100) && !(int.Parse(pageid) >= 1300 && int.Parse(pageid) <= 1400))
                                {
                                    result.AccessPage = "Access";
                                }
                                else
                                {
                                    result.AccessPage = "NoAccess";
                                }
                            }
                            else
                            {
                                result.AccessPage = "NoAccess";
                            }
                        }
                    }
                    result.GroupPermissions = group_permission; // Store the group permissions in the ViewModel
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in getuser_group_permision for user: {userName}: {ex.Message}";
                _logger.LogError(_loggMessage);
                _logger.LogError(ex, _loggMessage);
            }
            return result;
        }

        public async Task<string> GetAllCategoriesForTree(string userName, int userId, string groupId)
        {
            _logger.LogInformation($"Executing GetAllCategoriesForTree for user: {userName}");
            this.userName = userName;
            this.userId = userId;

            try
            {
                var flatObjects = await _context.Categories.ToListAsync();
                var treeNodes = FillRecursive(flatObjects);

                // Build HTML string for the tree
                var treeHtml = "<ul class=\'treeview\'>";
                foreach (var node in treeNodes)
                {
                    treeHtml += BuildTreeHtml(node);
                }
                treeHtml += "</ul>";

                return treeHtml;
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in GetAllCategoriesForTree for user: {userName}: {ex.Message}";
                _logger.LogError(_loggMessage);
                _logger.LogError(ex, _loggMessage);
                return "";
            }
        }

        private string BuildTreeHtml(TreeNode node)
        {
            string html = $"<li><a href=\'#\' data-id=\\'{node.SubMenu_ID}\\'>{node.SubMenu_Name_EN}</a>";
            if (node.Children != null && node.Children.Any())
            {
                html += "<ul>";
                foreach (var child in node.Children)
                {
                    html += BuildTreeHtml(child);
                }
                html += "</ul>";
            }
            html += "</li>";
            return html;
        }

        public async Task<DataTable> Getpage(string page, string userName, int userId)
        {
            _logger.LogInformation($"Executing Getpage for user: {userName}, page: {page}");
            this.userName = userName;
            this.userId = userId;
            int _step = 40200;

            try
            {
                var dataTable = new DataTable();
                string connectionString = _configuration.GetConnectionString("CONNECTION_STR_DNS");

                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "SELECT [Page_Name_EN], [Other_Details] from [dbo].[App_Pages] where [Page_Id] = @pageId";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@pageId", page);
                        var adapter = new SqlDataAdapter(command);
                        await connection.OpenAsync();
                        adapter.Fill(dataTable);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                _loggMessage = $"Getpage From DB, step: {_step}";
                _logger.LogError(ex, _loggMessage);
                _logger.LogError(ex, _loggMessage);
                return null;
            }
        }

        public async Task<bool> getPermission(string id, string _page, string _groupid, string userName, int userId)
        {
            _logger.LogInformation($"Executing getPermission for user: {userName}, page: {_page}");
            this.userName = userName;
            this.userId = userId;

            try
            {
                var groupPermissionpage = await _context.GroupsPermissions
                    .Where(x => x.Group_Id == _groupid && x.Page_Id == _page && x.Application_ID == _applicationID && x.Access == true)
                    .ToListAsync();

                if (groupPermissionpage.Count == 0)
                {
                    List<Users_Permissions> usersPermissionpage;
                    if (_page == "0") // Assuming 0 is a special page ID
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == _applicationID)
                            .ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                    }

                    if (usersPermissionpage.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        int pageIdInt = int.Parse(_page);
                        if (pageIdInt >= 1300 && pageIdInt <= 1400 && _groupid == GroupType.Group_Status.AdminAuthorized)
                        {
                            return true;
                        }
                        else if (pageIdInt >= 1 && pageIdInt <= 100 && _groupid == GroupType.Group_Status.SystemAdmin)
                        {
                            return true;
                        }
                        else if (!(pageIdInt >= 1 && pageIdInt <= 100) && !(pageIdInt >= 1300 && pageIdInt <= 1400))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    List<Users_Permissions> usersPermissionpage;
                    if (_page == "0")
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == _applicationID)
                            .ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                        if (usersPermissionpage.Count == 0)
                        {
                            return true;
                        }

                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            int pageIdInt = int.Parse(_page);
                            if (pageIdInt >= 1300 && pageIdInt <= 1400 && _groupid == GroupType.Group_Status.AdminAuthorized)
                            {
                                return true;
                            }
                            else if (pageIdInt >= 1 && pageIdInt <= 100 && _groupid == GroupType.Group_Status.SystemAdmin)
                            {
                                return true;
                            }
                            else if (!(pageIdInt >= 1 && pageIdInt <= 100) && !(pageIdInt >= 1300 && pageIdInt <= 1400))
                            {
                                return true;
                            }
                        }

                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == false && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            return false;
                        }
                    }
                }
                return false; // Default return if no conditions met
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error when get getPermission, Check Error Table for details. Error get getPermission: {ex.Message}";
                _logger.LogError(ex, _loggMessage);
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<bool> getPermission1(string id, string _page, string _groupid, string userName, int userId)
        {
            _logger.LogInformation($"Executing getPermission1 for user: {userName}, page: {_page}");
            this.userName = userName;
            this.userId = userId;

            try
            {
                var groupPermissionpage = await _context.GroupsPermissions
                    .Where(x => x.Group_Id == _groupid && x.Page_Id == _page && (x.Add == true || x.Delete == true || x.Access == true || x.Reverse == true || x.Update == true || x.Post == true) && x.Application_ID == _applicationID && x.Access == true)
                    .ToListAsync();

                if (groupPermissionpage.Count == 0)
                {
                    List<Users_Permissions> usersPermissionpage;
                    if (_page == "0")
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == _applicationID)
                            .ToListAsync();
                    }
                    else
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                    }

                    if (usersPermissionpage.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        int pageIdInt = int.Parse(_page);
                        if (pageIdInt >= 1300 && pageIdInt <= 1400 && _groupid == GroupType.Group_Status.AdminAuthorized)
                        {
                            return true;
                        }
                        else if (pageIdInt >= 1 && pageIdInt <= 100 && _groupid == GroupType.Group_Status.SystemAdmin)
                        {
                            return true;
                        }
                        else if (!(pageIdInt >= 1 && pageIdInt <= 100) && !(pageIdInt >= 1300 && pageIdInt <= 1400))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    List<Users_Permissions> usersPermissionpage;
                    if (_page == "0")
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == _applicationID)
                            .ToListAsync();
                        return true;
                    }
                    else
                    {
                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                        if (usersPermissionpage.Count == 0)
                        {
                            return true;
                        }

                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            int pageIdInt = int.Parse(_page);
                            if (pageIdInt >= 1300 && pageIdInt <= 1400 && _groupid == GroupType.Group_Status.AdminAuthorized)
                            {
                                return true;
                            }
                            else if (pageIdInt >= 1 && pageIdInt <= 100 && _groupid == GroupType.Group_Status.SystemAdmin)
                            {
                                return true;
                            }
                            else if (!(pageIdInt >= 1 && pageIdInt <= 100) && !(pageIdInt >= 1300 && pageIdInt <= 1400))
                            {
                                return true;
                            }
                        }

                        usersPermissionpage = await _context.UsersPermissions
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == false && x.Application_ID == _applicationID && x.ActionID == 6)
                            .ToListAsync();
                        if (usersPermissionpage.Count > 0)
                        {
                            return false;
                        }
                    }
                }
                return false; // Default return if no conditions met
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error when get getPermission1, Check Error Table for details. Error get getPermission1: {ex.Message}";
                _logger.LogError(ex, _loggMessage);
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<bool> Ge_t(string x, string userName, int userId)
        {
            _logger.LogInformation($"Executing Ge_t for user: {userName}");
            this.userName = userName;
            this.userId = userId;

            try
            {
                // Logic from original Ge_t function needs to be implemented here
                // This seems to be a permission check based on the input string 'x'
                return true; // Placeholder
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in Ge_t: {ex.Message}";
                _logger.LogError(_loggMessage);
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<LoginResultViewModel> Login(LoginViewModel model)
        {
            _logger.LogInformation($"Login attempt for user: {model.Username}");

            var result = new LoginResultViewModel { Success = false };

            try
            {
                var user = await _context.UsersTbl.SingleOrDefaultAsync(u => u.User_Name == model.Username && u.User_Password == model.Password);

                if (user != null)
                {
                    result.Success = true;
                    result.Username = user.User_Name;
                    result.UserId = user.ID;
                    result.BranchId = user.Branch_ID;
                    result.ComId = user.COM_ID;
                    result.GroupId = user.Group_Id;
                }
                else
                {
                    result.ErrorMessage = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error during login for user {model.Username}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, model.Username, "", "", "", "");
                _logger.LogError(ex, _loggMessage);
                result.ErrorMessage = "An error occurred during login.";
            }

            return result;
        }
    }
}

