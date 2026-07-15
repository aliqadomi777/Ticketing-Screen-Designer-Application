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
CREATE DATABASE ticketDesignerDB;
GO
USE ticketDesignerDB;
GO

CREATE TABLE Banks (
    BankID INT PRIMARY KEY IDENTITY ,
    BankName NVARCHAR (100) NOT NULL UNIQUE,
);



CREATE TABLE ButtonTypes(
    TypeID INT PRIMARY KEY IDENTITY,
    TypeName VARCHAR (100) NOT NULL UNIQUE,
);

CREATE TABLE Services (
    ServiceID INT PRIMARY KEY IDENTITY,
    ServicesName VARCHAR(100) NOT NULL UNIQUE,
);

CREATE TABLE Screens (
    ScreenID INT PRIMARY KEY IDENTITY,
    ScreenName NVARCHAR(100) NOT NULL,
    IsActive BIT DEFAULT 0 NOT NULL,
    ModifiedAt DATETIMEOFFSET DEFAULT SYSUTCDATETIME() NOT NULL,
    BankID INT NOT NULL FOREIGN KEY REFERENCES Banks(BankID) ON DELETE CASCADE,
    CONSTRAINT UniqueConstraintScreens UNIQUE (BankID, ScreenName),
);


CREATE TABLE Buttons (
    ButtonID INT PRIMARY KEY IDENTITY,
    ButtonNameEN NVARCHAR(100) NOT NULL ,
    ButtonNameAR NVARCHAR(100) NOT NULL ,
    ButtonType INT NOT NULL  FOREIGN KEY REFERENCES ButtonTypes(TypeID) ON DELETE CASCADE,
    ScreenID INT NOT NULL  FOREIGN KEY REFERENCES Screens(ScreenID) ON DELETE CASCADE,
    ModifiedAt DATETIMEOFFSET DEFAULT SYSUTCDATETIME() NOT NULL,
    CONSTRAINT UniqueConstraintScreenEN UNIQUE (ScreenID,ButtonNameEN),
    CONSTRAINT UniqueConstraintScreenAR UNIQUE (ScreenID,ButtonNameAR),
);



CREATE TABLE Tickets(
    TicketID INT PRIMARY KEY IDENTITY,
    ServiceID INT NOT NULL FOREIGN KEY REFERENCES Services(ServiceID) ON DELETE CASCADE,
    ButtonID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE,
);

CREATE TABLE Messages (
    MessageID INT PRIMARY KEY IDENTITY,
    MessageEN NVARCHAR(500) NOT NULL,
    MessageAR NVARCHAR(500) NOT NULL,
    ButtonID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE,

);



INSERT INTO ButtonTypes (TypeName) VALUES 
('Issue Ticket'),
('Show Message');



INSERT INTO Services (ServicesName) VALUES
('Open Account'),
('Take a Loan'),
('Wire Transfers'),
('Debit and Credit Cards'),
('Foreign Currency Exchange'),
('Safe Deposit Boxes'),
('Bank Guarantees'),
('Wealth and Investment Management');
GO

-- TRIGGER: Updates Screens timestamp
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
CREATE UNIQUE NONCLUSTERED INDEX UniqueIndexActiveScreen
ON Screens(BankID)
WHERE IsActive = 1;
