using Microsoft.EntityFrameworkCore;
using Web.Api.Base.Enums;
using Web.Api.Data.Entities;

namespace Web.Api.Data.AppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<VpApplicationUser> VpApplicationUsers { get; set; }
        public DbSet<VpEmployee> VpEmployees { get; set; }
        public DbSet<VpManager> VpManagers { get; set; }
        public DbSet<VpAdmin> VpAdmins { get; set; }
        public DbSet<VpAccountant> VpAccountants { get; set; }
        public DbSet<VpExpense> VpExpenses { get; set; }
        public DbSet<VpExpenseForm> VpExpenseForms { get; set; }
        public DbSet<VpExpenseFormHistory> VpExpenseFormHistories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<VpApplicationUser>()
            .ToTable("VpApplicationUser")
            .HasDiscriminator<UserRoleEnum>("Role")
            .HasValue<VpEmployee>(UserRoleEnum.Employee)
            .HasValue<VpManager>(UserRoleEnum.Manager)
            .HasValue<VpAccountant>(UserRoleEnum.Accountant)
            .HasValue<VpAdmin>(UserRoleEnum.Admin);
            // Employee - Manager (Many-to-One)
            modelBuilder.Entity<VpEmployee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Employees)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee - ExpenseForm (One-to-Many)
            modelBuilder.Entity<VpExpenseForm>()
                .HasOne(ef => ef.VpEmployee)
                .WithMany(e => e.VpExpenseForms)
                .HasForeignKey(ef => ef.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Manager - ExpenseForm (Many-to-One)
            modelBuilder.Entity<VpExpenseForm>()
                .HasOne(ef => ef.VpManager)
                .WithMany() // Managers might not have a collection of forms explicitly
                .HasForeignKey(ef => ef.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Accountant - ExpenseForm (Many-to-One)
            modelBuilder.Entity<VpExpenseForm>()
                .HasOne(ef => ef.VpAccountant)
                .WithMany() // Accountants might not have a collection of forms explicitly
                .HasForeignKey(ef => ef.AccountantId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExpenseForm - Expense (One-to-Many)
            modelBuilder.Entity<VpExpense>()
                .HasOne(e => e.VpExpenseForm)
                .WithMany(ef => ef.Expenses)
                .HasForeignKey(e => e.ExpenseFormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<VpExpenseFormHistory>()
                .HasOne(history => history.ExpenseForm)
                .WithMany(form => form.VpExpenseFormHistories)
                .HasForeignKey(history => history.ExpenseFormId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
