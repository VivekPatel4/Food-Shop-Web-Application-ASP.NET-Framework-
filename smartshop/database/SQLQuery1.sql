CREATE TABLE [users] (
    [userid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(50) NULL,
    [username] VARCHAR(50) NULL UNIQUE,
    [mobile] VARCHAR(50) NULL,
    [email] VARCHAR(50) NULL UNIQUE,
    [address] VARCHAR(MAX) NULL,
    [postcode] VARCHAR(50) NULL,
    [password] VARCHAR(50) NULL,
    [imageurl] VARCHAR(MAX) NULL,
    [createddate] DATETIME NULL
);

CREATE TABLE [contact] (
    [contactid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(50) NULL,
    [email] VARCHAR(50) NULL,
    [subject] VARCHAR(200) NULL,
    [message] VARCHAR(MAX) NULL,
    [createddate] DATETIME NULL
);


CREATE TABLE [categories] (
    [categoryid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(50) NULL,
    [imageurl] VARCHAR(MAX) NULL,
    [isactive] BIT NULL,
    [createddate] DATETIME NULL
);

CREATE TABLE [products] (
    [productid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(50) NULL,
    [description] VARCHAR(MAX) NULL,
    [price] DECIMAL(18,2) NULL,
    [quantity] INT NULL,
    [isactive] BIT NULL,
    [categoryid] INT NULL, -- Foreign key
    [imageurl] VARCHAR(MAX) NULL,
    [createddate] DATETIME NULL,
    FOREIGN KEY ([categoryid]) REFERENCES [categories]([categoryid])
);

CREATE TABLE [carts] (
    [cartid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [productid] INT NULL, -- Foreign key
    [quantity] INT NULL,
    [userid] INT NULL, -- Foreign key
    FOREIGN KEY ([productid]) REFERENCES [products]([productid]),
    FOREIGN KEY ([userid]) REFERENCES [users]([userid])
);

CREATE TABLE [payment] (
    [paymentid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [name] VARCHAR(50) NULL,
    [cardno] VARCHAR(50) NULL,
    [expirydate] VARCHAR(50) NULL,
    [cvvno] VARCHAR(50) NULL,
    [address] VARCHAR(MAX) NULL,
    [paymentmode] VARCHAR(50) NULL
);

CREATE TABLE [orders] (
    [orderdetailsid] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    [orderno] VARCHAR(50) NULL UNIQUE, -- Changed from VARCHAR(MAX) to VARCHAR(50)
    [productid] INT NULL, -- Foreign key
    [quantity] INT NULL,
    [userid] INT NULL, -- Foreign key
    [status] VARCHAR(50) NULL,
    [paymentid] INT NULL, -- Foreign key
    [orderdate] DATETIME NULL,
    FOREIGN KEY ([productid]) REFERENCES [products]([productid]),
    FOREIGN KEY ([userid]) REFERENCES [users]([userid]),
    FOREIGN KEY ([paymentid]) REFERENCES [payment]([paymentid])
);
