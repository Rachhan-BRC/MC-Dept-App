-- Tables used by DrawingItemForm / DrawingItemDetailForm
-- Run once on the MC database (same DB as SQLConnect).

CREATE TABLE tbDrawingItem (
    DrawingID      INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ItemName       NVARCHAR(150) NOT NULL,
    ItemPart       NVARCHAR(150) NULL,
    Customer       NVARCHAR(100) NULL,
    DocumentNo     NVARCHAR(100) NULL,
    DrawingNo      NVARCHAR(100) NULL,
    PrepareBy      NVARCHAR(100) NULL,
    PEChecker      NVARCHAR(100) NULL,
    QAChecker      NVARCHAR(100) NULL,
    Approval       NVARCHAR(100) NULL,
    EffectiveDate  DATE          NULL,
    RevNo          NVARCHAR(20)  NULL,
    RefText        NVARCHAR(200) NULL,   -- e.g. "REF: PE-RE-19 Rev: 01 Date: 08-02-2020" (editable on the form)
    Status         NVARCHAR(20)  NOT NULL CONSTRAINT DF_tbDrawingItem_Status DEFAULT ('Active'), -- 'Active' / 'Deleted'
    CreateDate     DATETIME      NULL,
    CreateBy       NVARCHAR(100) NULL,
    UpdateDate     DATETIME      NULL,
    UpdateBy       NVARCHAR(100) NULL
);
GO

CREATE TABLE tbDrawingItemDetail (
    DetailID    INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DrawingID   INT NOT NULL CONSTRAINT FK_tbDrawingItemDetail_tbDrawingItem REFERENCES tbDrawingItem(DrawingID),
    RowNo       INT NOT NULL,
    WireCable   NVARCHAR(200) NULL,   -- Cable / Wire No.(MFR)
    WireColor   NVARCHAR(100) NULL,
    WireLength  NVARCHAR(50)  NULL,   -- e.g. 65±1
    CNA_Pin     NVARCHAR(100) NULL,   -- Connector A : CN (pin)
    CNA_Strip   NVARCHAR(50)  NULL,   -- Connector A : Strip (mm)
    CNA_PN      NVARCHAR(100) NULL,   -- Connector A : P/N
    CNB_Strip   NVARCHAR(50)  NULL,   -- Connector B : Strip (mm)
    CNB_PN      NVARCHAR(100) NULL,   -- Connector B : P/N
    CNB_Pin     NVARCHAR(100) NULL,   -- Connector B : CN (pin)
    SameMask    INT           NULL    -- bit flags: which columns show the "same as above" arrow
);
GO

CREATE INDEX IX_tbDrawingItemDetail_DrawingID ON tbDrawingItemDetail(DrawingID, RowNo);
GO

-- If tbDrawingItem already exists, add the new column instead:
IF COL_LENGTH('tbDrawingItem', 'RefText') IS NULL
    ALTER TABLE tbDrawingItem ADD RefText NVARCHAR(200) NULL;
GO

IF COL_LENGTH('tbDrawingItemDetail', 'SameMask') IS NULL
    ALTER TABLE tbDrawingItemDetail ADD SameMask INT NULL;
GO
