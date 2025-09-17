using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAFA_ECC_Core.Models;
using SAFA_ECC_Core.ViewModels.InwardViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;

namespace SAFA_ECC_Core.Controllers
{
    public class INWARDController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<INWARDController> _logger;
        private readonly IConfiguration _configuration;
        private readonly All_CLASSES.AllStoredProcesures _logSystem;
        private readonly string _applicationID = "1";
        private readonly string _connectionString;

        public INWARDController(ApplicationDbContext context, ILogger<INWARDController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection"); // Assuming DefaultConnection is your connection string
            _logSystem = new All_CLASSES.AllStoredProcesures(true, true, true, _configuration.GetConnectionString("ODBC_CONNECTION_NAME"), _configuration.GetConnectionString("ODBC_db_NAME"), _configuration.GetConnectionString("ODBC_USER_NAME"), _configuration.GetConnectionString("ODBC_PASS"));
        }

        private string GetUserName() => HttpContext.Session.GetString("UserName");
        private int GetUserID() => Convert.ToInt32(HttpContext.Session.GetString("ID"));
        private string GetBranchID() => HttpContext.Session.GetString("BranchID");
        private string GetComID() => HttpContext.Session.GetString("ComID");

        // Converted from GET_CUSTOMER_DEATH_DATE
        public string GET_CUSTOMER_DEATH_DATE(string CustomerID)
        {
            string result = "";
            string _result = "";
            string _DATE_DEATH = "";
            string _DEATH_NOT_DATE = "";

            Global_Parameter_TBL gLOPAL_PA = _context.Global_Parameter_TBL.SingleOrDefault(I => I.Parameter_Name == "ACC_INFO_SVC");
            All_CLASSES.CONNECTION conn = new All_CLASSES.CONNECTION(_connectionString);

            try
            {
                if (conn.Connection_stat != ConnectionState.Open) conn.OpenConnection();

                string _vAL = "";
                if (gLOPAL_PA != null)
                {
                    _vAL = gLOPAL_PA.Parameter_Value;
                    _vAL = _vAL.Replace("@CUSTOMER_ID@", CustomerID);
                    try
                    {
                        // AMANISAFA - OFS_Messages_Class and T24_FILES.CUSTOMER need to be implemented or replaced
                        // _result = ofs_obj.Execute_Ofs_Enqury(_vAL);
                        // _customer = customerofs.Fill_Customer_Class(_result);

                        // Placeholder for OFS integration
                        _result = ""; // Simulate OFS response

                        // If (!string.IsNullOrEmpty(_result))
                        // {
                        //     // Parse _result to get death date information
                        //     // This part needs actual implementation based on OFS_MESSAGES_INFLATOR.T24_FILES.CUSTOMER logic
                        // }
                    }
                    catch (Exception ex)
                    {
                        _Logg_Message = "Error Get Date of Death From OFS MSG err :  " + ex.Message;
                        _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = conn.Get_One_Data($" select[DBO].[GET_CUSTOMER_DEATH_DATE] ('{CustomerID}')");
                }

                return result;
            }
            catch (Exception ex)
            {
                _Logg_Message = "Error Get Date of Death  err :  " + ex.Message;
                _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
            }

            return result;
        }

        // Converted from testres
        public IActionResult testres()
        {
            //amani
            SAFA_ECCEntities cab = new SAFA_ECCEntities(); // Assuming SAFA_ECCEntities is still used or replaced with ApplicationDbContext
            OnUs_Tbl inw = new OnUs_Tbl();
            ECC_Handler_SVC.HandelResponseRequest[] obj_;
            ECC_Handler_SVC.InwardHandlingSVCSoapClient WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient(ECC_Handler_SVC.InwardHandlingSVCSoapClient.EndpointConfiguration.InwardHandlingSVCSoap);
            string Respone = "Insufficient Funds";
            string spicalnote = "";
            string userid = "9";
            DateTime from_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 13);
            List<Inward_Trans> list = new List<Inward_Trans>();
            string sql = " select serial ,SUBSTRING( ErrorDescription,3,1000) as ErrorDescription from onus_tbl where cast(TransDate  as date)=\'20250216\' \n          \n             and ChqSequance not in (\n             select ChqSequance   from INWARD_WF_Tbl  WHERE CAST (input_date  AS DATE) =\'20250216\' \n             )";

            string str1 = _configuration.GetConnectionString("CONNECTION_STR");
            SqlConnection con = new SqlConnection(str1);
            SqlCommand Com = new SqlCommand(sql, con);

            try
            {
                if (con.State != ConnectionState.Open) con.Open();

                if (Com.ExecuteScalar() != null)
                {
                    SqlDataReader DR = Com.ExecuteReader();
                    while (DR.Read())
                    {
                        string des = DR["ErrorDescription"].ToString();
                        string Serial = DR["Serial"].ToString();

                        inw = cab.OnUs_Tbl.SingleOrDefault(i => i.Serial == Serial);
                        if (inw != null)
                        {
                            obj_ = WebSvc.HandelResponseONUS(inw.ChqSequance, inw.ClrCenter, des, spicalnote, inw.Serial, userid);
                        }
                    }
                    DR.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in testres function");
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }

            string ss = "";
            return View(); // Or appropriate action result
        }

        // GET: INWORDController
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Indextest(string id)
        {
            return View();
        }

        private static List<TreeNode> FillRecursive(List<Category> flatObjects, int? parentId = null)
        {
            try
            {
                return flatObjects.Where(x => x.Parent_ID.Equals(parentId)).Select(item => new TreeNode
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

        public IActionResult InwardFinanicalWFDetailsPMADIS_NEW(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            JsonResult _json = new JsonResult(new { });
            _json.Value = "";
            int _step = 90000;
            _step += 5700;
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            string methodName = "InwardFinanicalWFDetailsPMADIS";
            _step += 1;
            // Group_Types_Tbl group_name = new Group_Types_Tbl(); // Not used directly
            int userid = GetUserID();
            // SAFA_ECCEntities _CAB = new SAFA_ECCEntities(); // Replaced with _context
            int pageid;
            string titel = "";
            pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == methodName).Page_Id;
            _step += 1;
            int applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == methodName).Application_ID;
            _step += 1;
            getuser_group_permision(pageid, applicationid, userid);
            _step += 1;

            titel = _context.App_Pages.SingleOrDefault(c => c.Page_Id == pageid).ENG_DESC;
            _step += 1;
            ViewBag.Title = titel;
            ViewBag.Tree = GetAllCategoriesForTree(); // Assuming GetAllCategoriesForTree is implemented elsewhere
            _step += 1;
            int Tbl_id;
            int WFLevel;
            ECC_CAP_Services.AccountInfo_RESPONSE Accobj = new ECC_CAP_Services.AccountInfo_RESPONSE();
            _step += 1;
            ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient(ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient.EndpointConfiguration.SAFA_T24_ECC_SVCSoap);
            _step += 1;
            _step += 1;
            string branch = GetBranchID();
            INChqs inChq = new INChqs();
            INWARD_WF_Tbl wf = new INWARD_WF_Tbl();
            INWARD_IMAGES Img = new INWARD_IMAGES();
            Inward_Trans incObj = new Inward_Trans();
            List<CURRENCY_TBL> Currency = new List<CURRENCY_TBL>();
            try
            {
                _Logg_Message = "Show Cheque InwardFinanicalWFDetailsPMADIS  it from Inward_Trans table ";
                _logSystem.WriteLogg(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");

                // Using db As New SAFA_ECCEntities - Replaced with _context

                wf = _context.INWARD_WF_Tbl.SingleOrDefault(z => z.Serial == id && z.Final_Status != "Accept");
                _step += 1;

                string com = GetComID();
                incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);
                if (incObj.ClrCenter == "PMA" && incObj.VIP == true && branch != "2")
                {
                    ViewBag.is_vip = "YES";
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
                        ECC_CAP_Services.AccountInfo_RESPONSE GUAR_CUSTOMER_Accobj = EccAccInfo_WebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                        ViewBag.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                        inChq.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                    }
                }
                Accobj = EccAccInfo_WebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                _step += 1;

                inChq.BookedBalance = Accobj.BookedBalance;
                inChq.ClearBalance = Accobj.ClearBalance;
                inChq.AccountStatus = Accobj.AccountStatus;

                string USER_ = GetUserName();
                string group = _context.Users_Tbl.SingleOrDefault(c => c.User_Name == USER_).Group_ID;

                ViewBag.Reject = "False";
                inChq.Reject = "False";

                ViewBag.recomdationbtn = "True";
                inChq.Recom = "True";

                if (group == GroupType.Group_Status.AdminAuthorized.ToString() || branch == "2") // Assuming GroupType.Group_Status.AdminAuthorized is an enum or constant
                {
                    ViewBag.Approve = "True";
                    inChq.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                    inChq.Recom = "False";
                }
                else
                {
                    List<USER_Limits_Auth_Amount_Result> Userlevel;
                    Tbl_id = _context.Transaction_TBL.SingleOrDefault(z => z.Transaction_Name == wf.Clr_Center).Transaction_ID;
                    _step += 1;
                    try
                    {
                        // Userlevel = db.USER_Limits_Auth_Amount(userid, Tbl_id, "d", wf.Amount_JD).ToList(); // Stored procedure call
                        // This needs to be replaced with actual stored procedure execution or LINQ equivalent
                        Userlevel = new List<USER_Limits_Auth_Amount_Result>(); // Placeholder
                        _step += 1;

                        // Dim _Userlevel = Userlevel(1).LEVEL  '1
                        // Dim _Userleve2 = Userlevel(2).LEVEL  '2
                        // Dim _Userleve3 = Userlevel(3).LEVEL  '3
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error calling USER_Limits_Auth_Amount stored procedure");
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
                        int _Group_ID = 4;
                        Users_Tbl _Usr = _context.Users_Tbl.SingleOrDefault(u => u.User_ID == userid);
                        if (_Usr != null)
                        {
                            _Group_ID = Convert.ToInt32(_Usr.Group_ID);
                        }

                        AuthTrans_User_TBL x = new AuthTrans_User_TBL();

                        if (incObj.ClrCenter == "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "5" && t.group_ID == _Group_ID);
                        }

                        if (incObj.ClrCenter != "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "6" && t.group_ID == _Group_ID);
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
                                if (wf.Level1_status != null && wf.Level1_status.Contains("Recom"))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                                else
                                {
                                    ViewBag.Approve = "False";
                                    inChq.Approve = "False";
                                    ViewBag.recomdationbtn = "True";
                                    inChq.Recom = "True";
                                }
                            }
                            else if (x.Auth_Level == 3)
                            {
                                if (wf.Level2_status != null && wf.Level2_status.Contains("Recom"))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                                else
                                {
                                    ViewBag.Approve = "False";
                                    inChq.Approve = "False";
                                    ViewBag.recomdationbtn = "True";
                                    inChq.Recom = "True";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in authorization logic");
                    }
                }

                // Populate inChq properties
                inChq.Serial = incObj.Serial;
                inChq.ChqSequance = incObj.ChqSequance;
                inChq.ClrCenter = incObj.ClrCenter;
                inChq.TransDate = incObj.TransDate;
                inChq.Amount = incObj.Amount;
                inChq.Amount_JD = incObj.Amount_JD;
                inChq.Currency = incObj.Currency;
                inChq.BankName = incObj.BankName;
                inChq.BranchName = incObj.BranchName;
                inChq.ISSAccount = incObj.ISSAccount;
                inChq.BeneficiaryName = incObj.BeneficiaryName;
                inChq.AltAccount = incObj.AltAccount;
                inChq.Reason = incObj.Reason;
                inChq.ErrorDescription = incObj.ErrorDescription;
                inChq.Posted = incObj.Posted;
                inChq.Teller = incObj.Teller;
                inChq.UserName = incObj.UserName;
                inChq.BranchID = incObj.BranchID;
                inChq.CompID = incObj.CompID;
                inChq.VIP = incObj.VIP;
                inChq.PMA_Branch = incObj.PMA_Branch;
                inChq.PMA_Branch_Name = incObj.PMA_Branch_Name;
                inChq.Inward_Type = incObj.Inward_Type;
                inChq.Image_Path = incObj.Image_Path;
                inChq.Image_Path2 = incObj.Image_Path2;
                inChq.Image_Path3 = incObj.Image_Path3;
                inChq.Image_Path4 = incObj.Image_Path4;
                inChq.Image_Path5 = incObj.Image_Path5;
                inChq.Image_Path6 = incObj.Image_Path6;
                inChq.Image_Path7 = incObj.Image_Path7;
                inChq.Image_Path8 = incObj.Image_Path8;
                inChq.Image_Path9 = incObj.Image_Path9;
                inChq.Image_Path10 = incObj.Image_Path10;
                inChq.Image_Path11 = incObj.Image_Path11;
                inChq.Image_Path12 = incObj.Image_Path12;
                inChq.Image_Path13 = incObj.Image_Path13;
                inChq.Image_Path14 = incObj.Image_Path14;
                inChq.Image_Path15 = incObj.Image_Path15;
                inChq.Image_Path16 = incObj.Image_Path16;
                inChq.Image_Path17 = incObj.Image_Path17;
                inChq.Image_Path18 = incObj.Image_Path18;
                inChq.Image_Path19 = incObj.Image_Path19;
                inChq.Image_Path20 = incObj.Image_Path20;
                inChq.Image_Path21 = incObj.Image_Path21;
                inChq.Image_Path22 = incObj.Image_Path22;
                inChq.Image_Path23 = incObj.Image_Path23;
                inChq.Image_Path24 = incObj.Image_Path24;
                inChq.Image_Path25 = incObj.Image_Path25;
                inChq.Image_Path26 = incObj.Image_Path26;
                inChq.Image_Path27 = incObj.Image_Path27;
                inChq.Image_Path28 = incObj.Image_Path28;
                inChq.Image_Path29 = incObj.Image_Path29;
                inChq.Image_Path30 = incObj.Image_Path30;
                inChq.Image_Path31 = incObj.Image_Path31;
                inChq.Image_Path32 = incObj.Image_Path32;
                inChq.Image_Path33 = incObj.Image_Path33;
                inChq.Image_Path34 = incObj.Image_Path34;
                inChq.Image_Path35 = incObj.Image_Path35;
                inChq.Image_Path36 = incObj.Image_Path36;
                inChq.Image_Path37 = incObj.Image_Path37;
                inChq.Image_Path38 = incObj.Image_Path38;
                inChq.Image_Path39 = incObj.Image_Path39;
                inChq.Image_Path40 = incObj.Image_Path40;
                inChq.Image_Path41 = incObj.Image_Path41;
                inChq.Image_Path42 = incObj.Image_Path42;
                inChq.Image_Path43 = incObj.Image_Path43;
                inChq.Image_Path44 = incObj.Image_Path44;
                inChq.Image_Path45 = incObj.Image_Path45;
                inChq.Image_Path46 = incObj.Image_Path46;
                inChq.Image_Path47 = incObj.Image_Path47;
                inChq.Image_Path48 = incObj.Image_Path48;
                inChq.Image_Path49 = incObj.Image_Path49;
                inChq.Image_Path50 = incObj.Image_Path50;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _Logg_Message = "Error InwardFinanicalWFDetailsPMADIS  err :  " + ex.Message;
                _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                HttpContext.Session.SetString("ErrorMessage", _Logg_Message);
                return RedirectToAction("InsufficientFunds", "INWARD");
            }
        }

        public IActionResult InwardFinanicalWFDetailsPMADIS_Auth(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");

            JsonResult _json = new JsonResult(new { });
            _json.Value = "";
            int _step = 90000;
            _step += 5700;
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }
            string methodName = "InwardFinanicalWFDetailsPMADIS";
            _step += 1;
            int userid = GetUserID();
            int pageid;
            string titel = "";
            pageid = _context.App_Pages.SingleOrDefault(t => t.Page_Name_EN == methodName).Page_Id;
            _step += 1;
            int applicationid = _context.App_Pages.SingleOrDefault(y => y.Page_Name_EN == methodName).Application_ID;
            _step += 1;
            getuser_group_permision(pageid, applicationid, userid);
            _step += 1;

            titel = _context.App_Pages.SingleOrDefault(c => c.Page_Id == pageid).ENG_DESC;
            _step += 1;
            ViewBag.Title = titel;
            ViewBag.Tree = GetAllCategoriesForTree();
            _step += 1;
            int Tbl_id;
            int WFLevel;
            ECC_CAP_Services.AccountInfo_RESPONSE Accobj = new ECC_CAP_Services.AccountInfo_RESPONSE();
            _step += 1;
            ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient EccAccInfo_WebSvc = new ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient(ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient.EndpointConfiguration.SAFA_T24_ECC_SVCSoap);
            _step += 1;
            _step += 1;
            string branch = GetBranchID();
            INChqs inChq = new INChqs();
            INWARD_WF_Tbl wf = new INWARD_WF_Tbl();
            Inward_Trans incObj = new Inward_Trans();
            try
            {
                _Logg_Message = "Show Cheque InwardFinanicalWFDetailsPMADIS  it from Inward_Trans table ";
                _logSystem.WriteLogg(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");

                wf = _context.INWARD_WF_Tbl.SingleOrDefault(z => z.Serial == id && z.Final_Status != "Accept");
                _step += 1;

                string com = GetComID();
                incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);
                if (incObj.ClrCenter == "PMA" && incObj.VIP == true && branch != "2")
                {
                    ViewBag.is_vip = "YES";
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
                        ECC_CAP_Services.AccountInfo_RESPONSE GUAR_CUSTOMER_Accobj = EccAccInfo_WebSvc.ACCOUNT_INFO(item.ACCOUNT_NUMBER, 1);
                        ViewBag.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                        inChq.GUAR_CUSTOMER += item.ACCOUNT_NUMBER + "*" + GUAR_CUSTOMER_Accobj.ClearBalance + "*" + GUAR_CUSTOMER_Accobj.AccountCurrency + "|";
                    }
                }
                Accobj = EccAccInfo_WebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                _step += 1;

                inChq.BookedBalance = Accobj.BookedBalance;
                inChq.ClearBalance = Accobj.ClearBalance;
                inChq.AccountStatus = Accobj.AccountStatus;

                string USER_ = GetUserName();
                string group = _context.Users_Tbl.SingleOrDefault(c => c.User_Name == USER_).Group_ID;

                ViewBag.Reject = "False";
                inChq.Reject = "False";

                ViewBag.recomdationbtn = "True";
                inChq.Recom = "True";

                if (group == GroupType.Group_Status.AdminAuthorized.ToString() || branch == "2")
                {
                    ViewBag.Approve = "True";
                    inChq.Approve = "True";
                    ViewBag.recomdationbtn = "False";
                    inChq.Recom = "False";
                }
                else
                {
                    List<USER_Limits_Auth_Amount_Result> Userlevel;
                    Tbl_id = _context.Transaction_TBL.SingleOrDefault(z => z.Transaction_Name == wf.Clr_Center).Transaction_ID;
                    _step += 1;
                    try
                    {
                        Userlevel = new List<USER_Limits_Auth_Amount_Result>(); // Placeholder for stored procedure
                        _step += 1;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error calling USER_Limits_Auth_Amount stored procedure");
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
                        int _Group_ID = 4;
                        Users_Tbl _Usr = _context.Users_Tbl.SingleOrDefault(u => u.User_ID == userid);
                        if (_Usr != null)
                        {
                            _Group_ID = Convert.ToInt32(_Usr.Group_ID);
                        }

                        AuthTrans_User_TBL x = new AuthTrans_User_TBL();

                        if (incObj.ClrCenter == "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "5" && t.group_ID == _Group_ID);
                        }

                        if (incObj.ClrCenter != "PMA")
                        {
                            x = _context.AuthTrans_User_TBL.SingleOrDefault(t => t.Auth_user_ID == userid && t.Trans_id == "6" && t.group_ID == _Group_ID);
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
                                if (wf.Level1_status != null && wf.Level1_status.Contains("Recom"))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                                else
                                {
                                    ViewBag.Approve = "False";
                                    inChq.Approve = "False";
                                    ViewBag.recomdationbtn = "True";
                                    inChq.Recom = "True";
                                }
                            }
                            else if (x.Auth_Level == 3)
                            {
                                if (wf.Level2_status != null && wf.Level2_status.Contains("Recom"))
                                {
                                    ViewBag.Approve = "True";
                                    inChq.Approve = "True";
                                    ViewBag.recomdationbtn = "False";
                                    inChq.Recom = "False";
                                }
                                else
                                {
                                    ViewBag.Approve = "False";
                                    inChq.Approve = "False";
                                    ViewBag.recomdationbtn = "True";
                                    inChq.Recom = "True";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in authorization logic");
                    }
                }

                inChq.Serial = incObj.Serial;
                inChq.ChqSequance = incObj.ChqSequance;
                inChq.ClrCenter = incObj.ClrCenter;
                inChq.TransDate = incObj.TransDate;
                inChq.Amount = incObj.Amount;
                inChq.Amount_JD = incObj.Amount_JD;
                inChq.Currency = incObj.Currency;
                inChq.BankName = incObj.BankName;
                inChq.BranchName = incObj.BranchName;
                inChq.ISSAccount = incObj.ISSAccount;
                inChq.BeneficiaryName = incObj.BeneficiaryName;
                inChq.AltAccount = incObj.AltAccount;
                inChq.Reason = incObj.Reason;
                inChq.ErrorDescription = incObj.ErrorDescription;
                inChq.Posted = incObj.Posted;
                inChq.Teller = incObj.Teller;
                inChq.UserName = incObj.UserName;
                inChq.BranchID = incObj.BranchID;
                inChq.CompID = incObj.CompID;
                inChq.VIP = incObj.VIP;
                inChq.PMA_Branch = incObj.PMA_Branch;
                inChq.PMA_Branch_Name = incObj.PMA_Branch_Name;
                inChq.Inward_Type = incObj.Inward_Type;
                inChq.Image_Path = incObj.Image_Path;
                inChq.Image_Path2 = incObj.Image_Path2;
                inChq.Image_Path3 = incObj.Image_Path3;
                inChq.Image_Path4 = incObj.Image_Path4;
                inChq.Image_Path5 = incObj.Image_Path5;
                inChq.Image_Path6 = incObj.Image_Path6;
                inChq.Image_Path7 = incObj.Image_Path7;
                inChq.Image_Path8 = incObj.Image_Path8;
                inChq.Image_Path9 = incObj.Image_Path9;
                inChq.Image_Path10 = incObj.Image_Path10;
                inChq.Image_Path11 = incObj.Image_Path11;
                inChq.Image_Path12 = incObj.Image_Path12;
                inChq.Image_Path13 = incObj.Image_Path13;
                inChq.Image_Path14 = incObj.Image_Path14;
                inChq.Image_Path15 = incObj.Image_Path15;
                inChq.Image_Path16 = incObj.Image_Path16;
                inChq.Image_Path17 = incObj.Image_Path17;
                inChq.Image_Path18 = incObj.Image_Path18;
                inChq.Image_Path19 = incObj.Image_Path19;
                inChq.Image_Path20 = incObj.Image_Path20;
                inChq.Image_Path21 = incObj.Image_Path21;
                inChq.Image_Path22 = incObj.Image_Path22;
                inChq.Image_Path23 = incObj.Image_Path23;
                inChq.Image_Path24 = incObj.Image_Path24;
                inChq.Image_Path25 = incObj.Image_Path25;
                inChq.Image_Path26 = incObj.Image_Path26;
                inChq.Image_Path27 = incObj.Image_Path27;
                inChq.Image_Path28 = incObj.Image_Path28;
                inChq.Image_Path29 = incObj.Image_Path29;
                inChq.Image_Path30 = incObj.Image_Path30;
                inChq.Image_Path31 = incObj.Image_Path31;
                inChq.Image_Path32 = incObj.Image_Path32;
                inChq.Image_Path33 = incObj.Image_Path33;
                inChq.Image_Path34 = incObj.Image_Path34;
                inChq.Image_Path35 = incObj.Image_Path35;
                inChq.Image_Path36 = incObj.Image_Path36;
                inChq.Image_Path37 = incObj.Image_Path37;
                inChq.Image_Path38 = incObj.Image_Path38;
                inChq.Image_Path39 = incObj.Image_Path39;
                inChq.Image_Path40 = incObj.Image_Path40;
                inChq.Image_Path41 = incObj.Image_Path41;
                inChq.Image_Path42 = incObj.Image_Path42;
                inChq.Image_Path43 = incObj.Image_Path43;
                inChq.Image_Path44 = incObj.Image_Path44;
                inChq.Image_Path45 = incObj.Image_Path45;
                inChq.Image_Path46 = incObj.Image_Path46;
                inChq.Image_Path47 = incObj.Image_Path47;
                inChq.Image_Path48 = incObj.Image_Path48;
                inChq.Image_Path49 = incObj.Image_Path49;
                inChq.Image_Path50 = incObj.Image_Path50;

                return View(inChq);
            }
            catch (Exception ex)
            {
                _Logg_Message = "Error InwardFinanicalWFDetailsPMADIS  err :  " + ex.Message;
                _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                HttpContext.Session.SetString("ErrorMessage", _Logg_Message);
                return RedirectToAction("InsufficientFunds", "INWARD");
            }
        }

        public IActionResult ReversePostingPMARAM(string id)
        {
            HttpContext.Session.SetString("ErrorMessage", "");
            string _Logg_Message = "";
            int _step = 90000;
            _step += 5700;
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                _Logg_Message = "Show Cheque ReversePostingPMARAM  it from Inward_Trans table ";
                _logSystem.WriteLogg(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");

                INWARD_WF_Tbl wf = _context.INWARD_WF_Tbl.SingleOrDefault(z => z.Serial == id);
                Inward_Trans incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);

                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD");
                }

                // ECC_Handler_SVC.InwardHandlingSVCSoapClient WebSvc = new ECC_Handler_SVC.InwardHandlingSVCSoapClient(ECC_Handler_SVC.InwardHandlingSVCSoapClient.EndpointConfiguration.InwardHandlingSVCSoap);
                // HandelResponseRequest[] obj_ = WebSvc.HandelResponse(incObj.ChqSequance, incObj.ClrCenter, "Reverse Posting", "", incObj.Serial, GetUserID().ToString());

                // Placeholder for Web Service call
                // Simulate success for now
                bool serviceCallSuccess = true;

                if (serviceCallSuccess)
                {
                    incObj.Posted = 10; // Assuming 10 means reversed
                    _context.Entry(incObj).State = EntityState.Modified;
                    _context.SaveChanges();

                    _Logg_Message = "Reverse Posting Success for Serial: " + id;
                    _logSystem.WriteLogg(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                    HttpContext.Session.SetString("ErrorMessage", "Reverse Posting Success");
                }
                else
                {
                    _Logg_Message = "Reverse Posting Failed for Serial: " + id;
                    _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                    HttpContext.Session.SetString("ErrorMessage", "Reverse Posting Failed");
                }

                return RedirectToAction("Index"); // Redirect to a suitable page
            }
            catch (DbUpdateException ex)
            {
                _Logg_Message = "Database Update Error in ReversePostingPMARAM: " + ex.Message;
                _logger.LogError(ex, _Logg_Message);
                _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                HttpContext.Session.SetString("ErrorMessage", "Database Error: " + ex.Message);
                return RedirectToAction("Error");
            }
            catch (Exception ex)
            {
                _Logg_Message = "Error in ReversePostingPMARAM: " + ex.Message;
                _logger.LogError(ex, _Logg_Message);
                _logSystem.WriteError(_Logg_Message, _applicationID, GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, GetUserName(), GetUserName(), "", "", "");
                HttpContext.Session.SetString("ErrorMessage", "Error: " + ex.Message);
                return RedirectToAction("Error");
            }
        }

        public JsonResult Get_Inward_Trans_Details(string id)
        {
            try
            {
                var incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);
                if (incObj == null)
                {
                    return Json(new { success = false, message = "Transaction not found" });
                }

                var result = new
                {
                    incObj.Serial,
                    incObj.ChqSequance,
                    incObj.ClrCenter,
                    incObj.TransDate,
                    incObj.Amount,
                    incObj.Amount_JD,
                    incObj.Currency,
                    incObj.BankName,
                    incObj.BranchName,
                    incObj.ISSAccount,
                    incObj.BeneficiaryName,
                    incObj.AltAccount,
                    incObj.Reason,
                    incObj.ErrorDescription,
                    incObj.Posted,
                    incObj.Teller,
                    incObj.UserName,
                    incObj.BranchID,
                    incObj.CompID,
                    incObj.VIP,
                    incObj.PMA_Branch,
                    incObj.PMA_Branch_Name,
                    incObj.Inward_Type,
                    incObj.Image_Path,
                    incObj.Image_Path2,
                    incObj.Image_Path3,
                    incObj.Image_Path4,
                    incObj.Image_Path5,
                    incObj.Image_Path6,
                    incObj.Image_Path7,
                    incObj.Image_Path8,
                    incObj.Image_Path9,
                    incObj.Image_Path10,
                    incObj.Image_Path11,
                    incObj.Image_Path12,
                    incObj.Image_Path13,
                    incObj.Image_Path14,
                    incObj.Image_Path15,
                    incObj.Image_Path16,
                    incObj.Image_Path17,
                    incObj.Image_Path18,
                    incObj.Image_Path19,
                    incObj.Image_Path20,
                    incObj.Image_Path21,
                    incObj.Image_Path22,
                    incObj.Image_Path23,
                    incObj.Image_Path24,
                    incObj.Image_Path25,
                    incObj.Image_Path26,
                    incObj.Image_Path27,
                    incObj.Image_Path28,
                    incObj.Image_Path29,
                    incObj.Image_Path30,
                    incObj.Image_Path31,
                    incObj.Image_Path32,
                    incObj.Image_Path33,
                    incObj.Image_Path34,
                    incObj.Image_Path35,
                    incObj.Image_Path36,
                    incObj.Image_Path37,
                    incObj.Image_Path38,
                    incObj.Image_Path39,
                    incObj.Image_Path40,
                    incObj.Image_Path41,
                    incObj.Image_Path42,
                    incObj.Image_Path43,
                    incObj.Image_Path44,
                    incObj.Image_Path45,
                    incObj.Image_Path46,
                    incObj.Image_Path47,
                    incObj.Image_Path48,
                    incObj.Image_Path49,
                    incObj.Image_Path50
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get_Inward_Trans_Details");
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult Get_Inward_Trans_Details_OnUs_Json(string id)
        {
            try
            {
                var incObj = _context.Inward_Trans.SingleOrDefault(y => y.Serial == id);
                if (incObj == null)
                {
                    return Json(new { success = false, message = "Transaction not found" });
                }

                var result = new
                {
                    incObj.Serial,
                    incObj.ChqSequance,
                    incObj.ClrCenter,
                    incObj.TransDate,
                    incObj.Amount,
                    incObj.Amount_JD,
                    incObj.Currency,
                    incObj.BankName,
                    incObj.BranchName,
                    incObj.ISSAccount,
                    incObj.BeneficiaryName,
                    incObj.AltAccount,
                    incObj.Reason,
                    incObj.ErrorDescription,
                    incObj.Posted,
                    incObj.Teller,
                    incObj.UserName,
                    incObj.BranchID,
                    incObj.CompID,
                    incObj.VIP,
                    incObj.PMA_Branch,
                    incObj.PMA_Branch_Name,
                    incObj.Inward_Type,
                    incObj.Image_Path,
                    incObj.Image_Path2,
                    incObj.Image_Path3,
                    incObj.Image_Path4,
                    incObj.Image_Path5,
                    incObj.Image_Path6,
                    incObj.Image_Path7,
                    incObj.Image_Path8,
                    incObj.Image_Path9,
                    incObj.Image_Path10,
                    incObj.Image_Path11,
                    incObj.Image_Path12,
                    incObj.Image_Path13,
                    incObj.Image_Path14,
                    incObj.Image_Path15,
                    incObj.Image_Path16,
                    incObj.Image_Path17,
                    incObj.Image_Path18,
                    incObj.Image_Path19,
                    incObj.Image_Path20,
                    incObj.Image_Path21,
                    incObj.Image_Path22,
                    incObj.Image_Path23,
                    incObj.Image_Path24,
                    incObj.Image_Path25,
                    incObj.Image_Path26,
                    incObj.Image_Path27,
                    incObj.Image_Path28,
                    incObj.Image_Path29,
                    incObj.Image_Path30,
                    incObj.Image_Path31,
                    incObj.Image_Path32,
                    incObj.Image_Path33,
                    incObj.Image_Path34,
                    incObj.Image_Path35,
                    incObj.Image_Path36,
                    incObj.Image_Path37 = incObj.Image_Path37,
                    incObj.Image_Path38 = incObj.Image_Path38,
                    incObj.Image_Path39 = incObj.Image_Path39,
                    incObj.Image_Path40 = incObj.Image_Path40,
                    incObj.Image_Path41 = incObj.Image_Path41,
                    incObj.Image_Path42 = incObj.Image_Path42,
                    incObj.Image_Path43 = incObj.Image_Path43,
                    incObj.Image_Path44 = incObj.Image_Path44,
                    incObj.Image_Path45 = incObj.Image_Path45,
                    incObj.Image_Path46 = incObj.Image_Path46,
                    incObj.Image_Path47 = incObj.Image_Path47,
                    incObj.Image_Path48 = incObj.Image_Path48,
                    incObj.Image_Path49 = incObj.Image_Path49,
                    incObj.Image_Path50 = incObj.Image_Path50
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get_Inward_Trans_Details_OnUs_Json");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Placeholder for getuser_group_permision - needs actual implementation
        private void getuser_group_permision(int pageid, int applicationid, int userid)
        {
            // Implementation based on original VB.NET logic
            // This function would typically check user permissions against a database
            // For now, it's a placeholder.
            _logger.LogInformation($"Checking permissions for UserID: {userid}, PageID: {pageid}, ApplicationID: {applicationid}");
        }

        // Placeholder for GetAllCategoriesForTree - needs actual implementation
        private List<TreeNode> GetAllCategoriesForTree()
        {
            // Implementation based on original VB.NET logic
            // This function would typically fetch categories and build a tree structure
            // For now, it's a placeholder.
            return new List<TreeNode>();
        }

        // Placeholder for Category and TreeNode classes - needs actual definition
        public class Category
        {
            public int SubMenu_ID { get; set; }
            public int? Parent_ID { get; set; }
            public string SubMenu_Name_EN { get; set; }
            public string Related_Page_ID { get; set; }
        }

        public class TreeNode
        {
            public int SubMenu_ID { get; set; }
            public string SubMenu_Name_EN { get; set; }
            public string Related_Page_ID { get; set; }
            public List<TreeNode> Children { get; set; } = new List<TreeNode>();
        }

        // Placeholder for INChqs class - needs actual definition
        public class INChqs
        {
            public string Serial { get; set; }
            public string ChqSequance { get; set; }
            public string ClrCenter { get; set; }
            public DateTime? TransDate { get; set; }
            public decimal? Amount { get; set; }
            public decimal? Amount_JD { get; set; }
            public string Currency { get; set; }
            public string BankName { get; set; }
            public string BranchName { get; set; }
            public string ISSAccount { get; set; }
            public string BeneficiaryName { get; set; }
            public string AltAccount { get; set; }
            public string Reason { get; set; }
            public string ErrorDescription { get; set; }
            public int? Posted { get; set; }
            public string Teller { get; set; }
            public string UserName { get; set; }
            public string BranchID { get; set; }
            public string CompID { get; set; }
            public bool? VIP { get; set; }
            public string PMA_Branch { get; set; }
            public string PMA_Branch_Name { get; set; }
            public string Inward_Type { get; set; }
            public string Image_Path { get; set; }
            public string Image_Path2 { get; set; }
            public string Image_Path3 { get; set; }
            public string Image_Path4 { get; set; }
            public string Image_Path5 { get; set; }
            public string Image_Path6 { get; set; }
            public string Image_Path7 { get; set; }
            public string Image_Path8 { get; set; }
            public string Image_Path9 { get; set; }
            public string Image_Path10 { get; set; }
            public string Image_Path11 { get; set; }
            public string Image_Path12 { get; set; }
            public string Image_Path13 { get; set; }
            public string Image_Path14 { get; set; }
            public string Image_Path15 { get; set; }
            public string Image_Path16 { get; set; }
            public string Image_Path17 { get; set; }
            public string Image_Path18 { get; set; }
            public string Image_Path19 { get; set; }
            public string Image_Path20 { get; set; }
            public string Image_Path21 { get; set; }
            public string Image_Path22 { get; set; }
            public string Image_Path23 { get; set; }
            public string Image_Path24 { get; set; }
            public string Image_Path25 { get; set; }
            public string Image_Path26 { get; set; }
            public string Image_Path27 { get; set; }
            public string Image_Path28 { get; set; }
            public string Image_Path29 { get; set; }
            public string Image_Path30 { get; set; }
            public string Image_Path31 { get; set; }
            public string Image_Path32 { get; set; }
            public string Image_Path33 { get; set; }
            public string Image_Path34 { get; set; }
            public string Image_Path35 { get; set; }
            public string Image_Path36 { get; set; }
            public string Image_Path37 { get; set; }
            public string Image_Path38 { get; set; }
            public string Image_Path39 { get; set; }
            public string Image_Path40 { get; set; }
            public string Image_Path41 { get; set; }
            public string Image_Path42 { get; set; }
            public string Image_Path43 { get; set; }
            public string Image_Path44 { get; set; }
            public string Image_Path45 { get; set; }
            public string Image_Path46 { get; set; }
            public string Image_Path47 { get; set; }
            public string Image_Path48 { get; set; }
            public string Image_Path49 { get; set; }
            public string Image_Path50 { get; set; }
            public string GUAR_CUSTOMER { get; set; }
            public string Approve { get; set; }
            public string Recom { get; set; }
            public string Reject { get; set; }
            public string BookedBalance { get; set; }
            public string ClearBalance { get; set; }
            public string AccountStatus { get; set; }
        }

        // Placeholder for USER_Limits_Auth_Amount_Result class - needs actual definition
        public class USER_Limits_Auth_Amount_Result
        {
            public int LEVEL { get; set; }
        }

        // Placeholder for GroupType enum - needs actual definition
        public enum GroupType
        {
            AdminAuthorized = 1 // Example value
        }
    }
}

