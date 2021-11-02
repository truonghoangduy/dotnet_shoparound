using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Catergory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public string Name { set; get; }

        public int? ParrentId { set; get; }
        public Catergory Parrent { set; get; }

        public string Description { set; get; }
    }
}