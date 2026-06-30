CREATE TABLE Banks (
    BankID INT PRIMARY KEY IDENTITY ,
    BankName VARCHAR (100) NOT NULL UNIQUE,
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
    ScreenName VARCHAR(100) NOT NULL,
    IsActive BIT DEFAULT 1 NOT NULL,
    ModifiedAt DATETIMEOFFSET DEFAULT SYSUTCDATETIME() NOT NULL,
    BankID INT NOT NULL FOREIGN KEY REFERENCES Banks(BankID) ON DELETE CASCADE,
    CONSTRAINT UniqueConstraintScreens UNIQUE (BankID, ScreenName),
);


CREATE TABLE Buttons (
    ButtonID INT PRIMARY KEY IDENTITY,
    ButtonNameEN VARCHAR(100) NOT NULL ,
    ButtonNameAR NVARCHAR(100) NOT NULL ,
    ButtonType INT NOT NULL  FOREIGN KEY REFERENCES ButtonTypes(TypeID) ON DELETE CASCADE,
    ScreenID INT NOT NULL  FOREIGN KEY REFERENCES Screens(ScreenID) ON DELETE CASCADE,
    ModifiedAt DATETIMEOFFSET DEFAULT SYSUTCDATETIME() NOT NULL,
    CONSTRAINT UniqueConstraintScreenEN UNIQUE (ScreenID,ButtonNameEN),
    CONSTRAINT UniqueConstraintScreenAR UNIQUE (ScreenID,ButtonNameAR),
);



CREATE TABLE Tickets(
    TicketID INT PRIMARY KEY IDENTITY,
    ServicesID INT FOREIGN KEY REFERENCES Services(ServiceID) ON DELETE CASCADE,
    ButtonID INT FOREIGN KEY REFERENCES Buttons(ButtonID) ON DELETE CASCADE,
    CONSTRAINT UniqueConstraintTickets UNIQUE (ButtonID)
);

CREATE TABLE Messages (
    MessageID INT PRIMARY KEY IDENTITY,
    MessageEN VARCHAR(500) NOT NULL,
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


