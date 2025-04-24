CREATE database Travel;

CREATE TABLE Destination (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Country NVARCHAR(100) NOT NULL
);

CREATE TABLE Trip (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX),
    DateFrom DATE NOT NULL,
    DateTo DATE NOT NULL,
    Price DECIMAL(10,2),
    DestinationId INT FOREIGN KEY REFERENCES Destination(Id)
);

CREATE TABLE Guide (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Biography NVARCHAR(MAX),
    Email NVARCHAR(100) NOT NULL
);

-- bridge table between trip and Guide
CREATE TABLE TripGuide (
    TripId INT FOREIGN KEY REFERENCES Trip(Id),
    GuideId INT FOREIGN KEY REFERENCES Guide(Id),
    PRIMARY KEY (TripId, GuideId)
);

CREATE TABLE Wishlist (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES ApplicationUser(Id),
    TripId INT FOREIGN KEY REFERENCES Trip(Id)
);

drop table Wishlist

CREATE TABLE Wishlist (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    TripId INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETUTCDATE(),

    CONSTRAINT FK_Wishlist_User FOREIGN KEY (UserId) REFERENCES ApplicationUser(Id),
    CONSTRAINT FK_Wishlist_Trip FOREIGN KEY (TripId) REFERENCES Trip(Id)
);
ALTER TABLE Wishlist
ADD DesiredDateFrom DATETIME2 NULL,
    DesiredDateTo DATETIME2 NULL;

CREATE TABLE Log (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES ApplicationUser(Id),
    Action NVARCHAR(100),
    Entity NVARCHAR(100),
    EntityId INT,
    Timestamp DATETIME DEFAULT GETDATE()
);

CREATE TABLE ApplicationUser (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    PhoneNumber NVARCHAR(20),
    IsAdmin BIT DEFAULT 0
);


-- checking jtw token and user
SELECT * FROM ApplicationUser;
SELECT * FROM ApplicationUser WHERE UserName = 'john';
DELETE FROM ApplicationUser WHERE UserName = 'dcaric';

SELECT * FROM TripGuide;

SELECT * FROM Trip;
SELECT * FROM sys.tables;
SELECT * FROM Wishlist;

USE Travel;
CREATE USER [DESKTOP-69AGKI8\Dario] FOR LOGIN [DESKTOP-69AGKI8\Dario];
ALTER ROLE db_owner ADD MEMBER [DESKTOP-69AGKI8\Dario];
USE master
delete from Wishlist where id=1


-- INSERT statements for Destination table (approximately 30 destinations across Croatia)
INSERT INTO Destination (Name, Country) VALUES
('Dubrovnik', 'Croatia'),
('Split', 'Croatia'),
('Hvar', 'Croatia'),
('Zadar', 'Croatia'),
('Rovinj', 'Croatia'),
('Pula', 'Croatia'),
('Korčula', 'Croatia'),
('Brač', 'Croatia'),
('Šibenik', 'Croatia'),
('Trogir', 'Croatia'),
('Zagreb', 'Croatia'),
('Plitvice Lakes National Park', 'Croatia'),
('Krka National Park', 'Croatia'),
('Mljet National Park', 'Croatia'),
('Paklenica National Park', 'Croatia'),
('Risnjak National Park', 'Croatia'),
('Northern Velebit National Park', 'Croatia'),
('Brijuni National Park', 'Croatia'),
('Kornati National Park', 'Croatia'),
('Pag', 'Croatia'),
('Rab', 'Croatia'),
('Lošinj', 'Croatia'),
('Cres', 'Croatia'),
('Vis', 'Croatia'),
('Lastovo', 'Croatia'),
('Opatija', 'Croatia'),
('Cavtat', 'Croatia'),
('Primošten', 'Croatia'),
('Makarska', 'Croatia'),
('Slano', 'Croatia');

-- INSERT statements for Trip table (approximately 25 trips)

-- Cycling through Slavonia - using Osijek as a representative destination
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Cycling through Slavonia', 'Discover the rural beauty of the Slavonia region by bike.', '2025-09-20', '2025-09-23', 260.00, (SELECT Id FROM Destination WHERE Name = 'Osijek'));

-- Birdwatching in Lonjsko Polje - using Sisak as a nearby destination (adjust if needed)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Birdwatching in Lonjsko Polje', 'Experience the diverse birdlife of Lonjsko Polje Nature Park.', '2025-05-05', '2025-05-06', 120.00, (SELECT Id FROM Destination WHERE Name = 'Sisak'));

-- Hiking in Risnjak National Park
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Hiking in Risnjak National Park', 'Explore the diverse trails of Risnjak National Park.', '2025-06-10', '2025-06-12', 195.50, (SELECT Id FROM Destination WHERE Name = 'Risnjak National Park'));

-- Exploring the Caves of Istria - using Pula as a base
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Exploring the Caves of Istria', 'Discover the fascinating underground world of Istrian caves.', '2025-07-25', '2025-07-26', 155.00, (SELECT Id FROM Destination WHERE Name = 'Pula'));

-- Dubrovnik City Walls Walk
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Dubrovnik City Walls Walk', 'Walk along the iconic city walls of Dubrovnik.', '2025-08-15', '2025-08-15', 55.00, (SELECT Id FROM Destination WHERE Name = 'Dubrovnik'));


Great to hear that the previous INSERT statements worked! Here are another 20 example trips across Croatia to further populate your Trip table:

SQL

-- Another 20 INSERT statements for Trip table

-- Zagreb Christmas Market Tour (seasonal)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Zagreb Christmas Market Magic', 'Experience the award-winning Zagreb Christmas Markets.', '2025-12-01', '2025-12-03', 210.00, (SELECT Id FROM Destination WHERE Name = 'Zagreb'));

-- Sailing around the Elaphiti Islands (near Dubrovnik)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Elaphiti Islands Sailing Adventure', 'Sail through the stunning Elaphiti archipelago near Dubrovnik.', '2025-06-10', '2025-06-14', 650.50, (SELECT Id FROM Destination WHERE Name = 'Dubrovnik'));

-- Hiking the Velebit Trail (near Northern Velebit NP)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Velebit Mountain Hiking Expedition', 'Explore the diverse landscapes of the Velebit mountain range.', '2025-07-01', '2025-07-05', 390.75, (SELECT Id FROM Destination WHERE Name = 'Northern Velebit National Park'));

-- Exploring the island of Vis by scooter
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Vis Island Scooter Adventure', 'Discover the hidden coves and charming villages of Vis by scooter.', '2025-08-05', '2025-08-08', 320.00, (SELECT Id FROM Destination WHERE Name = 'Vis'));

-- Kayaking in the Telašćica Nature Park (near Kornati)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Telašćica Nature Park Kayak Tour', 'Paddle through the stunning cliffs and bays of Telašćica.', '2025-07-15', '2025-07-17', 265.25, (SELECT Id FROM Destination WHERE Name = 'Kornati National Park'));

-- Wine and Olive Oil Tasting in Istria (near Rovinj)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Istrian Wine & Olive Oil Experience', 'Indulge in the flavors of Istria with local wine and olive oil tastings.', '2025-09-05', '2025-09-06', 185.00, (SELECT Id FROM Destination WHERE Name = 'Rovinj'));

-- Exploring the Walls of Ston (near Slano)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Ston Walls and Salt Pans Tour', 'Discover the impressive Walls of Ston and the ancient salt pans.', '2025-06-25', '2025-06-26', 115.50, (SELECT Id FROM Destination WHERE Name = 'Slano'));

-- Rafting on the Cetina River (near Split/Omiš)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Cetina River White Water Rafting', 'Experience an exciting white water rafting adventure on the Cetina River.', '2025-07-20', '2025-07-20', 85.00, (SELECT Id FROM Destination WHERE Name = 'Split')); -- Omiš is near Split

-- Exploring the Blue Cave on Biševo Island (near Vis)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Biševo Blue Cave Excursion', 'Witness the magical blue light inside the famous Blue Cave.', '2025-08-10', '2025-08-10', 95.75, (SELECT Id FROM Destination WHERE Name = 'Vis'));

-- Discovering the island of Cres by bike
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Cres Island Cycling Holiday', 'Explore the rugged beauty and griffon vultures of Cres by bike.', '2025-09-10', '2025-09-14', 410.00, (SELECT Id FROM Destination WHERE Name = 'Cres'));

-- A culinary tour of Zagreb
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Taste of Zagreb Food Tour', 'Savor the local cuisine with a guided food tour of Zagreb.', '2025-05-15', '2025-05-16', 145.25, (SELECT Id FROM Destination WHERE Name = 'Zagreb'));

-- Exploring the lavender fields of Hvar
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Hvar Lavender Fields Experience (seasonal)', 'Visit the fragrant lavender fields of Hvar during blooming season.', '2025-07-05', '2025-07-06', 70.00, (SELECT Id FROM Destination WHERE Name = 'Hvar'));

-- Hiking in Paklenica National Park (different trails)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Paklenica Canyon Hiking Adventure', 'Discover the dramatic canyons and trails of Paklenica National Park.', '2025-06-20', '2025-06-22', 230.50, (SELECT Id FROM Destination WHERE Name = 'Paklenica National Park'));

-- A boat trip to the island of Lastovo
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Lastovo Archipelago Boat Trip', 'Explore the remote and stunning Lastovo archipelago by boat.', '2025-08-20', '2025-08-22', 315.00, (SELECT Id FROM Destination WHERE Name = 'Lastovo'));

-- Discovering the charm of Opatija's Riviera
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Opatija Riviera Promenade Walk', 'Enjoy a leisurely walk along the beautiful coastal promenade of Opatija.', '2025-05-20', '2025-05-21', 90.00, (SELECT Id FROM Destination WHERE Name = 'Opatija'));

-- Exploring the ancient Salona ruins (near Split)
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Salona Roman Ruins Exploration', 'Discover the fascinating remains of the ancient Roman city of Salona.', '2025-06-15', '2025-06-15', 65.75, (SELECT Id FROM Destination WHERE Name = 'Split'));

-- A visit to the island of Rab and its sandy beaches
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Rab Island Sandy Beaches Getaway', 'Relax on the beautiful sandy beaches and explore the charming town of Rab.', '2025-07-25', '2025-07-28', 480.00, (SELECT Id FROM Destination WHERE Name = 'Rab'));

-- Exploring the Kornati islands by sailboat
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Kornati Islands Sailing Expedition', 'Experience the unique beauty of the Kornati islands on a sailing trip.', '2025-08-25', '2025-08-28', 720.25, (SELECT Id FROM Destination WHERE Name = 'Kornati National Park'));

-- A historical walking tour of Šibenik
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Šibenik Historical City Walk', 'Discover the rich history and architecture of the city of Šibenik.', '2025-09-15', '2025-09-16', 105.00, (SELECT Id FROM Destination WHERE Name = 'Šibenik'));

-- Exploring the island of Lošinj and its aromatherapy gardens
INSERT INTO Trip (Name, Description, DateFrom, DateTo, Price, DestinationId) VALUES
('Lošinj Aromatherapy & Island Tour', 'Discover the fragrant herbs and beautiful landscapes of Lošinj.', '2025-06-05', '2025-06-08', 355.50, (SELECT Id FROM Destination WHERE Name = 'Lošinj'));


SELECT * FROM Trip WHERE Price IS NULL OR DestinationId IS NULL;
delete from trip where id=5