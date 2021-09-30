using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using PRBD_Framework;
using System.Collections.Generic;

namespace prbd_1920_a01
{

    public class Model : DbContext
    {

        public Model() : base("prbd_1920_a01")
        {

            Database.SetInitializer<Model>(new DropCreateDatabaseIfModelChanges<Model>());
        }

        protected Model(DbCompiledModel model) : base(model) { }

        public DbSet<Member> Members { get; set; }
        public DbSet<Activite> Activites { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<Team> Teams { get; set; }

        private List<Member> ListeProfesseur = new List<Member>();
        private List<Salle> ListSalle = new List<Salle>();

        private void getProfesseurs()
        {
            foreach (Member m in Members)
            {
                if (m.Teacher())
                {
                    ListeProfesseur.Add(m);
                }
            }
        }

        private void getSalles()
        {
            foreach (Salle s in Salles)
            {
                ListSalle.Add(s);
            }
        }

        public void DeleteInscription(Member inscrit, Activite activite)
        {
            Inscriptions.Remove(Inscription.GetInscription(inscrit, activite));
            SaveChanges();
        }
        public Inscription CreateInscription(Member inscrit, Activite activite)
        {
            var inscription = Inscriptions.Create();
            inscription.Eleve = inscrit;
            inscription.Activite = activite;
            Inscriptions.Add(inscription);
            SaveChanges();
            return inscription;
        }

        private Member CreateMember(string pseudo, string password, DateTime ddn, Role role = Role.Member)
        {
            var member = Members.Create();
            member.Pseudo = pseudo;
            member.Password = password;
            member.Role = role;
            member.FirstName = "pré" + pseudo;
            member.Birthdate = ddn;
            member.LastName = "Lastname" + pseudo;
            member.Email = pseudo + "@gmail.com";
            if (member.Role == Role.Member)
            {

                member.Solde = randomINT(35, 250);
            }

            Members.Add(member);
            return member;
        }


        private int randomINT(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private Activite CreateActivite(string nomCours, DateTime horaire, Salle salle, Member professeur, Type type)
        {
            var act = Activites.Create();
            act.NomCours = nomCours;
            act.Horaire = horaire;
            act.MaxParticipant = 10;
            act.Type = type;
            act.Lieux = salle;
            act.Professeur = professeur;
            professeur.Professeurs.Add(act);
            Activites.Add(act);
            return act;
        }


        private Salle CreateHall(String nomSalle, string adresse)
        {
            var salle = Salles.Create();
            salle.NomSalle = nomSalle;
            salle.Adresse = adresse;
            Salles.Add(salle);
            return salle;
        }

        private Inscription createActivityInscription(Member m, Activite a)
        {
            var ins = Inscriptions.Create();
            ins.Eleve = m;
            ins.Activite = a;
            m.Eleves.Add(ins);
            Inscriptions.Add(ins);
            return ins;

        }

        private Competition createCompet(String nom, DateTime horaire, Salle salle)
        {
            var compet = Competitions.Create();
            compet.Nom = nom;
            compet.Horaire = horaire;
            compet.LotGagnant = 90;
            compet.Salle = salle;

            foreach (Team t in Teams)
            {
                compet.Team.Add(t);
            }
            Competitions.Add(compet);
            return compet;
        }

        private Team createTeam(string nom)
        {
            var team = Teams.Create();
            team.Nom = nom;
            Teams.Add(team);
            return team;
        }

        public void SeedData()
        {

            if (Members.Count() == 0)
            {

                Console.Write("Seeding members... ");
                var ddn = new DateTime(randomINT(1960, 1975), randomINT(1, 12), randomINT(1, 28), 19, 30, 0);
                var admin = CreateMember("admin", "admin", ddn, Role.Manager);
                var professeur = CreateMember("prof", "prof", ddn.AddYears(10), Role.Teacher);
                var eleve = CreateMember("eleve", "eleve", ddn.AddYears(20));
                for (int i = 0; i < 35; i++)
                {
                    CreateMember("eleve" + i + "", "eleve", ddn.AddYears(20));
                    if (i % 3 == 0)
                    {
                        CreateMember("professeur" + i, "prof", ddn.AddYears(10), Role.Teacher);
                    }
                    if (i < 4)
                    {
                        CreateMember("admin" + i, "admin" + i, ddn, Role.Manager);
                    }
                }
                SaveChanges();
                Console.WriteLine("ok");
            }
            if (Salles.Count() == 0)
            {
                Console.Write("Seeding halls... ");
                var salle1 = CreateHall("Ceria", "1070 Anderlecht");
                var salle2 = CreateHall("Adeps", "1160 Auderghem");
                var salle3 = CreateHall("Gym Jette", "1090 Jette");
                var salle4 = CreateHall("Ecole du Cirque", "1080 Molenbeek");
                for (int i = 1; i < 15; i++)
                {
                    CreateHall("Salle De Sport" + i, "10" + i + "0 Bruxelles");
                }
                SaveChanges();
                Console.WriteLine("ok");
            }

            if (Activites.Count() == 0)
            {
                Console.Write("Seeding activity...");
                getProfesseurs();
                getSalles();
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 30, 0);
                for (int i = 1; i < 10; i++)
                {
                    if (i % 2 == 0)
                    {
                        CreateActivite("Cours n°" + i * i, date.AddDays(i + 1), ListSalle[i], ListeProfesseur[i], Type.Cours);
                    }
                    else
                    {
                        CreateActivite("Event n°" + i * i * i, date.AddDays(i * 5), ListSalle[i], ListeProfesseur[i], Type.Evenement);
                    }
                }
                SaveChanges();
                Console.WriteLine("ok");
            }

            if (Teams.Count() == 0)
            {
                Console.Write("Seeding teams...");
                for (int i = 1; i <= 3; i++)
                {
                    createTeam("Team n°" + i);
                }

                SaveChanges();
                Console.WriteLine("ok");
            }

            if (Competitions.Count() == 0)
            {
                Console.Write("Seeding competitions...");
                getSalles();
                var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 00, 0);
                for (int i = 1; i < 6; i++)
                {
                    createCompet("Competitions n°" + i, date.AddDays(i * 6), ListSalle[i]);
                }
                SaveChanges();
                Console.WriteLine("ok");
            }

            if (Inscriptions.Count() == 0)
            {
                Console.Write("seeding inscriptions...");
                foreach (Member m in Members)
                {
                    foreach (Activite a in Activites)
                    {
                        if (a.Activites.Count() < a.MaxParticipant)
                        {
                            if (m.Role == Role.Member)
                            {
                                createActivityInscription(m, a);
                            }
                        }
                    }
                }

                SaveChanges();
                Console.WriteLine("ok");
            }

        }
    }
}