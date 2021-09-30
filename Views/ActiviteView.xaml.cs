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
using System.Windows.Forms;

namespace prbd_1920_a01
{

    public partial class ActiviteView : UserControlBase
    {

        public ActiviteView()
        {
            DataContext = this;
            Activites = new ObservableCollection<Activite>(App.Model.Activites.Where(a => a.Horaire >= DateTime.Now).OrderBy(a => a.Horaire));
            Delete = new RelayCommand<Activite>(act => DeleteAction(act));
            Edit = new RelayCommand<Activite>((act) => App.NotifyColleagues(AppMessages.MSG_EDIT_DATA_ACTIVITE, act));
            Selected = new RelayCommand<Activite>(act => SubAction(act));
            Kick = new RelayCommand<Inscription>(m => KickEleve(m.Eleve));
            View = new RelayCommand<Activite>(act => ViewAction(act));
            InitializeComponent();

        }


        public ICommand Selected { get; set; }
        public ICommand Edit { get; set; }
        public ICommand Delete { get; set; }
        public ICommand View { get; set; }
        public ICommand Kick { get; set; }

        private Activite activ;

        public bool CanRead { get => App.CurrentUser.Role != Role.Member; }
        public bool ViewPressed { get => activ != null; }
        public bool IsFULL { get => true; }

      

        private Activite selectedActivite;
        public Activite SelectedActivite
        {
            get => selectedActivite;
            set => SetProperty(ref selectedActivite, value);
        }

        private Member selectedParticipant;
        public Member SelectedParticipant
        {
            get => selectedParticipant;
            set => SetProperty(ref selectedParticipant, value);
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
            get => "Le " + String.Format("{0:dd / MM / yyyy HH:mm}", horaire);
            set => SetProperty(ref horaire, value);
        }

        private ObservableCollection<Activite> activites;
        public ObservableCollection<Activite> Activites
        {
            get => activites;
            set => SetProperty(ref activites, value);
        }

        private ObservableCollection<Inscription> participants;
        public ObservableCollection<Inscription> Participants
        {
            get => participants;
            set => SetProperty(ref participants, value);
        }


        private void Refresh()
        {
            Activites = new ObservableCollection<Activite>(App.Model.Activites.Where(a => a.Horaire >= DateTime.Now).OrderBy(a => a.Horaire));
        }

        private void DeleteAction(Activite a)
        {
            if (a.Type == Type.Cours)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Etes-vous sûre de vouloir supprimer le cours " + a.NomCours + " ? ", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    a.Delete();
                    Refresh();
                }
            }
            else
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Etes-vous sûre de vouloir supprimer l'evenement " + a.NomCours + " ? ", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                if (result == MessageBoxResult.Yes)
                {
                    a.Delete();
                    Refresh();
                }
            }
        }

        private void SubAction(Activite activite)
        {
            if (activite.MaxParticipant > activite.Activites.Count())
            {
                SubscriptionAction(activite);
                App.NotifyColleagues(AppMessages.MSG_REFRESH_DATA_USER);
            }
            else
            {
                if(App.CurrentUser.Eleves.Where(c => c.Activite == activite).Count() != 0)
                {
                    SubscriptionAction(activite);
                } else
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Malheureusement le cours " + activite.NomCours + " est complet" + "\n"
                                                                        + "Voir les cours non complet? ", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning,
                                                                        MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                        Activites = new ObservableCollection<Activite>(App.Model.Activites.Where(u => u.Activites.Count() < u.MaxParticipant));
                    else
                        Refresh();
                }
            }
        }

        private void SubscriptionAction(Activite activite)
        {
            var query = from i in App.CurrentUser.Eleves where i.Activite == activite select i;
            if (query.Count() == 0)
            {
                if (App.CurrentUser.Solde == 0 || App.CurrentUser.Solde < activite.Prix)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Votre solde est insuffisant ! \n Voulez-vous le recharger ?", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                    if (result == MessageBoxResult.Yes)
                    {
                        App.NotifyColleagues(AppMessages.MSG_MY_SOLDE);
                    }
                }
                else
                {
                    var newSub = App.CurrentUser.Subscribe(activite, null);
                    App.CurrentUser.Sub(newSub);
                }

            }
            else
            {
                var subs = query.First();
                App.CurrentUser.UnSub(subs);

            }
            Refresh();
        }

        private void KickEleve(Member m)
        {
            var kickIt = (from a in activ.Activites where a.Eleve == m select a).First();
            Console.WriteLine("Remboursement de l'élève : " +kickIt.Eleve.Pseudo);
            kickIt.Eleve.addSolde(10);
            activ.Activites.Remove(kickIt);
            App.Model.SaveChanges();
            participants = new ObservableCollection<Inscription>(activ.Activites.OrderBy(p => p.Eleve.Birthdate));
            RaisePropertyChanged(nameof(Participants));
            Refresh();
        }

        private void ViewAction(Activite a)
        {
            activ = a;
            nom = a.NomCours;
            horaire = a.Horaire.ToString();
            participants = new ObservableCollection<Inscription>(a.Activites.OrderBy(p => p.Eleve.Birthdate));
            RaisePropertyChanged(nameof(Nom));
            RaisePropertyChanged(nameof(Horaire));
            RaisePropertyChanged(nameof(ViewPressed));
            RaisePropertyChanged(nameof(participants));
        }

        private void setComponent()
        {
            
        }

    }
}
