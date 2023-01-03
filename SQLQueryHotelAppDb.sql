CREATE DATABASE HotelAppDB;

USE HotelAppDB;

CREATE TABLE Categories
([CategoryId] nvarchar(50) NOT NULL PRIMARY KEY,
[Bed] int);

CREATE TABLE Rooms 
(RoomId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
[Size] decimal(4,2),
[ExtraBed] int,
[Type] nvarchar(50),
FOREIGN KEY(Type) REFERENCES Categories(CategoryId));

CREATE TABLE Guests 
(GuestId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
[Name] nvarchar(50),
[Age] int,
[Street] nvarchar(50),
[City] nvarchar(50),
[PostalCode] int,
[Phone] nvarchar(50));

CREATE TABLE Bookings
(BookingId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
[BookingDate] DateTime,
[CheckInDate] DateTime,
[CheckOutDate] DateTime,
[RoomId] int,
[GuestId] int,
FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId),
FOREIGN KEY (GuestId) REFERENCES Guests(GuestId));

INSERT INTO Categories
VALUES 
('Single', 1),
('Double', 2);

INSERT INTO Rooms
VALUES
(37, null, 'Single'),
(40, null, 'Single'),
(45, null, 'Double'),
(60, null, 'Double');

UPDATE Rooms SET 
ExtraBed = 2 WHERE RoomId = 3;

INSERT INTO Guests
VALUES
('Steve Smith',30,'Test Road 1','Rockley', 12345,'123-456-7894'),
('Glen Mcgrath', 20,'Demo Road 7','Chatford',67890,'128-656-6891'),
('Adam Gilchrist',22,'Main Boulv. 11','Transview',14725,'928-657-3002'),
('Ricky Ponting',25,'Super Street 89','Elside',25836,'628-676-1009');

INSERT INTO Bookings
VALUES
('2022-12-03', '2022-12-31', '2023-01-01', 2, 4),
('2022-12-30', '2023-01-06', '2023-01-08', 3, 2),
('2023-01-03', '2023-01-04', '2023-01-05', 1, 1),
('2023-01-03', '2023-01-07', '2023-01-09', 4, 1),
('2022-12-25', '2022-12-26', '2022-12-27', 1, 1),
('2022-12-12', '2022-12-15', '2022-12-19', 2, 3);


----------------SELECT/WHERE/ORDER BY (possible sample statements)----------------

SELECT * FROM Categories 
WHERE CategoryId = 'Single';

SELECT * FROM Rooms 
WHERE Type = 'Double';

SELECT GuestId, Name, Age, Phone, 
CONCAT(Street, ' ', City, ' ', PostalCode) AS Address
FROM Guests 
WHERE Age > 20
AND Name LIKE 'Steve%' OR Name LIKE '%Ponting';

SELECT * FROM Bookings 
WHERE BookingDate > '2022-12-01' 
AND BookingDate < '2022-12-31' 
Order By BookingDate;

----------------JOINS/GROUP BY/SUB QUERY (possible sample statements)----------------

SELECT * FROM Bookings b
INNER JOIN Guests g
ON b.GuestId = g.GuestId
INNER JOIN Rooms r
ON b.RoomId = r.RoomId
ORDER BY BookingDate;

SELECT 
RoomId AS [Room Number],
GuestId As [Guest Id],
COUNT(BookingId) AS [No of Bookings]
FROM Bookings
GROUP BY RoomId, GuestId
ORDER BY 1;

SELECT * FROM Guests g
WHERE g.Age = (SELECT MIN(age) FROM Guests);

SELECT * FROM Bookings b
INNER JOIN Rooms r
ON b.RoomId = r.RoomId
WHERE b.RoomId = 
(SELECT r.RoomId FROM Rooms r WHERE r.Type = 'Double' AND r.ExtraBed IS NOT NULL);
