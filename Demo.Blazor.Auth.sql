SELECT *  FROM [Demo.Blazor.Auth].[dbo].[AspNetUsers]
SELECT *  FROM [Demo.Blazor.Auth].[dbo].[AspNetRoles]
SELECT *  FROM [Demo.Blazor.Auth].[dbo].[AspNetRoleClaims]

SELECT	r.Id AS [RoleId], r.Name, rc.ClaimType, rc.ClaimValue, rc.Id  AS [RoleClaimId]
FROM	[Demo.Blazor.Auth].[dbo].[AspNetRoles] AS r
		LEFT JOIN [Demo.Blazor.Auth].[dbo].[AspNetRoleClaims] AS rc ON rc.RoleId = r.Id

SELECT	u.Email, r.Name AS [Role Name], rc.ClaimType, rc.ClaimValue
FROM	dbo.AspNetUsers AS u
		LEFT JOIN	dbo.AspNetUserRoles		AS ur ON ur.UserId = u.Id
		INNER JOIN	dbo.AspNetRoles			AS r  ON r.Id = ur.RoleId
		LEFT JOIN	dbo.AspNetRoleClaims	AS rc ON rc.RoleId = r.Id

/*
INSERT INTO [Demo.Blazor.Auth].[dbo].[AspNetRoleClaims](RoleId, ClaimType, ClaimValue) 
VALUES ('7f0bf293-895e-4e27-a176-815dcd3ed7ca', 'Permission.User', 'CanAdd')
*/
/* 
UPDATE [Demo.Blazor.Auth].[dbo].[AspNetRoleClaims]
SET	ClaimType = 'Permission.Role', ClaimValue = 'CanView' 
WHERE Id = 1 
*/