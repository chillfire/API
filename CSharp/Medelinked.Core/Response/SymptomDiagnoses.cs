using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
    public class SymptomDiagnoses
    {
        public string Message { get; set; }
        public List<Diagnosis> Diagnoses { get; set; }
    }
}