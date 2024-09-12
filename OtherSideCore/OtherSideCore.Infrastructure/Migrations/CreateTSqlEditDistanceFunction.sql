SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Edit_Distance](@s nvarchar(3999), @t nvarchar(3999), @max int)
RETURNS int
AS
BEGIN

DECLARE @minimal_distance int, @i int, @j int, @s_len int, @t_len int, @t_substring nvarchar(3999), @current_distance int

SELECT @i = 0, @j = 0, @s_len = LEN(@s), @t_len = LEN(@t)

IF @s_len = @t_len
	return dbo.[Levenshtein](@s,@t, @max)
ELSE IF @s_len > @t_len
	return dbo.[Levenshtein](@s,@t, @max)
ELSE
	SET @minimal_distance = @t_len

	WHILE @i < @t_len - @s_len
	BEGIN
		SET @t_substring = SUBSTRING(@t, @i, @s_len)
		SET @current_distance = dbo.[Levenshtein](@s, @t_substring, @max)
		IF @current_distance < @minimal_distance
			SET @minimal_distance = @current_distance
		SET @i = @i+1
	END

	DECLARE @edit_distance int

	SET @edit_distance = dbo.[Levenshtein](@s, @t, @max)

	IF @edit_distance < @minimal_distance
		SET @minimal_distance = @edit_distance

	return @minimal_distance
END