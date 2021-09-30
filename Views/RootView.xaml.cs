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
using System.Windows.Shapes;

namespace prbd_1920_a01 {
    public partial class RootView : WindowBase {

        public ICommand WinClose { get; set; }

        private void EntryToApp()
        {
                ShowMainView(); 
                Close();  
        }

        private static void ShowMainView() {

            var mainView = new MainView();
            mainView.Show();
            Application.Current.MainWindow = mainView;
        }

        public RootView() {
            
            DataContext = this;
            App.Register(this, AppMessages.MSG_CLOSE_VIEW, () =>
             {
                 Close();
             });
            App.Register(this, AppMessages.MSG_MEMBER_LOGIN, () =>
            {
                EntryToApp();
            });

            WinClose = new RelayCommand(App.Current.Shutdown);
            InitializeComponent();
        }

    }
}
