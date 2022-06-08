CREATE TABLE dbo.Journal (
    JournalId INT IDENTITY(1, 1) PRIMARY KEY,
    JournalEntry NVARCHAR(MAX) NULL,
    Created DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Modified DATETIME NOT NULL DEFAULT GETUTCDATE(),
);
GO

CREATE TRIGGER dbo.Journal_Modified
   ON dbo.Journal
   AFTER UPDATE
AS
BEGIN
   SET NOCOUNT ON;
   UPDATE dbo.Journal
   SET Modified = GETUTCDATE() FROM dbo.Journal j JOIN Inserted i ON 
   j.JournalId = i.JournalId
END;
GO