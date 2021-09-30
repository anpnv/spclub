using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_a01 { 
    /// <summary>
    /// Interaction logic for EditCompetitionView.xaml
    /// </summary>
    public partial class EditCompetitionView : UserControlBase
{
        private Competition Competition;
        public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }


        public EditCompetitionView(Competition competition)
        {
            DataContext = this;
            Competition = competition;

            Salles = new ObservableCollection<Salle>(App.Model.Salles);

            Cancel = new RelayCommand(CancelAction);
            Save = new RelayCommand(SaveAction);

            InitializeComponent();
        }

        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_COMPETITION_VIEW);
        }

        private void SaveAction()
        {
            App.Model.SaveChanges();
            RaisePropertyChanged(nameof(Competition));
            App.NotifyColleagues(AppMessages.MSG_COMPETITION_VIEW);
        }


        private String nom;
        public String Nom
        {
            get => Competition.Nom;
            set
            {
                if (value == Competition.Nom)
                    return;
                Competition.Nom = value;
                SetProperty(ref nom, value, () => Validate());

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
            get => Competition.Salle;
            set
            {
                Competition.Salle = value;
                SetProperty(ref salleselected, value);
            }
        }


        private DateTime date;
        public DateTime Date
        {
            get => Competition.Horaire;
            set
            {
                if (value == Competition.Horaire)
                    return;
                Competition.Horaire = value;
                SetProperty(ref date, value, () => Validate());

            }

        }

        private int heures;
        public int Heures
        {
            get => Competition.Horaire.Hour;
            set
            {
                SetProperty(ref heures, value, () => Validate());

            }

        }

        private int minutes;
        public int Minutes
        {
            get => Competition.Horaire.Minute;
            set
            {
                SetProperty(ref minutes, value);
            }
        }

        private int lotGagnant;
        public int LotGagnant
        {
            get => Competition.LotGagnant;
            set
            {
                if (value == Competition.LotGagnant)
                    return;
                Competition.LotGagnant = value;
                SetProperty(ref lotGagnant, value, () => Validate());

            }

        }
    }
}
