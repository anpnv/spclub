using PRBD_Framework;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace prbd_1920_a01
{

    public partial class CompetitionsView : UserControlBase
    {
        public CompetitionsView()
        {

            DataContext = this;
            getCompet();
            Edit = new RelayCommand<Competition>(c => App.NotifyColleagues(AppMessages.MSG_EDIT_DATA_COMPETITION, c));
            Delete = new RelayCommand<Competition>(c => DeleteAction(c));
            View = new RelayCommand<Competition>(c => ViewAction(c));
            Selected = new RelayCommand<Team>(t => SubscriptionAction(compet, t));
            AddTeam = new RelayCommand(() => 
            {
                newTeam = App.Model.Teams.Create();
                AddTeamPressed = true; 
            });

            CreateTheTeam = new RelayCommand(() =>
             {
                 compet.AddTeam(newTeam);
                 teams = new ObservableCollection<Team>(compet.Team);
                 RaisePropertyChanged(nameof(Teams));
                 Refresh();
                 AddTeamPressed = false;
                 newTeam = null;
                 nomTeam = "";
                 RaisePropertyChanged(nameof(AddTeamPressed));
                 RaisePropertyChanged(nameof(Teams));
                 RaisePropertyChanged(nameof(newTeam));
                 RaisePropertyChanged(nameof(nomTeam));

             });
            InitializeComponent();
        }


        public ICommand Edit { get; set; }
        public ICommand Delete { get; set; }
        public ICommand View { get; set; }
        public ICommand Selected { get; set; }
        public ICommand AddTeam { get; set; }
        public ICommand CreateTheTeam { get; set; }

        private Competition compet;
        private Team newTeam;

        public bool ViewPressed { get => compet != null; }
        
        

        private bool addTeamPressed;
        public bool AddTeamPressed
        { 
            get => addTeamPressed;
            set => SetProperty(ref addTeamPressed, value);
        }

        private void ViewAction(Competition c)
        {
            compet = c;
            nom = c.Nom;
            horaire = c.Horaire.ToString();
            teams = new ObservableCollection<Team>(c.Team);
            RaisePropertyChanged(nameof(Nom));
            RaisePropertyChanged(nameof(Horaire));
            RaisePropertyChanged(nameof(ViewPressed));
            RaisePropertyChanged(nameof(teams));
        }
        
        private void SubscriptionAction(Competition compet, Team team)
        {
            App.CurrentUser.SubUnsubTeam(compet, team);
            teams = new ObservableCollection<Team>(compet.Team);
            RaisePropertyChanged(nameof(Teams));
        }


        private string nom;
        public string Nom
        {
            get => nom;
            set => SetProperty(ref nom, value);
        }

        private string nomTeam;
        public string NomTeam
        {
            get => nomTeam;
            set {
                newTeam.Nom = value;
                SetProperty(ref nomTeam, value); 
            }
        }


        private string horaire;
        public string Horaire
        {
            get => "Le " + String.Format("{0:dd / MM / yyyy HH:mm}", horaire);
            set => SetProperty(ref horaire, value);
        }

        private ObservableCollection<Team> teams;
        public ObservableCollection<Team> Teams
        {
            get => teams;
            set => SetProperty(ref teams, value);
        }

        private ObservableCollection<Competition> competitions;
        public ObservableCollection<Competition> Competitions { 
            get => competitions; 
            set => SetProperty(ref competitions, value); 
        }

        private Team selectedTeam;
        public Team SelectedTeam
        {
            get => selectedTeam;
            set => SetProperty(ref selectedTeam, value);
        }

        private Competition selectedCompetitions;
        public Competition SelectedCompetitions 
        {
            get => selectedCompetitions; 
            set => SetProperty<Competition>(ref selectedCompetitions, value); 
        }

        private bool teamIsSelected;
        public bool TeamIsSelected
        {
            get => teamIsSelected;
            set => SetProperty(ref teamIsSelected, value);
        }



        private void getCompet()
        {
            var compet = (from c in App.Model.Competitions
                          where c.Horaire >= DateTime.Now
                          where c.Team.Where(m => m.EstGagnant).Count() < 1
                          select c).OrderBy(a => a.Horaire);

            Competitions = new ObservableCollection<Competition>(compet);
        }

        private void Refresh()
        {
            Competitions = new ObservableCollection<Competition>(App.Model.Competitions);
        }

        private void DeleteAction(Competition c)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Etes-vous sûre de vouloir supprimer la compétition " + c.Nom + " ? ", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                c.Delete();
                Refresh();
            }
        }


    }
}
