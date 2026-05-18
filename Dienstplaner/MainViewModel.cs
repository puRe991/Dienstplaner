using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Dienstplaner.Helpers;
using Dienstplaner.Models;

namespace Dienstplaner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Mitarbeiter> MitarbeiterListe { get; set; }
        public ObservableCollection<Schicht> SchichtListe { get; set; }
        public ObservableCollection<Zuweisung> Zuweisungen { get; set; }

        private Mitarbeiter _ausgewaehlterMitarbeiter;
        public Mitarbeiter AusgewaehlterMitarbeiter
        {
            get { return _ausgewaehlterMitarbeiter; }
            set { _ausgewaehlterMitarbeiter = value; OnPropertyChanged(); }
        }

        private Schicht _ausgewaehlteSchicht;
        public Schicht AusgewaehlteSchicht
        {
            get { return _ausgewaehlteSchicht; }
            set { _ausgewaehlteSchicht = value; OnPropertyChanged(); }
        }

        private string _neuerMitarbeiterName;
        public string NeuerMitarbeiterName
        {
            get { return _neuerMitarbeiterName; }
            set { _neuerMitarbeiterName = value; OnPropertyChanged(); }
        }

        private string _neuerMitarbeiterQualifikation;
        public string NeuerMitarbeiterQualifikation
        {
            get { return _neuerMitarbeiterQualifikation; }
            set { _neuerMitarbeiterQualifikation = value; OnPropertyChanged(); }
        }

        private int _neuesMitarbeiterWochenlimit = 40;
        public int NeuesMitarbeiterWochenlimit
        {
            get { return _neuesMitarbeiterWochenlimit; }
            set { _neuesMitarbeiterWochenlimit = value; OnPropertyChanged(); }
        }

        private string _neueSchichtName;
        public string NeueSchichtName
        {
            get { return _neueSchichtName; }
            set { _neueSchichtName = value; OnPropertyChanged(); }
        }

        private DateTime _neueSchichtStart = DateTime.Today.AddHours(6);
        public DateTime NeueSchichtStart
        {
            get { return _neueSchichtStart; }
            set { _neueSchichtStart = value; OnPropertyChanged(); }
        }

        private DateTime _neueSchichtEnde = DateTime.Today.AddHours(14);
        public DateTime NeueSchichtEnde
        {
            get { return _neueSchichtEnde; }
            set { _neueSchichtEnde = value; OnPropertyChanged(); }
        }

        private int _neueSchichtKapazitaet = 1;
        public int NeueSchichtKapazitaet
        {
            get { return _neueSchichtKapazitaet; }
            set { _neueSchichtKapazitaet = value; OnPropertyChanged(); }
        }

        private string _neueSchichtQualifikation;
        public string NeueSchichtQualifikation
        {
            get { return _neueSchichtQualifikation; }
            set { _neueSchichtQualifikation = value; OnPropertyChanged(); }
        }

        private string _statusNachricht;
        public string StatusNachricht
        {
            get { return _statusNachricht; }
            set { _statusNachricht = value; OnPropertyChanged(); }
        }

        public ICommand MitarbeiterHinzufuegenCommand { get; set; }
        public ICommand SchichtHinzufuegenCommand { get; set; }
        public ICommand ZuweisenCommand { get; set; }

        public MainViewModel()
        {
            MitarbeiterListe = new ObservableCollection<Mitarbeiter>();
            SchichtListe = new ObservableCollection<Schicht>();
            Zuweisungen = new ObservableCollection<Zuweisung>();

            MitarbeiterHinzufuegenCommand = new RelayCommand(HinzufuegenMitarbeiter);
            SchichtHinzufuegenCommand = new RelayCommand(HinzufuegenSchicht);
            ZuweisenCommand = new RelayCommand(Zuweisen);

            MitarbeiterListe.Add(new Mitarbeiter { Id = 1, Name = "Max Mustermann", Qualifikation = "Pflege", WochenstundenLimit = 40, IstAktiv = true });
            MitarbeiterListe.Add(new Mitarbeiter { Id = 2, Name = "Anna Beispiel", Qualifikation = "Intensiv", WochenstundenLimit = 32, IstAktiv = true });

            SchichtListe.Add(new Schicht
            {
                Id = 1,
                Name = "Frühschicht",
                Start = DateTime.Today.AddHours(6),
                Ende = DateTime.Today.AddHours(14),
                BenoetigteMitarbeiter = 2,
                BenoetigteQualifikation = "Pflege"
            });

            StatusNachricht = "Bereit für die Planung.";
        }

        private void HinzufuegenMitarbeiter(object obj)
        {
            if (string.IsNullOrWhiteSpace(NeuerMitarbeiterName))
            {
                StatusNachricht = "Bitte einen Mitarbeiternamen eingeben.";
                return;
            }

            if (NeuesMitarbeiterWochenlimit <= 0)
            {
                StatusNachricht = "Das Wochenstundenlimit muss größer als 0 sein.";
                return;
            }

            MitarbeiterListe.Add(new Mitarbeiter
            {
                Id = MitarbeiterListe.Count + 1,
                Name = NeuerMitarbeiterName.Trim(),
                Qualifikation = (NeuerMitarbeiterQualifikation ?? string.Empty).Trim(),
                WochenstundenLimit = NeuesMitarbeiterWochenlimit,
                IstAktiv = true
            });

            NeuerMitarbeiterName = string.Empty;
            NeuerMitarbeiterQualifikation = string.Empty;
            NeuesMitarbeiterWochenlimit = 40;
            StatusNachricht = "Mitarbeiter wurde erfolgreich angelegt.";
        }

        private void HinzufuegenSchicht(object obj)
        {
            if (string.IsNullOrWhiteSpace(NeueSchichtName))
            {
                StatusNachricht = "Bitte einen Schichtnamen eingeben.";
                return;
            }

            if (NeueSchichtEnde <= NeueSchichtStart)
            {
                StatusNachricht = "Das Schichtende muss nach dem Schichtstart liegen.";
                return;
            }

            if (NeueSchichtKapazitaet <= 0)
            {
                StatusNachricht = "Die Schichtkapazität muss größer als 0 sein.";
                return;
            }

            SchichtListe.Add(new Schicht
            {
                Id = SchichtListe.Count + 1,
                Name = NeueSchichtName.Trim(),
                Start = NeueSchichtStart,
                Ende = NeueSchichtEnde,
                BenoetigteMitarbeiter = NeueSchichtKapazitaet,
                BenoetigteQualifikation = (NeueSchichtQualifikation ?? string.Empty).Trim()
            });

            NeueSchichtName = string.Empty;
            NeueSchichtKapazitaet = 1;
            NeueSchichtQualifikation = string.Empty;
            NeueSchichtStart = DateTime.Today.AddHours(6);
            NeueSchichtEnde = DateTime.Today.AddHours(14);
            StatusNachricht = "Schicht wurde erfolgreich angelegt.";
        }

        private void Zuweisen(object obj)
        {
            if (AusgewaehlterMitarbeiter == null || AusgewaehlteSchicht == null)
            {
                StatusNachricht = "Bitte Mitarbeiter und Schicht auswählen.";
                return;
            }

            if (!AusgewaehlterMitarbeiter.IstAktiv)
            {
                StatusNachricht = "Mitarbeiter ist nicht aktiv.";
                return;
            }

            if (!string.IsNullOrWhiteSpace(AusgewaehlteSchicht.BenoetigteQualifikation) &&
                !string.Equals(AusgewaehlterMitarbeiter.Qualifikation, AusgewaehlteSchicht.BenoetigteQualifikation, StringComparison.OrdinalIgnoreCase))
            {
                StatusNachricht = "Qualifikation passt nicht zur Schicht.";
                return;
            }

            if (Zuweisungen.Any(z => z.MitarbeiterId == AusgewaehlterMitarbeiter.Id && z.SchichtId == AusgewaehlteSchicht.Id))
            {
                StatusNachricht = "Mitarbeiter ist dieser Schicht bereits zugewiesen.";
                return;
            }

            int belegung = Zuweisungen.Count(z => z.SchichtId == AusgewaehlteSchicht.Id);
            if (belegung >= AusgewaehlteSchicht.BenoetigteMitarbeiter)
            {
                StatusNachricht = "Schicht ist bereits voll besetzt.";
                return;
            }

            var schichtenDesMitarbeiters = SchichtListe.Where(s => Zuweisungen.Any(z => z.MitarbeiterId == AusgewaehlterMitarbeiter.Id && z.SchichtId == s.Id));
            bool zeitKonflikt = schichtenDesMitarbeiters.Any(s => UeberschneidetSich(s.Start, s.Ende, AusgewaehlteSchicht.Start, AusgewaehlteSchicht.Ende));
            if (zeitKonflikt)
            {
                StatusNachricht = "Mitarbeiter hat bereits eine überschneidende Schicht.";
                return;
            }

            double bereitsGeplanteStunden = schichtenDesMitarbeiters.Sum(s => (s.Ende - s.Start).TotalHours);
            double neueStunden = (AusgewaehlteSchicht.Ende - AusgewaehlteSchicht.Start).TotalHours;

            if (bereitsGeplanteStunden + neueStunden > AusgewaehlterMitarbeiter.WochenstundenLimit)
            {
                StatusNachricht = "Wochenstundenlimit würde überschritten.";
                return;
            }

            Zuweisungen.Add(new Zuweisung
            {
                MitarbeiterId = AusgewaehlterMitarbeiter.Id,
                SchichtId = AusgewaehlteSchicht.Id
            });

            AusgewaehlteSchicht.MitarbeiterNamen.Add(AusgewaehlterMitarbeiter.Name);
            StatusNachricht = "Zuweisung erfolgreich gespeichert.";
            OnPropertyChanged(nameof(SchichtListe));
        }

        private bool UeberschneidetSich(DateTime startA, DateTime endeA, DateTime startB, DateTime endeB)
        {
            return startA < endeB && startB < endeA;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
