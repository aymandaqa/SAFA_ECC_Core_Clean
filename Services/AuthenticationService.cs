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

namespace SAFA_ECC_Core_Clean.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly LogSystem _logSystem; // Assuming LogSystem is a utility class
        private readonly int _applicationID = 1; // Example Application ID
        private string _loggMessage = "";
        private string userName;
        private int userId;

        public AuthenticationService(ApplicationDbContext context, ILogger<AuthenticationService> logger, IConfiguration configuration, LogSystem logSystem)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _logSystem = logSystem;
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
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return "";
            }
        }

        private string BuildTreeHtml(TreeNode node)
        {
            string html = $"<li><a href=\'#\' data-id=\'{node.SubMenu_ID}\' >{node.SubMenu_Name_EN}</a>";
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
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                _logSystem.WriteLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID)
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
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<bool> Ge_t(string x, string userName, int userId)
        {
            _logger.LogInformation($"Executing Ge_t for user: {userName}, x: {x}");
            this.userName = userName;
            this.userId = userId;
            int _step = 40500;

            try
            {
                var page = await _context.MenuItemsTbl.Where(i => i.Parent_ID == x).ToListAsync();
                _step += 10;
                if (page.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Getpage From DB, step: {_step}";
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        // The USER_PAGE_PERMISSIONS_Result type needs to be defined in Models or ViewModels
        // For now, using a placeholder class
        private class USER_PAGE_PERMISSIONS_Result
        {
            public bool ACCESS { get; set; }
            // Add other properties as needed
        }
    }
}
ation($"Executing getuser_group_permision for user: {userName}, pageid: {pageid}");
            this.userName = userName;
            this.userId = userId;

            // Removed direct session manipulation from service layer
            // setSessionItem("permission_user_Group", null);
            // setSessionItem("AccessPage", "");
            string _groupid = groupId;

            try
            {
                var group_permission = new List<USER_PAGE_PERMISSIONS_Result>(); // Placeholder for actual result type
                // Assuming SAFA_ECCEntities is replaced by ApplicationDbContext
                // Menu_Items_Tbl is a model in ApplicationDbContext

                string pagename = "";
                var menuItem = await _context.MenuItemsTbl.SingleOrDefaultAsync(c => c.Related_Page_ID == pageid);

                if (menuItem != null)
                {
                    pagename = menuItem.SubMenu_Name_EN;
                }
                // Removed direct session manipulation from service layer
                // setSessionItem("pagename", pagename);

                // This part requires a stored procedure or direct EF Core query equivalent
                // For now, we'll simulate or use a placeholder.
                // group_permission = await _context.USER_PAGE_PERMISSIONS(applicationid, userid, pageid).ToListAsync();
                // Placeholder for USER_PAGE_PERMISSIONS_Result
                group_permission = new List<USER_PAGE_PERMISSIONS_Result>(); // Replace with actual call

                if (group_permission.Any())
                {
                    var user = await _context.UsersTbl.SingleOrDefaultAsync(x => x.User_ID == userid);

                    if (user != null)
                    {
                        foreach (var item in group_permission)
                        {
                            if (item.ACCESS == true)
                            {
                                if (int.Parse(pageid) >= 1300 && int.Parse(pageid) <= 1400 && _groupid == GroupType.Group_Status.AdminAuthorized)
                                {
                                    // Removed direct session manipulation from service layer
                                    // setSessionItem("AccessPage", "Access");
                                }
                                else if (int.Parse(pageid) >= 1 && int.Parse(pageid) <= 100 && _groupid == GroupType.Group_Status.SystemAdmin)
                                {
                                    // Removed direct session manipulation from service layer
                                    // setSessionItem("AccessPage", "Access");
                                }
                                else if (!(int.Parse(pageid) >= 1 && int.Parse(pageid) <= 100) && !(int.Parse(pageid) >= 1300 && int.Parse(pageid) <= 1400))
                                {
                                    // Removed direct session manipulation from service layer
                                    // setSessionItem("AccessPage", "Access");
                                }
                                else
                                {
                                    // Removed direct session manipulation from service layer
                                    // setSessionItem("AccessPage", "NoAccess");
                                }
                            }
                            else
                            {
                                // Removed direct session manipulation from service layer
                                // setSessionItem("AccessPage", "NoAccess");
                            }
                        }
                    }
                    // Removed direct session manipulation from service layer
                    // setSessionItem("permission_user_Group", group_permission);
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in getuser_group_permision for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
            }
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
                var treeHtml = "<ul class='treeview'>";
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
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return "";
            }
        }

        private string BuildTreeHtml(TreeNode node)
        {
            string html = $"<li><a href='#' data-id='{node.SubMenu_ID}'>{node.SubMenu_Name_EN}</a>";
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
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                _logSystem.WriteLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID)
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
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<bool> Ge_t(string x, string userName, int userId)
        {
            _logger.LogInformation($"Executing Ge_t for user: {userName}, x: {x}");
            this.userName = userName;
            this.userId = userId;
            int _step = 40500;

            try
            {
                var page = await _context.MenuItemsTbl.Where(i => i.Parent_ID == x).ToListAsync();
                _step += 10;
                if (page.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Getpage From DB, step: {_step}";
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        // The GetAllCategoriesForTree method should be updated to remove session dependencies
        // The original VB.NET code for GetAllCategoriesForTree was:
        // Private Shared Function FillRecursive(ByVal flatObjects As List(Of Category), ByVal Optional parentId As Integer? = Nothing) As List(Of TreeNode)
        //    Return flatObjects.Where(Function(x) x.Parent_ID.Equals(parentId)).[Select](Function(item) New TreeNode With {
        //        .SubMenu_Name_EN = item.SubMenu_Name_EN,
        //        .SubMenu_ID = item.SubMenu_ID,
        //        .Related_Page_ID = item.Related_Page_ID,
        //        .Children = FillRecursive(flatObjects, item.SubMenu_ID)
        //    }).ToList()
        // End Function

        // The C# equivalent for GetAllCategoriesForTree in the service layer should return the HTML string directly
        // and not rely on session for intermediate storage.

        // This method needs to be implemented based on the VB.NET logic for GetAllCategoriesForTree
        // and should return a string (HTML) representing the tree structure.
        // It should not take Func<string, object> getSessionItem, Action<string, object> setSessionItem as parameters.

        // This is the updated signature for GetAllCategoriesForTree in IAuthenticationService
        // Task<string> GetAllCategoriesForTree(string userName, int userId, string groupId);

        // The implementation for GetAllCategoriesForTree is already above, I will remove the old one.


        // The USER_PAGE_PERMISSIONS_Result type needs to be defined in Models or ViewModels
        // For now, using a placeholder class
        private class USER_PAGE_PERMISSIONS_Result
        {
            public bool ACCESS { get; set; }
            // Add other properties as needed
        }
    }
}

        {
            _logger.LogInformation($"Executing getuser_group_permision for user: {userName}, pageid: {pageid}");
            this.userName = userName;
            this.userId = userId;

            setSessionItem("permission_user_Group", null);
            setSessionItem("AccessPage", "");
            string _groupid = groupId;

            try
            {
                var group_permission = new List<USER_PAGE_PERMISSIONS_Result>(); // Placeholder for actual result type
                // Assuming SAFA_ECCEntities is replaced by ApplicationDbContext
                // Menu_Items_Tbl is a model in ApplicationDbContext

                string pagename = "";
                var menuItem = await _context.MenuItemsTbl.SingleOrDefaultAsync(c => c.Related_Page_ID == pageid);

                if (menuItem != null)
                {
                    pagename = menuItem.SubMenu_Name_EN;
                }
                setSessionItem("pagename", pagename);

                // This part requires a stored procedure or direct EF Core query equivalent
                // For now, we'll simulate or use a placeholder.
                // group_permission = await _context.USER_PAGE_PERMISSIONS(applicationid, userid, pageid).ToListAsync();
                // Placeholder for USER_PAGE_PERMISSIONS_Result
                group_permission = new List<USER_PAGE_PERMISSIONS_Result>(); // Replace with actual call

                if (group_permission.Any())
                {
                    var user = await _context.UsersTbl.SingleOrDefaultAsync(x => x.User_ID == userid);

                    if (user != null)
                    {
                        foreach (var item in group_permission)
                        {
                            if (item.ACCESS == true)
                            {
                                if (int.Parse(pageid) >= 1300 && int.Parse(pageid) <= 1400 && _groupid == GroupType.Group_Status.AdminAuthorized)
                                {
                                    setSessionItem("AccessPage", "Access");
                                }
                                else if (int.Parse(pageid) >= 1 && int.Parse(pageid) <= 100 && _groupid == GroupType.Group_Status.SystemAdmin)
                                {
                                    setSessionItem("AccessPage", "Access");
                                }
                                else if (!(int.Parse(pageid) >= 1 && int.Parse(pageid) <= 100) && !(int.Parse(pageid) >= 1300 && int.Parse(pageid) <= 1400))
                                {
                                    setSessionItem("AccessPage", "Access");
                                }
                                else
                                {
                                    setSessionItem("AccessPage", "NoAccess");
                                }
                            }
                            else
                            {
                                setSessionItem("AccessPage", "NoAccess");
                            }
                        }
                    }
                    setSessionItem("permission_user_Group", group_permission);
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in getuser_group_permision for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID, GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
            }
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
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                _logSystem.WriteLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
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
                            .Where(x => x.UserID == id && x.PageID == _page && x.Value == true && x.Application_ID == _applicationID)
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
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<bool> Ge_t(string x, string userName, int userId)
        {
            _logger.LogInformation($"Executing Ge_t for user: {userName}, x: {x}");
            this.userName = userName;
            this.userId = userId;
            int _step = 40500;

            try
            {
                var page = await _context.MenuItemsTbl.Where(i => i.Parent_ID == x).ToListAsync();
                _step += 10;
                if (page.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _loggMessage = $"Getpage From DB, step: {_step}";
                _logSystem.WriteTraceLogg(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return false;
            }
        }

        public async Task<string> GetAllCategoriesForTree(string userName, int userId, string groupId, Func<string, object> getSessionItem, Action<string, object> setSessionItem)
        {
            _logger.LogInformation($"Executing GetAllCategoriesForTree for user: {userName}");
            this.userName = userName;
            this.userId = userId;
            int _step = 40600;

            try
            {
                List<Category> categories = new List<Category>();
                var allMenuItems = await _context.MenuItemsTbl.ToListAsync(); 
                DataTable dt = new DataTable();
                dt.Columns.Add("Related_Page_ID", typeof(string));
                dt.Columns.Add("SubMenu_ID", typeof(int));
                dt.Columns.Add("SubMenu_Name_EN", typeof(string));
                dt.Columns.Add("Parent_ID", typeof(int));
                dt.Columns.Add("UserType", typeof(string));

                foreach (var item in allMenuItems)
                {
                    dt.Rows.Add(item.Related_Page_ID, item.SubMenu_ID, item.SubMenu_Name_EN, item.Parent_ID, item.UserType);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    string Group_ID = groupId; 

                    foreach (DataRow row in dt.Rows)
                    {
                        if (int.Parse(Group_ID) >= 3)
                        {
                            if (row["UserType"].ToString() == "ALL" || row["UserType"].ToString() == "Auth")
                            {
                                categories.Add(new Category
                                {
                                    Related_Page_ID = row["Related_Page_ID"].ToString(),
                                    SubMenu_ID = Convert.ToInt32(row["SubMenu_ID"]),
                                    SubMenu_Name_EN = row["SubMenu_Name_EN"].ToString(),
                                    Parent_ID = (Convert.ToInt32(row["Parent_ID"]) != 0) ? (int?)Convert.ToInt32(row["Parent_ID"]) : null
                                });
                            }
                        }
                        else
                        {
                            if (row["UserType"].ToString() == "ALL" || row["UserType"].ToString() == "NotAuth")
                            {
                                categories.Add(new Category
                                {
                                    Related_Page_ID = row["Related_Page_ID"].ToString(),
                                    SubMenu_ID = Convert.ToInt32(row["SubMenu_ID"]),
                                    SubMenu_Name_EN = row["SubMenu_Name_EN"].ToString(),
                                    Parent_ID = (Convert.ToInt32(row["Parent_ID"]) != 0) ? (int?)Convert.ToInt32(row["Parent_ID"]) : null
                                });
                            }
                        }
                    }

                    _step += 10;
                    List<TreeNode> headerTree = FillRecursive(categories, null);
                    StringBuilder root_li = new StringBuilder();
                    string down1_names = "";
                    string down2_names = "";
                    string down3_names = "";
                    bool page_permission;

                    foreach (var item in headerTree)
                    {
                        page_permission = await getPermission(getSessionItem("ID").ToString(), item.Related_Page_ID, groupId, userName, userId);
                        _step += 10;
                        if (page_permission == true)
                        {
                            bool hasChildAccess = false;
                            foreach (var i in item.Children)
                            {
                                if (await getPermission(getSessionItem("ID").ToString(), i.SubMenu_ID.ToString(), groupId, userName, userId))
                                {
                                    hasChildAccess = true;
                                    break;
                                }
                            }

                            if (hasChildAccess)
                            {
                                if (item.Children == null || item.Related_Page_ID == "0")
                                {
                                    root_li.Append($"<li  ><span class=\"caret\"id = \"{item.SubMenu_ID}\"onclick =\"getexpanditem(this.id);\"><b>{item.SubMenu_Name_EN}</b></span><ul class=\"nested\"> ");
                                }
                                else
                                {
                                    DataTable rediretcpage = await Getpage(item.Related_Page_ID, userName, userId);
                                    if (rediretcpage != null && rediretcpage.Rows.Count > 0)
                                    {
                                        root_li.Append($"<li  ><span class=\"caret\" id = \"{item.SubMenu_ID}\" onclick =\"getexpanditem(this.id);\"><b><a style=\"font-size:18px\" href=\"{rediretcpage.Rows[0]["Other_Details"]}\">{item.SubMenu_Name_EN}</a></b></span><ul class=\"nested\"> ");
                                    }
                                }

                                _step += 10;
                                down1_names = "";

                                foreach (var down1 in item.Children)
                                {
                                    down2_names = "";
                                    down3_names = "";

                                    page_permission = await getPermission1(getSessionItem("ID").ToString(), down1.Related_Page_ID, groupId, userName, userId);
                                    bool x = await Ge_t(down1.SubMenu_ID.ToString(), userName, userId);
                                    if (down1.Related_Page_ID == "0" && x == true)
                                    {
                                        page_permission = true;
                                    }

                                    if (page_permission == true)
                                    {
                                        foreach (var down2 in down1.Children)
                                        {
                                            page_permission = await getPermission1(getSessionItem("ID").ToString(), down2.Related_Page_ID, groupId, userName, userId);
                                            if (page_permission == true)
                                            {
                                                if (down2.Children == null || down2.Related_Page_ID == "0")
                                                {
                                                }
                                                else
                                                {
                                                    DataTable rediretcpage = await Getpage(down2.Related_Page_ID, userName, userId);
                                                    if (rediretcpage != null && rediretcpage.Rows.Count > 0)
                                                    {
                                                        down2_names += $"<li ><b><a style=\"font-size:14px\" href=\"{rediretcpage.Rows[0]["Other_Details"]}\">{down2.SubMenu_Name_EN}</a></b></li></span>";
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (down1.Children == null || down1.Related_Page_ID == "0")
                                    {
                                        int _coun1t = down1.Children.Count;
                                        if (down1.Children.Count > 0)
                                        {
                                            page_permission = await getPermission(getSessionItem("ID").ToString(), down1.Children[_coun1t - 1].Related_Page_ID, groupId, userName, userId);
                                            if (page_permission == true)
                                            {
                                                down1_names += $"<li class=\"test\"><b<span class=\"caret\" id = \"{down1.Children[_coun1t - 1].SubMenu_ID}\" onclick =\"getexpanditem(this.id);\"><a style=\"font-size:14px\" href=\"#\">{down1.SubMenu_Name_EN}</a></span> <ul class=\"nested\">{down2_names}</span> </ul>";
                                            }
                                        }
                                        else
                                        {
                                            page_permission = await getPermission1(getSessionItem("ID").ToString(), down1.Related_Page_ID, groupId, userName, userId);
                                            if (page_permission == true)
                                            {
                                                down1_names += $"<li class=\"test1\"  ><b><a style=\"font-size:8px\" href=\"#\"><ul style=\"font-size:14px\" >{down1.SubMenu_Name_EN}</ul></b></a></span> <ul class=\"nested\">{down2_names}</span> </ul>";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DataTable rediretcpage = await Getpage(down1.Related_Page_ID, userName, userId);
                                        int _coun1t = down1.Children.Count;

                                        if (down1.Children.Count > 0)
                                        {
                                            page_permission = await getPermission(getSessionItem("ID").ToString(), down1.Children[_coun1t - 1].Related_Page_ID, groupId, userName, userId);
                                            if (page_permission == true)
                                            {
                                                down1_names += $"<li class=\"test\"><b<span class=\"caret\" id = \"{down1.Children[_coun1t - 1].SubMenu_ID}\" onclick =\"getexpanditem(this.id);\"><a style=\"font-size:14px\" href=\"{rediretcpage.Rows[0]["Other_Details"]}\"><ul>{down1.SubMenu_Name_EN}</ul></a></span> <ul class=\"nested\">{down2_names}</span> </ul>";
                                            }
                                        }
                                        else
                                        {
                                            page_permission = await getPermission1(getSessionItem("ID").ToString(), down1.Related_Page_ID, groupId, userName, userId);
                                            if (page_permission == true)
                                            {
                                                down1_names += $"<li class=\"test1\"  ><b><a style=\"font-size:8px\" href=\"{rediretcpage.Rows[0]["Other_Details"]}\"><ul style=\"font-size:14px\" >{down1.SubMenu_Name_EN}</ul></b></a></span> <ul class=\"nested\">{down2_names}</span> </ul>";
                                            }
                                        }
                                    }
                                }
                                _step += 10;
                                root_li.Append($"{down1_names}</ul><div><hr style = \"width:100% ; color = black\"></div>");
                            }
                            _step += 10;
                        }
                    }
                    _step += 10;
                    return $"<div><ul class=\"myUL\" id = \"Tree_mnue\">{root_li}</div></ul>";
                }

                return "Record Not Found!!";
            }
            catch (Exception ex)
            {
                _loggMessage = $"Error in GetAllCategoriesForTree for user: {userName}: {ex.Message}";
                _logSystem.WriteError(_loggMessage, _applicationID.ToString(), GetType().Name, MethodBase.GetCurrentMethod().Name, userName, userId.ToString(), "", "", "");
                _logger.LogError(ex, _loggMessage);
                return "Error";
            }
        }




        public async Task<LoginResultViewModel> Login(LoginViewModel model)
        {
            // This is a placeholder implementation. You'll need to replace this with your actual authentication logic.
            // For example, you might query a database to validate the user's credentials.

            if (model.UserName == "admin" && model.Password == "password")
            {
                return new LoginResultViewModel
                {
                    Success = true,
                    Message = "Login successful",
                    RedirectUrl = "/Home/Index",
                    UserName = "admin",
                    UserID = "1",
                    GroupID = "1",
                    BranchID = "1",
                    BranchName = "Main Branch",
                    UserType = "Admin",
                    ApplicationID = "1",
                    CompanyID = "1",
                    CompanyName = "SAFA",
                    CompanyBranchID = "1",
                    CompanyBranchName = "Main Branch",
                    CompanyBranchType = "Main",
                    CompanyBranchAddress = "123 Main St",
                    CompanyBranchPhone = "555-555-5555",
                    CompanyBranchFax = "555-555-5555",
                    CompanyBranchEmail = "admin@safa.com",
                    CompanyBranchWebsite = "www.safa.com",
                    CompanyBranchLogo = "logo.png",
                    CompanyBranchCurrency = "USD",
                    CompanyBranchCurrencySymbol = "$",
                    CompanyBranchCurrencyName = "US Dollar",
                    CompanyBranchCurrencyDecimalPlaces = "2",
                    CompanyBranchCurrencyRate = "1",
                    CompanyBranchCurrencyRateDate = DateTime.Now.ToShortDateString(),
                    CompanyBranchCurrencyRateTime = DateTime.Now.ToShortTimeString(),
                    CompanyBranchCurrencyRateUser = "admin",
                    CompanyBranchCurrencyRateStatus = "Active",
                    CompanyBranchCurrencyRateDescription = ""
                };
            }
            else
            {
                return new LoginResultViewModel
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }
        }

