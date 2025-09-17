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

