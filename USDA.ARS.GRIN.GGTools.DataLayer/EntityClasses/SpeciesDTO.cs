

namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class SpeciesDTO
    {
        public int ID { get; set; }
        public int GenusID { get; set; }
        public string GenusName { get; set; }
        public string SpeciesName { get; set; }
        public string SpeciesAuthority { get; set; }
        public string Protologue { get; set; }
        public string ProtologueVirtualPath { get; set; }
        public string Note { get; set; }
    }
}
