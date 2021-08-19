use MainCompanyPlanner;
SET NOCOUNT ON
declare ParentTasks cursor
for select ParentId 
from HierarchyDb 
group by ParentId;

declare @parentId uniqueidentifier;
declare @errorCount int = 0;
open ParentTasks;

fetch ParentTasks into @parentId;
while @@fetch_status=0
begin
	print '--- Проверка родительского элемента с Id ' + cast(@parentId as varchar(64)) + ' ---';
	declare TasksByParent cursor
	for select ChildId, LineOrder
	from HierarchyDB
	where ParentId = @parentId
	order by LineOrder;

	declare @correctLineOrder int;
	set @correctLineOrder = 1;
	declare @childId uniqueidentifier, @currentLineOrder int;
	
	open TasksByParent;
	fetch TasksByParent into @childId, @currentLineOrder;
	while @@fetch_status=0
	begin
	
		if @currentLineOrder != @correctLineOrder
		begin
		update HierarchyDB
			set LineOrder = @correctLineOrder
			where ChildId = @childId;
			set @errorCount +=1;
			print '>>> ОШИБКА: Id строки ' + cast(@childId as varchar(64));
		end
			
		set @correctLineOrder+=1;
		fetch TasksByParent into @childId, @currentLineOrder;
	
	end
	close TasksByParent;
	deallocate TasksByParent
	fetch ParentTasks into @parentId;
end
close ParentTasks;
deallocate ParentTasks;
print 'Количество ошибок в базе c не NULL parent: ' + cast(@errorCount as char(10))
go
--NULL parent
print '--- Проверка родительского элемента с Id NULL ---';
declare @errorCount int = 0;
declare TasksByParent cursor
	for select ChildId, LineOrder
	from HierarchyDB
	where ParentId is NULL
	order by LineOrder;

	declare @correctLineOrder int;
	set @correctLineOrder = 1;
	declare @childId uniqueidentifier, @currentLineOrder int;
	
	open TasksByParent;
	fetch TasksByParent into @childId, @currentLineOrder;
	while @@fetch_status=0
	begin
		if @currentLineOrder != @correctLineOrder
		begin
		update HierarchyDB
			set LineOrder = @correctLineOrder
			where ChildId = @childId;
			set @errorCount +=1;
			print '>>> ОШИБКА: Id строки ' + cast(@childId as varchar(64));
		end
			
		set @correctLineOrder+=1;
		fetch TasksByParent into @childId, @currentLineOrder;
	
	end
	close TasksByParent;
	deallocate TasksByParent

print 'Количество ошибок в базе c NULL parent: ' + cast(@errorCount as char(10))