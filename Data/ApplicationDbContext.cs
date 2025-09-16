using Microsoft.EntityFrameworkCore;
using SAFA_ECC_Core_Clean.Models;

namespace SAFA_ECC_Core_Clean.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<Users_Tbl> Users_Tbl { get; set; }
        public DbSet<Companies_Tbl> Companies_Tbl { get; set; }
        public DbSet<Groups_Tbl> Groups_Tbl { get; set; }
        public DbSet<Company_Types_Tbl> Company_Types_Tbl { get; set; }
        public DbSet<Group_Types_Tbl> Group_Types_Tbl { get; set; }

        // Transaction Tables
        public DbSet<Inward_Trans> Inward_Trans { get; set; }
        public DbSet<Outward_Trans> Outward_Trans { get; set; }
        public DbSet<Post_Dated_Trans> Post_Dated_Trans { get; set; }

        // Supporting Tables
        public DbSet<Return_Codes_Tbl> Return_Codes_Tbl { get; set; }
        public DbSet<Banks_Tbl> Banks_Tbl { get; set; }
        public DbSet<Bank_Branches_Tbl> Bank_Branches_Tbl { get; set; }

        // Image Tables
        public DbSet<Cheque_Images_Tbl> Cheque_Images_Tbl { get; set; }
        public DbSet<Cheque_Images_Link_Tbl> Cheque_Images_Link_Tbl { get; set; }

        // Authorization Tables
        public DbSet<AuthTrans_User_TBL> AuthTrans_User_TBL { get; set; }
        public DbSet<AuthTrans_User_TBL_Auth> AuthTrans_User_TBL_Auth { get; set; }
        public DbSet<Users_Permissions> Users_Permissions { get; set; }
        public DbSet<Group_Permissions> Group_Permissions { get; set; } // Added Group_Permissions

        // Hold Tables
        public DbSet<Hold_CHQ> Hold_CHQ { get; set; }

        // E-Channels Tables
        public DbSet<E_Channels_Cheques> E_Channels_Cheques { get; set; }
        public DbSet<EChannels_Imgs> EChannels_Imgs { get; set; }

        // File Management Tables
        public DbSet<FileRecords_Tbl> FileRecords_Tbl { get; set; }

        // Email Tables
        public DbSet<Email> Email { get; set; }

        // Restriction Tables
        public DbSet<POSTING_RESTRICTION_TBL> POSTING_RESTRICTION_TBL { get; set; }

        // T24 Tables
        public DbSet<T24_Work_Day_Tbl> T24_Work_Day_Tbl { get; set; }

        // Clearing Tables
        public DbSet<ClrFiles_Tbl> ClrFiles_Tbl { get; set; }

        // Workflow Tables
        public DbSet<INWARD_WF_Tbl> INWARD_WF_Tbl { get; set; }
        public DbSet<OUTWARD_WF_Tbl> OUTWARD_WF_Tbl { get; set; }

        // Image Tables
        public DbSet<Outward_Imgs> Outward_Imgs { get; set; }

        // Admin Tables
        public DbSet<Password_Policies_TBL> Password_Policies_TBL { get; set; }
        public DbSet<App_Pages> App_Pages { get; set; }
        public DbSet<OnUs_Tbl> OnUs_Tbl { get; set; }
        public DbSet<Menu_Items_Tbl> Menu_Items_Tbl { get; set; }

        // Currency and Account Tables
        public DbSet<CURRENCY_TBL> CURRENCY_TBL { get; set; }
        public DbSet<ACCOUNT_ALT_NO> ACCOUNT_ALT_NO { get; set; }
        public DbSet<Drawee_Account_Decrypt_Tbl> Drawee_Account_Decrypt_Tbl { get; set; }
        public DbSet<InwardCustomerDuesReport> InwardCustomerDuesReport { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Users_Tbl>()
                .HasOne(u => u.Companies_Tbl)
                .WithMany(c => c.Users) // Changed from c.Users_Tbl to c.Users
                .HasForeignKey(u => u.Company_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Users_Tbl>()
                .HasOne(u => u.Groups_Tbl)
                .WithMany(g => g.Users) // Changed from g.Users_Tbl to g.Users
                .HasForeignKey(u => u.Group_ID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Companies_Tbl>()
                .HasOne(c => c.Company_Types_Tbl) // Changed from c.CompanyType to c.Company_Types_Tbl
                .WithMany(ct => ct.Companies_Tbl)
                .HasForeignKey(c => c.Company_Type)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Groups_Tbl>()
                .HasOne(g => g.Group_Types_Tbl) // Changed from g.GroupType to g.Group_Types_Tbl
                .WithMany(gt => gt.Groups_Tbl)
                .HasForeignKey(g => g.Group_Type)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure default values
            modelBuilder.Entity<Users_Tbl>()
                .Property(u => u.Creation_Date)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Users_Tbl>()
                .Property(u => u.IsLockedOut)
                .HasDefaultValue(false);

            modelBuilder.Entity<Users_Tbl>()
                .Property(u => u.IsDisabled)

                .HasDefaultValue(false);

            modelBuilder.Entity<Users_Tbl>()
                .Property(u => u.LoginStatus)
                .HasDefaultValue(false);

            modelBuilder.Entity<Users_Tbl>()
                .Property(u => u.No_Of_Attempts)
                .HasDefaultValue(0);

            // Configure indexes
            modelBuilder.Entity<Users_Tbl>()
                .HasIndex(u => u.UserName) // Changed from User_Name to UserName
                .IsUnique();

            modelBuilder.Entity<Companies_Tbl>()
                .HasIndex(c => c.Company_Code)
                .IsUnique();

            modelBuilder.Entity<Groups_Tbl>()
                .HasIndex(g => g.Group_Name) // Changed from Group_Code to Group_Name as Group_Code is not defined in Groups_Tbl
                .IsUnique();

            // Configure Transaction Tables
            modelBuilder.Entity<Inward_Trans>(entity =>
            {
                entity.HasKey(e => e.Serial);
                entity.Property(e => e.Serial).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.ChqSequance);
                entity.HasIndex(e => e.DrwChqNo);
                entity.HasIndex(e => e.TransDate);
            });

            modelBuilder.Entity<Outward_Trans>(entity =>
            {
                entity.HasKey(e => e.Serial);
                entity.Property(e => e.Serial).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.ChqSequance);
                entity.HasIndex(e => e.DrwChqNo);
                entity.HasIndex(e => e.TransDate);
            });

            modelBuilder.Entity<Post_Dated_Trans>(entity =>
            {
                entity.HasKey(e => e.Serial);
                entity.Property(e => e.Serial).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.ChqSequance);
                entity.HasIndex(e => e.DrwChqNo);
                entity.HasIndex(e => e.DueDate);
            });

            // Configure Supporting Tables
            modelBuilder.Entity<Return_Codes_Tbl>(entity =>
            {
                entity.HasKey(e => e.ReturnCode);
            });

            modelBuilder.Entity<Banks_Tbl>(entity =>
            {
                entity.HasKey(e => e.Bank_Id);
            });

            modelBuilder.Entity<Bank_Branches_Tbl>(entity =>
            {
                entity.HasKey(e => new { e.BankCode, e.BranchCode });
                
                entity.HasOne(e => e.Bank)
                    .WithMany(e => e.Branches)
                    .HasForeignKey(e => e.BankCode)
                    .HasPrincipalKey(e => e.Bank_Id);
            });

            // Configure Image Tables
            modelBuilder.Entity<Cheque_Images_Tbl>(entity =>
            {
                entity.HasKey(e => e.Serial);
                entity.Property(e => e.Serial).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Cheque_Images_Link_Tbl>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).ValueGeneratedOnAdd();
                
                entity.HasOne(e => e.ChequeImage)
                    .WithMany(e => e.ImageLinks)
                    .HasForeignKey(e => e.ImageID)
                    .HasPrincipalKey(e => e.Serial);
            });

            // Configure Authorization Tables
            modelBuilder.Entity<AuthTrans_User_TBL>(entity =>
            {
                entity.HasKey(e => e.ID); // Changed from Auth_Trans_ID to ID
                entity.Property(e => e.ID).ValueGeneratedOnAdd(); // Changed from Auth_Trans_ID to ID
            });

            modelBuilder.Entity<AuthTrans_User_TBL_Auth>(entity =>
            {
                entity.HasKey(e => e.ID); // Changed from Auth_Trans_ID to ID
                entity.Property(e => e.ID).ValueGeneratedOnAdd(); // Changed from Auth_Trans_ID to ID
            });

            modelBuilder.Entity<Users_Permissions>(entity =>
            {
                entity.HasKey(e => new { e.Application_ID, e.UserID, e.PageID, e.ActionID });
                
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserID)
                    .HasPrincipalKey(e => e.User_ID);
            });

            modelBuilder.Entity<Group_Permissions>(entity => // Added Group_Permissions configuration
            {
                entity.HasKey(e => new { e.Application_ID, e.GroupID, e.PageID, e.ActionID });
                
                entity.HasOne(e => e.Group)
                    .WithMany()
                    .HasForeignKey(e => e.GroupID)
                    .HasPrincipalKey(e => e.Group_Id);
            });

            // Configure Return Code relationships
            modelBuilder.Entity<Inward_Trans>()
                .HasOne(e => e.ReturnCodeNavigation)
                .WithMany(e => e.InwardTransactions)
                .HasForeignKey(e => e.ReturnCode)
                .HasPrincipalKey(e => e.ReturnCode);

            modelBuilder.Entity<Outward_Trans>()
                .HasOne(e => e.ReturnCodeNavigation)
                .WithMany(e => e.OutwardTransactions)
                .HasForeignKey(e => e.ReturnedCode)
                .HasPrincipalKey(e => e.ReturnCode);
        }
    }
}


