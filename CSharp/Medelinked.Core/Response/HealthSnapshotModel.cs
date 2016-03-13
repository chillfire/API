using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	public class HealthSnapshotModel
	{
		public Guid SnapshotID { get; set; }
	    public DateTime CreateDate { get; set; }
	    public int Mood { get; set; }
	    public int PhysicalState { get; set; }
	    public string MoodDuration { get; set; }
	    public string MainConcern { get; set; }
	    public string MainSymptoms { get; set; }
	   	public bool IncludeWellnessData { get; set; }
	    public string MedicalRecordToBeIncluded { get; set; }
	    public string ProviderSharedWith { get; set; }
	}
}