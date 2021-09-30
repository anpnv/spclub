using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_1920_a01
{

    public enum Type { Cours, Evenement }

    public class Activite : EntityBase<Model>
    {
        [Key]
        public int IdActivite { get; set; }
        public string NomCours { get; set; }
        public DateTime Horaire { get; set; }
        public int MaxParticipant { get; set; }
        public Type Type { get; set; }    
        public virtual Salle Lieux { get; set; }
        public virtual Member Professeur { get; set; }
        [NotMapped]
        public bool ActiviteIsSelected
        {
            get
            { 
                var query = from i in App.CurrentUser.Eleves where i.Activite.IdActivite == this.IdActivite select i;
                return query.Count() != 0;
            }
            private set { }
        }

        [NotMapped]
        public string NbParticipant
        {
            get => Activites.Count() + " / " + MaxParticipant;    
        }


        public int Prix { get; set; } = 10;

        public virtual ICollection<Inscription> Activites { get; set; } = new HashSet<Inscription>();

        public bool IsRegistered(string PseudoMember)
        {
            return App.Model.Inscriptions.Find(PseudoMember) == null ? false : true;
        }




        public override string ToString()
        {
            return "[" + Type.ToString().ToUpper()[0] + "] " + NomCours 
                + Horaire.Day.ToString("00") + "/" + Horaire.Month.ToString("00") 
                + " - " + Horaire.Hour.ToString() + ":" + Horaire.Minute.ToString();
        }

        public void Delete()
        {
            Model.Activites.Remove(this);
            Model.SaveChanges();

        }

        public void CreateActivite(Member professeur)
        {
            professeur.Professeurs.Add(this);
            App.Model.Activites.Add(this);
            App.Model.SaveChanges();

        }
    }
}
