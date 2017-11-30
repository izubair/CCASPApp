CREATE TABLE [dbo].[IssueDetail]
(
	[IssueDetailId] INT NOT NULL PRIMARY KEY,
	[IssueID] INT FOREIGN KEY references Issues(IssueId),
	[DeptID] INT FOREIGN KEY references Depts(DeptId),
	[Details] VARCHAR(256) NOT NULL
)
