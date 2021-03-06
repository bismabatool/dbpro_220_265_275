USE [master]
GO
/****** Object:  Database [DB58]    Script Date: 4/28/2019 1:27:09 PM ******/
CREATE DATABASE [DB58]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB58', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DB58.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DB58_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\DB58_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DB58] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB58].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB58] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB58] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB58] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB58] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB58] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB58] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB58] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB58] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB58] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB58] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB58] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB58] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB58] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB58] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB58] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DB58] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB58] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB58] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB58] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB58] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB58] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB58] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB58] SET RECOVERY FULL 
GO
ALTER DATABASE [DB58] SET  MULTI_USER 
GO
ALTER DATABASE [DB58] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB58] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB58] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB58] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB58] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DB58', N'ON'
GO
ALTER DATABASE [DB58] SET QUERY_STORE = OFF
GO
USE [DB58]
GO
/****** Object:  Table [dbo].[tbl_Login]    Script Date: 4/28/2019 1:27:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Login](
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[UserID] [varchar](13) NULL,
	[isApproved] [bit] NOT NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Login] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[AGE] [int] NOT NULL,
	[Gender] [varchar](1) NOT NULL,
	[CNIC] [varchar](13) NOT NULL,
	[DOB] [date] NOT NULL,
	[Contact] [varchar](15) NOT NULL,
	[Postal_Address] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CNIC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[employeeApproveView]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create vIEW [dbo].[employeeApproveView] as 
Select CNIC, CONCAT(FirstName,' ',LastName) as Name, Email, Contact, Postal_Address, isApproved, isActive 
FROM tbl_Login INNER JOIN Employee ON tbl_Login.UserID=Employee.CNIC
GO
/****** Object:  Table [dbo].[Designation]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designation](
	[DesignationID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeID] [varchar](13) NOT NULL,
	[PostTitle] [varchar](30) NOT NULL,
	[Salary] [int] NOT NULL,
	[JoiningDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DesignationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[currentEmployeeView]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create vIEW [dbo].[currentEmployeeView] as 
Select CNIC, CONCAT(FirstName,' ',LastName) as Name, Email, Contact, Postal_Address,  
PostTitle, Salary, isApproved, isActive
FROM tbl_Login INNER JOIN Employee ON tbl_Login.UserID=Employee.CNIC 
INNER JOIN Designation ON Designation.EmployeeID=Employee.CNIC WHere isApproved='1' AND isActive='1'
GO
/****** Object:  View [dbo].[empRegisterView]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create view [dbo].[empRegisterView] as
Select * from Employee INNER JOIN tbl_Login ON CNIC=UserID
GO
/****** Object:  Table [dbo].[Loan]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Loan](
	[LoanID] [int] IDENTITY(1,1) NOT NULL,
	[LoanType] [varchar](30) NOT NULL,
	[InterestRate] [float] NOT NULL,
	[MaxAmount] [float] NOT NULL,
 CONSTRAINT [PK__Loan__4F5AD437E26414FC] PRIMARY KEY CLUSTERED 
(
	[LoanID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanApplication]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanApplication](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[LoanID] [int] NOT NULL,
	[EmployeeID] [varchar](13) NOT NULL,
	[PrincipalAmount] [int] NOT NULL,
	[Verified] [bit] NOT NULL,
	[Reason] [varchar](600) NOT NULL,
	[AppliedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanDetails]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanDetails](
	[LoanDetailID] [int] IDENTITY(1,1) NOT NULL,
	[LoanID] [int] NOT NULL,
	[Detail] [varchar](300) NOT NULL,
	[Category] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LoanDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoanStatus]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoanStatus](
	[LoanStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[PrincipleAmount] [float] NOT NULL,
	[RemainingAmount] [float] NOT NULL,
	[Tenure] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LoanStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lookup]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lookup](
	[LookupID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](25) NOT NULL,
	[Category] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LookupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Designation] ON 

INSERT [dbo].[Designation] ([DesignationID], [EmployeeID], [PostTitle], [Salary], [JoiningDate], [ToDate]) VALUES (1, N'1234567890987', N'Manager', 70000, CAST(N'2019-04-27T18:29:08.530' AS DateTime), CAST(N'2019-04-27T18:29:08.530' AS DateTime))
SET IDENTITY_INSERT [dbo].[Designation] OFF
INSERT [dbo].[Employee] ([FirstName], [LastName], [AGE], [Gender], [CNIC], [DOB], [Contact], [Postal_Address]) VALUES (N'Ali', N'Zaman', 32, N'M', N'0987654321567', CAST(N'2016-10-11' AS Date), N'3299922', N'hbashbj')
INSERT [dbo].[Employee] ([FirstName], [LastName], [AGE], [Gender], [CNIC], [DOB], [Contact], [Postal_Address]) VALUES (N'Aplha', N'Beta', 33, N'M', N'1234567890987', CAST(N'1998-09-09' AS Date), N'0900087666', N'hscvhjvhjdhsj')
SET IDENTITY_INSERT [dbo].[Loan] ON 

INSERT [dbo].[Loan] ([LoanID], [LoanType], [InterestRate], [MaxAmount]) VALUES (1, N'Inserest Based', 2.52, 100000)
SET IDENTITY_INSERT [dbo].[Loan] OFF
SET IDENTITY_INSERT [dbo].[Lookup] ON 

INSERT [dbo].[Lookup] ([LookupID], [Title], [Category]) VALUES (1, N'Policy', N'Loan_Category')
INSERT [dbo].[Lookup] ([LookupID], [Title], [Category]) VALUES (2, N'Condition', N'Loan_Category')
INSERT [dbo].[Lookup] ([LookupID], [Title], [Category]) VALUES (3, N'Eligibility', N'Loan_Category')
INSERT [dbo].[Lookup] ([LookupID], [Title], [Category]) VALUES (4, N'Employee', N'Login_Type')
INSERT [dbo].[Lookup] ([LookupID], [Title], [Category]) VALUES (5, N'Admin', N'Login_Type')
SET IDENTITY_INSERT [dbo].[Lookup] OFF
INSERT [dbo].[tbl_Login] ([Email], [Password], [UserID], [isApproved], [isActive]) VALUES (N'admin@gmail.com', N'abc', NULL, 1, 1)
INSERT [dbo].[tbl_Login] ([Email], [Password], [UserID], [isApproved], [isActive]) VALUES (N'hello@gmail.com', N'abc', N'1234567890987', 1, 1)
ALTER TABLE [dbo].[Designation] ADD  DEFAULT (getdate()) FOR [JoiningDate]
GO
ALTER TABLE [dbo].[Designation] ADD  DEFAULT (getdate()) FOR [ToDate]
GO
ALTER TABLE [dbo].[LoanApplication] ADD  DEFAULT (getdate()) FOR [AppliedDate]
GO
ALTER TABLE [dbo].[LoanStatus] ADD  DEFAULT (getdate()) FOR [Tenure]
GO
ALTER TABLE [dbo].[tbl_Login] ADD  CONSTRAINT [DF_tbl_Login_isApproved]  DEFAULT ((0)) FOR [isApproved]
GO
ALTER TABLE [dbo].[tbl_Login] ADD  CONSTRAINT [DF_tbl_Login_isActive]  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Designation]  WITH CHECK ADD  CONSTRAINT [FK__Designati__Emplo__656C112C] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([CNIC])
GO
ALTER TABLE [dbo].[Designation] CHECK CONSTRAINT [FK__Designati__Emplo__656C112C]
GO
ALTER TABLE [dbo].[LoanApplication]  WITH CHECK ADD FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([CNIC])
GO
ALTER TABLE [dbo].[LoanApplication]  WITH CHECK ADD  CONSTRAINT [FK__LoanAppli__LoanI__75A278F5] FOREIGN KEY([LoanID])
REFERENCES [dbo].[Loan] ([LoanID])
GO
ALTER TABLE [dbo].[LoanApplication] CHECK CONSTRAINT [FK__LoanAppli__LoanI__75A278F5]
GO
ALTER TABLE [dbo].[LoanDetails]  WITH CHECK ADD FOREIGN KEY([Category])
REFERENCES [dbo].[Lookup] ([LookupID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoanDetails]  WITH CHECK ADD  CONSTRAINT [FK__LoanDetai__LoanI__7C4F7684] FOREIGN KEY([LoanID])
REFERENCES [dbo].[Loan] ([LoanID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LoanDetails] CHECK CONSTRAINT [FK__LoanDetai__LoanI__7C4F7684]
GO
ALTER TABLE [dbo].[LoanStatus]  WITH CHECK ADD FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[LoanApplication] ([ApplicationID])
GO
ALTER TABLE [dbo].[tbl_Login]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Login_Employee] FOREIGN KEY([UserID])
REFERENCES [dbo].[Employee] ([CNIC])
GO
ALTER TABLE [dbo].[tbl_Login] CHECK CONSTRAINT [FK_tbl_Login_Employee]
GO
/****** Object:  StoredProcedure [dbo].[employeeAccountApproveView]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[employeeAccountApproveView] as 
BEGIN
Select CNIC, CONCAT(FirstName,' ',LastName) as Name, Email, Contact, Postal_Address, isApproved 
FROM tbl_Login INNER JOIN Employee ON tbl_Login.UserID=Employee.CNIC
END
EXEC employeeAccountApproveView
GO
/****** Object:  StoredProcedure [dbo].[vl]    Script Date: 4/28/2019 1:27:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[vl] as
Begin
Select LoanID,LoanType from Loan
Select LookupID,Title from Lookup WHERE LookupID<4
end
GO
USE [master]
GO
ALTER DATABASE [DB58] SET  READ_WRITE 
GO
