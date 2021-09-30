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
   
    public partial class LoginView : UserControlBase
    {
        public LoginView()
        {
            DataContext = this;
            Login = new RelayCommand(LoginAction,
                () => { return Validate(); });
            InitializeComponent();
        }

        public ICommand Login { get; set; }
        public ICommand Cancel { get; set; }

        private void LoginAction()
        {
            if (Validate())
            { 
                var member = App.Model.Members.Where(u => u.Pseudo == Pseudo).SingleOrDefault();
                App.CurrentUser = member;
                App.NotifyColleagues(AppMessages.MSG_MEMBER_LOGIN);
            }
        }

        private string pseudo;
        public string Pseudo
        {
            get => pseudo;
            set => SetProperty<string>(ref pseudo, value, () => Validate());
        }

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty<string>(ref password, value, () => Validate());
        }

        public override bool Validate()
        {
            ClearErrors();
            var member = App.Model.Members.Where(u => u.Pseudo == Pseudo).SingleOrDefault();
            if (!ValidateLogin(member) || !ValidatePwd(member))
                RaiseErrors();
            return !HasErrors;
        }

        private bool ValidateLogin(Member member)
        {
            if (string.IsNullOrEmpty(Pseudo))
            {
                AddError("Pseudo", Properties.Resources.Error_Required);
            }
            else
            {
                if (Pseudo.Length < 3)
                {
                    AddError("Pseudo", Properties.Resources.Error_LengthGreaterEqual3);
                }
                else
                {
                    if (member == null)
                    {
                        AddError("Pseudo", Properties.Resources.Error_DoesNotExist);
                    }
                }
            }
            return !HasErrors;
        }

        private bool ValidatePwd(Member member)
        {
            if (string.IsNullOrEmpty(Password))
            {
                AddError("Password", Properties.Resources.Error_Required);
            }
            else if (!member.Password.Equals(Password))
            {
                AddError("Password", Properties.Resources.Error_WrongPassword);
            }
            return !HasErrors;
        }
    }
}
