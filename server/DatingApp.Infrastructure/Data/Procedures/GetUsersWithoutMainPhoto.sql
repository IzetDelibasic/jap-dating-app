CREATE PROCEDURE GetUsersWithoutMainPhoto
AS
BEGIN
    SELECT 
        u.UserName AS Username
    FROM 
        AspNetUsers u
    WHERE 
        NOT EXISTS (
            SELECT 1
            FROM Photos p
            WHERE p.AppUserId = u.Id AND p.IsMain = 1
        )
    ORDER BY 
        u.UserName;
END;