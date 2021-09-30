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
    /// Interaction logic for EditTeamView.xaml
    /// </summary>
    public partial class EditTeamView : UserControlBase
    {

        private Team Team;
        public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }

        public EditTeamView(Team team)
        {
            Team = team;


            Cancel = new RelayCommand(CancelAction);
            Save = new RelayCommand(SaveAction);


            DataContext = this;
            InitializeComponent();

        }



        private String nom;
        public String Nom
        {
            get => Team.Nom;
            set
            {
                if (value == Team.Nom)
                    return;
                Team.Nom = value;
                SetProperty(ref nom, value, () => Validate());

            }

        }

        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_COMPETITION_VIEW);

        }


        private void SaveAction()
        {
            App.Model.SaveChanges();
            RaisePropertyChanged(nameof(Team));
            App.NotifyColleagues(AppMessages.MSG_COMPETITION_VIEW);
        }

    }
}
