namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {

        public int Id { get; set; }
        public string Name { get; set; }
        //when we expose that to the end user of our controller, 
        //we do not want to send that date
        public DateTime CreatedDate { get; set; }

    }
}
