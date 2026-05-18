using System;
using System.Collections.Generic;

namespace Dienstplaner.Models
{
    public class Schicht
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abteilung { get; set; }
        public DateTime Start { get; set; }
        public DateTime Ende { get; set; }
        public int BenoetigteMitarbeiter { get; set; }
        public string BenoetigteQualifikation { get; set; }
        public List<string> MitarbeiterNamen { get; set; } = new List<string>();
    }
}
