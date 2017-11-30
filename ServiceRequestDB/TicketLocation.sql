CREATE TABLE [dbo].[TicketLocation]
(
	[TicketLocId] INT NOT NULL IDENTITY PRIMARY KEY,
	[TicketId] INT FOREIGN KEY references Tickets(TicketId),
	[Latitude] float NOT NULL,
	[Longitude] float NOT NULL,
	[HouseNo] INT NULL,
	[Street] VARCHAR(60) NULL,
	[City] VARCHAR(30) NOT NULL,
	[State] VARCHAR(30) NOT NULL,
	[PostalCode] VARCHAR(30) NULL,
	[Country] VARCHAR(60) NULL,
	[Location] VARCHAR(256) NOT NULL,
	[ParcelNo] VARCHAR(30) NULL,
	[CrossSt1] VARCHAR(30) NULL,
	[CrossSt2] VARCHAR(30) NULL
)
