using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_1920_a01
{

    public class Inscription : EntityBase<Model>
    {
        [Key]
        public int IdInscription { get; set; }
        [Required]
        public virtual Member Eleve { get; set; }
        public virtual Activite Activite { get; set; } 
        public virtual Competition Competition { get; set; }
        public virtual Team Team { get; set; }

        
        public static bool EstInscritTeam (Member eleve,Competition compet)
        {
            
             var query = from i in App.CurrentUser.Teams where  i.Team != null && i.Eleve == eleve && i.Competition == compet select i ;
             return query.Count() != 0;
            
        }

        public string getAllName
        {
            get
            {
                if(Competition != null)
                {
                    return Competition.Nom;
                } else
                {
                    return Activite.NomCours;
                }
            }
        }

        public string getType
        {
            get  {
                if(Competition != null)
                {
                    return "Compétition";
                } else
                {
                    return Activite.Type.ToString();
                }
            }
        }


        public static Inscription GetInscription (Member inscrit, Activite activite)
        {
            return (from Inscription 
                    in App.Model.Inscriptions
                    where Inscription.Eleve.Pseudo == inscrit.Pseudo 
                    && Inscription.Activite.IdActivite == activite.IdActivite select Inscription).FirstOrDefault();
        }

        public static Inscription GetInscriptionCompet(Member inscrit, Team team)
        {
            var data = (from Inscription
                    in App.Model.Inscriptions
                    where Inscription.Eleve.Pseudo == inscrit.Pseudo
                    && Inscription.Team.IdTeam == team.IdTeam
                    select Inscription).FirstOrDefault();
            return data;
        }

    }
}
