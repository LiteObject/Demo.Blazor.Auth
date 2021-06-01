# Demo.Blazor.Auth

-- To see all claims associated with a role
SELECT	r.Id AS [RoleId], r.Name, rc.ClaimType, rc.ClaimValue, rc.Id  AS [RoleClaimId]
FROM	[Demo.Blazor.Auth].[dbo].[AspNetRoles] AS r
		LEFT JOIN [Demo.Blazor.Auth].[dbo].[AspNetRoleClaims] AS rc ON rc.RoleId = r.Id

-- To see all users' roles & claims
SELECT	u.Email, r.Name AS [Role Name], rc.ClaimType, rc.ClaimValue
FROM	dbo.AspNetUsers AS u
		LEFT JOIN	dbo.AspNetUserRoles		AS ur ON ur.UserId = u.Id
		INNER JOIN	dbo.AspNetRoles			AS r  ON r.Id = ur.RoleId
		LEFT JOIN	dbo.AspNetRoleClaims	AS rc ON rc.RoleId = r.Id
