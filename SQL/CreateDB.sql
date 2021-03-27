use master
create database CompPlannerDB
go
use CompPlannerDB

create table Manufacture(
ID int primary key identity(1,1),
M_Name nvarchar(80),
Letter_Num nvarchar(50),
Specification_Num nvarchar(50),
Specification_Cost decimal(18, 3)
);
create table In_Production(
ID int primary key identity(1,1),
Number nvarchar(30),
Giving_Date date,
Executor_Name nvarchar(80),
Completion_Date date
);
create table Complectation(
ID int primary key identity(1,1),
Complectation nvarchar(100),
C_Date date,
Comp_Percentage float
);
create table Giving(
ID int primary key identity(1,1),
G_State bit,
Bill nvarchar(50),
Report nvarchar(50),
Return_Report nvarchar(50),
Receiving_Date date
);
create table Production_Plan(
ID int primary key identity(1,1),
Inc_Doc nvarchar(100),
Manag_Doc nvarchar(100),
Task_Name nvarchar(300) not null,
P_Count int,
P_Date date,
Complectation_ID int foreign key references Complectation(ID),
Manufacture_ID int foreign key references Manufacture(ID),
Expend_Num nvarchar(30),
Expend_Date date,
Giving_ID int foreign key references Giving(ID),
In_Production_ID int foreign key references In_Production(ID),
Note nvarchar(MAX),
Parent_ID int foreign key references Production_Plan(ID),
Completion bit not null default(0)
);


create table Users(
ID int primary key identity(1,1),
U_Login nvarchar(15) not null,
U_Password nvarchar(15) not null,
U_Name nvarchar(50),
Surname nvarchar(50),
Birthday date,
Position nvarchar(50)
);
create table Reports(
ID int primary key identity(1,1),
R_Description nvarchar(MAX),
R_State bit not null default(0)
);
create table Tasks(
ID int primary key identity(1,1),
From_ID int not null foreign key references Users(ID),
To_ID int not null foreign key references Users(ID),
Header nvarchar(30) not null,
Complete_Date date,
T_Description nvarchar(MAX),
Report_ID int foreign key references Reports(ID)
);
