using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetFormApp.Models
{
    public class FormData
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string Address { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Image { get; set; }
        public FormData()
        {
            if(Name ==null)
            {
                Name = "";
            }
             if(Email  ==null)
            {
                Email  = "";
            }
             if(Address ==null)
            {
                Address = "";
            }
            if(Image== null)
            {
                Image = new byte[0];
                
            }
            

        }
    }
}
