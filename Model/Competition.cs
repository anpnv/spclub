using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_1920_a01
{
    public class Competition : EntityBase<Model>
    {
        [Key]
        public int IdCompet { get; set; }
        public string Nom { get; set; }
        public DateTime Horaire { get; set; }
        public int LotGagnant { get; set; }
        public virtual Salle Salle { get; set; }

        public int Prix { get; set; } = 5;
        public int SelectedIndex { get; internal set; }

        public virtual ICollection<Inscription> Competitions { get; set; } = new HashSet<Inscription>();
        public virtual ICollection<Team> Team { get; set; } = new HashSet<Team>();



        [NotMapped]
        public bool CompetIsSelected
        {
            get
            {
                var query = from i in App.CurrentUser.Eleves where i.Competition.IdCompet == this.IdCompet select i;
                return query.Count() != 0;
            }
            private set { }
        }

        [NotMapped]
        public bool CompetIsFull
        {
            get => Team.Count() == Team.Where(t => t.Teams.Count() == t.TAILLE_EQUIPE).Count();
        }

        [NotMapped]
        public int NbTeam
        {
            get => Team.Count();
        }

         [NotMapped]
         public String TeamWinner
         {
          //get => Team.Where(t => t.EstGagnant).First().Nom;
          get => Team.Where(t => t.EstGagnant).ToString();

        }

        public void setGagnant()
        {
            ResetWinner();
            double min = Team.Min(t => t.Resultat);
            Team best = Team.Where(t => t.Resultat == min).First();
            best.Winner();
            addPrix(best);
            Model.SaveChanges();
        }


        private void addPrix(Team winner)
        {
            int tierLot = LotGagnant / 3;
            foreach (var t in winner.Teams)
                t.Eleve.addSolde(tierLot);
            
        }

        private void removeSolde(Team toRemove)
        {
            int tierLot = LotGagnant / 3;
            foreach (var t in toRemove.Teams)
                t.Eleve.removeSolde(tierLot);
            
        }

        private void ResetWinner()
        {
            foreach ( Team t in Team)
            {
                if (t.EstGagnant)
                {
                    removeSolde(t);
                    t.Looser();
                } 
            }
        }


        public bool CompetFini()
        {
            return Team.Where(t => t.Resultat > 0).Count() == Team.Count();
        }

        protected Competition() { }

        

        public override string ToString()
        {
            return Nom;
        }

        internal void Delete()
        {
            Model.Competitions.Remove(this);
            Model.SaveChanges();
        }

        public void AddTeam(Team team)
        {
            Team.Add(team);
            Model.SaveChanges();
        }

        public void DeleteTeam(Team team)
        {
            Team.Remove(team);
            Model.SaveChanges();
        }

        
    }
}
