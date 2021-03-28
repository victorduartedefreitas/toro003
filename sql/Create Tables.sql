use TORO
go

create table [Account](
	[AccountId] uniqueidentifier not null default(newid()),
	[Cpf] varchar(11) not null,
	[Name] varchar(100) not null,
	[Bank] varchar(3) not null,
	[Branch] varchar(6) not null,
	[AccountNumber] varchar(6) not null,
	[Amount] decimal(10,3) not null,
	constraint PK_ACCOUNT primary key ([AccountId]),
	constraint UN_ACCOUNT_CPF unique ([Cpf]),
	constraint UN_ACCOUNT_NUMBER unique ([AccountNumber]),
	constraint CHK_ACCOUNT_AMOUNT check ([Amount] >= 0))

create table [AccountHistory](
	[HistoryId] uniqueidentifier not null default(newid()),
	[AccountId] uniqueidentifier not null,
	[Date] datetimeoffset not null default(getdate()),
	[TransactionType] int not null,
	[Description] varchar(200) not null,
	[Amount] decimal(10,3) not null,
	constraint PK_ACCOUNTHISTORY primary key ([HistoryId]),
	constraint FK_ACCOUNTHISTORY_ACCOUNT foreign key ([AccountId]) references [Account]([AccountId]),
	constraint CHK_ACCOUNTHISTORY_AMOUNT check ([Amount] >= 0))