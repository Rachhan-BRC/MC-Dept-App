-- Table used by RegisterApplicatorForm (Admin > Register Applicator)
-- Run once on the MC database (same DB as SQLConnect).

IF OBJECT_ID('tbMasterApplicator', 'U') IS NULL
BEGIN
    CREATE TABLE tbMasterApplicator (
        SysNo       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Applicator  NVARCHAR(100) NOT NULL,
        Remark      NVARCHAR(500) NULL,
        RegDate     DATETIME      NULL,
        RegBy       NVARCHAR(100) NULL,
        UpdateDate  DATETIME      NULL,
        UpdateBy    NVARCHAR(100) NULL,
        CONSTRAINT UQ_tbMasterApplicator_Applicator UNIQUE (Applicator)
    );
END
GO
