/****** Script for SelectTopNRows command from SSMS  ******/
SELECT * FROM vw_GRINGlobal_Cooperator
WHERE ID IN
(
SELECT 
	--[sys_user_id]
 --     ,[user_name]
 --     ,[password]
 --     ,[is_enabled]
      [cooperator_id]
      --,[created_date]
      --,[created_by]
      --,[modified_date]
      --,[modified_by]
      --,[owned_date]
      --,[owned_by]
  FROM [gringlobal].[dbo].[sys_user]
  WHERE sys_user_id IN
  (
  613,608,586,595,587,605,640
  
  ))
 