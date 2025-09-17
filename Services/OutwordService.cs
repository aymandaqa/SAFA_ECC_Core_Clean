using Microsoft.Extensions.Logging;
using SAFA_ECC_Core_Clean.Data;
using SAFA_ECC_Core_Clean.Models;
using SAFA_ECC_Core_Clean.ViewModels.OutwordViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace SAFA_ECC_Core_Clean.Services
{
    public class OutwordService : IOutwordService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OutwordService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public OutwordService(ApplicationDbContext context, ILogger<OutwordService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<IActionResult> CheckImg()
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            // Original VB.NET code:
            // Dim db As New SAFA_ECCEntities
            // Dim out As New Outward_Imgs
            // Dim ret_out As New Outward_Trans
            // Dim retout As New List(Of Outward_Trans)
            // Dim chq_seq As String = ""
            // ret_out = db.Outward_Trans.SingleOrDefault(Function(i) i.InputBrn = "808" And i.Posted = AllEnums.Cheque_Status.Returne And i.Serial = "1146042")
            // chq_seq = ret_out.ChqSequance
            // out = db.Outward_Imgs.SingleOrDefault(Function(i) i.ChqSequance = chq_seq)
            // Return View(out)

            var retOut = await _context.Outward_Trans.SingleOrDefaultAsync(i => i.InputBrn == "808" && i.Posted == (int)AllEnums.Cheque_Status.Returne && i.Serial == "1146042");
            if (retOut == null)
            {
                return new ViewResult { ViewName = "Error" }; // Or handle as appropriate
            }

            string chqSeq = retOut.ChqSequance;
            var outwardImg = await _context.Outward_Imgs.SingleOrDefaultAsync(i => i.ChqSequance == chqSeq);

            return new ViewResult { ViewName = "check_img", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = outwardImg } };
        }

        public async Task<List<SelectListItem>> BindHoldType()
        {
            var holdTypes = new List<SelectListItem>();
            // Assuming AllEnums.HoldType is accessible and contains values
            foreach (var type in Enum.GetValues(typeof(AllEnums.HoldType)))
            {
                holdTypes.Add(new SelectListItem { Text = type.ToString(), Value = ((int)type).ToString() });
            }
            return holdTypes;
        }

        public async Task<List<SelectListItem>> GetAllCategoriesForTree()
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning an empty list.
            return await Task.FromResult(new List<SelectListItem>());
        }

        public async Task<ActionResult> GetHold_CHQ()
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            // The original VB.NET code has commented out permission checks and ViewBag assignments.
            // We will assume these are handled elsewhere or will be implemented as needed.

            // Example of how to get ViewBag data from service if needed:
            // var bindHoldTypes = await BindHoldType();
            // var treeCategories = await GetAllCategoriesForTree();

            // For now, return a simple ViewResult.
            return new ViewResult { ViewName = "Hold_CHQ" };
        }

        public async Task<ActionResult> Hold_CHQ(Hold_CHQ Hold, string HOLD_TYPE, string Reserved)
        {
            // Assuming Session is handled by the controller and passed as needed, or replaced with a proper state management solution.
            // For now, we'll simulate session access or assume it's passed.
            // You might need to inject IHttpContextAccessor or similar for real session access.

            // Re-bind ViewBag data if needed for returning the view
            var bindHoldTypes = await BindHoldType();
            var treeCategories = await GetAllCategoriesForTree();

            if (Hold.Note1 == null || Hold.DrwAcctNo == null || Hold.DrwBankNo == null || Hold.DrwBranchNo == null || Hold.DrwChqNo == null || (Hold.Reson1 == null && Hold.Reson2 == null))
            {
                // Simulate Session.Item("Hold_CHQ_ERR")
                // In a real application, you'd use TempData or ModelState for error messages
                // ViewBag.Hold_CHQ_ERR = "Please Fill All Filed";
                return new ViewResult { ViewName = "Hold_CHQ", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = Hold } };
            }

            try
            {
                var _Hold = await _context.Hold_CHQ.SingleOrDefaultAsync(c => c.DrwAcctNo == Hold.DrwAcctNo && c.DrwBankNo == Hold.DrwBankNo && c.DrwBranchNo == Hold.DrwBranchNo && c.DrwChqNo == Hold.DrwChqNo);

                if (_Hold == null)
                {
                    _Hold = new Hold_CHQ();

                    _Hold.DrwAcctNo = Hold.DrwAcctNo;
                    _Hold.DrwBankNo = Hold.DrwBankNo;
                    _Hold.DrwBranchNo = Hold.DrwBranchNo;
                    _Hold.DrwChqNo = Hold.DrwChqNo;
                    // _Hold.InputBy = Session.Item("UserName"); // Replace with actual user context
                    _Hold.InputDate = DateTime.Now;
                    // _Hold.History += " Recored Added by : " + Session.Item("UserName") + ", AT: " + DateTime.Now; // Replace with actual user context
                    _Hold.Reson1 = Hold.Reson1;
                    _Hold.Reson2 = Hold.Reson2;
                    _Hold.Type = HOLD_TYPE;
                    _Hold.Note1 = Hold.Note1;
                    _Hold.Reserved = (Reserved.ToUpper() == "ON") ? 1 : 0;

                    _context.Hold_CHQ.Add(_Hold);
                    await _context.SaveChangesAsync();

                    // ViewBag.Hold_CHQ_ERR = "Done Successfully";
                    return new ViewResult { ViewName = "Hold_CHQ", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = new Hold_CHQ() } };
                }
                else
                {
                    // ViewBag.Hold_CHQ_ERR = "This CHQ Aleardy Exis";
                    return new ViewResult { ViewName = "Hold_CHQ", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = Hold } };
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // _LogSystem.WriteError(...);
                // ViewBag.Hold_CHQ_ERR = ex.Message;
                return new ViewResult { ViewName = "Hold_CHQ", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = Hold } };
            }
        }

        public async Task<IActionResult> ReturnDiscountChq()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree
            // In ASP.NET Core, ViewBag is part of the Controller or ViewContext, not directly accessible in a service.
            // We will return the data needed for the view, and the controller will populate ViewBag.
            // For now, we return a ViewResult and let the controller handle ViewBag population.

            return new ViewResult { ViewName = "returndiscountchq" };
        }

        public async Task<string> Get_Deacrypted_Account(string Drw_Account, string ChqNo)
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"get Get_Deacrypted_Account, PDC controller");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If Decrypt_CAB_Acc_No is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how Decrypt_CAB_Acc_No is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[Decrypt_CAB_Acc_No]({Drw_Account}, {ChqNo})").FirstOrDefaultAsync();

                // Placeholder for actual decryption logic or database call
                result = "Decrypted_" + Drw_Account + "_" + ChqNo; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error get Get_Deacrypted_Account: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task<IActionResult> Rejected_Out_Request()
        {
            // Assuming Session.Item("UserName") is handled by authentication middleware.
            // If not authenticated, the controller should redirect to login.

            // Logging
            _logger.LogInformation("start Rejected_Out_Request / Authentication controller");

            // Populate ViewBag.Tree
            // This will be handled by the controller.

            // Assuming user is authenticated
            var authDetails = await _context.Auth_Tran_Details_TBL
                                            .Where(x => (x.First_level_status == "Reject" || x.Second_level_status == "Reject") && (x.TBL_ID == 2 || x.TBL_ID == 6 || x.TBL_ID == 3))
                                            .ToListAsync();

            if (authDetails != null && authDetails.Any())
            {
                foreach (var item in authDetails)
                {
                    var outwardTrans = await _context.Outward_Trans.SingleOrDefaultAsync(u => u.ChqSequance == item.Chq_Serial);
                    if (outwardTrans != null)
                    {
                        if (!string.IsNullOrEmpty(item.RejectReason))
                        {
                            var parts = item.RejectReason.Split('|');
                            if (parts.Length > 0)
                            {
                                var lastPart = parts[parts.Length - 1];
                                item.RejectReason = lastPart.Split(';')[0];
                            }
                        }
                        var company = await _context.Companies_Tbl.SingleOrDefaultAsync(v => v.Company_ID == item.Branch_code);
                        if (company != null)
                        {
                            item.Note4 = company.Company_Name_AR;
                        }
                    }
                }
                return new ViewResult { ViewName = "Rejected_Out_Request", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = authDetails } };
            }

            return new ViewResult { ViewName = "Rejected_Out_Request" };
        }

        public async Task<IActionResult> RepresnetDisDetails(string id)
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Logging
            _logger.LogInformation($"Show Post Date Cheque to verify it from Post_Dated_Trans table for ID: {id}");

            // Populate ViewBag.Tree will be handled by the controller.

            var outChq = new OutChqs(); // Assuming OutChqs is a ViewModel or Model
            var Img = new Outward_Imgs(); // Assuming Outward_Imgs is a Model
            var pdcObj = new Outward_Trans(); // Assuming Outward_Trans is a Model
            List<CURRENCY_TBL> Currency = new List<CURRENCY_TBL>(); // Assuming CURRENCY_TBL is a Model

            try
            {
                pdcObj = await _context.Outward_Trans.SingleOrDefaultAsync(y => y.Serial == id && y.Posted == AllEnums.Cheque_Status.Returne);
                Currency = await _context.CURRENCY_TBL.ToListAsync();

                if (pdcObj == null)
                {
                    var oldout = await _context.Outward_Trans_Discount_Old.SingleOrDefaultAsync(y => y.Serial == id && y.Posted == AllEnums.Cheque_Status.Returne);
                    if (oldout != null)
                    {
                        // Map oldout to pdcObj or a new ViewModel if needed
                        // For now, let's assume we can work with oldout directly or map it.
                        // This part needs specific mapping logic based on the actual models.
                        // Example: pdcObj = new Outward_Trans { /* map properties from oldout */ };
                    }
                }

                // Further logic to populate outChq and Img based on pdcObj and Currency
                // This will involve more database queries and mapping.

                // For now, return a simple ViewResult with the populated data.
                return new ViewResult { ViewName = "RepresnetDisDetails", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = outChq } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in RepresnetDisDetails: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> GetOutwordPDC()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree will be handled by the controller.

            // Logging
            _logger.LogInformation("Get Outword PDC");

            try
            {
                // The VB.NET code has a lot of commented-out permission checks and ViewBag assignments.
                // These will be handled in the Controller or through middleware/filters in ASP.NET Core.

                // The original method returns a View(). We need to return a ViewResult.
                return new ViewResult { ViewName = "GetOutwordPDC" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetOutwordPDC: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> Out_VerficationDetails(string id)
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Logging
            _logger.LogInformation($"Out_VerficationDetails for ID: {id}");

            var outChq = new OutChqs(); // Assuming OutChqs is a ViewModel or Model
            var Img = new Outward_Imgs(); // Assuming Outward_Imgs is a Model
            var pdcObj = new Outward_Trans(); // Assuming Outward_Trans is a Model
            List<CURRENCY_TBL> Currency = new List<CURRENCY_TBL>(); // Assuming CURRENCY_TBL is a Model

            try
            {
                pdcObj = await _context.Outward_Trans.SingleOrDefaultAsync(y => y.Serial == id && y.Posted == AllEnums.Cheque_Status.New);
                Currency = await _context.CURRENCY_TBL.ToListAsync();

                // Further logic to populate outChq and Img based on pdcObj and Currency
                // This will involve more database queries and mapping.

                // For now, return a simple ViewResult with the populated data.
                return new ViewResult { ViewName = "Out_VerficationDetails", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = outChq } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Out_VerficationDetails: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> OutwordDateVerfication()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree will be handled by the controller.

            // Logging
            _logger.LogInformation("OutwordDateVerfication");

            try
            {
                // The VB.NET code has a lot of commented-out permission checks and ViewBag assignments.
                // These will be handled in the Controller or through middleware/filters in ASP.NET Core.

                // The original method returns a View(). We need to return a ViewResult.
                return new ViewResult { ViewName = "OutwordDateVerfication" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OutwordDateVerfication: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> OUTWORD()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree will be handled by the controller.

            // Logging
            _logger.LogInformation("OUTWORD");

            try
            {
                // The VB.NET code has a lot of commented-out permission checks and ViewBag assignments.
                // These will be handled in the Controller or through middleware/filters in ASP.NET Core.

                // The original method returns a View(). We need to return a ViewResult.
                return new ViewResult { ViewName = "OUTWORD" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OUTWORD: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> OUTWORD(Outward_Trans outward_Trans, string submit)
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree will be handled by the controller.

            // Logging
            _logger.LogInformation("OUTWORD (POST)");

            try
            {
                // The VB.NET code has a lot of logic for handling the POST request.
                // This needs to be carefully converted to C#.

                // For now, returning a placeholder.
                return new ViewResult { ViewName = "OUTWORD" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OUTWORD (POST): {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<string> Get_Post_Rest_Code(int CUST_POSTING_RESTRICTION, int ACC_POSTING_RESTRICTION, int TBL_ID)
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"Get_Post_Rest_Code");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If Get_Post_Rest_Code is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how Get_Post_Rest_Code is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[Get_Post_Rest_Code]({CUST_POSTING_RESTRICTION}, {ACC_POSTING_RESTRICTION}, {TBL_ID})").FirstOrDefaultAsync();

                // Placeholder for actual logic or database call
                result = "Post_Rest_Code_" + CUST_POSTING_RESTRICTION + "_" + ACC_POSTING_RESTRICTION + "_" + TBL_ID; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_Post_Rest_Code: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task<string> Get_Final_Posting_Restrection(int CUST_POSTING_RESTRICTION, int ACC_POSTING_RESTRICTION, int TBL_ID)
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"Get_Final_Posting_Restrection");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If Get_Final_Posting_Restrection is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how Get_Final_Posting_Restrection is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[Get_Final_Posting_Restrection]({CUST_POSTING_RESTRICTION}, {ACC_POSTING_RESTRICTION}, {TBL_ID})").FirstOrDefaultAsync();

                // Placeholder for actual logic or database call
                result = "Final_Posting_Restrection_" + CUST_POSTING_RESTRICTION + "_" + ACC_POSTING_RESTRICTION + "_" + TBL_ID; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_Final_Posting_Restrection: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task<IActionResult> Pendding_OutWord_Request()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree will be handled by the controller.

            // Logging
            _logger.LogInformation("Pendding_OutWord_Request");

            try
            {
                // The VB.NET code has a lot of commented-out permission checks and ViewBag assignments.
                // These will be handled in the Controller or through middleware/filters in ASP.NET Core.

                // The original method returns a View(). We need to return a ViewResult.
                return new ViewResult { ViewName = "Pendding_OutWord_Request" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Pendding_OutWord_Request: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> Pendding_OutWord_Request_Auth()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Populate ViewBag.Tree will be handled by the controller.

            // Logging
            _logger.LogInformation("Pendding_OutWord_Request_Auth");

            try
            {
                // The VB.NET code has a lot of commented-out permission checks and ViewBag assignments.
                // These will be handled in the Controller or through middleware/filters in ASP.NET Core.

                // The original method returns a View(). We need to return a ViewResult.
                return new ViewResult { ViewName = "Pendding_OutWord_Request_Auth" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Pendding_OutWord_Request_Auth: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<IActionResult> getOutword_WF_Details(string id)
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Logging
            _logger.LogInformation($"getOutword_WF_Details for ID: {id}");

            var outChq = new OutChqs(); // Assuming OutChqs is a ViewModel or Model
            var Img = new Outward_Imgs(); // Assuming Outward_Imgs is a Model
            var pdcObj = new Outward_Trans(); // Assuming Outward_Trans is a Model
            List<CURRENCY_TBL> Currency = new List<CURRENCY_TBL>(); // Assuming CURRENCY_TBL is a Model

            try
            {
                pdcObj = await _context.Outward_Trans.SingleOrDefaultAsync(y => y.Serial == id && y.Posted == AllEnums.Cheque_Status.New);
                Currency = await _context.CURRENCY_TBL.ToListAsync();

                // Further logic to populate outChq and Img based on pdcObj and Currency
                // This will involve more database queries and mapping.

                // For now, return a simple ViewResult with the populated data.
                return new ViewResult { ViewName = "getOutword_WF_Details", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = outChq } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getOutword_WF_Details: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }

        public async Task<string> Get_OFS_HttpLink()
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"Get_OFS_HttpLink");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If Get_OFS_HttpLink is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how Get_OFS_HttpLink is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[Get_OFS_HttpLink]()").FirstOrDefaultAsync();

                // Placeholder for actual logic or database call
                result = "http://example.com/ofs_http_link"; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_OFS_HttpLink: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task FillRecursive(List<Category> flatObjects, List<TreeNode> recursiveObjects, int? parentId = null)
        {
            // This method is recursive and needs to be carefully converted.
            // For now, returning a placeholder.
            await Task.CompletedTask;
        }

        public async Task<IActionResult> getuser_group_permision(int pageid, int applicationid, int userid)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> Getpage(string pagename)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> Ge_t(int id)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> getlist()
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<string> GENERATE_UNIQUE_CHEQUE_SEQUANCE(string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo)
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"GENERATE_UNIQUE_CHEQUE_SEQUANCE");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If GENERATE_UNIQUE_CHEQUE_SEQUANCE is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how GENERATE_UNIQUE_CHEQUE_SEQUANCE is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[GENERATE_UNIQUE_CHEQUE_SEQUANCE]({DrwChqNo}, {DrwBankNo}, {DrwBranchNo}, {DrwAcctNo})").FirstOrDefaultAsync();

                // Placeholder for actual logic or database call
                result = "UNIQUE_CHEQUE_SEQUANCE_" + DrwChqNo + "_" + DrwBankNo + "_" + DrwBranchNo + "_" + DrwAcctNo; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GENERATE_UNIQUE_CHEQUE_SEQUANCE: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task<IActionResult> getlockedpage(int pageid)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<string> EVALUATE_AMOUNT_IN_JOD(string CURANCY, double AMOUNT)
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"EVALUATE_AMOUNT_IN_JOD");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If EVALUATE_AMOUNT_IN_JOD is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how EVALUATE_AMOUNT_IN_JOD is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[EVALUATE_AMOUNT_IN_JOD]({CURANCY}, {AMOUNT})").FirstOrDefaultAsync();

                // Placeholder for actual logic or database call
                result = "EVALUATED_AMOUNT_" + CURANCY + "_" + AMOUNT; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in EVALUATE_AMOUNT_IN_JOD: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task<string> GetCurrencyCode(string Currency_Symbol)
        {
            string result = "";
            try
            {
                // Assuming _LogSystem is injected or available through a static helper
                // For now, we'll use a placeholder for logging.
                _logger.LogInformation($"GetCurrencyCode");

                // The original VB.NET code uses a custom CONNECTION class and direct SQL.
                // In ASP.NET Core with EF Core, we should use _context for database operations.
                // If GetCurrencyCode is a stored procedure or a database function, it needs to be mapped or called directly.
                // For demonstration, let's assume a direct SQL call if it's a database function.
                // This part needs careful conversion based on how GetCurrencyCode is implemented in the database.

                // Example if it's a raw SQL query (needs proper sanitization/parameterization in real app)
                // result = await _context.Database.SqlQuery<string>($"SELECT [DBO].[GetCurrencyCode]({Currency_Symbol})").FirstOrDefaultAsync();

                // Placeholder for actual logic or database call
                result = "CURRENCY_CODE_" + Currency_Symbol; // Replace with actual logic

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetCurrencyCode: {ex.Message}");
                return "Error"; // Or rethrow, or handle as appropriate
            }
        }

        public async Task<IActionResult> Update_ChqDate(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> Update_Out_ChqDate(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> Update_Out_ChqDate_Accept(string Serial, string BenName, string BenfAccBranch, string AcctType, string DrwChqNo, string DrwBankNo, string DrwBranchNo, string DrwAcctNo, double Amount, DateTime DueDate, string Currency, string BenfBnk, string BenfCardType, string BenfCardId, string BenAccountNo, string BenfNationality, string NeedTechnicalVerification, string WithUV, string SpecialHandling, string IsVIP, string DrwName)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> getSearchListDis_PMA(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }

        public async Task<IActionResult> getSearchListPDC(string Branchs, string STATUS, string BenAccNo, string AccType, string FromBank, string ToBank, string Currency, string ChequeSource, string Amount, string DRWAccNo, string ChequeNo, string waspdc)
        {
            // This method needs to be fully converted, including database access and logic.
            // For now, returning a placeholder.
            return new OkResult();
        }
    }
}
}

