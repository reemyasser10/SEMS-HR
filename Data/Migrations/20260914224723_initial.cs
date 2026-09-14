using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Permission");

            migrationBuilder.EnsureSchema(
                name: "Common");

            migrationBuilder.EnsureSchema(
                name: "User");

            migrationBuilder.EnsureSchema(
                name: "Tenant");

            migrationBuilder.CreateTable(
                name: "Tenant",
                schema: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachment",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContainerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoragePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachment_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailTemplate_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                schema: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantSetting",
                schema: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantSetting_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationEnum = table.Column<int>(type: "int", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RowText = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TranslatedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Translation_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationLanguage",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    IsRTL = table.Column<bool>(type: "bit", nullable: false),
                    Fk_Image = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationLanguage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationLanguage_Attachment_Fk_Image",
                        column: x => x.Fk_Image,
                        principalSchema: "Common",
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationLanguage_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lookup",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Image = table.Column<int>(type: "int", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lookup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lookup_Attachment_Fk_Image",
                        column: x => x.Fk_Image,
                        principalSchema: "Common",
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lookup_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhonePrefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkAddress_Governorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkAddress_District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordSalt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsLockedOut = table.Column<bool>(type: "bit", nullable: false),
                    IsBan = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LastLoginFailedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FK_Image = table.Column<int>(type: "int", nullable: true),
                    LastLoginBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Attachment_FK_Image",
                        column: x => x.FK_Image,
                        principalSchema: "Common",
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Functionality",
                schema: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Resource = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FunctionalityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functionality", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Functionality_Resource_Fk_Resource",
                        column: x => x.Fk_Resource,
                        principalSchema: "Permission",
                        principalTable: "Resource",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Functionality_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LookupLang",
                schema: "Common",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Source = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LookupLang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LookupLang_Lookup_Fk_Source",
                        column: x => x.Fk_Source,
                        principalSchema: "Common",
                        principalTable: "Lookup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Administrator",
                schema: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Administrator_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Administrator_User_Fk_User",
                        column: x => x.Fk_User,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    FirebaseToken = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Device_User_Fk_User",
                        column: x => x.Fk_User,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLogin",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalLogin_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExternalLogin_User_Fk_User",
                        column: x => x.Fk_User,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: false),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_Fk_User",
                        column: x => x.Fk_User,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: false),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    Fk_Role = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_Fk_Role",
                        column: x => x.Fk_Role,
                        principalSchema: "Permission",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User_Fk_User",
                        column: x => x.Fk_User,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Verification",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: false),
                    Fk_User = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verification_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Verification_User_Fk_User",
                        column: x => x.Fk_User,
                        principalSchema: "User",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_Functionality = table.Column<int>(type: "int", nullable: false),
                    Fk_Role = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedByUserID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByUserID = table.Column<int>(type: "int", nullable: true),
                    CreatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    UpdatedByDeviceID = table.Column<int>(type: "int", nullable: true),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fk_Tenant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Functionality_Fk_Functionality",
                        column: x => x.Fk_Functionality,
                        principalSchema: "Permission",
                        principalTable: "Functionality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_Fk_Role",
                        column: x => x.Fk_Role,
                        principalSchema: "Permission",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermission_Tenant_Fk_Tenant",
                        column: x => x.Fk_Tenant,
                        principalSchema: "Tenant",
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Common",
                table: "Lookup",
                columns: new[] { "Id", "ColorCode", "CreatedByDeviceID", "CreatedByIp", "CreatedByName", "CreatedByUserID", "Description", "EntityId", "EntityType", "Fk_Image", "Fk_Tenant", "IsActive", "Name", "SoftDelete", "Sort", "UpdatedAt", "UpdatedByDeviceID", "UpdatedByIp", "UpdatedByName", "UpdatedByUserID" },
                values: new object[,]
                {
                    { 11, null, null, null, null, null, null, 1, 1, null, null, true, "Male", false, 0, null, null, null, null, null },
                    { 12, null, null, null, null, null, null, 2, 1, null, null, true, "Female", false, 0, null, null, null, null, null },
                    { 13, null, null, null, null, null, null, 3, 1, null, null, true, "Unknown", false, 0, null, null, null, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_Fk_Tenant",
                schema: "Permission",
                table: "Administrator",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Administrator_Fk_User",
                schema: "Permission",
                table: "Administrator",
                column: "Fk_User",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLanguage_Fk_Image",
                schema: "Common",
                table: "ApplicationLanguage",
                column: "Fk_Image");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationLanguage_Fk_Tenant",
                schema: "Common",
                table: "ApplicationLanguage",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_Fk_Tenant",
                schema: "Common",
                table: "Attachment",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Fk_Tenant_Module_Key",
                schema: "Common",
                table: "Configuration",
                columns: new[] { "Fk_Tenant", "Module", "Key" },
                unique: true,
                filter: "[Fk_Tenant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Device_Fk_User_FirebaseToken",
                schema: "User",
                table: "Device",
                columns: new[] { "Fk_User", "FirebaseToken" },
                unique: true,
                filter: "[FirebaseToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplate_Fk_Tenant_Name",
                schema: "Common",
                table: "EmailTemplate",
                columns: new[] { "Fk_Tenant", "Name" },
                unique: true,
                filter: "[Fk_Tenant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogin_Fk_Tenant_ProviderKey",
                schema: "User",
                table: "ExternalLogin",
                columns: new[] { "Fk_Tenant", "ProviderKey" },
                unique: true,
                filter: "[Fk_Tenant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogin_Fk_User",
                schema: "User",
                table: "ExternalLogin",
                column: "Fk_User");

            migrationBuilder.CreateIndex(
                name: "IX_Functionality_Fk_Resource",
                schema: "Permission",
                table: "Functionality",
                column: "Fk_Resource");

            migrationBuilder.CreateIndex(
                name: "IX_Functionality_Fk_Tenant",
                schema: "Permission",
                table: "Functionality",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Lookup_Fk_Image",
                schema: "Common",
                table: "Lookup",
                column: "Fk_Image");

            migrationBuilder.CreateIndex(
                name: "IX_Lookup_Fk_Tenant_EntityType_Name",
                schema: "Common",
                table: "Lookup",
                columns: new[] { "Fk_Tenant", "EntityType", "Name" },
                unique: true,
                filter: "[Fk_Tenant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LookupLang_Fk_Source",
                schema: "Common",
                table: "LookupLang",
                column: "Fk_Source",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Fk_Tenant_Token",
                schema: "User",
                table: "RefreshToken",
                columns: new[] { "Fk_Tenant", "Token" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Fk_User",
                schema: "User",
                table: "RefreshToken",
                column: "Fk_User");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_Fk_Tenant",
                schema: "Permission",
                table: "Resource",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Fk_Tenant_Name",
                schema: "Permission",
                table: "Role",
                columns: new[] { "Fk_Tenant", "Name" },
                unique: true,
                filter: "[Fk_Tenant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_Fk_Functionality",
                schema: "Permission",
                table: "RolePermission",
                column: "Fk_Functionality");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_Fk_Role",
                schema: "Permission",
                table: "RolePermission",
                column: "Fk_Role");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_Fk_Tenant",
                schema: "Permission",
                table: "RolePermission",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Code",
                schema: "Tenant",
                table: "Tenant",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenant_Name",
                schema: "Tenant",
                table: "Tenant",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantSetting_Fk_Tenant_Key",
                schema: "Tenant",
                table: "TenantSetting",
                columns: new[] { "Fk_Tenant", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_Fk_Tenant_LanguageCode_ApplicationEnum_RowText",
                schema: "Common",
                table: "Translation",
                columns: new[] { "Fk_Tenant", "LanguageCode", "ApplicationEnum", "RowText" },
                unique: true,
                filter: "[Fk_Tenant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_FK_Image",
                schema: "User",
                table: "User",
                column: "FK_Image");

            migrationBuilder.CreateIndex(
                name: "IX_User_Fk_Tenant_UserName",
                schema: "User",
                table: "User",
                columns: new[] { "Fk_Tenant", "UserName" },
                unique: true,
                filter: "[SoftDelete] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Fk_Role",
                schema: "Permission",
                table: "UserRole",
                column: "Fk_Role");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Fk_Tenant",
                schema: "Permission",
                table: "UserRole",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_Fk_User_Fk_Role",
                schema: "Permission",
                table: "UserRole",
                columns: new[] { "Fk_User", "Fk_Role" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verification_Fk_Tenant",
                schema: "User",
                table: "Verification",
                column: "Fk_Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Verification_Fk_User",
                schema: "User",
                table: "Verification",
                column: "Fk_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrator",
                schema: "Permission");

            migrationBuilder.DropTable(
                name: "ApplicationLanguage",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Configuration",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Device",
                schema: "User");

            migrationBuilder.DropTable(
                name: "EmailTemplate",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "ExternalLogin",
                schema: "User");

            migrationBuilder.DropTable(
                name: "LookupLang",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "User");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "Permission");

            migrationBuilder.DropTable(
                name: "TenantSetting",
                schema: "Tenant");

            migrationBuilder.DropTable(
                name: "Translation",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Permission");

            migrationBuilder.DropTable(
                name: "Verification",
                schema: "User");

            migrationBuilder.DropTable(
                name: "Lookup",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Functionality",
                schema: "Permission");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Permission");

            migrationBuilder.DropTable(
                name: "User",
                schema: "User");

            migrationBuilder.DropTable(
                name: "Resource",
                schema: "Permission");

            migrationBuilder.DropTable(
                name: "Attachment",
                schema: "Common");

            migrationBuilder.DropTable(
                name: "Tenant",
                schema: "Tenant");
        }
    }
}
