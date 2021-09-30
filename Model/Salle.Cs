using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_1920_a01
{
    public class Salle : EntityBase<Model>
    {

        [Key]
        public int IdSalle { get; set; }
        public string NomSalle { get; set; }
        public string Adresse { get; set; }


        protected Salle() { }

        public override string ToString()
        {
            return NomSalle;
        }
    }
}
