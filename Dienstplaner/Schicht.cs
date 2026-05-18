using System;
using System.Collections.Generic;

namespace Dienstplaner.Models
{
    public class Schicht
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Start { get; set; }

        public DateTime Ende { get; set; }

        public int BenoetigteMitarbeiter { get; set; }

        public string BenoetigteQualifikation { get; set; }

        // NEU: Anzeige der zugewiesenen Mitarbeiter
        public List<string> MitarbeiterNamen { get; set; }

        public Schicht()
        {
            MitarbeiterNamen = new List<string>();
        }
    }
}