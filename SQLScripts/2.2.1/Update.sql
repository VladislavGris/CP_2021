go
update Formatting
set Color = '#FFFF0000'
where IsItalic = 1;
go
create procedure Update_Production_Plan @id uniqueidentifier,
										@incDoc nvarchar(100),
										@taskName nvarchar(100),
										@count int,
										@specCost nvarchar(100),
										@note nvarchar(max),
										@expanded bit,
										@completion smallint,
										@editingBy nvarchar(15)
as
begin
	set nocount on;
	update Production_Plan
	set Inc_Doc = @incDoc,
		Task_Name = @taskName,
		P_Count = @count,
		Specification_Cost = @specCost,
		Note = @note,
		Expanded = @expanded,
		Completion = @completion,
		EditingBy = @editingBy
	where Id = @id;
end
--drop procedure Update_Production_Plan
go
drop index idx_Complectation_Data on Complectation;
go
alter table Complectation
alter column Note nvarchar(max) null;
go
create index idx_Complectation_Data
on Complectation(Production_Task_Id) include(Id, Complectation, C_Date, Comp_Percentage, OnStorageDate, StateNumber, Rack, Shelf, Note);
go
create procedure UpdataComplecationData (
		@Id [uniqueidentifier],
		@Complectation [nvarchar](100),
		@C_Date [date],
		@Comp_Percentage [float],
		@OnStorageDate [datetime2](7),
		@StateNumber [nvarchar](100),
		@Rack [nvarchar](10),
		@Shelf [nvarchar](10),
		@Note [nvarchar](max)
		) as
begin
	SET NOCOUNT ON
	begin try
		begin tran
		UPDATE dbo.Complectation
			SET Complectation = @Complectation, C_Date = @C_Date, 
			Comp_Percentage = @Comp_Percentage, OnStorageDate = @OnStorageDate, 
			StateNumber = @StateNumber, Rack = @Rack, Shelf = @Shelf, Note = @Note
			WHERE Id = @Id
		commit
	end try
	begin catch
		rollback;
		print 'Error';
		THROW;
	end catch
end
--drop procedure UpdataComplecationData
go
--drop procedure UpdateConsumeAct
create procedure UpdateConsumeAct(
	@Id [uniqueidentifier],
	@ActNumber [nvarchar](100),
	@ActDate [date],
	@ActCreation [bit],
	@ByAct [bit],
	@Note [nvarchar](max)
) as
begin
	set nocount on;
	begin try
		begin tran
			update Act 
			set ActNumber = @ActNumber, ActDate = @ActDate, ActCreation = @ActCreation, ByAct = @ByAct, Note = @Note
			where Id = @Id;
		commit
	end try
	begin catch
		rollback;
		print 'Error';
		throw;
	end catch;
end
go
alter table Act
alter column Note nvarchar(max) null;
go
create procedure UpdateGivingData
    @G_State bit = NULL,
    @Bill nvarchar(100) = NULL,
    @Report nvarchar(100) = NULL,
    @Return_Report nvarchar(100) = NULL,
    @Receiving_Date date = NULL,
    @ReturnGiving bit,
    @PurchaseGoods nvarchar(100) = NULL,
    @Note nvarchar(max) = NULL,
    @Id uniqueidentifier as
begin
	begin try
		begin tran;
		UPDATE [dbo].[Giving]
		SET    [G_State] = @G_State, [Bill] = @Bill, [Report] = @Report, [Return_Report] = @Return_Report, [Receiving_Date] = @Receiving_Date, [ReturnGiving] = @ReturnGiving, [PurchaseGoods] = @PurchaseGoods, [Note] = @Note
		WHERE  Id = @Id
		commit;
	end try
	begin catch
		rollback;
		print 'Error';
		throw;
	end catch
end
--drop procedure UpdateGivingData
go
alter table Giving
alter column Note nvarchar(max) null;
go
--drop procedure UpdateInProductionData
CREATE PROC UpdateInProductionData
    @Id uniqueidentifier,
    @Number nvarchar(100) = NULL,
    @Giving_Date date = NULL,
    @Executor_Name nvarchar(70) = NULL,
    @Install_Executor_Name nvarchar(70) = NULL,
    @Completion_Date date = NULL,
    @Projected_Date date = NULL,
    @Note nvarchar(max) = NULL
AS 
	SET NOCOUNT ON 
	UPDATE [dbo].[In_Production]
	SET    [Number] = @Number, [Giving_Date] = @Giving_Date, [Executor_Name] = @Executor_Name, [Install_Executor_Name] = @Install_Executor_Name, [Completion_Date] = @Completion_Date, [Projected_Date] = @Projected_Date, [Note] = @Note
	WHERE  [Id] = @Id
go
alter table In_Production
alter column Note nvarchar(max) null;
go
--drop procedure UpdateManufactureData
CREATE PROC UpdateManufactureData
    @Id uniqueidentifier,
    @M_Name nvarchar(100) = NULL,
    @Letter_Num nvarchar(100) = NULL,
    @Specification_Num nvarchar(100) = NULL,
    @OnControl bit,
    @VipiskSpec bit,
    @PredictDate datetime2(7) = NULL,
    @FactDate datetime2(7) = NULL,
    @ExecutionAct bit,
    @ExecutionTerm datetime2(7) = NULL,
    @CalendarDays nvarchar(15) = NULL,
    @WorkingDays nvarchar(15) = NULL,
    @Note nvarchar(max) = NULL
AS 
	SET NOCOUNT ON 
	UPDATE [dbo].[Manufacture]
	SET    [M_Name] = @M_Name, [Letter_Num] = @Letter_Num, [Specification_Num] = @Specification_Num, [OnControl] = @OnControl, [VipiskSpec] = @VipiskSpec, [PredictDate] = @PredictDate, [FactDate] = @FactDate, [ExecutionAct] = @ExecutionAct, [ExecutionTerm] = @ExecutionTerm, [CalendarDays] = @CalendarDays, [WorkingDays] = @WorkingDays, [Note] = @Note
	WHERE  [Id] = @Id
go
alter table Manufacture
alter column Note nvarchar(max) null;
go
--drop procedure UpdatePaymentData
CREATE PROC UpdatePaymentData
    @Id uniqueidentifier,
    @Contract nvarchar(100) = NULL,
    @SpecificationSum nvarchar(20) = NULL,
    @Project nvarchar(100) = NULL,
    @PriceWithoutVAT nvarchar(100) = NULL,
    @Note nvarchar(max) = NULL,
    @IsFirstPayment bit,
    @FirstPaymentSum nvarchar(20) = NULL,
    @FirstPaymentDate datetime2(7) = NULL,
    @IsSecondPayment bit,
    @SecondPaymentSum nvarchar(20) = NULL,
    @SecondPaymentDate datetime2(7) = NULL,
    @IsFullPayment bit,
    @FullPaymentSum nvarchar(20) = NULL,
    @FullPaymentDate datetime2(7) = NULL
AS 
	SET NOCOUNT ON 

	UPDATE [dbo].[Payment]
	SET    [Contract] = @Contract, [SpecificationSum] = @SpecificationSum, [Project] = @Project, [PriceWithoutVAT] = @PriceWithoutVAT, [Note] = @Note, [IsFirstPayment] = @IsFirstPayment, [FirstPaymentSum] = @FirstPaymentSum, [FirstPaymentDate] = @FirstPaymentDate, [IsSecondPayment] = @IsSecondPayment, [SecondPaymentSum] = @SecondPaymentSum, [SecondPaymentDate] = @SecondPaymentDate, [IsFullPayment] = @IsFullPayment, [FullPaymentSum] = @FullPaymentSum, [FullPaymentDate] = @FullPaymentDate
	WHERE  [Id] = @Id
go
alter table [Payment]
alter column Note nvarchar(max) null;
go
alter table TimedGiving
alter column Note nvarchar(max) null;
go
create procedure UpdateTimedGiving @id uniqueidentifier, @isTimedGiving bit, @isSKBCheck bit, @isOECStorage bit,
	@skbNumber nvarchar(50), @fio nvarchar(150), @givingDate date, @returnDate date, @note nvarchar(max)
as
begin
set nocount on;
update TimedGiving
set IsTimedGiving = @isTimedGiving,
IsSKBCheck = @isSKBCheck,
IsOECStorage = @isOECStorage,
SKBNumber = @skbNumber,
FIO = @fio,
GivingDate = @givingDate,
ReturnDate = @returnDate,
Note = @note
where Id = @id
end;
--drop procedure UpdateTimedGiving