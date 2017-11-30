CREATE TABLE [dbo].[Tickets]
(
	[TicketId] INT NOT NULL PRIMARY KEY,
	ConstituentID INT NULL FOREIGN KEY references Constituents(ConstituentId),
	Service INT NOT NULL DEFAULT 0,
	IssueId INT FOREIGN KEY references Issues(IssueId),
	IssueDetailId INT FOREIGN KEY references IssueDetail(IssueDetailId),
	IssueAddInfoId INT FOREIGN KEY references IssueAddInfo(IssueAddInfoId),
	DateReported Date NULL,
	TimeReported Time NULL
)
