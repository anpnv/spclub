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
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prbd_1920_a01
{
    /// <summary>
    /// Logique d'interaction pour ResultCompetFiniView.xaml
    /// </summary>
    public partial class ResultCompetFiniView : UserControlBase
    {
        public ResultCompetFiniView()
        {
            getCompetFini();
            DataContext = this;
            InitializeComponent();
        }

        private ObservableCollection<Competition> competitionsFini;
        public ObservableCollection<Competition> CompetitionsFini { get => competitionsFini; set => SetProperty(ref competitionsFini, value); }
        private void getCompetFini()
        {

            var compet = (from c in App.Model.Competitions
                          where c.Horaire <= DateTime.Now
                          where c.Team.Where(m => m.EstGagnant).Count() <= 1
                          select c).OrderBy(a => a.Horaire);

            CompetitionsFini = new ObservableCollection<Competition>(compet);
        }
    }
}
