/******		Create and Hydrate the Database		******/
/******		Create Database						******/
USE [master]
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'Exercise') 
	BEGIN
		PRINT 'Database "Exercise" Does Not EXIST... Creating "Exercise" database now.'
		/****** Object:  Database [Exercise]    Script Date: 12/7/2023 2:07:19 PM ******/
		CREATE DATABASE [Exercise]
		 CONTAINMENT = NONE
		 ON  PRIMARY 
		( NAME = N'Exercise', FILENAME = N'/var/opt/mssql/data/Exercise.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
		 LOG ON 
		( NAME = N'Exercise_log', FILENAME = N'/var/opt/mssql/data/Exercise_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
		 WITH CATALOG_COLLATION = DATABASE_DEFAULT
	END
ELSE
	BEGIN
		PRINT 'Database "Exercise" EXISTS...'
	END

USE [master]
GO

/******		Create "svc_patients_api" Login			******/
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE [name] = 'svc_patients_api')
	BEGIN
		PRINT 'LOGIN "svc_patients_api" does NOT Exist...'
		CREATE LOGIN [svc_patients_api] WITH 
			PASSWORD=N'(Api-2023.SomeWord.Thing@)', 
			DEFAULT_DATABASE=[Exercise], 
			DEFAULT_LANGUAGE=[us_english], 
			CHECK_EXPIRATION=OFF, 
			CHECK_POLICY=ON
	END;
ELSE
		PRINT 'LOGIN "svc_patients_api" DOES Exist...'
GO

IF  EXISTS(SELECT * FROM sys.databases WHERE name = 'Exercise')
	BEGIN
		USE [Exercise]
	
			IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
			begin
				EXEC [Exercise].[dbo].[sp_fulltext_database] @action = 'enable'
			end

			ALTER DATABASE [Exercise] SET ANSI_NULL_DEFAULT OFF
			ALTER DATABASE [Exercise] SET ANSI_NULLS OFF 
			ALTER DATABASE [Exercise] SET ANSI_PADDING OFF 
			ALTER DATABASE [Exercise] SET ANSI_WARNINGS OFF 
			ALTER DATABASE [Exercise] SET ARITHABORT OFF 
			ALTER DATABASE [Exercise] SET AUTO_CLOSE OFF 
			ALTER DATABASE [Exercise] SET AUTO_SHRINK OFF 
			ALTER DATABASE [Exercise] SET AUTO_UPDATE_STATISTICS ON
			ALTER DATABASE [Exercise] SET CURSOR_CLOSE_ON_COMMIT OFF 
			ALTER DATABASE [Exercise] SET CURSOR_DEFAULT  GLOBAL 
			ALTER DATABASE [Exercise] SET CONCAT_NULL_YIELDS_NULL OFF 
			ALTER DATABASE [Exercise] SET NUMERIC_ROUNDABORT OFF 
			ALTER DATABASE [Exercise] SET QUOTED_IDENTIFIER OFF 
			ALTER DATABASE [Exercise] SET RECURSIVE_TRIGGERS OFF
			ALTER DATABASE [Exercise] SET  DISABLE_BROKER 
			ALTER DATABASE [Exercise] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
			ALTER DATABASE [Exercise] SET DATE_CORRELATION_OPTIMIZATION OFF 
			ALTER DATABASE [Exercise] SET TRUSTWORTHY OFF 
			ALTER DATABASE [Exercise] SET ALLOW_SNAPSHOT_ISOLATION OFF 
			ALTER DATABASE [Exercise] SET PARAMETERIZATION SIMPLE 
			ALTER DATABASE [Exercise] SET READ_COMMITTED_SNAPSHOT OFF
			ALTER DATABASE [Exercise] SET HONOR_BROKER_PRIORITY OFF 
			ALTER DATABASE [Exercise] SET RECOVERY FULL 
			ALTER DATABASE [Exercise] SET  MULTI_USER 
			ALTER DATABASE [Exercise] SET PAGE_VERIFY CHECKSUM
			ALTER DATABASE [Exercise] SET DB_CHAINING OFF
			ALTER DATABASE [Exercise] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
			ALTER DATABASE [Exercise] SET TARGET_RECOVERY_TIME = 60 SECONDS 
			ALTER DATABASE [Exercise] SET DELAYED_DURABILITY = DISABLED 
			ALTER DATABASE [Exercise] SET ACCELERATED_DATABASE_RECOVERY = OFF
			ALTER DATABASE [Exercise] SET QUERY_STORE = ON
			ALTER DATABASE [Exercise] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
			ALTER DATABASE [Exercise] SET  READ_WRITE 


		/******		Create "Patients" Table				******/
		SET ANSI_NULLS ON
		SET QUOTED_IDENTIFIER ON

		/****** Object:  Table [dbo].[Patients]		******/
		IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Patients]') AND type in (N'U'))
			BEGIN
				PRINT 'Table "[Patients]" Does Not EXIST... Creating Table now.'
				CREATE TABLE [dbo].[Patients](
					[PatientId] [INT] IDENTITY(1,1) NOT NULL,
					[FirstName] [VARCHAR](50) NOT NULL,
					[LastName] [VARCHAR](50) NOT NULL,
					[BirthDate] [DATETIME] NOT NULL,
					[GenderDescription] [VARCHAR](50) NULL,
					[DateCreated] [DATETIME] NULL,
					[DateUpdated] [DATETIME] NULL,
					[IsActive] [BIT] NULL,
				 CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED 
				(
					[PatientId] ASC
				)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
				) ON [PRIMARY]

				ALTER TABLE [dbo].[Patients] ADD  CONSTRAINT [DF_Patients_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
				ALTER TABLE [dbo].[Patients] ADD  CONSTRAINT [DF_Patients_DateUpdated]  DEFAULT (getdate()) FOR [DateUpdated]
				ALTER TABLE [dbo].[Patients] ADD  CONSTRAINT [DF_Patients_IsActive]  DEFAULT ((1)) FOR [IsActive]

				/****** Hydrate pre-fill Patient data prior to first CSV file upload		******/
				IF NOT EXISTS(SELECT [PatientId] FROM [dbo].[Patients] WHERE [FirstName] = 'Wednesday' AND [LastName] = 'Adams')
					BEGIN
						INSERT INTO [dbo].[Patients]
							([FirstName],[LastName],[BirthDate],[GenderDescription])
							VALUES('Wednesday','Adams', '1944-08-26 00:00:00', 'Female');
					END
				IF NOT EXISTS(SELECT [PatientId] FROM [dbo].[Patients] WHERE [FirstName] = 'Derek' AND [LastName] = 'Smalls')
					BEGIN
						INSERT INTO [dbo].[Patients]
							([FirstName],[LastName],[BirthDate],[GenderDescription])
							VALUES('Derek','Smalls', '1947-04-01 00:00:00', 'Male');
					END
				IF NOT EXISTS(SELECT [PatientId] FROM [dbo].[Patients] WHERE [FirstName] = 'Bruce' AND [LastName] = 'Campbell')
					BEGIN
						INSERT INTO [dbo].[Patients]
							([FirstName],[LastName],[BirthDate],[GenderDescription])
							VALUES('Bruce','Campbell', '1958-06-22 00:00:00', 'Male');
					END
				IF NOT EXISTS(SELECT [PatientId] FROM [dbo].[Patients] WHERE [FirstName] = 'Brett' AND [LastName] = 'Goldstein')
					BEGIN
						INSERT INTO [dbo].[Patients]
							([FirstName],[LastName],[BirthDate],[GenderDescription])
							VALUES('Brett','Goldstein', '1980-07-17 00:00:00', 'Male');
					END		
				IF NOT EXISTS(SELECT [PatientId] FROM [dbo].[Patients] WHERE [FirstName] = 'Dana ' AND [LastName] = 'Scully')
					BEGIN
						INSERT INTO [dbo].[Patients]
							([FirstName],[LastName],[BirthDate],[GenderDescription])
							VALUES('Dana ','Scully', '1964-02-23 00:00:00', 'Female');
					END		
				IF NOT EXISTS(SELECT [PatientId] FROM [dbo].[Patients] WHERE [FirstName] = 'Oprah' AND [LastName] = 'Winfrey')
					BEGIN
						INSERT INTO [dbo].[Patients]
							([FirstName],[LastName],[BirthDate],[GenderDescription])
							VALUES('Oprah ','Winfrey', '1959-01-29 00:00:00', 'Female');
					END	
				
			END;

		/****** Object:  User [svc_patients_api]			******/ 
		IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE [name] = N'svc_patients_api')
			BEGIN
				PRINT 'USER "svc_patients_api" does NOT Exist...'
				CREATE USER [svc_patients_api] FOR LOGIN [svc_patients_api] WITH DEFAULT_SCHEMA=[dbo]
				ALTER ROLE db_datareader ADD MEMBER svc_patients_api;
				ALTER ROLE db_datawriter ADD MEMBER svc_patients_api;
				ALTER ROLE db_owner ADD MEMBER svc_patients_api;
			END;
		ELSE
				PRINT 'USER "svc_patients_api" DOES Exist...'
	END

/******		User Defined Table Type					******/

IF NOT EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'PatientTVP' AND ss.name = N'dbo')
	BEGIN
		CREATE TYPE [dbo].[PatientTVP] AS TABLE (
		[firstName]				VARCHAR(50) NOT NULL,
		[lastName]				VARCHAR(50)  NOT NULL,
		[birthDate]				DATETIME NULL,
		[genderDescription]     VARCHAR(50)  NOT NULL);
	END
GO


/******		StoredProcedures						******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpsertPatient]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[usp_UpsertPatient] AS' 
END
GO

ALTER PROCEDURE [dbo].[usp_UpsertPatient] 
	@id					INT = NULL,
	@firstName			VARCHAR(50),
	@lastName			VARCHAR(50),
	@genderDescription	VARCHAR(50),
	@birthDate			DATETIME,
	@isActive			BIT = 1
AS
BEGIN
	SET NOCOUNT ON;
	Declare @RetVal INT;
	Set @RetVal = 0;
	
	BEGIN TRY;
		BEGIN TRANSACTION;
			--UPDATE IF EXISTS:
			UPDATE	[dbo].[Patients] WITH (UPDLOCK, SERIALIZABLE) 
			   SET	[FirstName]	 = @firstName,
					[LastName] = @lastName,
					[BirthDate] = @birthDate,
					[IsActive] = @isActive,
					[GenderDescription] = @genderDescription,
					[DateUpdated] = GETDATE()
			 WHERE	[PatientId] = @id;
			
			
			
			IF @@ROWCOUNT = 0	--INSERT New Row
			BEGIN
				INSERT	INTO [dbo].[Patients]([FirstName], [LastName], [BirthDate], [GenderDescription])
				VALUES	(@firstName, @lastName, @birthDate, @genderDescription);
				SELECT	@RetVal = @@IDENTITY;
			END
	
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH;
		DECLARE @msg nvarchar(max);
		SET @msg = ERROR_MESSAGE();
		RAISERROR (@msg, 16, 1);
		Set @RetVal = -1;
	
		ROLLBACK TRANSACTION;
	END CATCH;
	RETURN @RetVal;
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetPatients]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[usp_GetPatients] AS' 
END
GO

ALTER PROCEDURE [dbo].[usp_GetPatients] 
	@pageNum			INT = 1, 
	@pageSize			INT = 10,
	@sortBy				VARCHAR(50) = 'LastName',
	@sortAsc			BIT = 1,
	@searchTerm			VARCHAR(255) = '',
	@collectionTotal	INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @pagedItemIds TABLE(id INT);

	-- SEARCH TERM PRESENT
	IF(@searchTerm IS NOT NULL AND @searchTerm != '')
	BEGIN
		--	Select Paged Collection of matching Items
		INSERT  INTO @pagedItemIds
				SELECT	p.[PatientId]
				  FROM	[dbo].[Patients] p
				 WHERE	p.[LastName] LIKE '%'+ @searchTerm + '%' OR p.[FirstName] LIKE '%'+ @searchTerm + '%'
				 ORDER	BY 
						CASE
							WHEN	@sortAsc != 1 THEN ''
							WHEN	LOWER(@sortBy) = 'firstname' THEN [FirstName] 
						END ASC,
						CASE
							WHEN	@sortAsc != 1 THEN ''
							WHEN	LOWER(@sortBy)  = 'lastname' THEN [LastName]
						END ASC,
						CASE
							WHEN	@sortAsc != 1 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'dateupdated' THEN [DateUpdated]
						END ASC, 
						CASE
							WHEN	@sortAsc != 1 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'datecreated' THEN [DateCreated]
						END ASC,    
						CASE
							WHEN	@sortAsc != 1 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'birthdate' THEN [BirthDate]
						END ASC,
						CASE
							WHEN	@sortAsc != 1 THEN ''
							WHEN	LOWER(@sortBy)  = 'genderdescription' THEN [GenderDescription] 
						END ASC,
						CASE
							WHEN	@sortAsc != 0 THEN ''
							WHEN	LOWER(@sortBy) = 'firstname' THEN [FirstName] 
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN ''
							WHEN	LOWER(@sortBy)  = 'lastname' THEN [LastName]
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'dateupdated' THEN [DateUpdated]
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'datecreated' THEN [DateCreated] 
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'birthdate' THEN [BirthDate]
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN ''
							WHEN	LOWER(@sortBy)  = 'genderdescription' THEN [GenderDescription] 
						END DESC
				OFFSET  (@pageNum - 1) * @pageSize ROWS
				FETCH	NEXT @pageSize ROWS ONLY;
	END
	ELSE
	BEGIN
		--  SEARCH TERM ABSENT
		--	Select Paged Collection of matching Items
		INSERT  INTO @pagedItemIds
				SELECT	p.[PatientId]
				FROM	[dbo].[Patients] p
				ORDER	BY 
						CASE
							WHEN	@sortAsc != 1 THEN ''
							WHEN	LOWER(@sortBy) = 'firstname' THEN [FirstName] 
						END ASC,
						CASE
							WHEN	@sortAsc != 1 THEN ''
							WHEN	LOWER(@sortBy)  = 'lastname' THEN [LastName]
						END ASC,
						CASE
							WHEN	@sortAsc != 1 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'dateupdated' THEN [DateUpdated]
						END ASC, 
						CASE
							WHEN	@sortAsc != 1 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'datecreated' THEN [DateCreated]
						END ASC,    
						CASE
							WHEN	@sortAsc != 1 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'birthdate' THEN [BirthDate]
						END ASC,
						CASE
							WHEN	@sortAsc != 1 THEN ''
							WHEN	LOWER(@sortBy)  = 'genderdescription' THEN [GenderDescription] 
						END ASC,
						CASE
							WHEN	@sortAsc != 0 THEN ''
							WHEN	LOWER(@sortBy) = 'firstname' THEN [FirstName] 
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN ''
							WHEN	LOWER(@sortBy)  = 'lastname' THEN [LastName]
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'dateupdated' THEN [DateUpdated]
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'datecreated' THEN [DateCreated] 
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN cast(null as date)
							WHEN	LOWER(@sortBy)  = 'birthdate' THEN [BirthDate]
						END DESC,
						CASE
							WHEN	@sortAsc != 0 THEN ''
							WHEN	LOWER(@sortBy)  = 'genderdescription' THEN [GenderDescription] 
						END DESC
				OFFSET  (@pageNum - 1) * @pageSize ROWS
				FETCH	NEXT @pageSize ROWS ONLY;
		END

	SELECT	p.[PatientId], p.[FirstName], p.[LastName], p.[BirthDate], p.[GenderDescription], p.[DateCreated], p.[DateUpdated], p.[IsActive]
	  FROM	@pagedItemIds i INNER
	  JOIN	[dbo].[Patients] p 
	    ON	p.PatientId = i.id;

	
	SELECT @collectionTotal = COUNT([PatientId]) FROM [dbo].[Patients]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_UpsertPatients]') AND type in (N'P', N'PC'))
BEGIN
	EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[usp_UpsertPatients] AS' 
END
GO
ALTER PROCEDURE [dbo].[usp_UpsertPatients] 
	@TVP		[dbo].[PatientTVP] READONLY
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @RetVal INT, @ItemCount INT, @Counter INT;
	SET @RetVal = 0;
	SET @Counter = 1;
	DECLARE @uploadedPatients TABLE(
			[PatientId] [INT] IDENTITY(1,1) NOT NULL,
			[FirstName] [VARCHAR](50) NOT NULL,
			[LastName] [VARCHAR](50) NOT NULL,
			[BirthDate] [DATETIME] NOT NULL,
			[GenderDescription] [VARCHAR](50) NULL)
	
	INSERT INTO @uploadedPatients
		SELECT firstName, lastName, birthDate, genderDescription FROM @TVP

	SELECT	@ItemCount = MAX(PatientId) FROM @uploadedPatients;

	  WHILE @Counter <= @ItemCount
	  	BEGIN TRY;
			BEGIN TRANSACTION;
				--UPDATE IF EXISTS (No PK ID avaialbe, so matching on FirstName & LastName):
				UPDATE	p WITH (UPDLOCK, SERIALIZABLE) 
				   SET	p.[BirthDate] = u.birthDate,
						p.[GenderDescription] = u.genderDescription,
						p.[DateUpdated] = GETDATE()
				  FROM	@uploadedPatients u
				  JOIN	[dbo].[Patients] p 
				    ON	LOWER(p.FirstName) = LOWER(u.firstName) AND
						LOWER(p.LastName) = LOWER(u.lastName) AND 
						u.PatientId = @Counter
			
				IF @@ROWCOUNT = 0	--INSERT New Row

				BEGIN
					INSERT	INTO [dbo].[Patients]([FirstName], [LastName], [BirthDate], [GenderDescription])
						SELECT	[FirstName], [LastName], [BirthDate], [GenderDescription]
						  FROM	@uploadedPatients u
						 WHERE	u.PatientId = @Counter
				END
			COMMIT TRANSACTION;
			SELECT @Counter = @Counter + 1
		END TRY
		BEGIN CATCH;
			DECLARE @msg nvarchar(max);
			SET @msg = ERROR_MESSAGE();
			RAISERROR (@msg, 16, 1);
			Set @RetVal = -1;
	
			ROLLBACK TRANSACTION;
		END CATCH;
	RETURN @RetVal;
END
GO