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
    public partial class MainView : WindowBase
    {


        public MainView()
        {
            WinClose = new RelayCommand(() => Application.Current.Shutdown());
            Logout = new RelayCommand(LogoutAction);
            MoveWindow = new RelayCommand(() => DragMove());

            DataContext = this;
            InitializeComponent();


            App.Register(this, AppMessages.MSG_COMPETITION_FINI, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new ResultCompetFiniView());
            });

            App.Register(this, AppMessages.MSG_MEMBERS_VIEW, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new MembersView());
            });

            App.Register(this, AppMessages.MSG_MY_SOLDE, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new MemberBalanceView(App.CurrentUser));
            });

            App.Register(this, AppMessages.MSG_REFRESH_DATA_USER, () =>
            {
                RaisePropertyChanged(nameof(PseudoLabel));
                RaisePropertyChanged(nameof(NameLabel));
                RaisePropertyChanged(nameof(Solde));

            });


            App.Register(this, AppMessages.MSG_STUDENT_VIEW, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new StudentView());
                
            });


            App.Register(this, AppMessages.MSG_ACTIVITE_VIEW, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new ActiviteView());

            });

            App.Register(this, AppMessages.MSG_COMPETITION_VIEW, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new CompetitionsView());

            });


            App.Register(this, AppMessages.MSG_PARTICIPATION, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new ParticipationView(App.CurrentUser));
            });



            App.Register<Member>(this, AppMessages.MSG_EDIT_DATA_USER, (m) =>
            {
                Content.Children.Clear();
                Content.Children.Add(new MemberDetailView(m));
            });

            App.Register<Activite>(this, AppMessages.MSG_EDIT_DATA_ACTIVITE, (a) =>
            {
                Content.Children.Clear();
                Content.Children.Add(new EditActiviteView(a));
            });

            App.Register<Competition>(this, AppMessages.MSG_EDIT_DATA_COMPETITION, (c) =>
            {
                Content.Children.Clear();
                Content.Children.Add(new EditCompetitionView(c));
            });

            App.Register<Team>(this, AppMessages.MSG_EDIT_DATA_TEAM, (t) =>
            {
                Content.Children.Clear();
                Content.Children.Add(new EditTeamView(t));
            });

            App.Register(this, AppMessages.MSG_NEW_HALL, () =>
            {
                var salle = App.Model.Salles.Create();
                Content.Children.Clear();
                Content.Children.Add(new CreateHallView(salle));
            });

            App.Register(this, AppMessages.MSG_NEW_ACTIVITE, () =>
            {
                var activite = App.Model.Activites.Create();
                Content.Children.Clear();
                Content.Children.Add(new CreateActivityView(activite));
            });

            App.Register(this, AppMessages.MSG_NEW_COMPETITION, () =>
            {
                var compet = App.Model.Competitions.Create();
                Content.Children.Clear();
                Content.Children.Add(new CreateCompetitionView(compet));
            });

            App.Register(this, AppMessages.MSG_NEW_MEMBER, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new CreateMemberView());
            });



            App.Register(this, AppMessages.MSG_MATCH_DETAIL, () =>
            {

                Content.Children.Clear();
                Content.Children.Add(new MatchDetailView());
            });



            App.Register(this, AppMessages.MSG_CLOSE_VIEW, () =>
            {
                Content.Children.Clear();
                Content.Children.Add(new ManagerView());
            });
            
            Accueil();
            selectedMenu = 0;
        }


        public ICommand MoveWindow { get; set; }
        public ICommand WinClose { get; set; }
        public ICommand Logout { get; set; }


        public string NameLabel { get => App.CurrentUser.FirstName[0] + ". " + App.CurrentUser.LastName.ToUpper(); }
        public string Solde { get => App.CurrentUser.Solde + " $"; }
        
        public string PseudoLabel { get => App.CurrentUser.Pseudo; }
        public string Role { get => App.CurrentUser.Role.ToString();}

        public bool CantRead { get => App.CurrentUser.Role != prbd_1920_a01.Role.Member; }
        public bool CanRead { get => App.CurrentUser.Role == prbd_1920_a01.Role.Member; }
        public bool IsNotAdmin { get => !App.CurrentUser.IsAdmin; }

        private int selectedMenu;
        public int SelectedMenu
        {
            get => selectedMenu;
            set
            {
                selectedMenu = value;
                int index = value;
                switch (index)
                {
                    case 0:
                        Content.Children.Clear();
                        Content.Children.Add(new StudentView());
                        break;
                    case 1:
                        Content.Children.Clear();
                        Content.Children.Add(new ActiviteView());
                        break;
                    case 2:
                        Content.Children.Clear();
                        Content.Children.Add(new CompetitionsView());
                        break;
                    case 3:
                        Content.Children.Clear();
                        Content.Children.Add(new MembersView());
                        break;
                    case 4:
                        Content.Children.Clear();
                        Content.Children.Add(new ManagerView());
                        break;     
                    default:
                        break;
                }
                RaisePropertyChanged(nameof(selectedMenu));
            }
        }

        private void Accueil()
        {
            if (CantRead)
            {
                Content.Children.Add(new ManagerView());
            }
            else
            {
                Content.Children.Add(new StudentView());
            }
        }
        private void LogoutAction()
        {
            App.CurrentUser = null;
            Login();
            Close();
        }

        private void Login()
        {
            var rootView = new RootView();
            Visibility = Visibility.Hidden;
            var res = rootView.ShowDialog();
            if (res == true)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Close();
            }
        }
    }
}

