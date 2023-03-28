using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_web.Models
{ 
    public class VillaNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VillaNo { get; set; }
        //Here ForeignKey("Villa") "Villa" is the name of the navigation property, which is called foreign key mapper
        [ForeignKey("Villa")]  //when we add foreign key, we need to add a navigation property for the VillaId
        public int VillaID { get; set; }

        public Villa Villa { get; set; }  //navigation property

        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
