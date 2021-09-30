using MaterialDesignThemes.Wpf;
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
    public partial class CreateActivityView : UserControlBase
    {

        public CreateActivityView(Activite activite)
        {
            DataContext = this;
            Activite = activite;
            Cancel = new RelayCommand(CancelAction);
            Confirm = new RelayCommand(ConfirmAction, () => { return Validate(); });


            Types = new ObservableCollection<Type>(Enum.GetValues(typeof(Type)).Cast<Type>());

            InitializeComponent();
        }

        public bool IsAv
        {
            get
            {

                if (Date.Year == DateTime.Now.Year)
                {
                    if (Date.Month == DateTime.Now.Month)
                    {
                        if (Date.Day == DateTime.Now.Day)
                        {

                            return false;
                        }
                    }
                }
                return true;

            }
        }


        public ICommand Cancel { get; set; }
        public ICommand Confirm { get; set; }
        public Activite Activite { get; set; }


        private string nom;
        public string Nom
        {
            get => nom;
            set
            {
                Activite.NomCours = value;
                SetProperty(ref nom, value, () => Validate());
            }
        }

        private int nbParticipant;
        public int NbParticipant
        {
            get => nbParticipant;
            set
            {
                Activite.MaxParticipant = value;
                SetProperty(ref nbParticipant, value);
            }
        }

        private int heures;
        public int Heures
        {
            get => heures;
            set => SetProperty(ref heures, value, () => Validate());
        }

        private int minutes;
        public int Minutes
        {
            get => minutes;
            set => SetProperty(ref minutes, value, () => Validate());

        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                if (date == DateTime.MinValue)
                    return DateTime.Now;
                return date;
            }
            set
            {

                SetProperty<DateTime>(ref date, value);

                RaisePropertyChanged(nameof(Date));
                RaisePropertyChanged(nameof(IsAv));
                getTeacher();
                getSalle();
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
            get => typeselected;
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
            get => salleselected;
            set
            {
                Activite.Lieux = value;
                SetProperty(ref salleselected, value);
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
            get => professeurSelected;
            set
            {
                Activite.Professeur = value;
                SetProperty(ref professeurSelected, value);
            }
        }

        private void getTeacher()
        {
            var teacherOccuper = (from q in App.Model.Activites
                                  where q.Horaire.Year == Date.Year
                                  where q.Horaire.Month == Date.Month
                                  where q.Horaire.Day == Date.Day
                                  select q.Professeur).FirstOrDefault();
            IEnumerable<Member> teacher = App.Model.Members;

            teacher = from t in App.Model.Members
                      where t.Role == Role.Teacher
                      select t;
            var teacherDispo = teacher.Where(a => a != teacherOccuper);

            Professeurs = new ObservableCollection<Member>(teacherDispo);
        }

        private void getSalle()
        {
            var salleOccuper = (from q in App.Model.Activites
                                where q.Horaire.Year == Date.Year
                                where q.Horaire.Month == Date.Month
                                where q.Horaire.Day == Date.Day
                                select q.Lieux).FirstOrDefault();
            IEnumerable<Salle> hall = App.Model.Salles;

            var hallDispo = hall.Where(a => a != salleOccuper);
            Salles = new ObservableCollection<Salle>(hallDispo);
        }

        private void CancelAction()
        {
            Activite = null;
            RaisePropertyChanged(nameof(Activite));
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
        }

        private void ConfirmAction()
        {
            Activite.Horaire = Date.AddHours(heures).AddMinutes(minutes);
            Activite.CreateActivite(professeurSelected);
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);

        }


        public override bool Validate()
        {

            ClearErrors();

            var nomSalle = App.Model.Activites.Where(a => a.NomCours == Nom).SingleOrDefault();
            if (string.IsNullOrEmpty(Nom))
                AddError("Nom", Properties.Resources.Error_Required);
            else
                if (nomSalle != null)
                AddError("Nom", Properties.Resources.Error_NotAvailable);
            if (Heures > 23)
                AddError("Heures", Properties.Resources.Error_NotAvailable);
            if (Heures < 0)
                AddError("Heures", Properties.Resources.Error_NotAvailable);
            if (Minutes > 59)
                AddError("Minutes", Properties.Resources.Error_NotAvailable);
            if (Minutes < 0)
                AddError("Minutes", Properties.Resources.Error_NotAvailable);
            
            return !HasErrors;
        }

    }
}
