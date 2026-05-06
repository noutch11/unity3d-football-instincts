<?php
$Name = $_POST['ClubName'];

include_once 'connect.php'; 

$sql = "SELECT Name, Owner, Members FROM clubs WHERE Name = '".$Name."'";
$result = mysqli_query ($con, $sql);

$numrows = mysqli_num_rows ($result);
if ($numrows > 0) {
	//output data
	while ($row = mysqli_fetch_assoc ($result)) {
		echo ($row['Name'] . "\t\t" . $row['Owner'] . "\t\t" . $row ['Members'] . "\n\n");
	 }
	} else {
		echo "No clubs exist with this name.";
	}

mysqli_close($con);

?>