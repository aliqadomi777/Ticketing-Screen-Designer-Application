--These commented commands used to drop tables if they exist 

/*
IF OBJECT_ID('dbo.Messages', 'U') IS NOT NULL DROP TABLE dbo.Messages;
IF OBJECT_ID('dbo.Tickets', 'U') IS NOT NULL DROP TABLE dbo.Tickets;
IF OBJECT_ID('dbo.Buttons', 'U') IS NOT NULL DROP TABLE dbo.Buttons;
IF OBJECT_ID('dbo.Screens', 'U') IS NOT NULL DROP TABLE dbo.Screens;
IF OBJECT_ID('dbo.Services', 'U') IS NOT NULL DROP TABLE dbo.Services;
IF OBJECT_ID('dbo.ButtonTypes', 'U') IS NOT NULL DROP TABLE dbo.ButtonTypes;
IF OBJECT_ID('dbo.Banks', 'U') IS NOT NULL DROP TABLE dbo.Banks;
*/ 
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ticketDesignerDB')
BEGIN
    CREATE DATABASE ticketDesignerDB;
END;
GO

USE ticketDesignerDB;
GO

IF OBJECT_ID('dbo.Banks', 'U') IS NULL
BEGIN
CREATE TABLE Banks (
    BankID INT PRIMARY KEY IDENTITY ,
    BankName NVARCHAR (100) NOT NULL UNIQUE
);
END;


IF OBJECT_ID('dbo.ButtonTypes', 'U') IS NULL
BEGIN
CREATE TABLE ButtonTypes(
    TypeID INT PRIMARY KEY IDENTITY,
    TypeName VARCHAR (100) NOT NULL UNIQUE
);
END;

IF OBJECT_ID('dbo.Services', 'U') IS NULL
BEGIN
CREATE TABLE Services (
    ServiceID INT PRIMARY KEY IDENTITY,
    ServicesName VARCHAR(100) NOT NULL UNIQUE
);
END;

IF OBJECT_ID('dbo.Screens', 'U') IS NULL
BEGIN
CREATE TABLE Screens (
    ScreenID INT PRIMARY KEY IDENTITY,
    ScreenName NVARCHAR(100) NOT NULL,
    IsActive BIT DEFAULT 0 NOT NULL,
    ModifiedAt DATETIMEOFFSET DEFAULT SYSUTCDATETIME() NOT NULL,
    BankID INT NOT NULL FOREIGN KEY REFERENCES Banks(BankID) ON DELETE CASCADE,
    CONSTRAINT UniqueConstraintScreens UNIQUE (BankID, ScreenName)
);
END;

IF OBJECT_ID('dbo.Buttons', 'U') IS NULL
BEGIN
CREATE TABLE Buttons (
    ButtonID INT PRIMARY KEY IDENTITY,
    ButtonNameEN NVARCHAR(100) NOT NULL ,
    ButtonNameAR NVARCHAR(100) NOT NULL ,
    ButtonType INT NOT NULL  FOREIGN KEY REFERENCES ButtonTypes(TypeID) ON DELETE CASCADE,
    ScreenID INT NOT NULL  FOREIGN KEY REFERENCES Screens(ScreenID) ON DELETE CASCADE,
    ModifiedAt DATETIMEOFFSET DEFAULT SYSUTCDATETIME() NOT NULL,
    CONSTRAINT UniqueConstraintScreenEN UNIQUE (ScreenID,ButtonNameEN),
    CONSTRAINT UniqueConstraintScreenAR UNIQUE (ScreenID,ButtonNameAR)
);
END;

IF OBJECT_ID('dbo.Tickets', 'U') IS NULL
BEGIN
CREATE TABLE Tickets(
    TicketID INT PRIMARY KEY IDENTITY,
    ServiceID INT NOT NULL FOREIGN KEY REFERENCES Services(ServiceID) ON DELETE CASCADE,
    ButtonID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE
);
END;

IF OBJECT_ID('dbo.Messages', 'U') IS NULL
BEGIN
CREATE TABLE Messages (
    MessageID INT PRIMARY KEY IDENTITY,
    MessageEN NVARCHAR(500) NOT NULL,
    MessageAR NVARCHAR(500) NOT NULL,
    ButtonID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE

);
END;
GO

INSERT INTO ButtonTypes (TypeName) 
SELECT v.TypeName 
FROM (VALUES ('Issue Ticket'), ('Show Message')) AS v(TypeName)
WHERE NOT EXISTS (SELECT 1 FROM ButtonTypes b WHERE b.TypeName = v.TypeName);

INSERT INTO Services (ServicesName) 
SELECT v.ServicesName 
FROM (VALUES 
    ('Open Account'),
    ('Take a Loan'),
    ('Wire Transfers'),
    ('Debit and Credit Cards'),
    ('Foreign Currency Exchange'),
    ('Safe Deposit Boxes'),
    ('Bank Guarantees'),
    ('Wealth and Investment Management')
) AS v(ServicesName)
WHERE NOT EXISTS (SELECT 1 FROM Services s WHERE s.ServicesName = v.ServicesName);
GO

-- TRIGGER: Updates Screens timestamp
DROP TRIGGER IF EXISTS  dbo.triggerModifiedAt_Screens
GO
CREATE TRIGGER dbo.triggerModifiedAt_Screens
ON dbo.Screens
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(ModifiedAt) RETURN;

    UPDATE s
    SET s.ModifiedAt = SYSUTCDATETIME()
    FROM dbo.Screens s
    INNER JOIN inserted i ON s.ScreenID = i.ScreenID;
END;
GO

DROP TRIGGER IF EXISTS  dbo.triggerModifiedAt_ButtonsDelete
GO
CREATE TRIGGER dbo.triggerModifiedAt_ButtonsDelete 
ON dbo.Buttons
AFTER DELETE 
AS 
BEGIN 
    SET NOCOUNT ON; 
    
    UPDATE s 
    SET s.ModifiedAt = SYSUTCDATETIME() 
    FROM dbo.Screens s 
    INNER JOIN deleted d ON s.ScreenID = d.ScreenID; 
END; 
GO


DROP TRIGGER IF EXISTS  dbo.triggerModifiedAt_Buttons
GO
CREATE TRIGGER dbo.triggerModifiedAt_Buttons
ON dbo.Buttons
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(ModifiedAt) RETURN;
    UPDATE b
    SET b.ModifiedAt = SYSUTCDATETIME()
    FROM dbo.Buttons b
    INNER JOIN inserted i ON b.ButtonID = i.ButtonID;
    IF TRIGGER_NESTLEVEL(@@PROCID, 'AFTER', 'DML') = 1
    BEGIN
        UPDATE s
        SET s.ModifiedAt = SYSUTCDATETIME()
        FROM dbo.Screens s
        INNER JOIN inserted i ON s.ScreenID = i.ScreenID;
    END;
END;
GO

DROP TRIGGER IF EXISTS dbo.triggerModifiedAt_Tickets
GO
-- Updates parent Button when Ticket changes
CREATE TRIGGER dbo.triggerModifiedAt_Tickets
ON dbo.Tickets
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE b
    SET b.ModifiedAt = SYSUTCDATETIME()
    FROM dbo.Buttons b
    INNER JOIN inserted i ON b.ButtonID = i.ButtonID;
END;
GO


DROP TRIGGER IF EXISTS  dbo.triggerModifiedAt_Messages
GO
CREATE TRIGGER dbo.triggerModifiedAt_Messages
ON dbo.Messages
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE b
    SET b.ModifiedAt = SYSUTCDATETIME()
    FROM dbo.Buttons b
    INNER JOIN inserted i ON b.ButtonID = i.ButtonID;
END;
GO

-- Prevents same bank having more than one active screen
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = 'UniqueIndexActiveScreen' 
    AND object_id = OBJECT_ID('dbo.Screens')
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UniqueIndexActiveScreen
    ON dbo.Screens(BankID)
    WHERE IsActive = 1;
END;
GO
