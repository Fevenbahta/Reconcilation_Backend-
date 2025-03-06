using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LIB.API.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InReconcileds",
                columns: table => new
                {
                    No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ACCOUNT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DISCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    INPUTING_BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debitor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderingAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeneficiaryAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InReconcileds", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "InRtgsAtss",
                columns: table => new
                {
                    No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debitor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderingAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeneficiaryAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InRtgsAtss", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "InRtgsCbcs",
                columns: table => new
                {
                    REFNO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ACCOUNT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DEBITOR_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DISCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    INPUTING_BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TRANSACTION_DATE = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InRtgsCbcs", x => x.REFNO);
                });

            migrationBuilder.CreateTable(
                name: "outReconcileds",
                columns: table => new
                {
                    No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ACCOUNT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DISCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    INPUTING_BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DATET = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debitor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderingAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeneficiaryAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outReconcileds", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "OutRtgsAtss",
                columns: table => new
                {
                    No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Debitor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderingAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeneficiaryAccount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcessingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutRtgsAtss", x => x.No);
                });

            migrationBuilder.CreateTable(
                name: "OutRtgsCbcs",
                columns: table => new
                {
                    REFNO = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ACCOUNT = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DEBITOR_NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DISCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMOUNT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    INPUTING_BRANCH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DATET = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutRtgsCbcs", x => x.REFNO);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InReconcileds");

            migrationBuilder.DropTable(
                name: "InRtgsAtss");

            migrationBuilder.DropTable(
                name: "InRtgsCbcs");

            migrationBuilder.DropTable(
                name: "outReconcileds");

            migrationBuilder.DropTable(
                name: "OutRtgsAtss");

            migrationBuilder.DropTable(
                name: "OutRtgsCbcs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
