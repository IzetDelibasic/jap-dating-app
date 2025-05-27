CREATE PROCEDURE GetPhotoApprovalStats
AS
BEGIN
    SELECT
        u.UserName AS Username,
        COUNT(CASE WHEN p.IsApproved = 1 THEN 1 END) AS ApprovedPhotos,
        COUNT(CASE WHEN p.IsApproved = 0 THEN 1 END) AS UnapprovedPhotos
    FROM
        AspNetUsers u
        LEFT JOIN
        Photos p ON u.Id = p.AppUserId
    GROUP BY 
        u.UserName
    ORDER BY 
        u.UserName;
END;