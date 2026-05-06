<?php


$Email = $_POST['Email'];

include_once 'connect.php';

$sql = "SELECT Username FROM userinfotable WHERE `Email` = '".$Email."'";
$result = mysqli_query ($con, $sql);

$numrows = mysqli_num_rows ($result);
if ($numrows > 0) {
	//output data
	while ($row = mysqli_fetch_assoc ($result)) {
		echo ($row['Username']);
	 }
	} else {
		echo "0 results";
	}

mysqli_close($con);


?>