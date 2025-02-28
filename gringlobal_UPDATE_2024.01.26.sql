USE [gringlobal]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Taxonomy_Verified_By_Cooperators_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Taxonomy_Verified_By_Cooperators_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Taxonomy_Species_SelectAll]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Taxonomy_Species_SelectAll]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Taxonomy_Family_Map_Insert]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Taxonomy_Family_Map_Insert]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Tables_Taxonomy_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Tables_Taxonomy_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Tables_By_App_User_Item_Folder_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Tables_By_App_User_Item_Folder_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Primary_Key_Field_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Table_Primary_Key_Field_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Owner_Update]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Table_Owner_Update]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Fields_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Table_Fields_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Field_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Table_Field_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Objects_Recent_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Sys_Objects_Recent_Select]
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Accession_Search]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP PROCEDURE IF EXISTS [dbo].[usp_GRINGlobal_Accession_Search]
GO
/****** Object:  View [dbo].[vw_GRINGlobal_Taxonomy_Species]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_GRINGlobal_Taxonomy_Species]
GO
/****** Object:  View [dbo].[vw_GRINGlobal_Accession]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_GRINGlobal_Accession]
GO
/****** Object:  View [dbo].[vw_GRINGlobal_App_User_Item_Folder]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_GRINGlobal_App_User_Item_Folder]
GO
/****** Object:  View [dbo].[vw_GRINGlobal_Sys_Table]    Script Date: 1/26/2024 12:57:37 PM ******/
DROP VIEW IF EXISTS [dbo].[vw_GRINGlobal_Sys_Table]
GO
/****** Object:  View [dbo].[vw_GRINGlobal_Sys_Table]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO









CREATE VIEW [dbo].[vw_GRINGlobal_Sys_Table]
AS
SELECT 
	st.database_area_code AS DatabaseAreaCode,
	st.sys_table_id AS ID,
	st.sys_table_id AS SysTableID,
	stl.sys_table_lang_id AS SysTableLangID,
	st.table_name AS SysTableName,
	st.is_enabled AS IsEnabled,
	CASE WHEN stl.sys_table_lang_id IS NULL THEN 'N' ELSE 'Y' END AS IsMapped,
	CASE WHEN stl.title IS NULL THEN st.table_name ELSE stl.title END AS SysTableTitle,
	REPLACE(stl.title,' ','') AS TableCode
FROM 
	sys_table st
LEFT OUTER JOIN
	sys_table_lang stl
ON
	st.sys_table_id = stl.sys_table_id
GO
/****** Object:  View [dbo].[vw_GRINGlobal_App_User_Item_Folder]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW [dbo].[vw_GRINGlobal_App_User_Item_Folder]
AS
SELECT 
	app_user_item_folder_id AS ID,
    REPLACE(folder_name,'Tab 1 Root Folder|','') AS FolderName,
    CASE WHEN auif.folder_type = 'TAXONOMY_FAMILY' THEN 'taxonomy_family_map' ELSE auif.folder_type END AS FolderType,
    description AS Description,
	category AS Category,
	data_type AS DataType,
	(SELECT TableCode FROM vw_GRINGlobal_Sys_Table WHERE SysTableName = data_type) AS DataTypeDescription,
	--vggs.TableCode,
	--vggs.TableTitle,
	properties AS Properties,
    note AS Note,
	is_favorite AS IsFavorite,
	(SELECT COUNT(app_user_item_list_id) FROM app_user_item_folder_list_map WHERE app_user_item_folder_id = auif.app_user_item_folder_id) AS TotalItems,
     created_date AS CreatedDate,
	 created_by AS CreatedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = auif.created_by) AS CreatedByCooperatorName,
	modified_date AS ModifiedDate,
	modified_by AS ModifiedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = auif.modified_by) AS ModifiedByCooperatorName
FROM 
	app_user_item_folder auif
--LEFT OUTER JOIN
--	vw_GRINGlobal_Sys_Table vggs
--ON
--	data_type = vggs.TableName
--WHERE
--	vggs.TableTitle IS NOT NULL
GO
/****** Object:  View [dbo].[vw_GRINGlobal_Accession]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO










CREATE VIEW [dbo].[vw_GRINGlobal_Accession]
AS
SELECT 
	a.accession_id AS ID,
	LTRIM(RTRIM(LTRIM(COALESCE(a.accession_number_part1, '') + ' ') + LTRIM(COALESCE(convert(varchar, a.accession_number_part2), '') + ' ') + COALESCE(a.accession_number_part3, ''))) AS AssembledName,
	a.taxonomy_species_id AS SpeciesID,
	a.status_code AS StatusCode,
    life_form_code AS LifeFormCode,
    improvement_status_code AS ImprovementStatusCode,
    reproductive_uniformity_code AS ReproductiveUniformityCode,
    initial_received_form_code AS InitialReceivedFormCode,
	life_cycle_code AS LifeCycleCode,
    life_habit_code AS LifeHabitCode,
    life_sex_code AS LifeSexCode,
    initial_received_date AS InitialReceivedDate,
	note AS Note,
	a.created_date AS CreatedDate,
	a.created_by AS CreatedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = a.created_by) AS CreatedByCooperatorName,
	a.modified_date AS ModifiedDate,
	a.modified_by AS ModifiedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = a.modified_by) AS ModifiedByCooperatorName,
	a.owned_date AS OwnedDate,
	a.owned_by AS OwnedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = a.owned_by) AS OwnedByCooperatorName
FROM
	accession a

GO
/****** Object:  View [dbo].[vw_GRINGlobal_Taxonomy_Species]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

















CREATE VIEW [dbo].[vw_GRINGlobal_Taxonomy_Species]
AS
SELECT 
	ts.taxonomy_species_id AS ID,
	ts.nomen_number AS NomenNumber,
	ts.current_taxonomy_species_id AS AcceptedID,
	(SELECT DISTINCT tg2.genus_name + ' ' + ts2.species_name 
	 FROM 
		taxonomy_species ts2 
	 JOIN
		taxonomy_genus tg2
	 ON
		ts2.taxonomy_genus_id = tg2.taxonomy_genus_id
	 WHERE 
		ts2.taxonomy_species_id = ts.current_taxonomy_species_id) AS AcceptedName,
	CASE
		WHEN 
			ts.taxonomy_species_id = ts.current_taxonomy_species_id 
		THEN
			'Y'
		ELSE 
			'N'
	END AS IsAcceptedName,
	ts.synonym_code AS SynonymCode,
	(SELECT REPLACE(CodeTitle,'Synonym','') FROM vw_GRINGlobal_Code_Value WHERE GroupName = 'TAXONOMY_SPECIES_QUALIFIER' AND Code = LTRIM(RTRIM(ts.synonym_code))) AS SynonymDescription,
	(CASE 
		WHEN ts.forma_name IS NOT NULL THEN 'FORM' 
		WHEN ts.subvariety_name IS NOT NULL THEN 'SUBVARIETY'
		WHEN ts.variety_name IS NOT NULL THEN 'VARIETY'
		WHEN ts.subspecies_name IS NOT NULL THEN 'SUBSPECIES'
		ELSE 'SPECIES'
	END) AS Rank,
	(CASE tg.hybrid_code WHEN 'X' THEN '×' WHEN '+' THEN '+' ELSE '' END) + '<i>' + tg.genus_name + ' ' +
    (CASE ts.is_specific_hybrid WHEN 'Y' THEN '</i>×<i>' ELSE '' END) + ts.species_name + '</i> ' + COALESCE (ts.species_authority, '') + ' ' +
    (CASE ts.is_subspecific_hybrid WHEN 'Y' THEN 'notho' ELSE '' END) +
    (CASE when ts.subspecies_name IS NOT NULL then 'subsp. <i>' + ts.subspecies_name + '</i> ' + COALESCE (ts.subspecies_authority, '') + ' ' else '' end) +
    (CASE ts.is_varietal_hybrid WHEN 'Y' THEN 'notho' ELSE '' END) +
    (CASE when ts.variety_name IS NOT NULL then 'var. <i>' + ts.variety_name + '</i> ' + COALESCE (ts.variety_authority, '') + ' ' else '' end) +
    (CASE ts.is_subvarietal_hybrid WHEN 'Y' THEN 'notho' ELSE '' END) +
    (CASE when ts.subvariety_name IS NOT NULL then 'subvar. <i>' + ts.subvariety_name + '</i> ' + COALESCE (ts.subvariety_authority, '') + ' ' else '' end) +
    (CASE when ts.forma_name IS NOT NULL then ts.forma_rank_type + ' <i>' + ts.forma_name + '</i> ' + COALESCE (ts.forma_authority, '') + ' ' else '' end) AS AssembledName,
	ts.name AS Name,
	ts.species_name AS SpeciesName,
    ts.name_authority AS NameAuthority,
    ts.protologue AS Protologue,
    ts.protologue_virtual_path AS ProtologueVirtualPath,
    ts.species_authority AS SpeciesAuthority,
 	ts.is_specific_hybrid AS IsSpecificHybrid,
	ts.hybrid_parentage AS HybridParentage,
	ts.subspecies_name AS SubspeciesName,
	ts.subspecies_authority AS SubspeciesAuthority,
    ts.is_subspecific_hybrid AS IsSubspecificHybrid,
    ts.variety_name AS VarietyName,
    ts.variety_authority AS VarietyAuthority,
    ts.is_varietal_hybrid AS IsVarietalHybrid,
    ts.subvariety_name AS SubvarietyName,
    ts.subvariety_authority AS SubvarietyAuthority,
	ts.is_subvarietal_hybrid AS IsSubvarietalHybrid,
    ts.forma_name AS FormaName,
    ts.forma_authority AS FormaAuthority,
    ts.forma_rank_type AS FormaRankType,
    ts.is_forma_hybrid AS IsFormaHybrid, 
 	tg.taxonomy_genus_id AS GenusID,
	tg.genus_name AS GenusName,
	tg.hybrid_code As GenusHybridCode,
	tg.subgenus_name AS SubGenusName,
    ts.verifier_cooperator_id AS VerifiedByCooperatorID,
	ts.is_web_visible AS IsWebVisible,
	(SELECT first_name + ' ' + last_name FROM cooperator where cooperator_id = ts.verifier_cooperator_id) AS VerifiedByCooperatorName,
    CASE WHEN (verifier_cooperator_id IS NOT NULL) AND (ts.name_verified_date IS NOT NULL) THEN 'Y' ELSE 'N' END AS IsVerified,
	ts.name_verified_date AS NameVerifiedDate,
    ts.alternate_name AS AlternateName,
	ts.common_fertilization_code AS CommonFertilizationCode,
	ts.priority1_site_id AS Priority1SiteID,
	ts.priority2_site_id AS Priority2SiteID,
	ts.restriction_code AS RestrictionCode,
	ts.note AS Note,
    ts.created_date AS CreatedDate,
	ts.created_by AS CreatedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator where cooperator_id = ts.created_by) AS CreatedByCooperatorName,
	ts.modified_date AS ModifiedDate,
	ts.modified_by AS ModifiedByCooperatorID,
	(SELECT first_name + ' ' + last_name FROM cooperator WHERE cooperator_id = ts.modified_by) AS ModifiedByCooperatorName,
	(SELECT COUNT(accession_id) FROM accession WHERE taxonomy_species_id = ts.taxonomy_species_id) AS AccessionCount
FROM 
	taxonomy_species ts
JOIN 
	taxonomy_genus tg
ON 
	tg.taxonomy_genus_id = ts.taxonomy_genus_id
	
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Accession_Search]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Accession_Search]
	@CreatedByCooperatorID INT = 0,
	@SpeciesID INT = 0
AS
BEGIN

SELECT 
	* 
FROM 
	vw_GRINGlobal_Accession
WHERE 
	(@CreatedByCooperatorID IS NULL OR CreatedByCooperatorID     =       @CreatedByCooperatorID)
AND
	(@SpeciesID IS NULL OR SpeciesID     =       @SpeciesID)
ORDER BY 
	AssembledName

END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Objects_Recent_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Objects_Recent_Select]
	@num_days INT
AS
BEGIN

SELECT 
	DATEDIFF(day, modify_date, GETDATE()) AS DEBUG,
	*
FROM 
	sys.objects 
WHERE 
	(type_desc = 'SQL_STORED_PROCEDURE' OR type_desc = 'VIEW')
AND 
	DATEDIFF(day, modify_date, GETDATE()) <= @num_days
AND
	schema_id = 1
ORDER BY 
	type,
	name 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Field_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Table_Field_Select]
	@table_name NVARCHAR(50),
	@field_name NVARCHAR(50)
AS
BEGIN
	SELECT [ID]
      ,[SysTableID]
      ,[SysTableName]
      ,[SysTableTitle]
      ,[FieldName]
      ,[FieldTitle]
      ,[FieldOrdinal]
      ,[FieldPurpose]
      ,[FieldType]
      ,[DefaultValue]
      ,[IsPrimaryKey]
      ,[IsForeignKey]
      ,[ForeignKeyTableFieldID]
      ,[ForeignKeyDataviewName]
      ,[IsNullable]
      ,[GUIHint]
      ,[IsReadOnly]
      ,[MinLength]
      ,[MaxLength]
      ,[NumericPrecision]
      ,[NumericScale]
      ,[IsAutoIncrement]
      ,[GroupName]
	FROM 
		[vw_GRINGlobal_Sys_Table_Field]
	WHERE 
		SysTableName = @table_name
	AND
		FieldName = @field_name
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Fields_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Table_Fields_Select]
	@sys_table_name NVARCHAR(50)
AS
BEGIN
	SELECT [ID]
      ,[SysTableID]
      ,[SysTableName]
      ,[SysTableTitle]
      ,[FieldName]
      ,[FieldTitle]
      ,[FieldOrdinal]
      ,[FieldPurpose]
      ,[FieldType]
      ,[DefaultValue]
      ,[IsPrimaryKey]
      ,[IsForeignKey]
      ,[ForeignKeyTableFieldID]
      ,[ForeignKeyDataviewName]
      ,[IsNullable]
      ,[GUIHint]
      ,[IsReadOnly]
      ,[MinLength]
      ,[MaxLength]
      ,[NumericPrecision]
      ,[NumericScale]
      ,[IsAutoIncrement]
      ,[GroupName]
	FROM 
		[vw_GRINGlobal_Sys_Table_Field]
	WHERE 
		SysTableName = @sys_table_name
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Owner_Update]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Table_Owner_Update]
	@owned_by_from INT,
	@owned_by_to INT
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX)

	SELECT 
		'UPDATE ' + table_name + ' SET owned_by = ' + CONVERT(varchar, @owned_by_to) + ', owned_date = GETUTCDATE() WHERE owned_by = ' + CONVERT(varchar, @owned_by_from)
	FROM 
		sys_table
	WHERE 
		sys_table_id IN
			(SELECT sys_table_id FROM sys_table_field WHERE field_name = 'owned_by')
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Table_Primary_Key_Field_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Table_Primary_Key_Field_Select]
	@field_name NVARCHAR(50)
AS
BEGIN
	SELECT [ID]
      ,[SysTableID]
      ,[SysTableName]
      ,[SysTableTitle]
      ,[FieldName]
      ,[FieldTitle]
      ,[FieldOrdinal]
      ,[FieldPurpose]
      ,[FieldType]
      ,[DefaultValue]
      ,[IsPrimaryKey]
      ,[IsForeignKey]
      ,[ForeignKeyTableFieldID]
      ,[ForeignKeyDataviewName]
      ,[IsNullable]
      ,[GUIHint]
      ,[IsReadOnly]
      ,[MinLength]
      ,[MaxLength]
      ,[NumericPrecision]
      ,[NumericScale]
      ,[IsAutoIncrement]
      ,[GroupName]
	FROM 
		[vw_GRINGlobal_Sys_Table_Field]
	WHERE 
		FieldName = @field_name
	AND
		FieldPurpose = 'PRIMARY_KEY'
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Tables_By_App_User_Item_Folder_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Tables_By_App_User_Item_Folder_Select]
	@app_user_item_folder_id INT
AS
BEGIN
	SELECT 
		vgst.SysTableName,
		vgst.TableCode,
		vgst.SysTableTitle
	FROM 
		vw_GRINGlobal_Sys_Table vgst
	WHERE SysTableName IN
	(
		SELECT DISTINCT 
			REPLACE(IDType,'_ID','')
		FROM 
			vw_GRINGlobal_App_User_Item_List auil
		WHERE 
			AppUserItemFolderID = @app_user_item_folder_id
	)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Sys_Tables_Taxonomy_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Sys_Tables_Taxonomy_Select]
AS
BEGIN
	SELECT 
		[DatabaseAreaCode]
		,[ID]
		,[SysTableName]
		,[IsEnabled]
		,[IsMapped]
		,[SysTableTitle]
		,[TableCode]
	FROM 
		vw_GRINGlobal_Sys_Table
	WHERE 
		(DatabaseAreaCode = 'TAXONOMY' OR SysTableName IN ('citation','literature'))
	AND
		IsMapped = 'Y'
	-- TEMP
	AND
		ID <> 77
	ORDER BY
		SysTableTitle
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Taxonomy_Family_Map_Insert]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GRINGlobal_Taxonomy_Family_Map_Insert]
(
	@out_error_number INT = 0 OUTPUT,
	@out_taxonomy_family_map_id INT = 0 OUTPUT,
	@out_taxonomy_family2_id INT = 0 OUTPUT,
	@taxonomy_family_map_accepted_id int,
	@type_taxonomy_genus_id int,
	@taxonomy_order_id INT,
	@family_name nvarchar(25)  ,
	@family_authority nvarchar(250),
	@alternate_name NVARCHAR(25),
	@family_type_code NVARCHAR(20),
	@note NVARCHAR(MAX),
	@created_by int
)
AS
BEGIN
	DECLARE @ADMIN_COOP_ID INT = 48
	DECLARE @new_taxonomy_family_accepted_id INT

	BEGIN TRY
		INSERT INTO taxonomy_family2
		(
			accepted_taxonomy_family_map_id,
			type_taxonomy_genus_id,
			taxonomy_classification_id,
			family_name,
			family_authority,
			alternate_name,
			family_type_code,
			note,
			created_by,
			created_date
		)
		VALUES 
		(
			@taxonomy_family_map_accepted_id,
			@type_taxonomy_genus_id,
			@taxonomy_order_id,
			@family_name,
			@family_authority,
			@alternate_name,
			@family_type_code,
			@note,
			@created_by,
			GETDATE()
		)
		SELECT @out_taxonomy_family2_id = CAST(SCOPE_IDENTITY() AS INT)

		-- TEMP: COPY NEW REC. TO LEGACY TABLE
		INSERT INTO taxonomy_family
		(
			type_taxonomy_genus_id,
			taxonomy_classification_id,
			family_name,
			family_authority,
			alternate_name,
			family_type_code,
			note,
			created_by,
			created_date,
			owned_by,
			owned_date
		)
		VALUES 
		(
			@type_taxonomy_genus_id,
			@taxonomy_order_id,
			@family_name,
			@family_authority,
			@alternate_name,
			@family_type_code,
			@note,
			@created_by,
			GETDATE(),
			@created_by,
			GETDATE()
		)

		UPDATE 
			taxonomy_family2
		SET 
			accepted_taxonomy_family_map_id = @out_taxonomy_family2_id
		WHERE 
			taxonomy_family2_id = @out_taxonomy_family2_id
	
		INSERT INTO taxonomy_family_map
			(
				taxonomy_family2_id,
				authority,
				taxonomy_family_map_accepted_id,
				type_taxonomy_genus_id,
				note,
				created_by,
				created_date
			)
		VALUES
			(
				@out_taxonomy_family2_id,
				@family_authority,
				@taxonomy_family_map_accepted_id,
				@type_taxonomy_genus_id,
				@note,
				@created_by,
				GETDATE()
			)
		SELECT @out_taxonomy_family_map_id = CAST(SCOPE_IDENTITY() AS INT)

		UPDATE 
			taxonomy_family_map
		SET 
			taxonomy_family_map_accepted_id = @out_taxonomy_family_map_id
		WHERE
			taxonomy_family_map_id = @out_taxonomy_family_map_id

	END TRY
	BEGIN CATCH
		SELECT @out_error_number=ERROR_NUMBER()
		INSERT INTO 
			sys_db_error
		VALUES
		  (SUSER_SNAME(),
		   ERROR_NUMBER(),
		   ERROR_STATE(),
		   ERROR_SEVERITY(),
		   ERROR_LINE(),
		   ERROR_PROCEDURE(),
		   ERROR_MESSAGE(),
		   GETDATE());
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Taxonomy_Species_SelectAll]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Taxonomy_Species_SelectAll]
AS
BEGIN
SELECT TOP (100) [ID]
      ,[NomenNumber]
      ,[AcceptedID]
      ,[AcceptedName]
      ,[IsAcceptedName]
      ,[SynonymCode]
      ,[SynonymDescription]
      ,[Rank]
      ,[AssembledName]
      ,[Name]
      ,[SpeciesName]
      ,[NameAuthority]
      ,[Protologue]
      ,[ProtologueVirtualPath]
      ,[SpeciesAuthority]
      ,[IsSpecificHybrid]
      ,[HybridParentage]
      ,[SubspeciesName]
      ,[SubspeciesAuthority]
      ,[IsSubspecificHybrid]
      ,[VarietyName]
      ,[VarietyAuthority]
      ,[IsVarietalHybrid]
      ,[SubvarietyName]
      ,[SubvarietyAuthority]
      ,[IsSubvarietalHybrid]
      ,[FormaName]
      ,[FormaAuthority]
      ,[FormaRankType]
      ,[IsFormaHybrid]
      ,[GenusID]
      ,[GenusName]
      ,[GenusHybridCode]
      ,[SubGenusName]
      ,[VerifiedByCooperatorID]
      ,[IsWebVisible]
      ,[VerifiedByCooperatorName]
      ,[IsVerified]
      ,[NameVerifiedDate]
      ,[AlternateName]
      ,[CommonFertilizationCode]
      ,[Priority1SiteID]
      ,[Priority2SiteID]
      ,[RestrictionCode]
      ,[Note]
      ,[CreatedDate]
      ,[CreatedByCooperatorID]
      ,[CreatedByCooperatorName]
      ,[ModifiedDate]
      ,[ModifiedByCooperatorID]
      ,[ModifiedByCooperatorName]
  FROM [gringlobal].[dbo].[vw_GRINGlobal_Taxonomy_Species]
END
GO
/****** Object:  StoredProcedure [dbo].[usp_GRINGlobal_Taxonomy_Verified_By_Cooperators_Select]    Script Date: 1/26/2024 12:57:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GRINGlobal_Taxonomy_Verified_By_Cooperators_Select] 
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		SELECT DISTINCT
			ts.verifier_cooperator_id AS ID,
			c.last_name + ', ' + c.first_name AS FullName
		FROM taxonomy_species ts JOIN
			cooperator c
		ON
			ts.verifier_cooperator_id = c.cooperator_id
	END TRY
	BEGIN CATCH
	
	END CATCH
END
GO
