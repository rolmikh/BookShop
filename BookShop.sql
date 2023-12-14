set ansi_nulls on
go
set ansi_padding on
go
set quoted_identifier on
go

create database [Book_Shop]
go
use [Book_Shop]
go

create table [dbo].[Status_Order]
(
	[ID_Status_Order] [int] not null identity (1,1),
	[Name_Status_Order] [varchar] (20) not null,
	[IsDeleted] [int] null
	constraint [PK_Status_Order] primary key clustered
	([ID_Status_Order] ASC) on [PRIMARY],
	constraint [UQ_Name_Status_Order] unique ([Name_Status_Order]),
	constraint [CH_Deleted_Status] check ([IsDeleted] = 0 or [IsDeleted] = 1)
)
go


insert into [dbo].[Status_Order] ([Name_Status_Order])
values ('Оплачен'),('Собирается'),('Готов к выдаче')
go

create table [dbo].[Order]
(
	[ID_Order] [int] not null identity (1,1),
	[Status_Order_ID] [int] not null,
	[Number_Order] [varchar] (6) not null,
	[Date_Order] [date] not null,
	[Price_Order] [decimal] (38,2) null,
	[isDeleted] [int] not null
	constraint [PK_Order] primary key clustered
	([ID_Order] ASC) on [PRIMARY],
	constraint [UQ_Number_Order] unique ([Number_Order]),
	constraint [CH_Number_Order] check ([Number_Order] like '[0-9][0-9][0-9][0-9][0-9][0-9]'),
	constraint [FK_Status_Order] foreign key ([Status_Order_ID])
	references [dbo].[Status_Order] ([ID_Status_Order]),
	constraint [CH_Deleted_Order] check ([isDeleted] = 0 or [isDeleted] = 1),
	constraint [CH_Price_Order] check ([Price_Order] >= 0)

)
go



select * from [dbo].[Order]
create table [dbo].[Role]
(
	[ID_Role] [int] not null identity (1,1),
	[Name_Role] [varchar] (20) not null,
	[IsDeletedRole] [int] null
	constraint [PK_Role] primary key clustered
	([ID_Role] ASC) on [PRIMARY],
	constraint [UQ_Name_Role] unique ([Name_Role]),
	constraint [CH_Deleted_Role] check ([IsDeletedRole] = 0 or [IsDeletedRole] = 1)
)
go

insert into [dbo].[Role] ([Name_Role])
values ('Администратор'),('Пользователь'),('Сотрудник склада')
go
select * from [dbo].[Role]
create table [dbo].[User]
(
	[ID_User] [int] not null identity (1,1),
	[Surname_User] [varchar] (50) not null,
	[Name_User] [varchar] (50) not null,
	[Middle_Name_User] [varchar] (50) null,
	[Email_User] [varchar] (50) not null,
	[Password_User] [varchar] (150) not null,
	[Date_Birth_User] [date] not null,
	[Salt_User] [varchar] (20) null,
	[Role_ID] [int] not null,
	[isDeleted] [int] not null
	constraint [PK_User] primary key clustered
	([ID_User] ASC) on [PRIMARY],
	constraint [UQ_Email_User] unique ([Email_User]),
	constraint [FK_Role] foreign key ([Role_ID])
	references [dbo].[Role] ([ID_Role]),
	constraint [CH_Deleted_User] check ([isDeleted] = 0 or [isDeleted] = 1)

)
go

--alter table [dbo].[User] add constraint 

insert into [dbo].[User]([Surname_User],[Name_User],[Middle_Name_User],[Email_User],[Password_User],[Date_Birth_User],[Salt_User],[Role_ID],[isDeleted])
values('Рожковская','Людмила','Михайловна','ludaarozhkovskaya@gmail.com','yfgarf4*','29.09.2004','45ywt7ge',1,0)
--('Рожковская','Людмила','Михайловна','ludaarozhkovskaya@gmail.com','yfgarf4*','29.09.2004','45ywt7geao',2),
--('Рожковская','Людмила','Михайловна','ludaarozhkovskaya@gmail.com','yfgarf4*','29.09.2004','45ywt7geao',3)

create table [dbo].[Customer_Card]
(
	[ID_Customer_Card] [int] not null identity (1,1),
	[Number_Card] [varchar] (16) not null,
	[User_ID] [int] not null,
	[Validity_Period] [varchar] (5) not null,
	[CVV_Code] [varchar] (3) not null,
	[Salt_Card] [varchar] (8) null
	constraint [PK_Card] primary key clustered
	([ID_Customer_Card] ASC) on [PRIMARY],
	constraint [UQ_Number_Card] unique ([Number_Card]),
	constraint [UQ_CVV] unique ([CVV_Code]),
	constraint [FK_User_ID] foreign key ([User_ID])
	references [dbo].[User] ([ID_User])
)
go

create table [dbo].[Category]
(
	[ID_Category] [int] not null identity (1,1),
	[Name_Category] [varchar] (30) not null,
	[IsDeletedCategory] [int] null
	constraint [PK_Category] primary key clustered
	([ID_Category] ASC) on [PRIMARY],
	constraint [UQ_Name_Category] unique ([Name_Category]),
	constraint [CH_Deleted_Category] check ([IsDeletedCategory] = 0 or [IsDeletedCategory] = 1)
)
go



insert into [dbo].[Category]([Name_Category])
values('Комиксы'),('Художественная литература'),('Образование'),('Манга'),('Книги на английском языке')
go
insert into [dbo].[Category]([Name_Category])
values('Фэнтези')
go
select * from [dbo].[Category]
create table [dbo].[Product]
(
	[ID_Product] [int] not null identity (1,1),
	[Name_Book] [varchar] (30) not null,
	[Author] [varchar] (50) not null,
	[Count] [int] not null,
	[Article_Product] [varchar] (10) not null,
	[Category_ID] [int] not null,
	[Series] [varchar] (50) not null,
	[Cover_Type] [varchar] (30) not null,
	[Year_Of_Publication] [varchar] (4) not null,
	[Publishing_House] [varchar] (20) not null,
	[Age_Restriction] [varchar] (3) not null,
	[Photo_Book] [varchar](max) not null,
	[Number_Of_Pages] [int] not null,
	[Price_Book] [decimal] (38,2) not null,
	[Annotation] [varchar](max) not null,
	[isDeleted] [int] not null
	constraint [PK_Product] primary key clustered
	([ID_Product] ASC) on [PRIMARY],
	constraint [CH_Count] check ([Count] >= 0),
	constraint [CH_Article] check ([Article_Product] like '[0-9][0-9][0-9][0-9][0-9][0-9]'),
	constraint [CH_Year] check ([Year_Of_Publication] like '[0-9][0-9][0-9][0-9]'),
	constraint [FK_Category] foreign key ([Category_ID])
	references [dbo].[Category] ([ID_Category]),
	constraint [CH_Price_Book] check ([Price_Book] > 0),
	constraint [CH_Deleted_Product] check ([isDeleted] = 0 or [isDeleted] = 1)

)
go
insert into [dbo].[Product] ([Name_Book],[Author],[Count],[Article_Product],[Category_ID],[Series],[Cover_Type],[Year_Of_Publication],[Publishing_House],[Age_Restriction],[Photo_Book],[Number_Of_Pages],[Price_Book],[Annotation],[isDeleted])
values('Макабр','Нокс Мила',500,'123456',6,'Весь цикл в одном томе','Твердый переплет','2022','РОСМЭН','12+','..\source\makabr.jpg',1024,999,'Множество приключений и самые удивительные открытия - все три тома серии "Макабр" от Милы Нокс под одной обложкой',0)
go

select * from [dbo].[Product]

create table [dbo].[Basket]
(
	[ID_Basket] [int] not null identity (1,1),
	[User_ID] [int] not null,
	[Product_ID] [int] not null,
	[IsDeleted_Basket] [int] null
	constraint [PK_Basket] primary key clustered
	([ID_Basket] ASC) on [PRIMARY],
	constraint [FK_User_Basket] foreign key ([User_ID])
	references [dbo].[User] ([ID_User]),
	constraint [FK_Product_Basket] foreign key ([Product_ID])
	references [dbo].[Product] ([ID_Product]),
	constraint [CH_Deleted_Basket] check ([IsDeleted_Basket] = 0 or [IsDeleted_Basket] = 1)
)
go

insert into [dbo].[Basket] ([User_ID],[Product_ID],[IsDeleted_Basket])
values(13,7,0)

select * from [dbo].[Basket]


--alter table [dbo].[Basket] alter column [Price] [decimal] (38,2) not null

create table [dbo].[Order_Composition]
(
	[ID_Order_Composition] [int] not null identity (1,1),
	[Order_ID] [int] not null,
	[Basket_ID] [int] not null,
	[User_ID] [int] not null,
	[isDeleted] [int] not null
	constraint [PK_Order_Composition] primary key clustered
	([ID_Order_Composition] ASC) on [PRIMARY],
	constraint [FK_OrderComp] foreign key ([Order_ID])
	references [dbo].[Order] ([ID_Order]),
	constraint [FK_Basket] foreign key ([Basket_ID])
	references [dbo].[Basket] ([ID_Basket]),
	constraint [FK_User] foreign key ([User_ID])
	references [dbo].[User] ([ID_User]),
	constraint [CH_Deleted_Order_Composition] check ([isDeleted] = 0 or [isDeleted] = 1)
)
go



create table [dbo].[Warehouse]
(
	[ID_Warehouse] [int] not null identity (1,1),
	[Number_Warehouse] [varchar] (6) not null,
	[Address] [varchar] (50) not null,
	[IsDeletedWarehouse] [int] null
	constraint [PK_Warehouse] primary key clustered
	([ID_Warehouse] ASC) on [PRIMARY],
	constraint [UQ_Number_Warehouse] unique ([Number_Warehouse]),
	constraint [CH_Number_Warehouse] check ([Number_Warehouse] like '[0-9][0-9][0-9][0-9][0-9][0-9]'),
	constraint [CH_Deleted_Warehouse] check ([IsDeletedWarehouse] = 0 or [IsDeletedWarehouse] = 1)
)
go



insert into [dbo].[Warehouse]([Number_Warehouse],[Address])
values('555555','Пречистенский пер., 66, Москва')
go

create table [dbo].[Contract]
(
	[ID_Contract] [int] not null identity (1,1),
	[Number_Contract] [varchar] (6) not null,
	[Date_Contract] [date] null default getdate(),
	[IsDeletedContract] [int] null
	constraint [PK_Contract] primary key clustered
	([ID_Contract] ASC) on [PRIMARY],
	constraint [UQ_Number_Contract] unique ([Number_Contract]),
	constraint [CH_Number_Contract] check ([Number_Contract] like '[0-9][0-9][0-9][0-9][0-9][0-9]'),
	constraint [CH_Deleted_Contract] check ([IsDeletedContract] = 0 or [IsDeletedContract] = 1)
)
go



insert into [dbo].[Contract]([Number_Contract])
values('111111')
go

create table [dbo].[Delivery_Note]
(
	[ID_Delivery_Note] [int] not null identity (1,1),
	[Number_Delivery_Note] [varchar] (6) not null,
	[Date_Delivery_Note] [date] null default getdate(),
	[Contract_ID] [int] not null,
	[IsDeletedNote] [int] null
	constraint [PK_Delivery_Note] primary key clustered
	([ID_Delivery_Note] ASC) on [PRIMARY],
	constraint [UQ_Number_Delivery_Note] unique ([Number_Delivery_Note]),
	constraint [CH_Number_Delivery_Note] check ([Number_Delivery_Note] like '[0-9][0-9][0-9][0-9][0-9][0-9]'),
	constraint [FK_Contract] foreign key ([Contract_ID])
	references [dbo].[Contract] ([ID_Contract]),
	constraint [CH_Deleted_Delivery_Note] check ([IsDeletedNote] = 0 or [IsDeletedNote] = 1)
)
go

insert into [dbo].[Delivery_Note]([Number_Delivery_Note],[Contract_ID])
values('666777',1)

create table [dbo].[Supply]
(
	[ID_Supply] [int] not null identity (1,1),
	[Delivery_Note_ID] [int] not null,
	[Warehouse_ID] [int] not null,
	[Number_Supply] [varchar] (6) not null,
	[Date_Supply] [date] null default getdate(),
	[isDeleted] [int] not null
	constraint [PK_Supply] primary key clustered
	([ID_Supply] ASC) on [PRIMARY],
	constraint [UQ_NUmber_Supply] unique ([Number_Supply]),
	constraint [FK_Delivery] foreign key ([Delivery_Note_ID])
	references [dbo].[Delivery_Note] ([ID_Delivery_Note]),
	constraint [FK_Warehouse] foreign key ([Warehouse_ID])
	references [dbo].[Warehouse] ([ID_Warehouse]),
	constraint [CH_Deleted_Supply] check ([isDeleted] = 0 or [isDeleted] = 1)
)
go

insert into [dbo].[Supply]([Delivery_Note_ID],[Warehouse_ID],[Number_Supply],[isDeleted])
values (1,1,'888888',0)

select * from [dbo].[Supply]

create table [dbo].[Supply_Composition]
(
	[ID_Supply_Composition] [int] not null identity (1,1),
	[Product_ID] [int] not null,
	[Supply_ID] [int] not null,
	[Count_Supply] [int] not null,
	[Price_Supply] [decimal] (38,2) not null,
	[isDeleted] [int] not null
	constraint [PK_Supply_Composition] primary key clustered
	([ID_Supply_Composition] ASC) on [PRIMARY],
	constraint [FK_Product_Supply] foreign key ([Product_ID])
	references [dbo].[Product] ([ID_Product]),
	constraint [FK_Supply] foreign key ([Supply_ID])
	references [dbo].[Supply] ([ID_Supply]),
	constraint [CH_Count_Supply] check ([Count_Supply] > 0),
	constraint [CH_Deleted_Suply_Composition] check ([isDeleted] = 0 or [isDeleted] = 1)
)
go

insert into [dbo].[Supply_Composition] ([Product_ID],[Supply_ID],[Count_Supply],[Price_Supply],[isDeleted])
values (1,1,20,700,0)
go

create trigger [dbo].[Trigger_Delete_Basket]
on [dbo].[Basket]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Basket] set IsDeleted_Basket = 1 where ID_Basket = (select ID_Basket from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Order_Composition]
on [dbo].[Order_Composition]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Order_Composition] set IsDeleted = 1 where ID_Order_Composition = (select ID_Order_Composition from deleted)
end
go


create trigger [dbo].[Trigger_Delete_Order]
on [dbo].[Order]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Order] set IsDeleted = 1 where ID_Order = (select ID_Order from deleted)
end
go


create trigger [dbo].[Trigger_Delete_User]
on [dbo].[User]
instead of delete
as
begin
    set nocount on;
   update [dbo].[User] set IsDeleted = 1 where ID_User = (select ID_User from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Product]
on [dbo].[Product]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Product] set IsDeleted = 1 where ID_Product = (select ID_Product from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Supply_Composition]
on [dbo].[Supply_Composition]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Supply_Composition] set IsDeleted = 1 where ID_Supply_Composition = (select ID_Supply_Composition from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Supply]
on [dbo].[Supply]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Supply] set IsDeleted = 1 where ID_Supply = (select ID_Supply from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Category]
on [dbo].[Category]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Category] set  IsDeletedCategory = 1 where ID_Category = (select ID_Category from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Contract]
on [dbo].[Contract]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Contract] set  IsDeletedContract = 1 where ID_Contract = (select ID_Contract from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Note]
on [dbo].[Delivery_Note]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Delivery_Note] set  IsDeletedNote = 1 where ID_Delivery_Note = (select ID_Delivery_Note from deleted)
end
go


create trigger [dbo].[Trigger_Delete_Role]
on [dbo].[Role]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Role] set  IsDeletedRole = 1 where ID_Role = (select ID_Role from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Status]
on [dbo].[Status_Order]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Status_Order] set  IsDeleted = 1 where ID_Status_Order = (select ID_Status_Order from deleted)
end
go

create trigger [dbo].[Trigger_Delete_Warehouse]
on [dbo].[Warehouse]
instead of delete
as
begin
    set nocount on;
   update [dbo].[Warehouse] set  IsDeletedWarehouse = 1 where ID_Warehouse = (select ID_Warehouse from deleted)
end
go

create or alter view [Supply_View]
("Номер поставки","Дата поставки","Номер договора","Дата подписания","Номер накладной","Дата накладной","Номер склада","Адрес склада","Название книги","Количество поставки","Закупочная цена","Отпускная цена","Артикул товара")
as
	select [Number_Supply],[Date_Supply],[Number_Contract],[Date_Contract],[Number_Delivery_Note],[Date_Delivery_Note],[Number_Warehouse],[Address],[Name_Book],[Count_Supply],[Price_Supply],[Price_Book],[Article_Product] from [dbo].[Supply_Composition]
	inner join [dbo].[Supply] on [Supply_ID] = [dbo].[Supply].[ID_Supply]
	inner join [dbo].[Delivery_Note] on [Delivery_Note_ID] = [dbo].[Delivery_Note].[ID_Delivery_Note]
	inner join [dbo].[Contract] on [Contract_ID] = [dbo].[Contract].[ID_Contract]
	inner join [dbo].[Warehouse] on [Warehouse_ID] = [dbo].[Warehouse].[ID_Warehouse]
	inner join [dbo].[Product] on [Product_ID] = [dbo].[Product].[ID_Product]
go

select * from [Supply_View]
go

create or alter view [Supply_Composition_View]
("Название товара","Номер поставки","Дата поставки","Количество поставки","Стоимость товара")
as
	select [Name_Book],[Number_Supply],[Date_Supply],[Count_Supply],[Price_Supply] from [dbo].[Supply_Composition]
	inner join [dbo].[Supply] on [Supply_ID] = [dbo].[Supply].[ID_Supply]
	inner join [dbo].[Product] on [Product_ID] = [dbo].[Product].[ID_Product]

go

select * from [Supply_Composition_View]
go

create or alter view [Order_View]
("Номер заказа","Статус заказа","Дата оформления","Стоимость заказа","Состав заказа","Фамилия","Имя","Отчество","Электронная почта","Дата рождения")
as
	select [Number_Order],[Name_Status_Order],[Date_Order],[Price],[Name_Book],[Surname_User],[Name_User],[Middle_Name_User],[Email_User],[Date_Birth_User] from [dbo].[Order_Composition]
	inner join [dbo].[Order] on [Order_ID] = [dbo].[Order].[ID_Order]
	inner join [dbo].[Status_Order] on [ID_Status_Order] = [dbo].[Status_Order].[ID_Status_Order]
	inner join [dbo].[Basket] on [ID_Basket] = [dbo].[Basket].[ID_Basket]
	inner join [dbo].[User] on [ID_User] = [dbo].[User].[ID_User]
	inner join [dbo].[Product] on [ID_Product] = [dbo].[Product].[ID_Product]
go

select * from [Order_View]
go

use [Book_Shop]
go

create or alter function [dbo].[GetTotalPriceBasket](@User_ID [int])
returns [decimal](38,2)
with execute as caller
as
	begin
		declare @TotalPrice decimal (38,2)  
		declare @Price decimal (38,2)
		select @Price = sum([Price_Book]) from [dbo].[Product]
		inner join [dbo].[Basket] on [ID_Product] = [dbo].[Basket].[Product_ID]	
		where [User_ID] = @User_ID and [IsDeleted_Basket] = 0
		set @TotalPrice = @Price
		return @TotalPrice
	end
go
drop function [dbo].[GetTotalPriceBasket]
--drop trigger [dbo].[Trigger_Insert_Basket]
--drop procedure[dbo].[CalculateBasket]
--select * from [dbo].[Basket]
--select [dbo].[GetTotalPriceBasket](14)
----drop trigger [dbo].[Trigger_Insert_Basket]

--create or alter trigger [dbo].[Trigger_Insert_Basket]
--on [dbo].[Basket]
--instead of insert
--as
--declare @ID_Basket [int], @Price [decimal] (38,2), @User_ID [int], @Product_ID [int], @IsDeleted [int]
--begin
--   set nocount on;
--   declare @ProductPrice [decimal] (38,2);
--   select @ProductPrice = [Price_Book] from [dbo].[Product]
--   where [ID_Product] = (select [Product_ID] from [dbo].[Basket] where [ID_Basket] = @ID_Basket and [User_ID] = @User_ID);
--   select @ID_Basket = [ID_Basket], @Price = @ProductPrice, @User_ID = [User_ID], @Product_ID = [Product_ID], @IsDeleted = [IsDeleted_Basket] from inserted
--   --execute [dbo].[CalculateBasket] @ID_Basket, @Price, @User_ID, @Product_ID, @IsDeleted
--   --update [dbo].[Basket] set [Price] = @Price where [ID_Basket] = @ID_Basket
--end
--go

--insert into [dbo].[Basket] ([Price],[User_ID],[Product_ID],[IsDeleted])
--values(0.0,13,7,0)
--go

--select * from [dbo].[Product]
--drop procedure[dbo].[CalculateBasket]
--create or alter procedure [dbo].[CalculateBasket]
--@ID_Basket [int], @Price [decimal] (38,2), @User_ID [int], @Product_ID [int], @IsDeleted [int]
--as
--	begin try
--		set nocount on;
--		declare @ProductPrice [decimal] (38,2);
--		select @ProductPrice = [Price_Book] from [dbo].[Product]
--		where [ID_Product] = (select [Product_ID] from [dbo].[Basket] where [ID_Basket] = @ID_Basket);
--		set @Price = @ProductPrice 
--	end try
--	begin catch
--		print('Ошибка!')
--	end catch
--go


--execute [dbo].[CalculateBasket] @ID_Basket = 10, @Price = null, @User_ID = 13, @Product_ID = 6, @IsDeleted = 0
--select * from [dbo].[Product]
--select * from [dbo].[Basket]
