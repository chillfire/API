using System;
using System.Collections.Generic;

namespace Medelinked.Core.Request
{
    public class SymptomsToCheck
    {
        public string Gender { get; set; }
        public string Region { get; set; }
        public string Symptoms { get; set; }
        public string AgeGroup { get; set; }
        public string Pregnant { get; set; }
        public List<int> SelectedSymptoms { get; set; }
        public int YearOfBirth { get; set; }
    }
}