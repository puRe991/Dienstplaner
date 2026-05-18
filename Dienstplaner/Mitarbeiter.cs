namespace Dienstplaner.Models
{
    public class Mitarbeiter
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Qualifikation { get; set; }

        public int WochenstundenLimit { get; set; }

        public bool IstAktiv { get; set; }
    }
}