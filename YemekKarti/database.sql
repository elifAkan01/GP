CREATE TABLE Tarif (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(100),
    Yapilisi NVARCHAR(MAX),
    HazirlikSuresi INT,
    PismeSuresi INT,
    Enerji INT,
    GorselYolu NVARCHAR(255)
);

CREATE TABLE Malzeme (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    YemekID INT,
    Ad NVARCHAR(100),
    Birim NVARCHAR(50),
    Miktar DECIMAL(10,2)
);