using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFA_ECC_Core.Models;
using SAFA_ECC_Core.ViewModels.InwardViewModels;
using SAFA_ECC_Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace SAFA_ECC_Core.Controllers
{
    public class INWARDController : Controller
    {
        private readonly ILogger<INWARDController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly LogSystem _logSystem;
        private readonly ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient _eccAccInfoWebSvc;
        private readonly ECC_Handler_SVC.InwardHandlingSVCSoapClient _inwardHandlingSvc;

        public INWARDController(ILogger<INWARDController> logger, ApplicationDbContext context, LogSystem logSystem, ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient eccAccInfoWebSvc, ECC_Handler_SVC.InwardHandlingSVCSoapClient inwardHandlingSvc)
        {
            _logger = logger;
            _context = context;
            _logSystem = logSystem;
            _eccAccInfoWebSvc = eccAccInfoWebSvc;
            _inwardHandlingSvc = inwardHandlingSvc;
        }

        private string GetUserName() => HttpContext.Session.GetString("UserName");
        private string GetBranchID() => HttpContext.Session.GetString("BranchID");
        private string GetComID() => HttpContext.Session.GetString("ComID");
        private int GetUserID() => Convert.ToInt32(HttpContext.Session.GetString("ID"));

        public string GetCustomerDeathDate(string customerID)
        {
            string result = "";
            string _result = "";
            string _dateDeath = "";
            string _deathNotDate = "";

            Global_Parameter_TBL globalPa = _context.Global_Parameter_TBL.SingleOrDefault(i => i.Parameter_Name == "ACC_INFO_SVC");

            try
            {
                string _val = "";
                if (globalPa != null)
                {
                    _val = globalPa.Parameter_Value;
                    _val = _val.Replace("@CUSTOMER_ID@", customerID);
                    try
                    {
                        // AMANISAFA - This part needs actual implementation for OFS_Messages_Class and Execute_Ofs_Enqury
                        _result = ""; // ofs_obj.Execute_Ofs_Enqury(_val);

                        // Assuming OFS_MESSAGES_INFLATOR.T24_FILES.CUSTOMER and Fill_Customer_Class are implemented elsewhere
                        // _customer = customerofs.Fill_Customer_Class(_result);
                        // If (_customer != null)
                        // {
                        //     _dateDeath = _customer.GET_LOCAL_REFERENCE_VALUE(_customer.LOCAL_REFERENCE_FIELDS.DATE_DEATH);
                        //     if (string.IsNullOrEmpty(_dateDeath))
                        //     {
                        //         _deathNotDate = _customer.GET_LOCAL_REFERENCE_VALUE(_customer.LOCAL_REFERENCE_FIELDS.DEATH_NOT_DATE);
                        //         result = _deathNotDate;
                        //     }
                        //     else
                        //     {
                        //         result = _dateDeath;
                        //     }
                        // }
                    }
                    catch (Exception ex)
                    {
                        string loggMessage = $"Error Get Date of Death From OFS MSG err :  {ex.Message}";
                        _logSystem.WriteError(loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                        _logger.LogError(ex, loggMessage);
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    // This part needs a proper way to execute stored procedures or raw SQL in EF Core
                    // result = conn.Get_One_Data(" select[DBO].[GET_CUSTOMER_DEATH_DATE] (" & "'" & CustomerID & "')");
                    // For now, returning empty string or implementing a placeholder
                    result = ""; // Placeholder
                }

                return result;
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error Get Date of Death  err :  {ex.Message}";
                _logSystem.WriteError(loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                _logger.LogError(ex, loggMessage);
            }

            return result;
        }

        public ActionResult TestRes()
        {
            //amani
            // Dim cab As New SAFA_ECCEntities
            // Dim inw As New OnUs_Tbl
            // Dim obj_ As ECC_Handler_SVC.HandelResponseRequest()
            // Dim WebSvc As New ECC_Handler_SVC.InwardHandlingSVCSoapClient("InwardHandlingSVCSoap")
            string respone = "Insufficient Funds";
            string spicalnote = "";
            string userid = "9";
            DateTime from_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 13);
            List<Inward_Trans> list = new List<Inward_Trans>();
            string sql = @" select serial ,SUBSTRING( ErrorDescription,3,1000) as ErrorDescription from onus_tbl where cast(TransDate  as date)='20250216' 
          
             and ChqSequance not in (
             select ChqSequance   from INWARD_WF_Tbl  WHERE CAST (input_date  AS DATE) ='20250216' 
             )";
            
            string str1 = ConfigurationManager.AppSettings["CONNECTION_STR"];
            using (SqlConnection con = new SqlConnection(str1))
            {
                using (SqlCommand Com = new SqlCommand(sql, con))
                {
                    if (con.State != System.Data.ConnectionState.Open) con.Open();

                    using (SqlDataReader DR = Com.ExecuteReader())
                    {
                        while (DR.Read())
                        {
                            string des = DR["ErrorDescription"].ToString();
                            string serial = DR["Serial"].ToString();

                            OnUs_Tbl inw = _context.OnUs_Tbl.SingleOrDefault(i => i.Serial == serial);
                            if (inw != null)
                            {
                                // obj_ = _inwardHandlingSvc.HandelResponseONUS(inw.ChqSequance, inw.ClrCenter, des, spicalnote, inw.Serial, userid);
                                // Placeholder for actual service call
                            }
                        }
                    }
                }
            }

            string ss = "";
            return View(); // Or appropriate return type
        }

        // GET: INWORDController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Indextest(string id)
        {
            return View();
        }

        private static List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null)
        {
            try
            {
                return flatObjects.Where(x => x.Parent_ID.Equals(parentId))
                                  .Select(item => new TreeNode
                                  {
                                      SubMenu_Name_EN = item.SubMenu_Name_EN,
                                      SubMenu_ID = item.SubMenu_ID,
                                      Related_Page_ID = item.Related_Page_ID,
                                      Children = FillRecursive(flatObjects, item.SubMenu_ID)
                                  }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in FillRecursive: {ex.Message}");
                return new List<TreeNode>();
            }
        }

        public ActionResult InwardFinanicalWFDetailsPMADIS_NEW(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            JsonResult _json = new JsonResult(new { });
            // _json.Data = ""; // Not directly applicable in ASP.NET Core JsonResult
            int _step = 90000;
            _step += 5700;
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            string methodName = "InwardFinanicalWFDetailsPMADIS";
            _step += 1;
            // Group_Types_Tbl group_name = new Group_Types_Tbl(); // Not used
            int applicationid;
            int userid = GetUserID();
            // SAFA_ECCEntities _CAB = new SAFA_ECCEntities(); // Replaced by _context
            int pageid;
            string titel = "";
            pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == methodName).Page_Id;
            _step += 1;
            applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == methodName).Application_ID;
            _step += 1;
            // getuser_group_permision(pageid, applicationid, userid); // Needs implementation
            _step += 1;
            // If Session.Item("AccessPage") = "NoAccess" Then
            // Return RedirectToAction("block", "Login")
            // End If
            titel = _context.App_Pages.SingleOrDefault(c => c.Page_Id == pageid).ENG_DESC;
            _step += 1;
            ViewBag.Title = titel;
            // ViewBag.Tree = GetAllCategoriesForTree(); // Needs implementation
            _step += 1;
            int Tbl_id;
            int WFLevel; // Not used
            ECC_CAP_Services.AccountInfo_RESPONSE Accobj = new ECC_CAP_Services.AccountInfo_RESPONSE();
            _step += 1;
            // ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient("SAFA_T24_ECC_SVCSoap"); // Injected
            _step += 1;
            _step += 1;
            string branch = "";
            branch = GetBranchID();
            INChqs inChq = new INChqs(); // ViewModel for the view
            INWARD_WF_Tbl wf = new INWARD_WF_Tbl();
            // INWARD_IMAGES Img = new INWARD_IMAGES(); // Not used
            Inward_Trans incObj = new Inward_Trans();
            // List<CURRENCY_TBL> Currency = new List<CURRENCY_TBL>(); // Not used
            try
            {
                string _loggMessage = "Show Cheque InwardFinanicalWFDetailsPMADIS  it from Inward_Trans table ";
                _logSystem.WriteLogg(_loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                _logger.LogInformation(_loggMessage);

                // Using db As New SAFA_ECCEntities // Replaced by _context

                // WFLevel = db.INWARD_WF_Tbl.SingleOrDefault(Function(z) z.Serial = id).WF_LEVEL
                _step += 1;

                wf = _context.INWARD_WF_Tbl.SingleOrDefault(z => z.Serial == id && z.Final_Status != "Accept");
                _step += 1;

                string com = "";
                com = GetComID();
                incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);
                if (incObj.ClrCenter == "PMA")
                {
                    if (incObj.VIP == true && branch != "2")
                    {
                        ViewBag.is_vip = "YES";
                    }
                }

                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD");
                }
                _step += 1;
                List<T24_CAB_OVRDRWN_GUAR> GUAR_CUSTOMER = new List<T24_CAB_OVRDRWN_GUAR>();
                _step += 1;
                GUAR_CUSTOMER = _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount).ToList();
                _step += 1;
                ViewBag.GUAR_CUSTOMER = "";
                if (GUAR_CUSTOMER.Count == 0)
                {
                    ViewBag.GUAR_CUSTOMER = "Not Available";
                    inChq.GUAR_CUSTOMER = "Not Available";
                }
                else
                {
                    foreach (var item in GUAR_CUSTOMER)
                    {
                        ECC_CAP_Services.AccountInfo_RESPONSE GUAR_CUSTOMER_Accobj = _eccAccInfoWebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                        ViewBag.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                        inChq.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                    }
                }
                Accobj = _eccAccInfoWebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                _step += 1;

                inChq.BookedBalance = Accobj.BookedBalance;
                inChq.ClearBalance = Accobj.ClearBalance;
                inChq.AccountStatus = Accobj.AccountStatus;

                string USER_ = GetUserName();
                string group = "";
                group = _context.Users_Tbl.SingleOrDefault(c => c.User_Name == USER_).Group_ID.ToString(); // Assuming Group_ID is string or converted

                ViewBag.Reject = "False";
                inChq.Reject = "False";

                ViewBag.recomdationbtn = "True";
                inChq.Recom = "True";

                if (group == "AdminAuthorized" || branch == "2") // Assuming GroupType.Group_Status.AdminAuthorized is a string "AdminAuthorized"
                {
                    ViewBag.Approve = "True";
                    inChq.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                    inChq.Recom = "False";
                }
                else
                {
                    List<USER_Limits_Auth_Amount_Result> Userlevel = new List<USER_Limits_Auth_Amount_Result>(); // Placeholder
                    Tbl_id = _context.Transaction_TBL.SingleOrDefault(z => z.Transaction_Name == wf.Clr_Center).Transaction_ID;
                    _step += 1;
                    try
                    {
                        // Userlevel = _context.USER_Limits_Auth_Amount(userid, Tbl_id, "d", wf.Amount_JD).ToList(); // Stored procedure call
                        _step += 1;

                        // Dim _Userlevel = Userlevel(1).LEVEL  '1
                        // Dim _Userleve2 = Userlevel(2).LEVEL  '2
                        // Dim _Userleve3 = Userlevel(3).LEVEL  '3       
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error calling USER_Limits_Auth_Amount");
                    }

                    ViewBag.recomdationbtn = "True";
                    inChq.Recom = "True";

                    string user_name_wf = "";
                    string user_name_site = GetUserName();
                    if (wf.Level1_status != null)
                    {
                        // user_name_wf = wf.Level1_status.Split(":")(0);
                        user_name_wf = wf.Level1_status;
                    }

                    try
                    {
                        int _GroupID = 4;
                        Users_Tbl _Usr = _context.Users_Tbl.SingleOrDefault(u => u.User_ID == userid);
                        if (_Usr != null)
                        {
                            _GroupID = _Usr.Group_ID;
                        }

                        AuthTrans_User_TBL x = new AuthTrans_User_TBL();

                        if (incObj.ClrCenter == "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "5" && t.group_ID == _GroupID);
                        }

                        if (incObj.ClrCenter != "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "6" && t.group_ID == _GroupID);
                        }

                        if (x != null)
                        {
                            if (x.Auth_Level == 1)
                            {
                                ViewBag.Approve = "True";
                                inChq.Approve = "True";
                                ViewBag.recomdationbtn = "False";
                                inChq.Recom = "False";
                            }
                            else if (x.Auth_Level == 2)
                            {
                                if (wf.Level1_status != null && wf.Level1_status.Contains(user_name_site))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                            }
                            else if (x.Auth_Level == 3)
                            {
                                if (wf.Level1_status != null && wf.Level1_status.Contains(user_name_site) && wf.Level2_status != null && wf.Level2_status.Contains(user_name_site))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error checking user authorization");
                    }
                }

                // Populate inChq from incObj and other sources
                inChq.Serial = incObj.Serial;
                inChq.ChqSerial = incObj.ChqSerial;
                inChq.ChqNumber = incObj.ChqNumber;
                inChq.Amount = incObj.Amount;
                inChq.Amount_JD = incObj.Amount_JD;
                inChq.Currency = incObj.Currency;
                inChq.BeneficiaryName = incObj.BeneficiaryName;
                inChq.ISSAccount = incObj.ISSAccount;
                inChq.ISSName = incObj.ISSName;
                inChq.BankName = incObj.BankName;
                inChq.BranchName = incObj.BranchName;
                inChq.ClrCenter = incObj.ClrCenter;
                inChq.TransDate = incObj.TransDate;
                inChq.ValueDate = incObj.ValueDate;
                inChq.Reason = incObj.Reason;
                inChq.Status = incObj.Status;
                inChq.ErrorDescription = incObj.ErrorDescription;
                inChq.PayerBank = incObj.PayerBank;
                inChq.PayerBranch = incObj.PayerBranch;
                inChq.PayerName = incObj.PayerName;
                inChq.PayerAccount = incObj.PayerAccount;
                inChq.PayerID = incObj.PayerID;
                inChq.PayerMobile = incObj.PayerMobile;
                inChq.PayerAddress = incObj.PayerAddress;
                inChq.PayerEmail = incObj.PayerEmail;
                inChq.Posted = incObj.Posted;
                inChq.AltAccount = incObj.AltAccount;
                inChq.VIP = incObj.VIP;
                inChq.Auth_Amount = incObj.Auth_Amount;
                inChq.Auth_Level = incObj.Auth_Level;
                inChq.Auth_Status = incObj.Auth_Status;
                inChq.Auth_User = incObj.Auth_User;
                inChq.Auth_Date = incObj.Auth_Date;
                inChq.Auth_Reason = incObj.Auth_Reason;
                inChq.Auth_Note = incObj.Auth_Note;
                inChq.Auth_Type = incObj.Auth_Type;
                inChq.Auth_Time = incObj.Auth_Time;
                inChq.Auth_IP = incObj.Auth_IP;
                inChq.Auth_MAC = incObj.Auth_MAC;
                inChq.Auth_Host = incObj.Auth_Host;
                inChq.Auth_Browser = incObj.Auth_Browser;
                inChq.Auth_OS = incObj.Auth_OS;
                inChq.Auth_Device = incObj.Auth_Device;
                inChq.Auth_Location = incObj.Auth_Location;
                inChq.Auth_Latitude = incObj.Auth_Latitude;
                inChq.Auth_Longitude = incObj.Auth_Longitude;
                inChq.Auth_Accuracy = incObj.Auth_Accuracy;
                inChq.Auth_Altitude = incObj.Auth_Altitude;
                inChq.Auth_AltitudeAccuracy = incObj.Auth_AltitudeAccuracy;
                inChq.Auth_Heading = incObj.Auth_Heading;
                inChq.Auth_Speed = incObj.Auth_Speed;
                inChq.Auth_Timestamp = incObj.Auth_Timestamp;
                inChq.Auth_Error = incObj.Auth_Error;
                inChq.Auth_ErrorMessage = incObj.Auth_ErrorMessage;
                inChq.Auth_ErrorCode = incObj.Auth_ErrorCode;
                inChq.Auth_ErrorType = incObj.Auth_ErrorType;
                inChq.Auth_ErrorTime = incObj.Auth_ErrorTime;
                inChq.Auth_ErrorIP = incObj.Auth_ErrorIP;
                inChq.Auth_ErrorMAC = incObj.Auth_ErrorMAC;
                inChq.Auth_ErrorHost = incObj.Auth_ErrorHost;
                inChq.Auth_ErrorBrowser = incObj.Auth_ErrorBrowser;
                inChq.Auth_ErrorOS = incObj.Auth_ErrorOS;
                inChq.Auth_ErrorDevice = incObj.Auth_ErrorDevice;
                inChq.Auth_ErrorLocation = incObj.Auth_ErrorLocation;
                inChq.Auth_ErrorLatitude = incObj.Auth_ErrorLatitude;
                inChq.Auth_ErrorLongitude = incObj.Auth_ErrorLongitude;
                inChq.Auth_ErrorAccuracy = incObj.Auth_ErrorAccuracy;
                inChq.Auth_ErrorAltitude = incObj.Auth_ErrorAltitude;
                inChq.Auth_ErrorAltitudeAccuracy = incObj.Auth_ErrorAltitudeAccuracy;
                inChq.Auth_ErrorHeading = incObj.Auth_ErrorHeading;
                inChq.Auth_ErrorSpeed = incObj.Auth_ErrorSpeed;
                inChq.Auth_ErrorTimestamp = incObj.Auth_ErrorTimestamp;

                // Images
                List<INWARD_IMAGES> images = _context.INWARD_IMAGES.Where(x => x.Serial == id).ToList();
                ViewBag.Images = images;

                return View(inChq);
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error InwardFinanicalWFDetailsPMADIS_NEW: {ex.Message}";
                _logSystem.WriteError(loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                _logger.LogError(ex, loggMessage);
                return RedirectToAction("Error", "Home"); // Or handle error appropriately
            }
        }

        public ActionResult InwardFinanicalWFDetailsPMADIS(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            JsonResult _json = new JsonResult(new { });
            int _step = 90000;
            _step += 5700;
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            string methodName = "InwardFinanicalWFDetailsPMADIS";
            _step += 1;
            int applicationid;
            int userid = GetUserID();
            int pageid;
            string titel = "";
            pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == methodName).Page_Id;
            _step += 1;
            applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == methodName).Application_ID;
            _step += 1;
            // getuser_group_permision(pageid, applicationid, userid); // Needs implementation
            _step += 1;
            titel = _context.App_Pages.SingleOrDefault(c => c.Page_Id == pageid).ENG_DESC;
            _step += 1;
            ViewBag.Title = titel;
            // ViewBag.Tree = GetAllCategoriesForTree(); // Needs implementation
            _step += 1;
            int Tbl_id;
            int WFLevel; // Not used
            ECC_CAP_Services.AccountInfo_RESPONSE Accobj = new ECC_CAP_Services.AccountInfo_RESPONSE();
            _step += 1;
            _step += 1;
            _step += 1;
            string branch = GetBranchID();
            INChqs inChq = new INChqs();
            INWARD_WF_Tbl wf = new INWARD_WF_Tbl();
            Inward_Trans incObj = new Inward_Trans();
            try
            {
                string _loggMessage = "Show Cheque InwardFinanicalWFDetailsPMADIS  it from Inward_Trans table ";
                _logSystem.WriteLogg(_loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                _logger.LogInformation(_loggMessage);

                wf = _context.INWARD_WF_Tbl.SingleOrDefault(z => z.Serial == id && z.Final_Status != "Accept");
                _step += 1;

                string com = GetComID();
                incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);
                if (incObj.ClrCenter == "PMA")
                {
                    if (incObj.VIP == true && branch != "2")
                    {
                        ViewBag.is_vip = "YES";
                    }
                }

                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD");
                }
                _step += 1;
                List<T24_CAB_OVRDRWN_GUAR> GUAR_CUSTOMER = _context.T24_CAB_OVRDRWN_GUAR.Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount).ToList();
                _step += 1;
                ViewBag.GUAR_CUSTOMER = "";
                if (GUAR_CUSTOMER.Count == 0)
                {
                    ViewBag.GUAR_CUSTOMER = "Not Available";
                    inChq.GUAR_CUSTOMER = "Not Available";
                }
                else
                {
                    foreach (var item in GUAR_CUSTOMER)
                    {
                        ECC_CAP_Services.AccountInfo_RESPONSE GUAR_CUSTOMER_Accobj = _eccAccInfoWebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                        ViewBag.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                        inChq.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                    }
                }
                Accobj = _eccAccInfoWebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                _step += 1;

                inChq.BookedBalance = Accobj.BookedBalance;
                inChq.ClearBalance = Accobj.ClearBalance;
                inChq.AccountStatus = Accobj.AccountStatus;

                string USER_ = GetUserName();
                string group = _context.Users_Tbl.SingleOrDefault(c => c.User_Name == USER_).Group_ID.ToString();

                ViewBag.Reject = "False";
                inChq.Reject = "False";

                ViewBag.recomdationbtn = "True";
                inChq.Recom = "True";

                if (group == "AdminAuthorized" || branch == "2")
                {
                    ViewBag.Approve = "True";
                    inChq.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                    inChq.Recom = "False";
                }
                else
                {
                    List<USER_Limits_Auth_Amount_Result> Userlevel = new List<USER_Limits_Auth_Amount_Result>(); // Placeholder
                    Tbl_id = _context.Transaction_TBL.SingleOrDefault(z => z.Transaction_Name == wf.Clr_Center).Transaction_ID;
                    _step += 1;
                    try
                    {
                        // Userlevel = _context.USER_Limits_Auth_Amount(userid, Tbl_id, "d", wf.Amount_JD).ToList(); // Stored procedure call
                        _step += 1;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error calling USER_Limits_Auth_Amount");
                    }

                    ViewBag.recomdationbtn = "True";
                    inChq.Recom = "True";

                    string user_name_wf = "";
                    string user_name_site = GetUserName();
                    if (wf.Level1_status != null)
                    {
                        user_name_wf = wf.Level1_status;
                    }

                    try
                    {
                        int _GroupID = 4;
                        Users_Tbl _Usr = _context.Users_Tbl.SingleOrDefault(u => u.User_ID == userid);
                        if (_Usr != null)
                        {
                            _GroupID = _Usr.Group_ID;
                        }

                        AuthTrans_User_TBL x = new AuthTrans_User_TBL();

                        if (incObj.ClrCenter == "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "5" && t.group_ID == _GroupID);
                        }

                        if (incObj.ClrCenter != "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "6" && t.group_ID == _GroupID);
                        }

                        if (x != null)
                        {
                            if (x.Auth_Level == 1)
                            {
                                ViewBag.Approve = "True";
                                inChq.Approve = "True";
                                ViewBag.recomdationbtn = "False";
                                inChq.Recom = "False";
                            }
                            else if (x.Auth_Level == 2)
                            {
                                if (wf.Level1_status != null && wf.Level1_status.Contains(user_name_site))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                            }
                            else if (x.Auth_Level == 3)
                            {
                                if (wf.Level1_status != null && wf.Level1_status.Contains(user_name_site) && wf.Level2_status != null && wf.Level2_status.Contains(user_name_site))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error checking user authorization");
                    }
                }

                // Populate inChq from incObj and other sources
                inChq.Serial = incObj.Serial;
                inChq.ChqSerial = incObj.ChqSerial;
                inChq.ChqNumber = incObj.ChqNumber;
                inChq.Amount = incObj.Amount;
                inChq.Amount_JD = incObj.Amount_JD;
                inChq.Currency = incObj.Currency;
                inChq.BeneficiaryName = incObj.BeneficiaryName;
                inChq.ISSAccount = incObj.ISSAccount;
                inChq.ISSName = incObj.ISSName;
                inChq.BankName = incObj.BankName;
                inChq.BranchName = incObj.BranchName;
                inChq.ClrCenter = incObj.ClrCenter;
                inChq.TransDate = incObj.TransDate;
                inChq.ValueDate = incObj.ValueDate;
                inChq.Reason = incObj.Reason;
                inChq.Status = incObj.Status;
                inChq.ErrorDescription = incObj.ErrorDescription;
                inChq.PayerBank = incObj.PayerBank;
                inChq.PayerBranch = incObj.PayerBranch;
                inChq.PayerName = incObj.PayerName;
                inChq.PayerAccount = incObj.PayerAccount;
                inChq.PayerID = incObj.PayerID;
                inChq.PayerMobile = incObj.PayerMobile;
                inChq.PayerAddress = incObj.PayerAddress;
                inChq.PayerEmail = incObj.PayerEmail;
                inChq.Posted = incObj.Posted;
                inChq.AltAccount = incObj.AltAccount;
                inChq.VIP = incObj.VIP;
                inChq.Auth_Amount = incObj.Auth_Amount;
                inChq.Auth_Level = incObj.Auth_Level;
                inChq.Auth_Status = incObj.Auth_Status;
                inChq.Auth_User = incObj.Auth_User;
                inChq.Auth_Date = incObj.Auth_Date;
                inChq.Auth_Reason = incObj.Auth_Reason;
                inChq.Auth_Note = incObj.Auth_Note;
                inChq.Auth_Type = incObj.Auth_Type;
                inChq.Auth_Time = incObj.Auth_Time;
                inChq.Auth_IP = incObj.Auth_IP;
                inChq.Auth_MAC = incObj.Auth_MAC;
                inChq.Auth_Host = incObj.Auth_Host;
                inChq.Auth_Browser = incObj.Auth_Browser;
                inChq.Auth_OS = incObj.Auth_OS;
                inChq.Auth_Device = incObj.Auth_Device;
                inChq.Auth_Location = incObj.Auth_Location;
                inChq.Auth_Latitude = incObj.Auth_Latitude;
                inChq.Auth_Longitude = incObj.Auth_Longitude;
                inChq.Auth_Accuracy = incObj.Auth_Accuracy;
                inChq.Auth_Altitude = incObj.Auth_Altitude;
                inChq.Auth_AltitudeAccuracy = incObj.Auth_AltitudeAccuracy;
                inChq.Auth_Heading = incObj.Auth_Heading;
                inChq.Auth_Speed = incObj.Auth_Speed;
                inChq.Auth_Timestamp = incObj.Auth_Timestamp;
                inChq.Auth_Error = incObj.Auth_Error;
                inChq.Auth_ErrorMessage = incObj.Auth_ErrorMessage;
                inChq.Auth_ErrorCode = incObj.Auth_ErrorCode;
                inChq.Auth_ErrorType = incObj.Auth_ErrorType;
                inChq.Auth_ErrorTime = incObj.Auth_ErrorTime;
                inChq.Auth_ErrorIP = incObj.Auth_ErrorIP;
                inChq.Auth_ErrorMAC = incObj.Auth_ErrorMAC;
                inChq.Auth_ErrorHost = incObj.Auth_ErrorHost;
                inChq.Auth_ErrorBrowser = incObj.Auth_ErrorBrowser;
                inChq.Auth_ErrorOS = incObj.Auth_ErrorOS;
                inChq.Auth_ErrorDevice = incObj.Auth_ErrorDevice;
                inChq.Auth_ErrorLocation = incObj.Auth_ErrorLocation;
                inChq.Auth_ErrorLatitude = incObj.Auth_ErrorLatitude;
                inChq.Auth_ErrorLongitude = incObj.Auth_ErrorLongitude;
                inChq.Auth_ErrorAccuracy = incObj.Auth_ErrorAccuracy;
                inChq.Auth_ErrorAltitude = incObj.Auth_ErrorAltitude;
                inChq.Auth_ErrorAltitudeAccuracy = incObj.Auth_AltitudeAccuracy;
                inChq.Auth_ErrorHeading = incObj.Auth_ErrorHeading;
                inChq.Auth_ErrorSpeed = incObj.Auth_Speed;
                inChq.Auth_ErrorTimestamp = incObj.Auth_ErrorTimestamp;

                // Images
                List<INWARD_IMAGES> images = _context.INWARD_IMAGES.Where(x => x.Serial == id).ToList();
                ViewBag.Images = images;

                return View(inChq);
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error InwardFinanicalWFDetailsPMADIS: {ex.Message}";
                _logSystem.WriteError(loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                _logger.LogError(ex, loggMessage);
                return RedirectToAction("Error", "Home"); // Or handle error appropriately
            }
        }

        [HttpPost]
        public async Task<JsonResult> InwardFinanicalWFDetailsPMADIS_Auth(InwardFinanicalWFDetailsPMADIS_AuthViewModel model)
        {
            string _Logg_Message = "";
            string _ApplicationID = "1";
            string _UserName = GetUserName();
            string _BranchID = GetBranchID();
            int _UserID = GetUserID();

            try
            {
                if (string.IsNullOrEmpty(_UserName))
                {
                    return Json(new { status = "Error", message = "Session Expired" });
                }

                INWARD_WF_Tbl wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == model.Serial);
                if (wf == null)
                {
                    return Json(new { status = "Error", message = "Workflow entry not found." });
                }

                Inward_Trans incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == model.Serial);
                if (incObj == null)
                {
                    return Json(new { status = "Error", message = "Inward Transaction not found." });
                }

                // Update workflow status
                wf.Final_Status = model.Status;
                wf.Auth_User = _UserName;
                wf.Auth_Date = DateTime.Now;
                wf.Auth_Reason = model.Reason;
                wf.Auth_Note = model.Note;
                wf.Auth_Level = model.AuthLevel;
                wf.Auth_Type = model.AuthType;
                wf.Auth_Time = DateTime.Now.TimeOfDay;
                wf.Auth_IP = HttpContext.Connection.RemoteIpAddress?.ToString();
                // Add other auth details as needed

                // Update Inward_Trans status
                incObj.Auth_Status = model.Status;
                incObj.Auth_User = _UserName;
                incObj.Auth_Date = DateTime.Now;
                incObj.Auth_Reason = model.Reason;
                incObj.Auth_Note = model.Note;
                incObj.Auth_Level = model.AuthLevel;
                incObj.Auth_Type = model.AuthType;
                incObj.Auth_Time = DateTime.Now.TimeOfDay;
                incObj.Auth_IP = HttpContext.Connection.RemoteIpAddress?.ToString();
                // Add other auth details as needed

                await _context.SaveChangesAsync();

                _Logg_Message = $"InwardFinanicalWFDetailsPMADIS_Auth: Serial {model.Serial} Status {model.Status}";
                _logSystem.WriteLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, _UserName, _UserName, "", "", "");
                _logger.LogInformation(_Logg_Message);

                return Json(new { status = "Success", message = "Authorization updated successfully." });
            }
            catch (Exception ex)
            {
                _Logg_Message = $"Error InwardFinanicalWFDetailsPMADIS_Auth: {ex.Message}";
                _logSystem.WriteError(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, _UserName, _UserName, "", "", "");
                _logger.LogError(ex, _Logg_Message);
                return Json(new { status = "Error", message = "An error occurred during authorization." });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ReversePostingPMARAM(string serial)
        {
            string _Logg_Message = "";
            string _ApplicationID = "1";
            string _UserName = GetUserName();
            string _BranchID = GetBranchID();

            try
            {
                if (string.IsNullOrEmpty(_UserName))
                {
                    return Json(new { status = "Error", message = "Session Expired" });
                }

                INWARD_WF_Tbl wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == serial);
                if (wf == null)
                {
                    return Json(new { status = "Error", message = "Workflow entry not found." });
                }

                Inward_Trans incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == serial);
                if (incObj == null)
                {
                    return Json(new { status = "Error", message = "Inward Transaction not found." });
                }

                // Call external service
                ECC_Handler_SVC.HandelResponseRequest[] obj_ = _inwardHandlingSvc.HandelResponseONUS(incObj.ChqSequance, incObj.ClrCenter, "Reverse Posting", "", incObj.Serial, _UserName);

                // Update status based on service response if needed
                // For now, assuming success if no exception
                wf.Final_Status = "Reversed";
                incObj.Status = "Reversed";
                incObj.Posted = 0; // Assuming 0 means not posted

                await _context.SaveChangesAsync();

                _Logg_Message = $"ReversePostingPMARAM: Serial {serial} Reversed successfully.";
                _logSystem.WriteLogg(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, _UserName, _UserName, "", "", "");
                _logger.LogInformation(_Logg_Message);

                return Json(new { status = "Success", message = "Reverse posting initiated successfully." });
            }
            catch (Exception ex)
            {
                _Logg_Message = $"Error ReversePostingPMARAM: {ex.Message}";
                _logSystem.WriteError(_Logg_Message, _ApplicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, _UserName, _UserName, "", "", "");
                _logger.LogError(ex, _Logg_Message);
                return Json(new { status = "Error", message = "An error occurred during reverse posting." });
            }
        }

        public ActionResult Get_Inward_Trans_Details(string serial)
        {
            try
            {
                if (string.IsNullOrEmpty(GetUserName()))
                {
                    return RedirectToAction("Login", "Login");
                }

                Inward_Trans inwardTrans = _context.Inward_Trans.SingleOrDefault(t => t.Serial == serial);
                if (inwardTrans == null)
                {
                    return Json(new { status = "Error", message = "Inward Transaction not found." });
                }

                // You might want to return a ViewModel here instead of the raw model
                return Json(new { status = "Success", data = inwardTrans });
            }
            catch (Exception ex)
            {
                string loggMessage = $"Error Get_Inward_Trans_Details: {ex.Message}";
                _logSystem.WriteError(loggMessage, "1", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                _logger.LogError(ex, loggMessage);
                return Json(new { status = "Error", message = "An error occurred while retrieving transaction details." });
            }
        }
    }
}





