using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
    public class Diagnosis
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string KnowledgeWindowURL { get; set; }
        public string SystemHeading { get; set; }
        public float Weightage { get; set; }
        public string SnomedID { get; set; }
        public string ICD9ID { get; set; }
        public bool RedFlag { get; set; }
        public bool IsCommon { get; set; }
    }

    public class DiagnosisDetail
    {
        /// <summary>
        /// Issue name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Issue professional name
        /// </summary>
        public string ProfName { get; set; }
        
        /// <summary>
        /// Issue short description
        /// </summary>
        public string DescriptionShort { get; set; }
        
        /// <summary>
        /// Issue description
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Medical condition
        /// </summary>
        public string MedicalCondition { get; set; }
        
        /// <summary>
        /// Treatment description
        /// </summary>
        public string TreatmentDescription { get; set; }
        
        /// <summary>
        /// Issue synonyms (comma separated)
        /// </summary>
        public string Synonyms { get; set; }
        
        /// <summary>
        /// Possible symptoms (comma separated)
        /// </summary>
        public string PossibleSymptoms { get; set; }

        /// <summary>
        /// Information message
        /// </summary>
        public string Message { get; set; }
    }
}