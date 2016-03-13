<html>
 <head>
		<script src="//ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
		<style>
        body {
            font-family: Arial, sans-serif, Tahoma;
        }
		</style>
		<title>Zaptag API Test</title>
	</head>
 <body>
<?php
$providerKey = "B20B9287JHK89s";
$data = array("Username" => "sampleuser@test.com", "Password" => "password:01!", "ProviderKey" => $providerKey); 
$params = array("UserCredentials" => $data);                                                                   
$data_string = json_encode($params);   
echo 'JSON: '.$data_string;                                                                                
$tmp_fname = tempnam("/temp", "COOKIE");

 
$ch = curl_init("https://app.medelinked/api/user/Authenticate");
curl_setopt($ch, CURLOPT_COOKIEJAR, $tmp_fname);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);                                                                      
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");                                                                     
curl_setopt($ch, CURLOPT_POSTFIELDS, $data_string);                                                                  
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);                                                                      
curl_setopt($ch, CURLOPT_HTTPHEADER, array(                                                                          
    "Content-Type: application/json",                                                                                
    "Content-Length: " .strlen($data_string))                                                                       
);                                                                                                                   

$result = curl_exec($ch);
echo 'Curl error: ' . curl_error($ch);
echo '<br />Result: ' . $result;
$output = json_decode($result);
?>
		<h1>Medelinked API Tester</h1>
		<table>			
			<tr>
				<td>
					<strong>Patient Name:</strong>
				</td>
				<td colspan="2">
					<span id="status"><?php print($output->d->Name); ?></span>
				</td>
			</tr>
			<tr>
				<td><strong>Record:</strong></td>
				<td>
					<table border="1">
						<tr>
							<td>
								<b>Record Type</b>
							</td>
							<td>
								<b>Title</b>
							</td>
							<td>
								<b>Record ID</b>
							</td>
						</tr>
<?php

$ch = curl_init("https://app.medelinked.com/api/user/records");                                                                      
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "GET");                                                                                                                                     
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($ch, CURLOPT_COOKIEFILE, $tmp_fname);                                                                      
curl_setopt($ch, CURLOPT_HTTPHEADER, array(                                                                          
    "Content-Type: application/json")                                                                       
);                                                                                                                   
 
$result = curl_exec($ch);
echo 'Curl error: ' . curl_error($ch);
echo '<br />Result: ' . $result;

$output = json_decode($result);

foreach ($output->d->Records as $rec) {
    echo '<tr>';
	echo '<td>'.$rec->Category.'</td>';
	echo '<td>'.$rec->Title.'</td>';
	echo '<td>'.$rec->RecordID.'</td>';
	echo '</tr>';
}
?>					</table>
				</td>
				<td></td>
			</td>        
		</table>
		<h2>Post Update Data</h2>
		<table border="1">
			<tr>
				<td>
					<b>Record Type</b>
				</td>
				<td>
					<b>Title</b>
				</td>
				<td>
					<b>Date</b>
				</td>
				<td>
					<b>
						Description
					</b>
				</td>
			</tr>
				
<?php

$params2 = array("What" => "Dust Allergy", "When" => "26/05/2013", "Reaction" => "Rash", "First" => "Childhood");
$params3 = array("Name" => "Test.jpg", "Base64FileContents" => "iVBORw0KGgoAAAAEQ2dCSVAAIAYsuHdmAAAADUlIRFIAAABQAAAAMggGAAAAyy1c/AAAACBjSFJNAAB6JQAAgIMAAPn/AACA6QAAdTAAAOpgAAA6mAAAF2+SX8VGAAAACXBIWXMAAC4hAAAuIQEHW/z/AAAAGnRFWHRTb2Z0d2FyZQBQYWludC5ORVQgdjMuNS4xMUfzQjcAAACzSURBVO3YrQ3DMBCA0cBukL3Cs0VH8BwlQeYlQZWyQaWUmBSUhnQCF6dqpP5JNngnfSzogdzJzbHbZX1fAwEgQIAABRAgQIA1NPZtvl/P+XlupwHgO6UY8tZcDnuAvwCmGAACBFgP4Ni3OcWwapmnTcBlnlbf1vBPLAr4att+OqW3c1HAfw1AgAABWiLOGGeMQxogQICeszyoCiBAgAAFECBAgAIIECBAAQQIEKAAAgQIUAABVtQDl8njfQAAAABJRU5ErkJggg==");                                                                   
$data2 = array("Type" => "Record", "Category" => "Allergy", "Date" => "26/05/2013", "Title" => "Dust Allergy", "Description" => $params2, "ProviderKey" => $providerKey, "Files" => $params3); 
$allData = array("RecordDetails" => $data2);
$data_string2 = '{"RecordDetails":{"Type":"Record","Category":"Allergy","Date":"25\/05\/2013","Title":"Dust Allergy","Description":{"What":"Dust Allergy","Reaction":"Rash","First":"Childhood"},"ProviderKey":"QjIwQjkyODc=","Files":[{"Name":"Test5.jpg","Base64FileContents":"iVBORw0KGgoAAAAEQ2dCSVAAIAYsuHdmAAAADUlIRFIAAABQAAAAMggGAAAAyy1c\/AAAACBjSFJNAAB6JQAAgIMAAPn\/AACA6QAAdTAAAOpgAAA6mAAAF2+SX8VGAAAACXBIWXMAAC4hAAAuIQEHW\/z\/AAAAGnRFWHRTb2Z0d2FyZQBQYWludC5ORVQgdjMuNS4xMUfzQjcAAACzSURBVO3YrQ3DMBCA0cBukL3Cs0VH8BwlQeYlQZWyQaWUmBSUhnQCF6dqpP5JNngnfSzogdzJzbHbZX1fAwEgQIAABRAgQIA1NPZtvl\/P+XlupwHgO6UY8tZcDnuAvwCmGAACBFgP4Ni3OcWwapmnTcBlnlbf1vBPLAr4att+OqW3c1HAfw1AgAABWiLOGGeMQxogQICeszyoCiBAgAAFECBAgAIIECBAAQQIEKAAAgQIUAABVtQDl8njfQAAAABJRU5ErkJggg=="}]}}';                                                                                   
echo 'json: '.$data_string2;
 
$ch = curl_init("https://app.medelinked.com/api/user/AddRecord");
curl_setopt($ch, CURLOPT_COOKIEFILE, $tmp_fname);                                                                    
curl_setopt($ch, CURLOPT_CUSTOMREQUEST, "POST");                                                                     
curl_setopt($ch, CURLOPT_POSTFIELDS, $data_string2);                                                                  
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);                                                                      
curl_setopt($ch, CURLOPT_HTTPHEADER, array(                                                                          
    "Content-Type: application/json",                                                                                
    "Content-Length: " .strlen($data_string2))                                                                       
);                                                                                                                   
 
$result = curl_exec($ch);
$output = json_decode($result);

foreach ($output->d->Records as $rec) {
    echo '<tr>';
	echo '<td>'.$rec->Category.'</td>';
	echo '<td>'.$rec->Title.'</td>';
	echo '<td>'.$rec->Date.'</td>';
	echo '<td>'.$rec->Description->Description.'</td>';
	echo '</tr>';
}


?>        
		</table>
	</body>
</html>