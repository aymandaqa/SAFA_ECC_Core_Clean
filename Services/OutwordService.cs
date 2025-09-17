

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

