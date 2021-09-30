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
    /// Logique d'interaction pour MemberBalanceView.xaml
    /// </summary>
    public partial class MemberBalanceView : UserControlBase
    {
        public Member Member { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }
        public MemberBalanceView(Member member)
        {
            Member = member;

            Save = new RelayCommand(SaveAction);
            Cancel = new RelayCommand(CancelAction);
            InitializeComponent();
            DataContext = this;
        }

        private int solde;
        public int Solde
        {
            get =>  App.CurrentUser.Solde;
            set
            {
                SetProperty(ref solde, value);
            }
        }

        private int soldeAjouter; 
        public int SoldeAjouter 
        {     
            get => soldeAjouter; 
            set
            {
                App.CurrentUser.Solde += value;
                SetProperty(ref soldeAjouter, value);
            }
    }

        public void SaveAction()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Etes-vous sure de vouloir ajouter "
                                                                           + SoldeAjouter + " € " +
                                                                           "\n sur le compte de Parkour_School ? ",
                                                                           "", MessageBoxButton.YesNo,
                                                                           MessageBoxImage.Warning, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                App.CurrentUser.Solde = this.Solde;
                App.Model.SaveChanges();
                App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
                App.NotifyColleagues(AppMessages.MSG_STUDENT_VIEW);
                App.NotifyColleagues(AppMessages.MSG_REFRESH_DATA_USER);


            }
            else 
            {
                App.CurrentUser.Solde -= SoldeAjouter;
                App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
                App.NotifyColleagues(AppMessages.MSG_STUDENT_VIEW);
            }
        }
        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_STUDENT_VIEW);
        }

    }
}
