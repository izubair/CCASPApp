CREATE TABLE [dbo].[Tickets]
(
	[TicketId] INT NOT NULL IDENTITY PRIMARY KEY,
	[Subject] VARCHAR(132) NOT NULL,
	ConstituentID NVARCHAR(128) NULL,
	Service INT NOT NULL DEFAULT 0,
	IssueId INT FOREIGN KEY references Issues(IssueId),
	IssueDetailId INT FOREIGN KEY references IssueDetail(IssueDetailId),
	IssueAddInfoId INT FOREIGN KEY references IssueAddInfo(IssueAddInfoId),
	[Description] VARCHAR(256) NOT NULL,
	DateReported Date NULL,
	TimeReported Time NULL
)
