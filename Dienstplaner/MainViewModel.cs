using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
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
        public ObservableCollection<string> Abteilungen { get; set; }

        public ICollectionView MitarbeiterView { get; }
        public ICollectionView SchichtView { get; }

        public IEnumerable<string> VerfuegbareQualifikationen => MitarbeiterListe.Select(m => m.Qualifikation).Where(q => !string.IsNullOrWhiteSpace(q)).Distinct().OrderBy(q => q);

        private string _mitarbeiterFilterText;
        public string MitarbeiterFilterText { get => _mitarbeiterFilterText; set { _mitarbeiterFilterText = value; OnPropertyChanged(); MitarbeiterView.Refresh(); } }

        private string _gewaehlteAbteilungFilter = "Alle";
        public string GewaehlteAbteilungFilter { get => _gewaehlteAbteilungFilter; set { _gewaehlteAbteilungFilter = value; OnPropertyChanged(); MitarbeiterView.Refresh(); SchichtView.Refresh(); } }

        private Mitarbeiter _ausgewaehlterMitarbeiter;
        public Mitarbeiter AusgewaehlterMitarbeiter { get => _ausgewaehlterMitarbeiter; set { _ausgewaehlterMitarbeiter = value; OnPropertyChanged(); } }
        private Schicht _ausgewaehlteSchicht;
        public Schicht AusgewaehlteSchicht { get => _ausgewaehlteSchicht; set { _ausgewaehlteSchicht = value; OnPropertyChanged(); } }

        public string NeuerMitarbeiterName { get; set; }
        public string NeuerMitarbeiterQualifikation { get; set; }
        public string NeueMitarbeiterAbteilung { get; set; }
        public int NeuesMitarbeiterWochenlimit { get; set; } = 40;

        public string NeueSchichtName { get; set; }
        public DateTime NeueSchichtStart { get; set; } = DateTime.Today.AddHours(6);
        public DateTime NeueSchichtEnde { get; set; } = DateTime.Today.AddHours(14);
        public int NeueSchichtKapazitaet { get; set; } = 4;
        public string NeueSchichtQualifikation { get; set; }
        public string NeueSchichtAbteilung { get; set; }

        private string _statusNachricht;
        public string StatusNachricht { get => _statusNachricht; set { _statusNachricht = value; OnPropertyChanged(); } }

        public int GesamtMitarbeiter => MitarbeiterListe.Count;
        public int AktiveMitarbeiter => MitarbeiterListe.Count(m => m.IstAktiv);
        public int GesamtSchichten => SchichtListe.Count;
        public int OffeneBedarfe => SchichtListe.Sum(s => Math.Max(0, s.BenoetigteMitarbeiter - Zuweisungen.Count(z => z.SchichtId == s.Id)));

        public ICommand MitarbeiterHinzufuegenCommand { get; }
        public ICommand SchichtHinzufuegenCommand { get; }
        public ICommand ZuweisenCommand { get; }

        public MainViewModel()
        {
            MitarbeiterListe = new ObservableCollection<Mitarbeiter>();
            SchichtListe = new ObservableCollection<Schicht>();
            Zuweisungen = new ObservableCollection<Zuweisung>();
            Abteilungen = new ObservableCollection<string> { "Alle", "Pflege", "Intensiv", "Notaufnahme", "OP", "Radiologie", "Verwaltung" };

            MitarbeiterView = CollectionViewSource.GetDefaultView(MitarbeiterListe);
            MitarbeiterView.Filter = MitarbeiterFilter;
            SchichtView = CollectionViewSource.GetDefaultView(SchichtListe);
            SchichtView.Filter = SchichtFilter;

            MitarbeiterHinzufuegenCommand = new RelayCommand(HinzufuegenMitarbeiter);
            SchichtHinzufuegenCommand = new RelayCommand(HinzufuegenSchicht);
            ZuweisenCommand = new RelayCommand(Zuweisen);

            SeedMitarbeiter(70);
            SeedSchichten(28);
            UpdateDashboard();
            StatusNachricht = "Planungssystem bereit (Demo mit 70 Mitarbeitern geladen).";
        }

        private bool MitarbeiterFilter(object obj)
        {
            var m = obj as Mitarbeiter;
            if (m == null) return false;
            bool textOk = string.IsNullOrWhiteSpace(MitarbeiterFilterText) || m.Name.IndexOf(MitarbeiterFilterText, StringComparison.OrdinalIgnoreCase) >= 0;
            bool abtOk = GewaehlteAbteilungFilter == "Alle" || string.Equals(m.Abteilung, GewaehlteAbteilungFilter, StringComparison.OrdinalIgnoreCase);
            return textOk && abtOk;
        }

        private bool SchichtFilter(object obj)
        {
            var s = obj as Schicht;
            if (s == null) return false;
            return GewaehlteAbteilungFilter == "Alle" || string.Equals(s.Abteilung, GewaehlteAbteilungFilter, StringComparison.OrdinalIgnoreCase);
        }

        private void HinzufuegenMitarbeiter(object obj)
        {
            if (string.IsNullOrWhiteSpace(NeuerMitarbeiterName) || string.IsNullOrWhiteSpace(NeueMitarbeiterAbteilung)) { StatusNachricht = "Name und Abteilung sind Pflichtfelder."; return; }
            if (NeuesMitarbeiterWochenlimit <= 0) { StatusNachricht = "Wochenstundenlimit muss > 0 sein."; return; }

            MitarbeiterListe.Add(new Mitarbeiter
            {
                Id = MitarbeiterListe.Count + 1,
                Name = NeuerMitarbeiterName.Trim(),
                Qualifikation = (NeuerMitarbeiterQualifikation ?? string.Empty).Trim(),
                Abteilung = NeueMitarbeiterAbteilung.Trim(),
                WochenstundenLimit = NeuesMitarbeiterWochenlimit,
                IstAktiv = true
            });

            NeuerMitarbeiterName = string.Empty; NeuerMitarbeiterQualifikation = string.Empty; NeuesMitarbeiterWochenlimit = 40; OnPropertyChanged(nameof(NeuerMitarbeiterName)); OnPropertyChanged(nameof(NeuerMitarbeiterQualifikation)); OnPropertyChanged(nameof(NeuesMitarbeiterWochenlimit));
            UpdateDashboard();
            StatusNachricht = "Mitarbeiter hinzugefügt.";
        }

        private void HinzufuegenSchicht(object obj)
        {
            if (string.IsNullOrWhiteSpace(NeueSchichtName) || string.IsNullOrWhiteSpace(NeueSchichtAbteilung)) { StatusNachricht = "Schichtname und Abteilung sind Pflichtfelder."; return; }
            if (NeueSchichtEnde <= NeueSchichtStart) { StatusNachricht = "Ende muss nach Start liegen."; return; }
            if (NeueSchichtKapazitaet <= 0) { StatusNachricht = "Kapazität muss > 0 sein."; return; }

            SchichtListe.Add(new Schicht { Id = SchichtListe.Count + 1, Name = NeueSchichtName.Trim(), Start = NeueSchichtStart, Ende = NeueSchichtEnde, BenoetigteMitarbeiter = NeueSchichtKapazitaet, BenoetigteQualifikation = (NeueSchichtQualifikation ?? string.Empty).Trim(), Abteilung = NeueSchichtAbteilung.Trim() });
            NeueSchichtName = string.Empty; NeueSchichtKapazitaet = 4; NeueSchichtQualifikation = string.Empty; OnPropertyChanged(nameof(NeueSchichtName)); OnPropertyChanged(nameof(NeueSchichtKapazitaet)); OnPropertyChanged(nameof(NeueSchichtQualifikation));
            UpdateDashboard();
            StatusNachricht = "Schicht hinzugefügt.";
        }

        private void Zuweisen(object obj)
        {
            if (AusgewaehlterMitarbeiter == null || AusgewaehlteSchicht == null) { StatusNachricht = "Bitte Mitarbeiter und Schicht auswählen."; return; }
            if (!AusgewaehlterMitarbeiter.IstAktiv) { StatusNachricht = "Mitarbeiter ist nicht aktiv."; return; }
            if (!string.Equals(AusgewaehlterMitarbeiter.Abteilung, AusgewaehlteSchicht.Abteilung, StringComparison.OrdinalIgnoreCase)) { StatusNachricht = "Abteilung passt nicht zur Schicht."; return; }
            if (!string.IsNullOrWhiteSpace(AusgewaehlteSchicht.BenoetigteQualifikation) && !string.Equals(AusgewaehlterMitarbeiter.Qualifikation, AusgewaehlteSchicht.BenoetigteQualifikation, StringComparison.OrdinalIgnoreCase)) { StatusNachricht = "Qualifikation passt nicht."; return; }
            if (Zuweisungen.Any(z => z.MitarbeiterId == AusgewaehlterMitarbeiter.Id && z.SchichtId == AusgewaehlteSchicht.Id)) { StatusNachricht = "Bereits zugewiesen."; return; }
            if (Zuweisungen.Count(z => z.SchichtId == AusgewaehlteSchicht.Id) >= AusgewaehlteSchicht.BenoetigteMitarbeiter) { StatusNachricht = "Schicht voll besetzt."; return; }

            var bestehendeSchichten = SchichtListe.Where(s => Zuweisungen.Any(z => z.MitarbeiterId == AusgewaehlterMitarbeiter.Id && z.SchichtId == s.Id)).ToList();
            if (bestehendeSchichten.Any(s => UeberschneidetSich(s.Start, s.Ende, AusgewaehlteSchicht.Start, AusgewaehlteSchicht.Ende))) { StatusNachricht = "Zeitkonflikt vorhanden."; return; }

            double stunden = bestehendeSchichten.Sum(s => (s.Ende - s.Start).TotalHours) + (AusgewaehlteSchicht.Ende - AusgewaehlteSchicht.Start).TotalHours;
            if (stunden > AusgewaehlterMitarbeiter.WochenstundenLimit) { StatusNachricht = "Wochenstundenlimit überschritten."; return; }

            Zuweisungen.Add(new Zuweisung { MitarbeiterId = AusgewaehlterMitarbeiter.Id, SchichtId = AusgewaehlteSchicht.Id });
            AusgewaehlteSchicht.MitarbeiterNamen.Add(AusgewaehlterMitarbeiter.Name);
            UpdateDashboard();
            StatusNachricht = "Zuweisung erfolgreich.";
            OnPropertyChanged(nameof(SchichtListe));
        }

        private static bool UeberschneidetSich(DateTime startA, DateTime endeA, DateTime startB, DateTime endeB) => startA < endeB && startB < endeA;

        private void SeedMitarbeiter(int count)
        {
            var abt = new[] { "Pflege", "Intensiv", "Notaufnahme", "OP", "Radiologie", "Verwaltung" };
            var qual = new[] { "Pflege", "Intensiv", "Anästhesie", "OP", "Röntgen", "Koordination" };
            for (int i = 1; i <= count; i++)
            {
                MitarbeiterListe.Add(new Mitarbeiter { Id = i, Name = $"Mitarbeiter {i:000}", Abteilung = abt[i % abt.Length], Qualifikation = qual[i % qual.Length], WochenstundenLimit = 32 + (i % 3) * 8, IstAktiv = true });
            }
        }

        private void SeedSchichten(int days)
        {
            var abt = new[] { "Pflege", "Intensiv", "Notaufnahme", "OP", "Radiologie" };
            int id = 1;
            for (int d = 0; d < days; d++)
            {
                DateTime tag = DateTime.Today.AddDays(d);
                foreach (var a in abt)
                {
                    SchichtListe.Add(new Schicht { Id = id++, Name = $"{a} Früh", Abteilung = a, Start = tag.AddHours(6), Ende = tag.AddHours(14), BenoetigteMitarbeiter = 3, BenoetigteQualifikation = a == "OP" ? "OP" : a == "Radiologie" ? "Röntgen" : "Pflege" });
                    SchichtListe.Add(new Schicht { Id = id++, Name = $"{a} Spät", Abteilung = a, Start = tag.AddHours(14), Ende = tag.AddHours(22), BenoetigteMitarbeiter = 3, BenoetigteQualifikation = a == "OP" ? "OP" : a == "Radiologie" ? "Röntgen" : "Pflege" });
                }
            }
        }

        private void UpdateDashboard()
        {
            OnPropertyChanged(nameof(GesamtMitarbeiter)); OnPropertyChanged(nameof(AktiveMitarbeiter)); OnPropertyChanged(nameof(GesamtSchichten)); OnPropertyChanged(nameof(OffeneBedarfe)); OnPropertyChanged(nameof(VerfuegbareQualifikationen));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
