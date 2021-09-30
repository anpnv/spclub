using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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

namespace prbd_1920_a01 {

    public partial class CreateHallView : UserControlBase
    {
        public Salle Salle { get; set; }

        private string nomsalle;
        public string NomSalle
        {
            get => nomsalle;
            set
            {
                Salle.NomSalle = value;
                SetProperty(ref nomsalle, value, () => Validate());
            }
        }

        private string adresse;
        public string Adresse
        {
            get => adresse;
            set
            {
                Salle.Adresse = value;
                SetProperty(ref adresse, value, () => Validate());
            }
        }

        public ICommand Confirm { get; set; }
        public ICommand Cancel { get; set; }

        public CreateHallView(Salle salle )
        {
            DataContext = this;
            Salle = salle;
            Confirm = new RelayCommand(ConfirmAction, () => { return nomsalle != null && adresse != null & !HasErrors; });
            Cancel = new RelayCommand(CancelAction);
            InitializeComponent();
        }

        private void ConfirmAction()
        {
            if (Validate())
            {
            App.Model.Salles.Add(Salle);
            App.Model.SaveChanges();
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
            }
        }

        private void CancelAction()
        {
            NomSalle = null;
            Adresse = null;
            RaisePropertyChanged(nameof(Salle));
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
        }

        public override bool Validate()
        {
            ClearErrors();

            var salle = (from s in App.Model.Salles where s.NomSalle == NomSalle select s).SingleOrDefault();
            if (salle!= null)
            {
                AddError("NomSalle", Properties.Resources.Error_NotAvailable);
            }

            if (string.IsNullOrEmpty(NomSalle))
            {
                AddError("NomSalle", Properties.Resources.Error_Required);
            }
            if (string.IsNullOrEmpty(Adresse))
            {
                AddError("Adresse", Properties.Resources.Error_Required);
            }


            RaiseErrors();
            return !HasErrors;
        }

    }
}
