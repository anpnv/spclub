using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
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
    /// Interaction logic for CreateTeamView.xaml
    /// </summary>
    public partial class CreateTeamView : UserControlBase
    {
        public Team Team { get; set; }
        public Competition Competition { get; set; }


        private string nom;
        public string Nom
        {
            get => nom;
            set
            {
                Team.Nom = value;
                SetProperty(ref nom, value, () => Validate());
            }
        }

        public ICommand Confirm { get; set; }
        public ICommand Cancel { get; set; }

        public CreateTeamView(Competition competition,Team team)
        {
            Competition = competition;
            DataContext = this;
            Team = team;
            Confirm = new RelayCommand(ConfirmAction);
            Cancel = new RelayCommand(CancelAction);
            InitializeComponent();
        }

        private void ConfirmAction()
        {
            if (Validate())
            {
                Competition.AddTeam(Team);
                App.Model.SaveChanges();
                App.NotifyColleagues(AppMessages.MSG_COMPETITION_DETAIL,Competition);
            }
        }

        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_COMPETITION_DETAIL, Competition);
        }

        public override bool Validate()
        {
            ClearErrors();

            var team = (from t in App.Model.Teams where t.Nom == Nom select t).SingleOrDefault();
            if (team != null)
            {
                AddError("Nom", Properties.Resources.Error_NotAvailable);
            }

            if (string.IsNullOrEmpty(Nom))
            {
                AddError("Nom", Properties.Resources.Error_Required);
            }



            RaiseErrors();
            return !HasErrors;
        }
    }
}
