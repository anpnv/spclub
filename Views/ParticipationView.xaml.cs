using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prbd_1920_a01
{
    public partial class ParticipationView : UserControlBase
    {

        public Member Member { get; set; }
        public ICommand Cancel { get; set; } 
        public ICommand ActiviteBtn { get; set; }
        public ICommand CompetBtn { get; set; }

        private Activite selectedActivite;
        public Activite SelectedActivite
        {
            get => selectedActivite;
            set => SetProperty(ref selectedActivite, value);
        }
        private Competition selectedCompetition;
        public Competition SelectedCompetition
        {
            get => selectedCompetition;
            set => SetProperty(ref selectedCompetition, value);
        }


        private ObservableCollection<Inscription> competitions;
        public ObservableCollection<Inscription> Competitions
        {
            get => competitions;
            set => SetProperty(ref competitions, value);
        }


        public ObservableCollection<Inscription> activites;
        public ObservableCollection<Inscription> Activites
        {
            get => activites;
            set => SetProperty(ref activites, value);
        }

        public ObservableCollection<Activite> donneCours;
        public ObservableCollection<Activite> DonneCours
        {
            get => donneCours;
            set => SetProperty(ref donneCours, value);
        }

        private bool activiteIsSelected;
        public bool ActiviteIsSelected
        {
            get => activiteIsSelected;
            set => SetProperty(ref activiteIsSelected, value);
        }

        private bool competIsSelected;
        public bool CompetIsSelected
        {
            get => competIsSelected;
            set => SetProperty(ref competIsSelected, value);
        }

        public ParticipationView(Member member)
        {
            DataContext = this;
            Member = member;

            var listActivite = (from e in Member.Eleves where e.Activite != null where e.Activite.Horaire >= DateTime.Now select e).OrderBy(a => a.Activite.Horaire);
            var listCompet = (from e in Member.Teams where e.Competition != null where e.Competition.Horaire >= DateTime.Now select e).OrderBy(a => a.Competition.Horaire);
            var listeCours = (from e in Member.Professeurs where e.Activites != null select e).OrderBy(a => a.Horaire);

            if (member.CanEdit)
            {
               
                DonneCours = new ObservableCollection<Activite>(listeCours);
                
            } else
            {  
                Activites = new ObservableCollection<Inscription>(listActivite); 
                Competitions = new ObservableCollection<Inscription>(listCompet);
            }
            
            
            ActiviteBtn = new RelayCommand<Inscription>(act =>
            {
                SubscriptionAction(act.Activite, null);
                App.NotifyColleagues(AppMessages.MSG_REFRESH_DATA_USER);
                Activites = new ObservableCollection<Inscription>(listActivite);
                

            });
            CompetBtn = new RelayCommand<Inscription>(compet =>
            {
                SubscriptionAction(null, compet.Competition);
                App.NotifyColleagues(AppMessages.MSG_REFRESH_DATA_USER);
                Competitions = new ObservableCollection<Inscription>(listCompet);
            });
            Cancel = new RelayCommand(CancelAction);
           
            InitializeComponent();
        }

        

       

        private void SubscriptionAction(Activite activite, Competition competition)
        {
            var query = from i in App.CurrentUser.Eleves where i.Activite == activite  select i;
            var query2 = from i in App.CurrentUser.Teams where i.Competition == competition select i;
            if (query.Count() == 0 && query2.Count() ==0)
            {
                var newSub = App.CurrentUser.Subscribe(activite, competition);
                App.CurrentUser.Sub(newSub);
            }
            else
            {
                var subs = query.First();
                App.CurrentUser.UnSub(subs);
            }
        }

        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_STUDENT_VIEW);
        }


        

    }
}
