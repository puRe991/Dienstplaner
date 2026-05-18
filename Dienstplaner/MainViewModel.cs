using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Dienstplaner.Models;
using Dienstplaner.Helpers;

namespace Dienstplaner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Mitarbeiter> MitarbeiterListe { get; set; }
        public ObservableCollection<Schicht> SchichtListe { get; set; }
        public ObservableCollection<Zuweisung> Zuweisungen { get; set; }

        // Auswahl
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

        // Input Mitarbeiter
        private string _neuerMitarbeiterName;
        public string NeuerMitarbeiterName
        {
            get { return _neuerMitarbeiterName; }
            set { _neuerMitarbeiterName = value; OnPropertyChanged(); }
        }

        // Input Schicht
        private string _neueSchichtName;
        public string NeueSchichtName
        {
            get { return _neueSchichtName; }
            set { _neueSchichtName = value; OnPropertyChanged(); }
        }

        // Commands
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

            // Testdaten
            MitarbeiterListe.Add(new Mitarbeiter { Id = 1, Name = "Max Mustermann" });
            MitarbeiterListe.Add(new Mitarbeiter { Id = 2, Name = "Anna Beispiel" });

            SchichtListe.Add(new Schicht
            {
                Id = 1,
                Name = "Frühschicht",
                Start = DateTime.Now,
                Ende = DateTime.Now.AddHours(8),
                BenoetigteMitarbeiter = 2
            });
        }

        // Mitarbeiter hinzufügen
        private void HinzufuegenMitarbeiter(object obj)
        {
            if (string.IsNullOrWhiteSpace(NeuerMitarbeiterName))
                return;

            MitarbeiterListe.Add(new Mitarbeiter
            {
                Id = MitarbeiterListe.Count + 1,
                Name = NeuerMitarbeiterName
            });

            NeuerMitarbeiterName = "";
        }

        // Schicht hinzufügen
        private void HinzufuegenSchicht(object obj)
        {
            if (string.IsNullOrWhiteSpace(NeueSchichtName))
                return;

            SchichtListe.Add(new Schicht
            {
                Id = SchichtListe.Count + 1,
                Name = NeueSchichtName,
                Start = DateTime.Now,
                Ende = DateTime.Now.AddHours(8),
                BenoetigteMitarbeiter = 1
            });

            NeueSchichtName = "";
        }

        // Zuweisung mit Regeln
        private void Zuweisen(object obj)
        {
            if (AusgewaehlterMitarbeiter == null || AusgewaehlteSchicht == null)
                return;

            // Doppelprüfung
            foreach (var z in Zuweisungen)
            {
                if (z.MitarbeiterId == AusgewaehlterMitarbeiter.Id &&
                    z.SchichtId == AusgewaehlteSchicht.Id)
                    return;
            }

            // Kapazitätsprüfung
            int belegung = 0;

            foreach (var z in Zuweisungen)
            {
                if (z.SchichtId == AusgewaehlteSchicht.Id)
                    belegung++;
            }

            if (belegung >= AusgewaehlteSchicht.BenoetigteMitarbeiter)
                return;

            // Speichern
            Zuweisungen.Add(new Zuweisung
            {
                MitarbeiterId = AusgewaehlterMitarbeiter.Id,
                SchichtId = AusgewaehlteSchicht.Id
            });

            // Anzeige
            AusgewaehlteSchicht.MitarbeiterNamen.Add(AusgewaehlterMitarbeiter.Name);
            OnPropertyChanged(nameof(SchichtListe));
        }

        // PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}