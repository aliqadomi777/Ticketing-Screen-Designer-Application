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
    IsActive BIT DEFAULT 1 NOT NULL,
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
    ButtonID INT NOT NULL FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE,
    CONSTRAINT UniqueConstraintTickets UNIQUE (ButtonID)
);

CREATE TABLE Messages (
    MessageID INT PRIMARY KEY IDENTITY,
    MessageEN NVARCHAR(500) NOT NULL,
    MessageAR NVARCHAR(500) NOT NULL,
    ButtonID INT NOT NULL FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE,
    CONSTRAINT UniqueConstraintMessageEN UNIQUE (ButtonID,MessageEN),
    CONSTRAINT UniqueConstraintMessageAR UNIQUE (ButtonID,MessageAR),
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

/* turn this into a proc
DECLARE @BankID INT;       
DECLARE @NewScreenID INT; 

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRANSACTION;

    SELECT 1 
    FROM Banks WITH (UPDLOCK, HOLDLOCK) 
    WHERE BankID = @BankID;

    UPDATE Screens 
    SET IsActive = 0 
    WHERE BankID = @BankID AND IsActive = 1;

    UPDATE S 
    SET S.IsActive = 1 
    FROM Screens S
    WHERE S.ScreenID = @NewScreenID 
      AND S.BankID = @BankID;

COMMIT TRANSACTION;


*/

