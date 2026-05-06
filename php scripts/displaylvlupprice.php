<?php


$Username = $_POST['Username'];

include_once 'connect.php';

$sql = "SELECT LevelUpPrice FROM userinfotable WHERE `Username` = '".$Username."'";
$result = mysqli_query ($con, $sql);

$numrows = mysqli_num_rows ($result);
if ($numrows > 0) {
	//output data
	while ($row = mysqli_fetch_assoc ($result)) {
		echo ($row['LevelUpPrice']);
	 }
	} else {
		echo "0 results";
	}

mysqli_close($con);


?>