-- Adds the 6th applicator column used by MasterItemForm (Applicator1~6)
-- Run once on the MC database (same DB as SQLConnect). Already applied on MachineDB (2026-11-28).

IF COL_LENGTH('dbo.tbMasterItemPlan', 'Applicator6') IS NULL
BEGIN
    ALTER TABLE dbo.tbMasterItemPlan ADD Applicator6 nvarchar(50) NULL;
END
