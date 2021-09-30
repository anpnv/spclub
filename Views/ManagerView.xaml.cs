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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prbd_1920_a01
{

    public partial class ManagerView : UserControlBase
    {
        public ManagerView()
        {
            Salle = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_NEW_HALL));
            Activite = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_NEW_ACTIVITE));
            Membre = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_NEW_MEMBER));
            Competition = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_NEW_COMPETITION));
            Team = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_NEW_TEAM));
            Match = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_MATCH_DETAIL));
            DataContext = this;
            InitializeComponent();
        }

        public ICommand Salle { get; set; }
        public ICommand Activite { get; set; }
        public ICommand Membre { get; set; }
        public ICommand Competition { get; set; }
        public ICommand Team { get; set; }
        public ICommand Match { get; set; }
        public bool CanRead { get => App.CurrentUser.Role != Role.Teacher; }
    }
}
