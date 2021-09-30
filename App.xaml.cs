using PRBD_Framework;
using prbd_1920_a01.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace prbd_1920_a01
{
    public enum AppMessages
    {
        MSG_COMPETITION_FINI,
        MSG_COMPETITION_DETAIL,
        MSG_MEMBERS_VIEW,
        MSG_STUDENT_VIEW,
        MSG_ACTIVITE_VIEW,
        MSG_COMPETITION_VIEW,

        MSG_MY_CURSUS,
        MSG_MY_SOLDE,
        MSG_MEMBER_LOGIN,
        MSG_PARTICIPATION,
        MSG_DISPLAY_ACTIVITE,
        MSG_DISPLAY_MEMBER,

        MSG_REFRESH_DATA_USER,

        MSG_EDIT_DATA_USER,
        MSG_EDIT_DATA_ACTIVITE,
        MSG_EDIT_DATA_COMPETITION,
        MSG_EDIT_DATA_TEAM,

        MSG_MATCH_DETAIL,
        MSG_NEW_HALL,
        MSG_NEW_ACTIVITE,
        MSG_NEW_MEMBER,
        MSG_NEW_COMPETITION,
        MSG_NEW_TEAM,

        MSG_COMPET_CREATED,
        MSG_HALL_CREATED,
        
        MSG_PROFILE,
        MSG_CLOSE_VIEW
    }

    public partial class App : ApplicationBase
    {
        public static Model Model { get; private set; } = new Model();

        public static readonly string IMAGE_PATH =
            Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../../images");

        public static Member CurrentUser { get; set; }

        public static void CancelChanges()
        {
            Model.Dispose(); // Retire de la mémoire le modèle actuel
            Model = new Model(); // Recréation d'un nouveau modèle à partir de la DB
        }
        public App()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.Culture);
            ColdStart();
        }

        private void ColdStart()
        {
            Model.SeedData();
        }
    }
}
