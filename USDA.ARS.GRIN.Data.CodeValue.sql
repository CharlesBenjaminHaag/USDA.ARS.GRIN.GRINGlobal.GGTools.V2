USE [gringlobal]
GO

DECLARE	@return_value int,
		@out_error_number int,
		@out_code_value_id int

EXEC	@return_value = [dbo].[usp_GRINGlobal_Code_Value_Insert]
		@out_error_number = @out_error_number OUTPUT,
		@out_code_value_id = @out_code_value_id OUTPUT,
		@group_name = N'GGTOOLS_WORKSPACE',
		@code_value = N'ARM',
		@title = N'Access Rights Management',
		@description = N'Create and update GRIN-Global CT users, and manage permissions',
		@sys_lang_id = 1,
		@created_by = 48

SELECT	@out_error_number as N'@out_error_number',
		@out_code_value_id as N'@out_code_value_id'

SELECT	'Return Value' = @return_value


EXEC	@return_value = [dbo].[usp_GRINGlobal_Code_Value_Insert]
		@out_error_number = @out_error_number OUTPUT,
		@out_code_value_id = @out_code_value_id OUTPUT,
		@group_name = N'GGTOOLS_WORKSPACE',
		@code_value = N'ARM',
		@title = N'Access Rights Management',
		@description = N'Create and update GRIN-Global CT users, and manage permissions',
		@sys_lang_id = 1,
		@created_by = 48

SELECT	@out_error_number as N'@out_error_number',
		@out_code_value_id as N'@out_code_value_id'

SELECT	'Return Value' = @return_value


EXEC	@return_value = [dbo].[usp_GRINGlobal_Code_Value_Insert]
		@out_error_number = @out_error_number OUTPUT,
		@out_code_value_id = @out_code_value_id OUTPUT,
		@group_name = N'GGTOOLS_WORKSPACE',
		@code_value = N'TAX',
		@title = N'Taxonomy Data Management',
		@description = N'Manage all GRIN-Global taxonomical data.',
		@sys_lang_id = 1,
		@created_by = 48

SELECT	@out_error_number as N'@out_error_number',
		@out_code_value_id as N'@out_code_value_id'

SELECT	'Return Value' = @return_value


EXEC	@return_value = [dbo].[usp_GRINGlobal_Code_Value_Insert]
		@out_error_number = @out_error_number OUTPUT,
		@out_code_value_id = @out_code_value_id OUTPUT,
		@group_name = N'GGTOOLS_WORKSPACE',
		@code_value = N'CUR	',
		@title = N'Curator Utility',
		@description = N'View and edit accession and inventory data.',
		@sys_lang_id = 1,
		@created_by = 48

SELECT	@out_error_number as N'@out_error_number',
		@out_code_value_id as N'@out_code_value_id'

SELECT	'Return Value' = @return_value

EXEC	@return_value = [dbo].[usp_GRINGlobal_Code_Value_Insert]
		@out_error_number = @out_error_number OUTPUT,
		@out_code_value_id = @out_code_value_id OUTPUT,
		@group_name = N'GGTOOLS_WORKSPACE',
		@code_value = N'ORD',
		@title = N'Order Management',
		@description = N'Manage orders placed through GRIN-Global.',
		@sys_lang_id = 1,
		@created_by = 48

SELECT	@out_error_number as N'@out_error_number',
		@out_code_value_id as N'@out_code_value_id'

SELECT	'Return Value' = @return_value

GO
