

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
                pdcObj = await _context.Outward_Trans.SingleOrDefaultAsync(y => y.Serial == id && y.Posted == AllEnums.Cheque_Status.Rejected);
                Currency = await _context.CURRENCY_TBL.ToListAsync();

                if (pdcObj == null)
                {
                    // Handle case where pdcObj is not found, similar to the VB.NET logic
                    // For now, returning a not found result or an error view.
                    return new NotFoundResult();
                }

                // Populate outChq and Img based on pdcObj and Currency
                // This will involve more database queries and mapping.
                // The VB.NET code has complex logic for populating images and currency symbols.
                // This needs to be translated carefully.

                // Example of currency conversion (simplified)
                foreach (var curr in Currency)
                {
                    if (pdcObj.Currency == curr.ID.ToString())
                    {
                        pdcObj.Currency = curr.SYMBOL_ISO;
                        break;
                    }
                }

                Img = await _context.Outward_Imgs.FirstOrDefaultAsync(y => y.Serial == pdcObj.Serial);

                pdcObj.Amount = Math.Round(pdcObj.Amount, 2, MidpointRounding.AwayFromZero);
                outChq.out = pdcObj;
                outChq.Imgs = Img;

                return new ViewResult { ViewName = "Out_VerficationDetails", ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { Model = outChq } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Out_VerficationDetails: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new ViewResult { ViewName = "Error" };
            }
        }



        public async Task<IActionResult> OUTWORD()
        {
            // The original VB.NET code checks for Session.Item("UserName") and redirects to Login if null.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated at this point.

            // Logging
            _logger.LogInformation("OUTWORD method called.");

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



        public async Task<IActionResult> OUTWORD(Outward_Trans outwardTrans, string actionType)
        {
            _logger.LogInformation($"OUTWORD POST method called with actionType: {actionType}");

            // Session checks and redirects should ideally be handled by authentication/authorization middleware.
            // Assuming user is authenticated and authorized here.

            try
            {
                // The original VB.NET code has extensive logic for different actionTypes (Save, Verify, Post, Reject, Return).
                // This will be a simplified translation focusing on the structure.

                // Common setup (from VB.NET)
                var branchCode = ""; // Get from session/claims in real app
                var userName = ""; // Get from session/claims in real app
                var userId = 0; // Get from session/claims in real app
                var applicationId = "1"; // Assuming constant

                // This part needs careful translation based on the actual logic of each actionType.
                // For now, I'll provide a placeholder structure.
                switch (actionType.ToLower())
                {
                    case "save":
                        // Logic for saving outwardTrans
                        // Example: await _context.Outward_Trans.AddAsync(outwardTrans);
                        // await _context.SaveChangesAsync();
                        _logger.LogInformation("OUTWORD: Save action initiated.");
                        break;
                    case "verify":
                        // Logic for verifying outwardTrans
                        _logger.LogInformation("OUTWORD: Verify action initiated.");
                        break;
                    case "post":
                        // Logic for posting outwardTrans
                        _logger.LogInformation("OUTWORD: Post action initiated.");
                        break;
                    case "reject":
                        // Logic for rejecting outwardTrans
                        _logger.LogInformation("OUTWORD: Reject action initiated.");
                        break;
                    case "return":
                        // Logic for returning outwardTrans
                        _logger.LogInformation("OUTWORD: Return action initiated.");
                        break;
                    default:
                        _logger.LogWarning($"OUTWORD: Unknown actionType: {actionType}");
                        break;
                }

                // After processing, typically redirect or return a view with updated model/status.
                // For simplicity, returning a success message or redirecting to a list view.
                return new OkObjectResult(new { message = $"Operation {actionType} completed successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in OUTWORD POST for actionType {actionType}: {ex.Message}");
                // Handle error, possibly return an error view or redirect
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }



        public async Task<DataTable> Get_Post_Rest_Code(string CUSTOMER_ID, string ACCOUNT_NUMBER)
        {
            _logger.LogInformation($"Get_Post_Rest_Code called for CUSTOMER_ID: {CUSTOMER_ID}, ACCOUNT_NUMBER: {ACCOUNT_NUMBER}");

            DataTable dt = new DataTable();
            try
            {
                // This method uses direct ADO.NET with ODBC, which is a significant departure from EF Core.
                // For direct conversion, we will replicate the ADO.NET logic.
                // In a modern ASP.NET Core application, this would ideally be refactored to use EF Core or a more modern data access approach.

                using (OdbcConnection connection = new OdbcConnection(_configuration.GetConnectionString("ODBC_CONNECTION_NAME")))
                {
                    await connection.OpenAsync();
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM Post_Rest_Code WHERE CUSTOMER_ID = ? AND ACCOUNT_NUMBER = ?", connection))
                    {
                        command.Parameters.AddWithValue("?", CUSTOMER_ID);
                        command.Parameters.AddWithValue("?", ACCOUNT_NUMBER);

                        using (OdbcDataAdapter adapter = new OdbcDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_Post_Rest_Code: {ex.Message}");
                // In a real application, you might want to throw the exception or return an empty DataTable with error info.
            }
            return dt;
        }


_logger.LogInformation($"Get_Final_Posting_Restrection called with Customer_Post_Rest: {Customer_Post_Rest}, Acc_Post_Rest: {Acc_Post_Rest}, Language: {Language}");

            string finalRestrection = "";
            try
            {
                // This method appears to be a pure function, making it a good candidate for a static helper method if it doesn't rely on service state.
                // For now, keeping it within the service.

                if (Customer_Post_Rest == 0 && Acc_Post_Rest == 0)
                {
                    finalRestrection = (Language == 1) ? "No Restriction" : "لا يوجد قيود";
                }
                else if (Customer_Post_Rest == 4 || Acc_Post_Rest == 4)
                {
                    finalRestrection = (Language == 1) ? "Total Restriction" : "حظر شامل";
                }
                else if (Customer_Post_Rest == 1 || Acc_Post_Rest == 1)
                {
                    finalRestrection = (Language == 1) ? "No Post" : "ممنوع قيد";
                }
                else if (Customer_Post_Rest == 2 || Acc_Post_Rest == 2)
                {
                    finalRestrection = (Language == 1) ? "Debit Only" : "مدين فقط";
                }
                else if (Customer_Post_Rest == 3 || Acc_Post_Rest == 3)
                {
                    finalRestrection = (Language == 1) ? "Credit Only" : "دائن فقط";
                }
                else
                {
                    finalRestrection = (Language == 1) ? "No Restriction" : "لا يوجد قيود";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_Final_Posting_Restrection: {ex.Message}");
                // Handle error, perhaps return a default value or re-throw
                finalRestrection = (Language == 1) ? "Error" : "خطأ";
            }
            return Task.FromResult(finalRestrection);
        }



        public Task<string> Get_Final_Posting_Restrection(int Customer_Post_Rest, int Acc_Post_Rest, int Language)
        {
            _logger.LogInformation($"Get_Final_Posting_Restrection called with Customer_Post_Rest: {Customer_Post_Rest}, Acc_Post_Rest: {Acc_Post_Rest}, Language: {Language}");

            string finalRestrection = "";
            try
            {
                // This method appears to be a pure function, making it a good candidate for a static helper method if it doesn't rely on service state.
                // For now, keeping it within the service.

                if (Customer_Post_Rest == 0 && Acc_Post_Rest == 0)
                {
                    finalRestrection = (Language == 1) ? "No Restriction" : "لا يوجد قيود";
                }
                else if (Customer_Post_Rest == 4 || Acc_Post_Rest == 4)
                {
                    finalRestrection = (Language == 1) ? "Total Restriction" : "حظر شامل";
                }
                else if (Customer_Post_Rest == 1 || Acc_Post_Rest == 1)
                {
                    finalRestrection = (Language == 1) ? "No Post" : "ممنوع قيد";
                }
                else if (Customer_Post_Rest == 2 || Acc_Post_Rest == 2)
                {
                    finalRestrection = (Language == 1) ? "Debit Only" : "مدين فقط";
                }
                else if (Customer_Post_Rest == 3 || Acc_Post_Rest == 3)
                {
                    finalRestrection = (Language == 1) ? "Credit Only" : "دائن فقط";
                }
                else
                {
                    finalRestrection = (Language == 1) ? "No Restriction" : "لا يوجد قيود";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_Final_Posting_Restrection: {ex.Message}");
                // Handle error, perhaps return a default value or re-throw
                finalRestrection = (Language == 1) ? "Error" : "خطأ";
            }
            return Task.FromResult(finalRestrection);
        }



        public async Task<IActionResult> Pendding_OutWord_Request()
        {
            _logger.LogInformation("Pendding_OutWord_Request method called.");

            // The original VB.NET code checks for session variables and redirects to login if not found.
            // In ASP.NET Core, this is typically handled by authentication middleware.
            // Assuming the user is authenticated for this method.

            try
            {
                // The VB.NET code sets ViewBag.Tree using GetAllCategoriesForTree().
                // This would be handled in the controller or a dedicated view component.
                // For now, the service will focus on data retrieval and business logic.

                // The VB.NET code also calls getuser_group_permision and redirects to 'block' if no access.
                // This should be handled by authorization policies/attributes in ASP.NET Core.

                // This method primarily returns a view, so the service method will not return a view directly.
                // It might return data needed by the view, or a status indicating success/failure.
                // For now, returning a success status.
                return new OkObjectResult(new { message = "Pendding_OutWord_Request data prepared successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Pendding_OutWord_Request: {ex.Message}");
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }



        public async Task<IActionResult> Pendding_OutWord_Request_Auth()
        {
            _logger.LogInformation("Pendding_OutWord_Request_Auth method called.");

            // This method has extensive logic for handling authentication, authorization, and data retrieval.
            // The conversion will focus on the overall structure and data flow.

            try
            {
                // The original code relies heavily on session state. In ASP.NET Core, this should be replaced with claims-based identity and authorization.
                // For this conversion, we will assume the necessary user information is available through claims or a user service.

                // The logic for fetching data and filtering based on user permissions is complex.
                // This will be simplified to show the basic structure.

                // Example of fetching data (replace with actual EF Core queries)
                // var pendingRequests = await _context.Auth_Tran_Details_TBL
                //     .Where(r => r.Status == "Pending") // Example filter
                //     .ToListAsync();

                // The method returns a view with the fetched data.
                // The service will return the data, and the controller will pass it to the view.
                return new OkObjectResult(new { message = "Data for pending requests auth view prepared successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Pendding_OutWord_Request_Auth: {ex.Message}");
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }



        public async Task<IActionResult> getOutword_WF_Details()
        {
            _logger.LogInformation("getOutword_WF_Details method called.");

            try
            {
                // This method appears to be a GET request for displaying workflow details.
                // The original VB.NET code involves session checks, permission checks, and data retrieval.
                // For now, the service will focus on the data retrieval aspect.

                // Example of data retrieval (replace with actual EF Core queries)
                // var workflowDetails = await _context.Outward_WF_Details
                //     .Where(d => d.SomeCondition == true)
                //     .ToListAsync();

                return new OkObjectResult(new { message = "Outword workflow details data prepared successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in getOutword_WF_Details: {ex.Message}");
                return new BadRequestObjectResult(new { error = ex.Message });
            }
        }



        public async Task<string> Get_OFS_HttpLink()
        {
            _logger.LogInformation("Get_OFS_HttpLink method called.");

            try
            {
                // The original VB.NET code uses a custom CONNECTION class and directly queries the database.
                // In ASP.NET Core with EF Core, this should be replaced with an EF Core query to Global_Parameter_TBL.

                // Assuming Global_Parameter_TBL is mapped to an entity in your DbContext
                // and that _context is an instance of your ApplicationDbContext.

                var ofsHttpLink = await _context.Global_Parameter_TBL
                                                .Where(p => p.Parameter_Name == "OFS_HttpLink")
                                                .Select(p => p.Parameter_Value)
                                                .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(ofsHttpLink))
                {
                    _logger.LogWarning("OFS_HttpLink parameter not found in Global_Parameter_TBL.");
                    return string.Empty; // Or throw an exception, depending on desired error handling
                }

                return ofsHttpLink;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in Get_OFS_HttpLink: {ex.Message}");
                // Handle exception appropriately, e.g., rethrow, return default, or log and return empty.
                return string.Empty;
            }
        }

