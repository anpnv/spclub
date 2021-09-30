using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_a01
{
    /// <summary>
    /// Interaction logic for EditActiviteView.xaml
    /// </summary>
    public partial class EditActiviteView : UserControlBase
    {
        public Activite Activite { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }



        public EditActiviteView(Activite activite)
        {
            Activite = activite;

            Types = new ObservableCollection<Type>(Enum.GetValues(typeof(Type)).Cast<Type>()); 
            Salles = new ObservableCollection<Salle>(App.Model.Salles);
            getTeacher();

            Cancel = new RelayCommand(CancelAction);
            Save = new RelayCommand(SaveAction);
            
            InitializeComponent();
            DataContext = this;
        }

        private void SaveAction()
        {
            App.Model.SaveChanges();
            RaisePropertyChanged(nameof(Activite));
            App.NotifyColleagues(AppMessages.MSG_ACTIVITE_VIEW); 
        }

        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_ACTIVITE_VIEW); 
        }

        private string nom;
        public string Nom
        {
            get => Activite.NomCours;
            set
            {
                if (value == Activite.NomCours)
                    return;
                Activite.NomCours = value;
                SetProperty(ref nom, value, () => Validate());

            }
        }


        private ObservableCollection<Type> types;
        public ObservableCollection<Type> Types
        {
            get => types;
            set => SetProperty(ref types, value);
        }

        private Type typeselected;
        public Type TypeSelected
        {
            get => Activite.Type;
            set
            {
                Activite.Type = value;
                SetProperty(ref typeselected, value);
            }
        }


        private ObservableCollection<Salle> salles;
        public ObservableCollection<Salle> Salles
        {
            get => salles;
            set => SetProperty(ref salles, value);
        }

        private Salle salleselected;
        public Salle SalleSelected
        {
            get => Activite.Lieux;
            set
            {
                Activite.Lieux = value;
                SetProperty(ref salleselected, value);
            }
        }


        private DateTime date;
        public DateTime Date
        {
            get => Activite.Horaire;
            set
            {
                if (value == Activite.Horaire)
                    return;
                Activite.Horaire = value;
                SetProperty(ref date, value, () => Validate());

            }

        }

        private int heures;
        public int Heures
        {
            get => Activite.Horaire.Hour;
            set
            {
                SetProperty(ref heures, value, () => Validate());

            }

        }

        private int minutes;
        public int Minutes
        {
            get => Activite.Horaire.Minute;
            set
            {
                SetProperty(ref minutes, value);
            }
        }


        private ObservableCollection<Member> professeurs;
        public ObservableCollection<Member> Professeurs
        {
            get => professeurs;
            set => SetProperty(ref professeurs, value);
        }

        private Member professeurSelected;
        public Member ProfesseurSelected
        {
            get => Activite.Professeur;
            set
            {
                Activite.Professeur = value;
                SetProperty(ref professeurSelected, value);
            }
        }


        private int nbParticipant;
        public int NbParticipant
        {
            get => Activite.MaxParticipant;
            set
            {
                if (value == Activite.MaxParticipant)
                    return;
                Activite.MaxParticipant = value;
                SetProperty(ref nbParticipant, value, () => Validate());

            }

        }

        private void getTeacher()
        {
            IEnumerable<Member> teacher = App.Model.Members;

            teacher = from t in App.Model.Members
                      where t.Role == Role.Teacher
                      select t;
            Professeurs = new ObservableCollection<Member>(teacher);
        }

    }
}
