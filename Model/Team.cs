using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Common.EntitySql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_1920_a01
{
    public class Team : EntityBase<Model>
    {
        [Key]
        public int IdTeam { get; set; }

        public int IdCompetition { get; set; }
        public string Nom { get; set; }
        public Member[] Participant { get; set; }
        public bool EstGagnant { get; set; } = false;
        public int TAILLE_EQUIPE { get; } = 3;
        public double Resultat { get; set; }

        [NotMapped]
        public bool TeamIsSelected
        {
            get
            {
                
                 var query = from i in App.CurrentUser.Teams where i.Team.IdTeam == this.IdTeam  select i;
                return query.Count() != 0;
            }
            private set { }
        }

        public virtual ICollection<Inscription> Teams { get; set; } = new HashSet<Inscription>();
        public virtual ICollection<Member> Participants { get; set; } = new HashSet<Member>();

        protected Team() { }



        public override string ToString()
        {
            return Nom;
        }

      

        public void SetResultat(double res)
        {
            this.Resultat = res;
            Model.SaveChanges();
            Console.WriteLine("resultat changé en : " + res);
        }

        [NotMapped]
        public string NbParticipant
        {
            get => Teams.Count() + " / " + TAILLE_EQUIPE;
        }
        [NotMapped]
        public bool EstFull
        {
            get => TAILLE_EQUIPE == Teams.Count();
        }

        public void Delete()
        {
            Model.Teams.Remove(this);
            Model.SaveChanges();
        }

        public void Winner()
        {
            EstGagnant = true;
        }

        public void Looser()
        {
            EstGagnant = false;
        }

    }
}
