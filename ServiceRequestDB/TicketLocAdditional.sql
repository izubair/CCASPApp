CREATE TABLE [dbo].[TicketLocAdditional]
(
	[TicketLocAddId] INT NOT NULL PRIMARY KEY,
	[TicketId] INT FOREIGN KEY references Tickets(TicketId),
	[JurisdictionCode] INT NOT NULL DEFAULT 1,
	[MinorCivilDiv] INT NOT NULL DEFAULT 1,
	[StateAssembly] INT NOT NULL DEFAULT 1,
	[StateSenate] INT NOT NULL DEFAULT 1,
	[CityWard] INT NOT NULL DEFAULT 1,
	[CommissionDist] INT NOT NULL DEFAULT 1,
	[Commissioner] INT NOT NULL DEFAULT 1
)
