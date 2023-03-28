using System.ComponentModel.DataAnnotations;

namespace MagicVilla_web.Models.Dto 
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set; }
        //update, create all three exactly the same , here is a question as they are same why not we use 
        //VillaNumberDTO?, because in feuture changes may happen where create may different from update

    }
}
