// PLAN (pseudocode - follow carefully):
// 1. If you can afford to lose the failed migration or database contents: remove the migration and recreate it
//      - Run: Remove-Migration
//      - Fix the EF model so EF doesn't try to toggle IDENTITY
//      - Run: Add-Migration <Name> and Update-Database
// 2. If you must preserve data and the migration must change the IDENTITY property:
//      - Create a new temporary table with the desired column definition (IDENTITY or non-IDENTITY)
//      - Copy data from the old table into the temp table (use SET IDENTITY_INSERT ON when needed)
//      - Drop constraints that reference the old table/column (FKs, PKs, indexes) before swap
//      - Drop the old table
//      - Rename temp table to the original table name
//      - Recreate constraints (FKs, indexes, PKs) that you dropped
//      - Test thoroughly and backup the DB before running
// 3. Alternative for simple development DBs: drop the database and update: Update-Database will recreate schema

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourNamespace.Migrations
{
    public partial class FixOrderAndOrderItemSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // === OPTION A: If you can remove the migration and re-generate it ===
            // If you haven't applied this migration to a production DB, the easiest fix is:
            // 1) Run `Remove-Migration` in Package Manager Console
            // 2) Fix your entity classes so EF won't try to change the IDENTITY of an existing column
            // 3) Run `Add-Migration <Name>` and `Update-Database`
            //
            // === OPTION B: Manual safe swap that preserves data (SQL Server) ===
            // Replace table name and column list below with your actual table and columns.
            // Backup DB before running. Also handle dropping/recreating foreign keys that reference this table/column.
            //
            // Example swaps the `OrderItems` table to a new table whose `Id` is IDENTITY(1,1).
            migrationBuilder.Sql(@"
-- BEGIN manual column swap to change IDENTITY on [OrderItems].[Id]
SET XACT_ABORT ON;
BEGIN TRAN;

-- 1) Create temp table with the desired schema (Id as IDENTITY)
CREATE TABLE [OrderItems_temp] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [ProductId] INT NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [Quantity] INT NOT NULL
    -- Add other columns exactly as present in your original table
);

-- 2) If you need to preserve original Id values, enable IDENTITY_INSERT and copy data:
SET IDENTITY_INSERT [OrderItems_temp] ON;

INSERT INTO [OrderItems_temp] ([Id], [ProductId], [Price], [Quantity])
SELECT [Id], [ProductId], [Price], [Quantity] FROM [OrderItems];

SET IDENTITY_INSERT [OrderItems_temp] OFF;

-- 3) Drop or disable constraints that reference OrderItems (FKs from Orders or other tables).
--    You must manually drop them here if they exist. Example (replace names):
-- ALTER TABLE [SomeOtherTable] DROP CONSTRAINT [FK_SomeOtherTable_OrderItems];

-- 4) Drop the old table
DROP TABLE [OrderItems];

-- 5) Rename temp -> original
EXEC sp_rename 'OrderItems_temp', 'OrderItems';

-- 6) Recreate any dropped constraints (FKs, indexes) here.
-- Example:
-- ALTER TABLE [SomeOtherTable] ADD CONSTRAINT [FK_SomeOtherTable_OrderItems] FOREIGN KEY ([OrderItemId]) REFERENCES [OrderItems]([Id]);

COMMIT TRAN;
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the manual change if needed. Implement the reverse swap:
            // - Create a temp table that matches the old definition
            // - Copy data
            // - Drop the current table
            // - Rename temp back to original
            // WARNING: Implement Down only if you are sure of the previous schema.
            migrationBuilder.Sql(@"
-- DOWN: reverse of the Up swap (implement if/when required)
SET XACT_ABORT ON;
BEGIN TRAN;

-- Example reverse (adjust to previous non-IDENTITY definition if needed)
CREATE TABLE [OrderItems_old] (
    [Id] INT NOT NULL PRIMARY KEY, -- previous definition, maybe NOT IDENTITY
    [ProductId] INT NOT NULL,
    [Price] DECIMAL(18,2) NOT NULL,
    [Quantity] INT NOT NULL
);

SET IDENTITY_INSERT [OrderItems_old] ON;

INSERT INTO [OrderItems_old] ([Id], [ProductId], [Price], [Quantity])
SELECT [Id], [ProductId], [Price], [Quantity] FROM [OrderItems];

SET IDENTITY_INSERT [OrderItems_old] OFF;

DROP TABLE [OrderItems];

EXEC sp_rename 'OrderItems_old', 'OrderItems';

COMMIT TRAN;
");
        }
    }
}