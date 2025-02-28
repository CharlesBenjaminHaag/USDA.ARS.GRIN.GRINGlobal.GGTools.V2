using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class ViewModelBase
    {
        public bool IsAuthorized(int cooperatorId, string eventAction, string eventValue)
        {
            // TODO
            return false;
        }

        public void CheckAuthority(string authorities, string column)
        {
            //    //convert all author construction syntax to &
            //    authorities = Regex.Replace(authorities, @"\(|\)|,|\?| ex | non |sensu | et al\.", "&");
            //    string[] authList = authorities.Split('&');
            //    foreach (string auth in authList)
            //    {
            //        string authClean = auth.Trim();
            //        if (!String.IsNullOrEmpty(authClean))
            //        {
            //            var dtAuth = args.ReadData(@"SELECT * FROM taxonomy_author WHERE short_name = :auth", ":auth", authClean, DbType.String);
            //            if (dtAuth.Rows.Count < 1)
            //            {
            //                args.Cancel(column + " " + authClean + " could not be validated.");
            //            }
            //        }
            //    }
        }

        public void TODO_SPECIES_VALIDATION()
        { 
        //{
        //    public void TableRowSaving(ISaveDataTriggerArgs args)
        //    // pre update row trigger
        //    {

        //        if (args.SaveMode == SaveMode.Update || args.SaveMode == SaveMode.Insert)
        //        {
        //            var helper = args.Helper;
        //            // 2 - set shybrid flag if needed and lowercase species

        //            if (helper.AllFieldsExist("species_name"))
        //            {
        //                var species = helper.GetValue("species_name", "", true).ToString().ToLower();
        //                helper.SetValue("species_name", species, typeof(string), true);

        //                string sub = species.Substring(0, 2);
        //                if (sub == "x ")
        //                {
        //                    //args.Cancel("hybrid found");
        //                    //return;
        //                    helper.SetValue("is_specific_hybrid", "Y", typeof(string), true);
        //                    helper.SetValue("species_name", species.Substring(2), typeof(string), true);
        //                }
        //            }

        //            // 3 lower other columns 
        //            if (helper.AllFieldsExist("subspecies_name") && !helper.IsValueEmpty("subspecies_name"))
        //            { helper.SetValue("subspecies_name", helper.GetValue("subspecies_name", "", true).ToString().ToLower(), typeof(string), true); }

        //            if (helper.AllFieldsExist("variety_name") && !helper.IsValueEmpty("variety_name"))
        //            { helper.SetValue("variety_name", helper.GetValue("variety_name", "", true).ToString().ToLower(), typeof(string), true); }

        //            if (helper.AllFieldsExist("subvariety_name") && !helper.IsValueEmpty("subvariety_name"))
        //            { helper.SetValue("subvariety_name", helper.GetValue("subvariety_name", "", true).ToString().ToLower(), typeof(string), true); }

        //            if (helper.AllFieldsExist("forma_name") && !helper.IsValueEmpty("forma_name"))
        //            { helper.SetValue("forma_name", helper.GetValue("forma_name", "", true).ToString().ToLower(), typeof(string), true); }

        //            // 4 Check authors - currently does not specify which of the authors failed

        //            if (helper.FieldExists("species_authority") && !helper.IsValueEmpty("species_authority"))
        //            {
        //                CheckAuthority(args, helper.GetValue("species_authority", "", false).ToString(), "Species authority");
        //            }

        //            if (helper.FieldExists("subspecies_authority") && !helper.IsValueEmpty("subspecies_authority"))
        //            {
        //                CheckAuthority(args, helper.GetValue("subspecies_authority", "", false).ToString(), "Subspecies authority");
        //            }

        //            if (helper.FieldExists("variety_authority") && !helper.IsValueEmpty("variety_authority"))
        //            {
        //                CheckAuthority(args, helper.GetValue("variety_authority", "", false).ToString(), "Variety authority");
        //            }

        //            if (helper.FieldExists("subvariety_authority") && !helper.IsValueEmpty("subvariety_authority"))
        //            {
        //                CheckAuthority(args, helper.GetValue("subvariety_authority", "", false).ToString(), "Subvariety authority");
        //            }

        //            if (helper.FieldExists("forma_authority") && !helper.IsValueEmpty("forma_authority"))
        //            {
        //                CheckAuthority(args, helper.GetValue("forma_authority", "", false).ToString(), "Forma authority");
        //            }


        //            // 5 move psite2 to psite1 if null
        //            /*           "    IF :NEW.psite1 IS NULL AND :NEW.psite2 IS NOT NULL THEN
        //               :NEW.psite1 := :NEW.psite2;
        //               :NEW.psite2 := NULL;
        //           END IF;
        //       "*/
        //            if ((helper.AllFieldsExist("priority2_site_id") && !helper.IsValueEmpty("priority2_site_id")) && (helper.AllFieldsExist("priority1_site_id") && helper.IsValueEmpty("priority1_site_id")))
        //            {
        //                helper.SetValue("priority1_site_id", helper.GetValue("priority2_site_id", 0, true), typeof(int), true);
        //                helper.SetValue("priority2_site_id", DBNull.Value, typeof(int), false);
        //            }
        //            /*
        //                            -- set taxon field if not already done
        //            --
        //                IF :NEW.taxon IS NULL THEN
        //                    :NEW.taxon := t_genus||t_shybrid||' '||:NEW.species;
        //                    :NEW.taxauthor := :NEW.sauthor;
        //                    IF :NEW.forma IS NOT NULL THEN
        //                        :NEW.taxon := RTRIM(:NEW.taxon)||' '||:NEW.forma;
        //                        :NEW.taxauthor := :NEW.fauthor;
        //                    ELSIF :NEW.subvar IS NOT NULL THEN
        //                        :NEW.taxon := RTRIM(:NEW.taxon)||' subvar. '||:NEW.subvar;
        //                        :NEW.taxauthor := :NEW.svauthor;
        //                    ELSIF :NEW.var IS NOT NULL THEN
        //                        IF :NEW.varhybrid IS NOT NULL THEN
        //                            :NEW.taxon := RTRIM(:NEW.taxon)||' nothovar. '||:NEW.var;
        //                        ELSE
        //                            :NEW.taxon := RTRIM(:NEW.taxon)||' var. '||:NEW.var;
        //                        END IF;
        //                        :NEW.taxauthor := :NEW.varauthor;
        //                    ELSIF :NEW.subsp IS NOT NULL THEN
        //                        IF :NEW.ssphybrid IS NOT NULL THEN
        //                            :NEW.taxon := RTRIM(:NEW.taxon)||' nothosubsp. '||:NEW.subsp;
        //                        ELSE
        //                            :NEW.taxon := RTRIM(:NEW.taxon)||' subsp. '||:NEW.subsp;
        //                        END IF;
        //                        :NEW.taxauthor := :NEW.sspauthor;
        //                    END IF;
        //                END IF;
        //                             */



        //            // "name" field is the calculated name for this record.  It cannot be overridden, unlike in GC where it could.
        //            // it should therefore be marked as read only in the table mappings!
        //            //if (nonEmptyValue(helper, "name")){

        //            // TODO: move this loop into a method defined on the FieldMappingDataView interface
        //            int? genusID = (int?)(args.Helper.GetValue("taxonomy_genus_id", null, true));

        //            // fill in all the values up front so the logic below them is clearer
        //            var dt = args.ReadData(@"select genus_name,hybrid_code from taxonomy_genus where taxonomy_genus_id = :id", ":id", genusID, DbType.Int32);

        //            var genusName = "";
        //            if (dt.Rows.Count > 0)
        //            {
        //                genusName = dt.Rows[0]["genus_name"].ToString();
        //            }

        //            if (dt.Rows[0]["hybrid_code"].ToString() == "+") { genusName = "+" + genusName; }
        //            if (dt.Rows[0]["hybrid_code"].ToString() == "X") { genusName = "X " + genusName; }


        //            if (String.IsNullOrEmpty(genusName))
        //            {
        //                // we weren't given the primary key.  Try just pulling it from the row itself.
        //                genusName = args.Helper.GetValue("genus_name", null, true, true) as string;
        //            }

        //            var newForma = helper.GetValue("forma_name", null, false);
        //            var newFormaRankType = helper.GetValue("forma_rank_type", null, false);
        //            var newFormaAuthor = helper.GetValue("forma_authority", null, false);
        //            var newSubvariety = helper.GetValue("subvariety_name", null, false);
        //            var newSubvarietyAuthor = helper.GetValue("subvariety_authority", null, false);
        //            var newVariety = helper.GetValue("variety_name", null, false);
        //            var newVarietyAuthor = helper.GetValue("variety_authority", null, false);
        //            var newSubspecies = helper.GetValue("subspecies_name", null, false);
        //            var newSubspeciesAuthor = helper.GetValue("subspecies_authority", null, false);


        //            var newName = genusName + (helper.GetValue("is_specific_hybrid", "N", true).ToString().ToUpper() == "Y" ? " x " : " ") + helper.GetValue("species_name", "", true);
        //            var newAuthor = helper.GetValue("species_authority", null, false);

        //            if (!helper.IsValueEmpty("forma_name"))
        //            {
        //                newName = newName.TrimEnd() + " " + newFormaRankType + " " + newForma;
        //                newAuthor = newFormaAuthor;

        //            }
        //            else if (!helper.IsValueEmpty("subvariety_name"))
        //            {
        //                newName = newName.TrimEnd() + " subvar. " + newSubvariety;
        //                newAuthor = newSubvarietyAuthor;

        //            }
        //            else if (!helper.IsValueEmpty("variety_name"))
        //            {
        //                if (helper.GetValue("is_varietal_hybrid", "N", true).ToString().ToUpper() == "Y")
        //                {
        //                    newName = newName.TrimEnd() + " nothovar. " + newVariety;
        //                }
        //                else
        //                {
        //                    newName = newName.TrimEnd() + " var. " + newVariety;
        //                }
        //                newAuthor = newVarietyAuthor;
        //            }
        //            else if (!helper.IsValueEmpty("subspecies_name"))
        //            {
        //                if (helper.GetValue("is_subspecific_hybrid", "N", false).ToString().ToUpper() == "Y")
        //                {
        //                    newName = newName.TrimEnd() + " nothosubsp. " + newSubspecies;
        //                }
        //                else
        //                {
        //                    newName = newName.TrimEnd() + " subsp. " + newSubspecies;
        //                }
        //                newAuthor = newSubspeciesAuthor;
        //            }

        //            if (newName == null || newName.Trim().Length == 0)
        //            {
        //                newName = string.Empty;
        //            }
        //            helper.SetValue("name", (String.IsNullOrEmpty(newName) ? (object)DBNull.Value : (object)newName), typeof(string), true);

        //            if (helper.FieldExists("name_authority"))
        //            {
        //                helper.SetValue("name_authority", newAuthor, typeof(string), false);
        //            }

        //        }


        //    }

        //    public void TableRowSaved(ISaveDataTriggerArgs args)
        //    {
        //        // post row update trigger
        //        if (args.SaveMode == SaveMode.Insert)
        //        {

        //            // make current_taxonomy_species_id field match the taxonomy_species_id field if needed
        //            var helper = args.Helper;

        //            // make the nomen_number match the new id if needed...
        //            if (helper.IsValueEmpty("nomen_number"))
        //            {
        //                args.WriteData("update taxonomy_species set nomen_number = " + args.NewPrimaryKeyID + " where taxonomy_species_id = " + args.NewPrimaryKeyID);
        //            }

        //            if (helper.IsValueEmpty("current_taxonomy_species_id"))
        //            {
        //                args.WriteData(@"update taxonomy_species set current_taxonomy_species_id = :id1 where taxonomy_species_id = :id2", ":id1", args.NewPrimaryKeyID, DbType.Int32, ":id2", args.NewPrimaryKeyID, DbType.Int32);
        //            }
        //        }

        //        // if synonym then check for accessions to move and annotations to create
        //        if (args.SaveMode == SaveMode.Update)
        //        {
        //            var helper = args.Helper;
        //            int taxno = (int)helper.GetValue("taxonomy_species_id", 0, true);
        //            int validtaxno = (int)helper.GetValue("current_taxonomy_species_id", 0, true);




        //            if (taxno != validtaxno)
        //            {
        //                var dt = args.ReadData(@"select coalesce(count(*),0)  as ct from accession a where a.taxonomy_species_id =:taxno;", ":taxno", taxno);
        //                var dr = dt.Rows[0];
        //                int numaccs = (int)dr["ct"];
        //                //args.Cancel("Accession = " + dr["ct"]);
        //                //return;
        //                if (numaccs > 0)
        //                {
        //                    // int modified_by = (int)helper.GetValue("modified_by", 0, true);
        //                    // look up modified_by fresh
        //                    var dt2 = args.ReadData(@"SELECT modified_by FROM taxonomy_species WHERE taxonomy_species_id = :taxno", ":taxno", taxno);
        //                    var dr2 = dt2.Rows[0];
        //                    int modified_by = (int)dr2["modified_by"];


        //                    DateTime modified_date = DateTime.UtcNow;

        //                    // add annotations
        //                    args.WriteData(@"INSERT INTO accession_inv_annotation(annotation_type_code,annotation_date,annotation_date_code,annotation_cooperator_id,inventory_id,
        //                        old_taxonomy_species_id,new_taxonomy_species_id,created_date,created_by,owned_date,owned_by)
        //                        SELECT 'NOM-CHANGE',:modified_date,'MM/dd/yyyy',:modified_by,i.inventory_id,:taxno,:validtaxno,:modified_date,:modified_by,:modified_date,:modified_by
        //                        FROM accession a INNER JOIN inventory i ON a.accession_id = i.accession_id AND i.form_type_code = '**'
        //                        WHERE a.taxonomy_species_id = :taxno",
        //                            new DataParameters(
        //                              ":taxno", taxno, DbType.Int32,
        //                              ":validtaxno", validtaxno, DbType.Int32,
        //                              ":modified_by", modified_by, DbType.Int32,
        //                              ":modified_date", modified_date, DbType.DateTime2
        //                     ));

        //                    // modify accessions
        //                    args.WriteData(@"update accession set taxonomy_species_id=:validtaxno, modified_by=:modified_by, modified_date=:modified_date where taxonomy_species_id=:taxno",
        //                    new DataParameters(
        //                       ":taxno", taxno, DbType.Int32,
        //                       ":validtaxno", validtaxno, DbType.Int32,
        //                       ":modified_by", modified_by, DbType.Int32,
        //                       ":modified_date", modified_date, DbType.DateTime2
        //                       ));

        //                }


        //            }


        //        }



        //    }


        }

    }
}
