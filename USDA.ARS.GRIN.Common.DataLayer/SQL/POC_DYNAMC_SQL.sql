USE training
GO

--CREATE VIEW vw_GRINGlobal_Taxonomy_Rpt_NoSpeciesAuthor
--AS
--SELECT * FROM vw_GRINGlobal_Taxonomy_Species WHERE ID IN
--(SELECT taxonomy_species_id FROM taxonomy_species 
--WHERE taxonomy_species.species_authority IS NULL 
--AND ( taxonomy_species.species_name NOT LIKE  'spp.') 
--AND taxonomy_species.species_authority IS NULL 
--AND ( taxonomy_species.species_name NOT LIKE  'hybr.' ))


--CREATE VIEW vw_GRINGlobal_Taxonomy_Rpt_UnverifiedNodulation
--AS
--SELECT * FROM vw_GRINGlobal_Taxonomy_Species WHERE ID IN
--(select distinct t.taxonomy_species_id
--from taxonomy_species t
--join citation c on t.taxonomy_species_id = c.taxonomy_species_id
--where t.verifier_cooperator_id is null
--and c.type_code = 'NODULATION')

-- missing basio
--CREATE VIEW vw_GRINGlobal_Taxonomy_Rpt_MissingBasionym
--AS
--SELECT * FROM vw_GRINGlobal_Taxonomy_Species WHERE ID IN
--(select taxonomy_species_id
--from taxonomy_species ts
--where ts.name_authority like '%(%)%'
--AND ts.taxonomy_species_id = ts.current_taxonomy_species_id
--AND NOT EXISTS (SELECT * FROM taxonomy_species WHERE current_taxonomy_species_id = ts.taxonomy_species_id AND synonym_code = 'B'))

--ALTER VIEW vw_GRINGlobal_Taxonomy_Rpt_MissingBasionym
--AS
--SELECT * FROM vw_GRINGlobal_Taxonomy_Species WHERE ID IN
--(SELECT ts.taxonomy_species_id
--FROM taxonomy_species ts
--JOIN taxonomy_genus tg ON tg.taxonomy_genus_id = ts.taxonomy_genus_id
--WHERE ts.taxonomy_species_id = ts.current_taxonomy_species_id
--AND forma_name IS NOT NULL
--AND NOT EXISTS (SELECT * FROM taxonomy_species ts2
--JOIN taxonomy_genus tg2 ON tg2.taxonomy_genus_id = ts2.taxonomy_genus_id
--WHERE genus_name = tg.genus_name AND species_name = ts.species_name AND forma_name = ts.species_name AND taxonomy_species_id = current_taxonomy_species_id))
 

--CREATE VIEW vw_GRINGlobal_Taxonomy_Rpt_MissingAutonymForm
--AS
--SELECT * FROM vw_GRINGlobal_Taxonomy_Species WHERE ID IN
--(SELECT ts.taxonomy_species_id
--FROM taxonomy_species ts
--JOIN taxonomy_genus tg ON tg.taxonomy_genus_id = ts.taxonomy_genus_id
--WHERE ts.taxonomy_species_id = ts.current_taxonomy_species_id
--AND forma_name IS NOT NULL
--AND NOT EXISTS (SELECT * FROM taxonomy_species ts2
--JOIN taxonomy_genus tg2 ON tg2.taxonomy_genus_id = ts2.taxonomy_genus_id
--WHERE genus_name = tg.genus_name AND species_name = ts.species_name AND forma_name = ts.species_name AND taxonomy_species_id = current_taxonomy_species_id))
 