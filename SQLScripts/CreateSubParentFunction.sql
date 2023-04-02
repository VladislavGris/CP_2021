create function GetSubParent(@childId uniqueidentifier) returns nvarchar(max)
as
begin 
	declare @pos int;
	declare @taskName nvarchar(max);
	select @pos = max(Position) from GetParents(@childId); 
	select @taskName = Name from GetParents(@childId) where Position = @pos - 1;
	return @taskName;
end