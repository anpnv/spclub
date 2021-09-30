using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace prbd_1920_a01
{
    public partial class MembersView : UserControlBase
    {

        public MembersView()
        {
            
            DataContext = this;
            Members = new ObservableCollection<Member>(App.Model.Members);
            Edit = new RelayCommand<Member>(m => App.NotifyColleagues(AppMessages.MSG_EDIT_DATA_USER, m) );
            Delete = new RelayCommand<Member>(m => DeleteAction(m));
            View = new RelayCommand<Member>(m => ViewAction(m));
            DisplayMemberDetails = new RelayCommand<Member>(m =>  App.NotifyColleagues(AppMessages.MSG_DISPLAY_MEMBER, m) );
            InitializeComponent();
        }

        public ICommand Edit { get; set; }
        public ICommand Delete { get; set; }
        public ICommand View { get; set; }
        public ICommand DisplayMemberDetails { get; set; }

        private Member mem;
        public bool CanRead { get => App.CurrentUser.Role == Role.Manager; }
        private ObservableCollection<Member> members;
        public ObservableCollection<Member> Members { get => members; set => SetProperty(ref members, value); }
        private ObservableCollection<Inscription> estInscritsA;
        public ObservableCollection<Inscription> EstInscritsA { get => estInscritsA; set => SetProperty(ref estInscritsA, value); }

        private Member selectedMember;
        public Member SelectedMember { get => selectedMember; set => SetProperty<Member>(ref selectedMember, value); }

        
        public bool ViewPressed { get => mem != null; }


        private void Refresh()
        {
            Members = new ObservableCollection<Member>(App.Model.Members);
        }


        

        private void DeleteAction(Member m)
        {
       
            MessageBoxResult result = System.Windows.MessageBox.Show("Etes-vous sûre de vouloir supprimer le membre " + m.FirstName + " ? ", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                m.Delete();
                Refresh();
            }
        }

        private void ViewAction(Member m)
        {
            mem = m;
            EstInscritsA = new ObservableCollection<Inscription>(App.Model.Inscriptions.Where(i => i.Eleve.IdMember == m.IdMember));
            RaisePropertyChanged(nameof(ViewPressed));
            RaisePropertyChanged(nameof(estInscritsA));
        }
    }
}
