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
using System.Collections.ObjectModel;

namespace prbd_1920_a01
{

    public partial class SalleView : UserControlBase
    {


        private ObservableCollection<Salle> salles;
        public ObservableCollection<Salle> Salles { get => salles; set => SetProperty(ref salles, value); }

        public SalleView()
        {

            InitializeComponent();
            DataContext = this;
            Salles = new ObservableCollection<Salle>(App.Model.Salles);
            
        }


    }
}
