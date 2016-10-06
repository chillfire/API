using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Medelinked.Core.Client;
using Medelinked.Core.Request;
using Medelinked.Core.Response;

class SDKSample
{
	
	public async void AddUser()
	{
		Register UserDetails = new Register ();
		UserDetails.Email = "jas.singh@tekasco.co.uk";
		UserDetails.Password = "Singh010478"; //Set the password to be their Surname followed by the date of birth (ddMMyy)
		UserDetails.ConfirmPassword = "Singh010478";
		UserDetails.ProviderKey = ""; //This will need to be set per health provider - we will need a way to work this out
		UserDetails.Forename = "Jas";
		UserDetails.Surname = "Singh";
		UserDetails.Country = "United Kingdom";
		UserDetails.DOB = "01/04/1978"; //All date formats are in dd/MM/yyyy
		UserDetails.NationalHealthNumber = "123-456-7890"; //Equivalent to the NHS Number (not country specific)
		UserDetails.Sex = "Male";
		UserDetails.SendRegistrationEmail = true; //Will send a registeration email depending on the Provider Key specified

		//Check we have all of the core data
		if (String.IsNullOrEmpty (UserDetails.Email) || String.IsNullOrEmpty (UserDetails.Forename) || String.IsNullOrEmpty (UserDetails.Surname) || String.IsNullOrEmpty (UserDetails.Password) || String.IsNullOrEmpty (UserDetails.ConfirmPassword) || String.IsNullOrEmpty (UserDetails.NationalHealthNumber)) {
			//Throw an error
			//UIAlertView _error = new UIAlertView ("My Health", "Please complete all of the fields to change the password.", null, "Ok", null);
			//_error.Show ();
		}
		else {
			
			//Registration is done with the User Http Client as the Provider Key is used to determine the health provider
			User currentUser = await MedelinkedHttpClient.RegisterUserAsync (UserDetails);
			
			//Clearly something went wrong
			if (currentUser == null) {
				//UIAlertView _error = new UIAlertView ("My Health", "Unable to connect to Medelinked. Please try again.", null, "Ok", null);
				//_error.Show ();
			} else {
				
				//Again something went wrong
				if (currentUser.Message != "OK") {

					//UIAlertView _error = new UIAlertView ("My Health", currentUser.Message, null, "Ok", null);
					//_error.Show ();
				} else {
					UserState.LoggedInUser = currentUser;

					//Account created successfully
					//UIAlertView _error = new UIAlertView ("My Health", "Your account has been created successfully.", null, "Ok", null);
					//_error.Show ();

					//Redirect
					//LoadProfileScreen ();
				}
			}
		}
	}
	
	public async void ConnectAsProvider()
	{
		ProviderLoginModel provider = new ProviderLoginModel();
		provider.ProviderID = "ProviderKeyForProvider";
		provider.Passcode = "Password/codeForProvider";  //These are setup by Medelinked
		
		HealthProviders currentProvider = await MedelinkedProviderHttpClient.ConnectAsync (provider);
			
		//Clearly something went wrong
		if (currentProvider == null) {
			//UIAlertView _error = new UIAlertView ("My Health", "Unable to connect to Medelinked. Please try again.", null, "Ok", null);
			//_error.Show ();
		} else {
			
			//Again something went wrong
			if (currentProvider.Message != "OK") {

				//UIAlertView _error = new UIAlertView ("My Health", currentUser.Message, null, "Ok", null);
				//_error.Show ();
			} else {
				//All good and now can call the Provider API
			}
		}
	}
	
	public async void AddRecord()
	{
		//Connect as Provider if not done already - initialises the HTTP Client
		ConnectAsProvider();
		string category = "Medication";
		List<Record> records = new List<Record>();
		
		//Add a record
		Record newRecord = new Record();
		newRecord.Description = new RecordDescription ();
		newRecord.Files = new System.Collections.Generic.List<HealthFile> ();
		newRecord.Date = DateTime.Now.ToString ("dd/MM/yyyy"); //Date the record was created
		
		if (category == "Medication")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Aspirin";
			
			newRecord.Description.Name = "Aspirin";
			newRecord.Description.DoseValue = "1 tablet";
			newRecord.Description.Strength = "325mg";
			newRecord.Description.Frequency = "3 times a day";
			newRecord.Description.Instructions = "Take with water on empty stomach";
			newRecord.Description.Reaction = "Could cause stomach pain";
			newRecord.Description.TakeUntil = "Complete course";
		}
		else if (category == "Allergy")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Pollen Allergy";
			newRecord.Description.What = "Pollen Allergy";
			newRecord.Description.Reaction = "Rash";
			newRecord.Description.FirstObserved = "15/11/1995";
			newRecord.ExternalID = "16573532" //ID from external EHR or record system
			
			newRecord.Codes.Add(new MedicalCode() {
				Scheme = "SNOMED-CT",
				Code = "300910009",
				Origin = "Allergy to pollen (disorder)"
			});
		 

		}
		else if (category == "Condition")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Diabetes";
			newRecord.Description.Name = "Diabetes";
			newRecord.Description.OnsetDate = "12/02/2015";
			newRecord.Description.Status = "Managed through diet and exercise";
			newRecord.Description.LastEpisode = "23/09/2015";
		}
		else if (category == "Immunisation")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Influenza Vaccine";
			newRecord.Description.What = "Influenza Vaccine";
			newRecord.Description.When = "15/12/2015";
			newRecord.Description.Who = "Cape Road Surgery";
			newRecord.Description.Expiry = "14/12/2016";
		}
		else if (category == "Tests")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Full Blood Count";
			newRecord.Description.Name = "Full Blood Count";
			newRecord.Description.When = "15/12/2015";
			newRecord.Description.Who = "Cape Road Surgery";
			newRecord.Description.Status = "low haemoglobin";
		}
        else if (category == "Procedure")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Appendectomy";
			newRecord.Description.Purpose = "Removal of Appendix";
            		newRecord.Description.What = "Appendectomy";
			newRecord.Description.Doctor = "Dr Green";
			newRecord.Description.Who = "Warwick Hospital";
			newRecord.Description.Complications = "none";
            		newRecord.Description.Outcome = "Successfully removed vermiform appendix.";
		}
		else if (category == "Problem" || category == "Notes" || category == "Consultation")
		{
			newRecord.Category = category;
			newRecord.Type = "Record";
			newRecord.Title = "Consultation with Dr Green";
			newRecord.Description.Description = "Reviewed diabetes and further tests ordered.";
		}
		else if (category == "Blood Pressure")
		{
			newRecord.Category = category;
			newRecord.Type = "Wellness";
			newRecord.Title = "Blood Pressure Reading";
			newRecord.Description.Time = "11:15";
			newRecord.Description.Systolic = "116";
			newRecord.Description.Diastolic = "76";
		}
		else if (category == "Blood Glucose")
		{
			newRecord.Category = category;
			newRecord.Type = "Wellness";
			newRecord.Title = "Blood Glucose Reading";
			newRecord.Description.Time = "11:15";
			newRecord.Description.Reading = "4.8"; //(mmol/L)
		}
		else if (category == "Cholesterol")
		{
			newRecord.Category = category;
			newRecord.Type = "Wellness";
			newRecord.Title = "Blood Glucose Reading";
			newRecord.Description.Time = "11:15";
			newRecord.Description.Reading = "4.8"; //(mmol/L)
		}
		
		records.Add(newRecord);
		HealthData data = await MedelinkedProviderHttpClient.CommitRecords (records);

		if (data == null) {
			//Something has gone wrong	
		} else {
			
			if (data.Message == "OK") {

				//All good
			}
			else {
				//Some sort of error that can be read from data.Message property
			}
		}
	}
}
