﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Api.Data.AppDbContext;

#nullable disable

namespace Web.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240923145122_ExpenseFormHistoryAddedFix")]
    partial class ExpenseFormHistoryAddedFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Web.Api.Data.Entities.VpApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreateBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateofBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VpApplicationUser", (string)null);

                    b.HasDiscriminator<int>("Role");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExpenseFormId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReceiptNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseFormId");

                    b.ToTable("VpExpenses");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountantId")
                        .HasColumnType("int");

                    b.Property<string>("CreateBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrencyEnum")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("ExpenseStatusEnum")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RejectionDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AccountantId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("ManagerId");

                    b.ToTable("VpExpenseForms");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseFormHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Action")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("ExpenseFormId")
                        .HasColumnType("int");

                    b.Property<string>("MadeBy")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseFormId");

                    b.ToTable("VpExpenseFormHistories");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseFormLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ExpenseFormId")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("NewCurrency")
                        .HasColumnType("int");

                    b.Property<int?>("NewExpenseStatus")
                        .HasColumnType("int");

                    b.Property<decimal?>("NewTotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("OldCurrency")
                        .HasColumnType("int");

                    b.Property<int?>("OldExpenseStatus")
                        .HasColumnType("int");

                    b.Property<decimal?>("OldTotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseFormId");

                    b.ToTable("ExpenseFormLogs");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreateBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrencyEnum")
                        .HasColumnType("int");

                    b.Property<int>("ExpenseFormId")
                        .HasColumnType("int");

                    b.Property<string>("IbanNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ExpenseFormId")
                        .IsUnique();

                    b.ToTable("VpTransactions");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpAccountant", b =>
                {
                    b.HasBaseType("Web.Api.Data.Entities.VpApplicationUser");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpAdmin", b =>
                {
                    b.HasBaseType("Web.Api.Data.Entities.VpApplicationUser");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpEmployee", b =>
                {
                    b.HasBaseType("Web.Api.Data.Entities.VpApplicationUser");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.HasIndex("ManagerId");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpManager", b =>
                {
                    b.HasBaseType("Web.Api.Data.Entities.VpApplicationUser");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpense", b =>
                {
                    b.HasOne("Web.Api.Data.Entities.VpExpenseForm", "VpExpenseForm")
                        .WithMany("Expenses")
                        .HasForeignKey("ExpenseFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VpExpenseForm");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseForm", b =>
                {
                    b.HasOne("Web.Api.Data.Entities.VpAccountant", "VpAccountant")
                        .WithMany()
                        .HasForeignKey("AccountantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Web.Api.Data.Entities.VpEmployee", "VpEmployee")
                        .WithMany("VpExpenseForms")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Api.Data.Entities.VpManager", "VpManager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("VpAccountant");

                    b.Navigation("VpEmployee");

                    b.Navigation("VpManager");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseFormHistory", b =>
                {
                    b.HasOne("Web.Api.Data.Entities.VpExpenseForm", "ExpenseForm")
                        .WithMany("VpExpenseFormHistories")
                        .HasForeignKey("ExpenseFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpenseForm");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseFormLog", b =>
                {
                    b.HasOne("Web.Api.Data.Entities.VpExpenseForm", "VpExpenseForm")
                        .WithMany("VpExpenseFormLogs")
                        .HasForeignKey("ExpenseFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VpExpenseForm");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpTransaction", b =>
                {
                    b.HasOne("Web.Api.Data.Entities.VpExpenseForm", "ExpenseForm")
                        .WithOne()
                        .HasForeignKey("Web.Api.Data.Entities.VpTransaction", "ExpenseFormId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpenseForm");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpEmployee", b =>
                {
                    b.HasOne("Web.Api.Data.Entities.VpManager", "Manager")
                        .WithMany("Employees")
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpExpenseForm", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("VpExpenseFormHistories");

                    b.Navigation("VpExpenseFormLogs");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpEmployee", b =>
                {
                    b.Navigation("VpExpenseForms");
                });

            modelBuilder.Entity("Web.Api.Data.Entities.VpManager", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
