using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace prbd_1920_a01 {
    public partial class MemberDetailView : UserControlBase {

        public Member Member { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand Save { get; set; }

       

        public MemberDetailView(Member member) {
            Member = member;
            Cancel = new RelayCommand(CancelAction);
            Save = new RelayCommand(SaveAction);
            InitializeComponent();
            DataContext = this;
        }

        private string lastname;
        public string LastName
        {
            get => Member.LastName;
            set
            {
                if (value == Member.LastName)
                    return;
                Member.LastName = value;
                SetProperty(ref lastname, value, () => Validate());
            }
        }
        private string firstname;
        public string FirstName
        {
            get => Member.FirstName;     
            set
            {
                if (value == Member.FirstName)
                    return;
                Member.FirstName = value;
                SetProperty(ref firstname, value, () => Validate());

            }
        }
        private string password;
        public string Password
        {
            get => Member.Password;
            
            set
            {
                if (value == Member.Password)
                    return;
                Member.Password = value;
                SetProperty(ref password, value, () => Validate());
            }
        }
        private DateTime? birthdate;
        public DateTime? BirthDate
        {
            get => Member.Birthdate;
              set
            {
                if (value == Member.Birthdate)
                    return;
                Member.Birthdate = value;
                SetProperty(ref birthdate, value, () => Validate());
            }

        }
        private string adresse;
        public string Adresse
        {
            get =>  Member.Adresse;
            set 
            {
                if (value == Member.Adresse)
                    return;
                Member.Adresse = value;
                SetProperty(ref adresse, value, () => Validate());


            } 
        }
        private string email;
        public string Email
        {
            get => Member.Email;
            set
            {
                if (value == Member.Email)
                    return;
                Member.Email = value;
                SetProperty(ref email, value, () => Validate());

            }
        }

        private void SaveAction()
        {        
            App.Model.SaveChanges();
            RaisePropertyChanged(nameof(Member));

            if (App.CurrentUser.IsManager() || App.CurrentUser.Teacher())
            {
                App.NotifyColleagues(AppMessages.MSG_MEMBERS_VIEW);
            }
            else
            {
                App.NotifyColleagues(AppMessages.MSG_STUDENT_VIEW);
                App.NotifyColleagues(AppMessages.MSG_REFRESH_DATA_USER);
            }
                
            
        }

        private void CancelAction()
        {
            if (App.CurrentUser.IsManager() || App.CurrentUser.Teacher())
            {
                App.NotifyColleagues(AppMessages.MSG_MEMBERS_VIEW);
            }
            else
            {
                App.NotifyColleagues(AppMessages.MSG_STUDENT_VIEW);
            }
        }
    }
}
