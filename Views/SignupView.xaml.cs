using System;
using PRBD_Framework;
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

    public partial class SignupView : UserControlBase
    {
        public SignupView()
        {
            DataContext = this;
            Signup = new RelayCommand(ConfirmAction, () => { return Validate(); });
            InitializeComponent();
        }

        public ICommand Signup { get; set; }

        private string nom;
        public string Nom
        {
            get => nom;
            set => SetProperty<string>(ref nom, value, () => Validate());
        }

        private string prenom;
        public string Prenom
        {
            get => prenom;
            set => SetProperty<string>(ref prenom, value, () => Validate());
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
            set => SetProperty<string>(ref adresse, value);
        }

        private string email;
        public string Email
        {
            get => email;
            set => SetProperty<string>(ref email, value, () => Validate());
        }

        private string confirmpassword;
        public string ConfirmPassword
        {
            get => confirmpassword;
            set => SetProperty<string>(ref confirmpassword, value);
        }

        private string passwordSignin;
        public string PasswordSignin
        {
            get => passwordSignin;
            set => SetProperty<string>(ref passwordSignin, value);
        }

        private Member CreateAccount()
        {
            var member = App.Model.Members.Create();
            member.FirstName = prenom;
            member.LastName = nom;
            member.Birthdate = ddn;
            member.Adresse = adresse;
            member.Email = email;
            member.Password = PasswordSignin;
            member.Pseudo = GenerateUsername();
            member.Role = Role.Member;
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
            if( !ValidateDataForLogin())
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

            var query = (from u in App.Model.Members where u.Email == Email select u);
            if (string.IsNullOrEmpty(Email))
            {
                AddError("Email", "Email non disponnible");
            }else
            {
                if (query.Count() != 0)
                {
                    AddError("Email", "Email déjà utilisé");
                }
            }
            return !HasErrors;

        }


        private void ConfirmAction()
        {
            if (Validate())
            {
                var member = CreateAccount();
                App.CurrentUser = member;
                App.NotifyColleagues(AppMessages.MSG_MEMBER_LOGIN);
                
            }
        }



    }
}
