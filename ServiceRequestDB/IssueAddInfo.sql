CREATE TABLE [dbo].[IssueAddInfo]
(
	[IssueAddInfoId] INT NOT NULL PRIMARY KEY,
	[IssueID] INT FOREIGN KEY references Issues(IssueId),
	[IssueDetailID] INT FOREIGN KEY references IssueDetail(IssueDetailId),
	[DeptID] INT FOREIGN KEY references Depts(DeptId),
	[AdditionalInfo] VARCHAR(256)
)
