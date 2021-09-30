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
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class CreateMemberView : UserControlBase
    {

        public CreateMemberView()
        {
            DataContext = this;
            Confirm = new RelayCommand(ConfirmAction, () => { return Validate(); });
            Cancel = new RelayCommand(CancelAction);
            InitializeComponent();
        }

        public ICommand Cancel { get; set; }
        public ICommand Confirm { get; set; }
        public Member Member { get; set; }


        private string nom;
        public string Nom
        {
            get => nom;
            set => SetProperty(ref nom, value, () => Validate());
        }

        private string prenom;
        public string Prenom
        {
            get => prenom;
            set => SetProperty(ref prenom, value, () => Validate());
        }

        private DateTime ddn;
        public DateTime DDN
        {
            get
            {
                if (ddn == DateTime.MinValue)
                    return DateTime.Now;
                return ddn;
            }
            set => SetProperty<DateTime>(ref ddn, value);
        }

        private string adresse;
        public string Adresse
        {
            get => adresse;
            set => SetProperty(ref adresse, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private bool eleveSelected;
        public bool EleveSelected
        {
            get => eleveSelected;
            set => SetProperty(ref eleveSelected, value);
        }

        private bool professeurSelected;
        public bool ProfesseurSelected
        {
            get => professeurSelected;
            set => SetProperty(ref professeurSelected, value);
        }


        public Role setRole()
        {

            if (eleveSelected)
            {
                return Role.Member;
            } else 
            {
                return Role.Teacher;
            }
           
        }

        private Member CreateAccount()
        {
            var member = App.Model.Members.Create();

            member.FirstName = prenom;
            member.LastName = nom;
            member.Birthdate = ddn;
            member.Email = email;
            member.Adresse = adresse;
            member.Password = prenom;
            member.Pseudo = GenerateUsername();
            member.Role = setRole();
            App.Model.Members.Add(member);
            App.Model.SaveChanges();
            return member;
        }

        private string GenerateUsername()
        {
            return (ddn.Day.ToString("00") + ddn.Month.ToString("00") + prenom[0] + prenom[1] + nom).ToLower();
        }

        public override bool Validate()
        {
            ClearErrors();

            if (!ValidateDataForLogin())
                RaiseErrors();
            return !HasErrors;
        }

        private bool ValidateDataForLogin()
        {
            if (string.IsNullOrEmpty(Nom))
            {
                AddError("Nom", Properties.Resources.Error_Required);
            }
            if (string.IsNullOrEmpty(Prenom))
            {
                AddError("Prenom", Properties.Resources.Error_Required);
            }
            if (DateTime.Compare(DDN, DateTime.Now) == 0)
            {
                AddError("DDN", Properties.Resources.Error_Birthdate);
            }
            else
            {
                if (DDN.Year > DateTime.Now.Year)
                {
                    AddError("DDN", Properties.Resources.Error_NotAvailable);
                }
            }
            return !HasErrors;
        }

        private void ConfirmAction()
        {
            if (Validate())
            {
                var member = CreateAccount();
                App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);

            }
        }

        private void CancelAction()
        {
            App.NotifyColleagues(AppMessages.MSG_CLOSE_VIEW);
        }

    }
}
