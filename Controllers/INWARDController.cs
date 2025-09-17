_logger.LogError(ex, "Error in GET_CUSTOMER_DEATH_DATE for CustomerID: {CustomerID}", customerId);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: INWARD/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: INWARD/InwardFinanicalWFDetailsPMADIS_NEW
        public async Task<IActionResult> InwardFinanicalWFDetailsPMADIS_NEW(string id)
        {
            if (string.IsNullOrEmpty(GetUserName()))
            {
                return RedirectToAction("Login", "Login");
            }

            try
            {
                var page = await _context.App_Pages.SingleOrDefaultAsync(p => p.Page_Name_EN == "InwardFinanicalWFDetailsPMADIS");
                if (page == null)
                {
                    // Handle case where page is not found
                    return NotFound("Page configuration not found.");
                }

                // Permission check logic would go here
                // getuser_group_permision(page.Page_Id, page.Application_ID, GetUserId());
                // if (Session["AccessPage"] == "NoAccess") return Forbid();

                ViewBag.Title = page.ENG_DESC;
                // ViewBag.Tree = GetAllCategoriesForTree(); // This helper needs to be converted

                var wf = await _context.INWARD_WF_Tbl.SingleOrDefaultAsync(z => z.Serial == id && z.Final_Status != "Accept");
                if (wf == null)
                {
                    // Cheque already processed or not found
                    return RedirectToAction("SomeDefaultAction"); // Or a specific error view
                }

                var incObj = await _context.Inward_Trans.SingleOrDefaultAsync(y => y.Serial == id);
                if (incObj == null)
                {
                    return RedirectToAction("InsufficientFunds", "INWARD"); // Assuming this action exists
                }

                var inChq = new InwardFinanicalWFDetailsPMADISViewModel();

                // VIP Check
                if (incObj.ClrCenter == "PMA" && incObj.VIP == true && GetBranchId() != "2")
                {
                    inChq.IsVip = true;
                }

                // Guarantor Logic (Simplified - needs ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient conversion)
                var guarantors = await _context.T24_CAB_OVRDRWN_GUAR
                                        .Where(i => i.GUAR_CUSTOMER == incObj.ISSAccount)
                                        .ToListAsync();

                if (!guarantors.Any())
                {
                    inChq.GuarantorInfo = "Not Available";
                }
                else
                {
                    // Logic to call web service for each guarantor and build the string
                    inChq.GuarantorInfo = "Guarantor data needs web service integration.";
                }

                // Account Info Logic (Simplified - needs ECC_CAP_Services.SAFA_T24_ECC_SVCSoapClient conversion)
                // var accObj = EccAccInfo_WebSvc.ACCOUNT_INFO(incObj.AltAccount, 1);
                inChq.BookedBalance = "Data from T24";
                inChq.ClearBalance = "Data from T24";
                inChq.AccountStatus = "Data from T24";

                // Authorization Logic
                var user = await _context.Users_Tbl.SingleOrDefaultAsync(u => u.User_Name == GetUserName());
                var userGroup = user?.Group_ID ?? 0;
                var userBranch = GetBranchId();

                inChq.ShowRecommendButton = true;

                if (userGroup == (int)GroupType.Group_Status.AdminAuthorized || userBranch == "2")
                {
                    inChq.ShowApproveButton = true;
                    inChq.ShowRecommendButton = false;
                }
                else
                {
                    // Complex user limit logic needs to be converted here
                }

                // Populate the rest of the ViewModel from incObj
                inChq.Serial = incObj.Serial;
                inChq.Amount = incObj.Amount;
                inChq.DrwChqNo = incObj.DrwChqNo;
                // ... and so on for all other properties

                return View(inChq);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in InwardFinanicalWFDetailsPMADIS_NEW for ID: {ID}", id);
                // Return a user-friendly error view
                return View("Error");
            }
        }

        // Helper method to get UserName from session/context
        private string GetUserName()
        {
            // In ASP.NET Core, this is typically handled via User.Identity.Name
            return User.Identity.Name;
        }

        // Helper method to get BranchId from session/context
        private string GetBranchId()
        {
            // This should be retrieved from user claims, not session.
            // Placeholder implementation:
            var branchClaim = User.Claims.FirstOrDefault(c => c.Type == "BranchId");
            return branchClaim?.Value;
        }
    }
}

