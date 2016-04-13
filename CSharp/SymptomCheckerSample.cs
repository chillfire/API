using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Medelinked.Core.Client;
using Medelinked.Core.Request;
using Medelinked.Core.Response;

class Program
{
	static void Main(string[] args)
	{
		CheckSymptoms("1976", "Female");
	}
	
	private async void CheckSymptoms(string YearOfBirth, string Gender)
	{
		
		//Get the list of symptoms
		List<HealthItem> symptoms = await MedelinkedHttpClient.GetSymptomList ();
		
		foreach(var item in symptoms)
		{
			Console.Write(item.ID.ToString + " - " + item.Name);
		}
		
		//Convert the symptoms into a list of integers
		SymptomsToCheck symptoms2Check = new SymptomsToCheck();
		symptoms2Check.SelectedSymptoms = new List<int>();
		symptoms2Check.SelectedSymptoms.Add(symptoms[1].ID);
		symptoms2Check.SelectedSymptoms.Add(symptoms[22].ID);
		symptoms2Check.AgeGroup = YearOfBirth;
		symptoms2Check.Gender = Gender;
					
		//Select the symptoms and request a diagnosis
		resultingDiagnoses = await MedelinkedHttpClient.CheckSymptoms (symptoms);
		int diagnosesID = 0;
		
		if (resultingDiagnoses != null)
		{
			//Show details of the possible diagnoses
			foreach(var d in resultingDiagnoses.Diagnoses)
			{
				diagnosesID = d.ID;
				Console.Write(d.ID + " - " + d.Name + " (" + d.SystemHeading + ")");
			}
			
			//Load information about one of the diagnosis
			var details = await MedelinkedHttpClient.GetDiagnosisDetails (diagnosisID);

			if (details != null) {
				
				Console.Write("Name: " + details.Name);
				Console.Write("Clinical Name: " + details.ProfName);
				Console.Write("Summary: " + details.DescriptionShort);
				Console.Write("Description: " + details.Description);
			}	
		}
		
	}
}