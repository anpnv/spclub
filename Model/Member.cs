using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PRBD_Framework;
using System.Linq;
using System.Windows;

namespace prbd_1920_a01
{

    public enum Role { Member, Teacher, Manager }

    public class Member : EntityBase<Model>
    {

        [Key]
        public int IdMember { get; set; }
        public Role Role { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Pseudo { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public int Solde { get; set; }

        public virtual ICollection<Inscription> Eleves { get; set; } = new HashSet<Inscription>();
        public virtual ICollection<Activite> Professeurs { get; set; } = new HashSet<Activite>();
        public virtual ICollection<Inscription> Teams { get; set; } = new HashSet<Inscription>();



        protected Member() { }

        public bool CanEdit
        {
            get => this.Role != Role.Member;
        }

        public bool IsAdmin
        {
            get => this.Role == Role.Manager;
        }

        public bool IsEleve
        {
            get => this.Role == Role.Member;
        }

        public override string ToString()
        {
            return Pseudo;
        }

        public Member IsTeacher()
        {
            if (this.Role == Role.Teacher)
            {
                return this;
            }
            return null;
        }

        public bool IsManager()
        {
            return this.Role == Role.Manager;
        }

        public bool Teacher()
        {
            return this.Role == Role.Teacher;
        }


        public Inscription Subscribe(Activite activite, Competition compet)
        {
            var sub = Model.Inscriptions.Create();
            sub.Activite = activite;
            sub.Competition = compet;
            sub.Eleve = this;
            return sub;
        }

        public void SubUnsubTeam(Competition compet,Team team)
        {
            var inscription = Inscription.GetInscriptionCompet(App.CurrentUser, team);
            
            if (inscription != null )
            {
                if (inscription.Team != null)
                {
                    Teams.Remove(inscription);
                    App.Model.Inscriptions.Remove(inscription);
                    this.Solde += 5;
                }       
            }
            else 
            {
                if (!Inscription.EstInscritTeam(App.CurrentUser,compet))
                {
                    if (App.CurrentUser.Solde == 0 || App.CurrentUser.Solde < compet.Prix)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("Votre solde est insuffisant ! \n Voulez-vous le recharger ?", " ", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

                        if (result == MessageBoxResult.Yes)
                        {
                            App.NotifyColleagues(AppMessages.MSG_MY_SOLDE);
                        }
                    }
                    else
                    {
                        var sub = Model.Inscriptions.Create();
                        sub.Team = team;
                        sub.Eleve = this;
                        sub.Competition = compet;
                        App.CurrentUser.Teams.Add(sub);
                        App.Model.Inscriptions.Add(sub);
                        this.Solde -= 5;
                    }
                }
                else
                {
                     System.Windows.MessageBox.Show("Vous etes déjà inscrit dans une team ");

                }
                
            }

            App.Model.SaveChanges();

        }
        public void UnSub(Inscription subs)
        {

            if (subs.Team == null)
            {
                this.Solde += 10;
                App.CurrentUser.Eleves.Remove(subs);
            }
            else
            {
                this.Solde += 5;
                App.CurrentUser.Teams.Remove(subs);
            }
            App.Model.Inscriptions.Remove(subs);
            
            App.Model.SaveChanges();
        }

        public void Sub(Inscription newSub)
        {
            
            this.Solde -= 10;

            App.CurrentUser.Eleves.Add(newSub);
            App.Model.Inscriptions.Add(newSub);
            App.Model.SaveChanges();
        }

        public void Delete()
        {
            Model.Members.Remove(this);
            Model.SaveChanges();
        }

        public void addSolde(int gold)
        {
            this.Solde += gold;
        }

        public void removeSolde(int gold)
        {
            this.Solde -= gold;
        }



    }
}