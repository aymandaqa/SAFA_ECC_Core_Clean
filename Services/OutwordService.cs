

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

