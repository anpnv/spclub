using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace prbd_1920_a01
{
    public partial class MatchDetailView : UserControlBase
    {
        public Team team;



        public ICommand SetTime { get; set; }
        public ICommand ConfirmScore { get; set; }


        public MatchDetailView()
        {
            DataContext = this;
            SetTime = new RelayCommand<Team>((a) =>
            {
                team = a;
                a.SetResultat(team.Resultat);
                
            });

            ConfirmScore = new RelayCommand(confirmScoreAction, () =>
           {
               if(selectedCompetition != null)
               {
                   ForceRefresh();
                   return selectedCompetition.CompetFini();
               } 
               return false;
           });
            getCompetition();
            InitializeComponent();
        }

        private void confirmScoreAction()
        {
            if (selectedCompetition.CompetFini())
            {
                selectedCompetition.setGagnant();
                var team = SelectedCompetition.Team.Where(t => t.EstGagnant).First();
                teamGagnante = team.Nom + " a Gagner le prix de : " + SelectedCompetition.LotGagnant + " € " ;
                Console.WriteLine(TeamGagnante);
                ForceRefresh();
            }
        }

        public void ForceRefresh()
        {
            RaisePropertyChanged(nameof(TeamGagnante));
        }

        private string teamGagnante;
        public string TeamGagnante
        {
            get => teamGagnante;
            set => SetProperty(ref teamGagnante, value);
        }


        public bool hasCompet
        {
            get => SelectedCompetition != null;
        }

        private Team selectedTeam;
        public Team SelectedTeam
        {
            get => selectedTeam;
            set {
                SetProperty(ref selectedTeam, value);
                
            }
        }

        private Competition selectedCompetition;
        public Competition SelectedCompetition
        {
            get => selectedCompetition;
            set {
                SetProperty(ref selectedCompetition, value);
                nom = selectedCompetition.Nom;
                horaire = selectedCompetition.Horaire.ToString();
                lotGagnant = " 1e place : " + selectedCompetition.LotGagnant + " €";
                listeTeam = new ObservableCollection<Team>(selectedCompetition.Team);
                RaisePropertyChanged(nameof(Nom));
                RaisePropertyChanged(nameof(Horaire));
                RaisePropertyChanged(nameof(LotGagnant));
                RaisePropertyChanged(nameof(hasCompet));
                RaisePropertyChanged(nameof(listeTeam));
            }
        }




        private string nom;
        public string Nom
        {
            get => nom;
            set => SetProperty(ref nom, value);
        }

        private string horaire;
        public string Horaire
        {
            get => "Le " +String.Format("{0:dd / MM / yyyy HH:mm}", horaire);
            set => SetProperty(ref horaire, value);
        }

        private string lotGagnant;
        public string LotGagnant
        {
            get => lotGagnant;
            set => SetProperty(ref lotGagnant, value);
        }


        private ObservableCollection<Team> listeTeam;
        public ObservableCollection<Team> ListeTeam
        {
            get => listeTeam;
            set => SetProperty(ref listeTeam, value);
        }

        private double resultat;
        public double Resultat
        {
            get
            {
                return resultat;
            }
            set {
                team.Resultat = resultat;
                SetProperty(ref resultat, value); 
                
            }
        }


        private ObservableCollection<Competition> competition;
        public ObservableCollection<Competition> Competition
        {
            get => competition;
            set => SetProperty(ref competition, value);
        }

        private void getCompetition()
        {
            var compet = (from c in App.Model.Competitions
                          where c.Horaire >= DateTime.Now
                          where c.Team.Count() == c.Team.Where(t => t.Teams.Count()  == 3 ).Count()
                          where c.Team.Count() > 0
                          select c).OrderBy(a => a.Horaire);
            
            Competition = new ObservableCollection<Competition>(compet);
        }



    }
}
