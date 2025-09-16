using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;

namespace SAFA_ECC_Core_Clean.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            try
            {
                if (!await HasPermission("Index"))
                    return Forbid();

                var stats = new AdminDashboardViewModel
                {
                    TotalUsers = await _context.Users_Tbl.CountAsync(),
                    ActiveUsers = await _context.Users_Tbl.CountAsync(u => u.IsDisabled == false),
                    TotalGroups = await _context.Groups_Tbl.CountAsync(),
                    ActiveGroups = await _context.Groups_Tbl.CountAsync(g => g.Is_Active == true),
                    TotalCompanies = await _context.Companies_Tbl.CountAsync(),
                    ActiveCompanies = await _context.Companies_Tbl.CountAsync(c => c.Is_Active == true),
                    PendingAuthorizations = await _context.AuthTrans_User_TBL_Auth.CountAsync(a => a.status == "Pending"),
                    RecentUsers = await _context.Users_Tbl
                        .OrderByDescending(u => u.Creation_Date)
                        .Take(5)
                        .ToListAsync()
                };

                return View(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Admin Index");
                return View(new AdminDashboardViewModel());
            }
        }

        // GET: Admin/index2
        public async Task<IActionResult> index2()
        {
            try
            {
                if (!await HasPermission("index2"))
                    return Forbid();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Admin index2");
                return View();
            }
        }

        // GET: Admin/AddUser
        public async Task<IActionResult> AddUser()
        {
            try
            {
                if (!await HasPermission("AddUser"))
                    return Forbid();

                var model = new AddUserViewModel
                {
                    Companies = await _context.Companies_Tbl.Where(c => c.Is_Active == true).ToListAsync(),
                    Groups = await _context.Groups_Tbl.Where(g => g.Is_Active == true).ToListAsync()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddUser GET");
                return View(new AddUserViewModel());
            }
        }

        // POST: Admin/AddUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            try
            {
                if (!await HasPermission("AddUser"))
                    return Forbid();

                if (ModelState.IsValid)
                {
                    // Check if username already exists
                    var existingUser = await _context.Users_Tbl.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                    if (existingUser != null) 
                    {
                        ModelState.AddModelError("UserName", "اسم المستخدم موجود مسبقاً");
                        model.Companies = await _context.Companies_Tbl.Where(c => c.Is_Active == true).ToListAsync();
                        model.Groups = await _context.Groups_Tbl.Where(g => g.Is_Active == true).ToListAsync();
                        return View(model);
                    }

                    var user = new Users_Tbl
                    {
                        UserName = model.UserName,
                        Password = HashPassword(model.Password), // Changed from User_Password to Password
                        FullNameEN = model.FullName, // Assuming FullName is English for now
                        Email = model.Email,
                        Company_ID = model.CompanyId,
                        Group_ID = model.GroupId,
                        IsDisabled = false,
                        Creation_Date = DateTime.Now,
                        Created_By = User.Identity.Name
                    };

                    _context.Users_Tbl.Add(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم إضافة المستخدم بنجاح";
                    return RedirectToAction(nameof(userlist));
                }

                model.Companies = await _context.Companies_Tbl.Where(c => c.Is_Active == true).ToListAsync();
                model.Groups = await _context.Groups_Tbl.Where(g => g.Is_Active == true).ToListAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddUser POST");
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة المستخدم";
                return View(model);
            }
        }

        // GET: Admin/EditUser
        public async Task<IActionResult> EditUser(int id)
        {
            try
            {
                if (!await HasPermission("EditUser"))
                    return Forbid();

                var user = await _context.Users_Tbl.FindAsync(id);
                if (user == null)
                    return NotFound();

                var model = new EditUserViewModel
                {
                    UserId = user.User_ID,
                    UserName = user.UserName,
                FullName = user.FullNameEN ?? user.UserName,
                    Email = user.Email,
                    CompanyId = user.Company_ID ?? 0, // Handle nullable
                    GroupId = user.Group_ID ?? 0,     // Handle nullable
                    IsDisabled = user.IsDisabled ?? false, // Handle nullable
                    Companies = await _context.Companies_Tbl.Where(c => c.Is_Active == true).ToListAsync(),
                    Groups = await _context.Groups_Tbl.Where(g => g.Is_Active == true).ToListAsync()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditUser GET");
                return NotFound();
            }
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            try
            {
                if (!await HasPermission("EditUser"))
                    return Forbid();

                if (ModelState.IsValid)
                {
                    var user = await _context.Users_Tbl.FindAsync(model.UserId);
                    if (user == null)
                        return NotFound();

                    user.UserName = model.UserName;
                    user.FullNameEN = model.FullName;
                    user.Email = model.Email;
                    user.Company_ID = model.CompanyId;
                    user.Group_ID = model.GroupId;
                    user.IsDisabled = model.IsDisabled;
                    user.Last_Amend_Date = DateTime.Now;
                    user.Last_Amend_By = User.Identity.Name;

                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        user.Password = HashPassword(model.NewPassword); // Changed from User_Password to Password
                    }

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم تحديث المستخدم بنجاح";
                    return RedirectToAction(nameof(userlist));
                }

                model.Companies = await _context.Companies_Tbl.Where(c => c.Is_Active == true).ToListAsync();
                model.Groups = await _context.Groups_Tbl.Where(g => g.Is_Active == true).ToListAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditUser POST");
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث المستخدم";
                return View(model);
            }
        }

        // GET: Admin/userlist
        public async Task<IActionResult> userlist()
        {
            try
            {
                if (!await HasPermission("userlist"))
                    return Forbid();

                var users = await _context.Users_Tbl
                    .Include(u => u.Companies_Tbl) // Changed from u.Company to u.Companies_Tbl
                    .Include(u => u.Groups_Tbl)     // Changed from u.Group to u.Groups_Tbl
                    .OrderBy(u => u.UserName)
                    .ToListAsync();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in userlist");
                return View(new List<Users_Tbl>());
            }
        }

        // GET: Admin/Addgroup
        public async Task<IActionResult> Addgroup()
        {
            try
            {
                if (!await HasPermission("Addgroup"))
                    return Forbid();

                return View(new AddGroupViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Addgroup GET");
                return View(new AddGroupViewModel());
            }
        }

        // POST: Admin/Addgroup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Addgroup(AddGroupViewModel model)
        {
            try
            {
                if (!await HasPermission("Addgroup"))
                    return Forbid();

                if (ModelState.IsValid)
                {
                    var group = new Groups_Tbl
                    {
                        Group_Name_EN = model.GroupName, // Assign to Group_Name_EN
                        Group_Description = model.GroupDescription,
                        Is_Active = true,
                        Creation_Date = DateTime.Now,
                        Created_By = User.Identity.Name
                    };

                    _context.Groups_Tbl.Add(group);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم إضافة المجموعة بنجاح";
                    return RedirectToAction(nameof(grouplist));
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Addgroup POST");
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة المجموعة";
                return View(model);
            }
        }

        // GET: Admin/grouplist
        public async Task<IActionResult> grouplist()
        {
            try
            {
                if (!await HasPermission("grouplist"))
                    return Forbid();

                var groups = await _context.Groups_Tbl
                    .OrderBy(g => g.Group_Name_EN) // Order by Group_Name_EN
                    .ToListAsync();

                return View(groups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in grouplist");
                return View(new List<Groups_Tbl>());
            }
        }

        // GET: Admin/Add_Password_Policies
        public async Task<IActionResult> Add_Password_Policies()
        {
            try
            {
                if (!await HasPermission("Add_Password_Policies"))
                    return Forbid();

                return View(new PasswordPolicyViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add_Password_Policies GET");
                return View(new PasswordPolicyViewModel());
            }
        }

        // POST: Admin/Add_Password_Policies
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add_Password_Policies(PasswordPolicyViewModel model)
        {
            try
            {
                if (!await HasPermission("Add_Password_Policies"))
                    return Forbid();

                if (ModelState.IsValid)
                {
                    var policy = new Password_Policies_TBL
                    {
                        Policy_Name = model.PolicyName,
                        Min_Length = model.MinimumLength,
                        Max_Length = model.MaxLength,
                        Require_Uppercase = model.RequireUppercase,
                        Require_Lowercase = model.RequireLowercase,
                        Require_Numbers = model.RequireNumbers,
                        Require_Special_Chars = model.RequireSpecialChars,
                        Password_Expiry_Days = model.PasswordExpiryDays,
                        Is_Active = true,
                        Creation_Date = DateTime.Now, 
                        Created_By = User.Identity.Name
                    };

                    _context.Password_Policies_TBL.Add(policy);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "تم إضافة سياسة كلمة المرور بنجاح";
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Add_Password_Policies POST");
                TempData["ErrorMessage"] = "حدث خطأ أثناء إضافة سياسة كلمة المرور";
                return View(model);
            }
        }

        // Helper method to check permissions
        private async Task<bool> HasPermission(string actionName)
        {
            try
            {
                var userName = User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                    return false;

                var user = await _context.Users_Tbl.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                    return false;

                // Check if user has permission for this action
                var hasUserPermission = await _context.Users_Permissions // Changed from User_Permissions_Tbl to Users_Permissions
                    .AnyAsync(up => up.UserID == user.User_ID && 
                                   up.Page_Name == "Admin" && 
                                   up.Action_Name == actionName && 
                                   up.Value == true); // Changed Is_Allowed to Value

                if (hasUserPermission)
                    return true;

                // Check group permissions
                var hasGroupPermission = await _context.Group_Permissions // Changed from Group_Permissions_Tbl to Group_Permissions
                    .AnyAsync(gp => gp.GroupID == user.Group_ID && 
                                   gp.Page_Name == "Admin" && 
                                   gp.Action_Name == actionName && 
                                   gp.Is_Allowed == true);

                return hasGroupPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking permissions for {ActionName}", actionName);
                return false;
            }
        }

        // Helper method to hash passwords
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}

