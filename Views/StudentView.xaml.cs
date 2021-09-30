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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prbd_1920_a01
{
    /// <summary>
    /// Logique d'interaction pour StudentView.xaml
    /// </summary>
    public partial class StudentView : UserControlBase
    {
        
        public StudentView()
        {
            Participation = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_PARTICIPATION));
            EditProfile = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_EDIT_DATA_USER,App.CurrentUser));
            MySolde = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_MY_SOLDE));
            MyCursus = new RelayCommand(() => App.NotifyColleagues(AppMessages.MSG_MY_CURSUS));
           
            DataContext = this;
            InitializeComponent();
        }

        public ICommand Participation { get; set; }
        public ICommand EditProfile { get; set; }
        public ICommand MySolde { get; set; }
        public ICommand MyCursus { get; set; }
        public ICommand ResultCompet { get; set; }
        public bool CanRead { get => App.CurrentUser.Role == Role.Member; }
        public string Balance { get => "Mon Solde\n" + App.CurrentUser.Solde + " €"; }
        public string Resultat { get => "Résultat\n des\n compétitions"; }
    }
}
