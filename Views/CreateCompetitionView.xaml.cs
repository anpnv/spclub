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
    /// <summary>
    /// Interaction logic for CreateCompetitionView.xaml
    /// </summary>
    public partial class CreateCompetitionView : UserControlBase
    {

        public ICommand Cancel { get; set; }
        public ICommand Confirm { get; set; }
        public Competition Competition { get; set; }

        public CreateCompetitionView(Competition competition)
        {   
            DataContext = this;
            Competition = competition;

            Salles = new ObservableCollection<Salle>(App.Model.Salles);
            Cancel = new RelayCommand(CancelAction);
            Confirm = new RelayCommand(ConfirmAction);

            InitializeComponent();
        }


        private string nom;
        public string Nom
        {
            get => nom;
            set
            {
                Competition.Nom = value;
                SetProperty(ref nom, value);
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
                Competition.Salle = value;
                SetProperty(ref salleselected, value);
            }
        }

        private int heures;
        public int Heures
        {
            get => heures;
            set
            {

                SetProperty(ref heures, value);
            }
        }

        private int minutes;
        public int Minutes
        {
            get => minutes;
            set
            {

                SetProperty(ref minutes, value);
            }
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
            }
        }



        private int lotGagnant;
        public int LotGagnant
        {
            get => lotGagnant;
            set
            {
                Competition.LotGagnant = value;
                SetProperty(ref lotGagnant, value);
            }
        }

        private void CancelAction()
        {
            Competition = null;
            RaisePropertyChanged(nameof(Competition));
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);

        }

        private void ConfirmAction()
        {
            CreateCompetition();
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
            App.NotifyColleagues(AppMessages.MSG_REFRESH_DATA_USER);

        }

        private void CreateCompetition()
        {
            Competition.Horaire = Date.AddHours(heures).AddMinutes(minutes);
            App.Model.Competitions.Add(Competition);
            App.Model.SaveChanges(); 
        }
    }
}
