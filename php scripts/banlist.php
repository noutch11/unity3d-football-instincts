<?php

include_once 'connect.php';
 
$sql = "SELECT Username, Type, Status FROM userinfotable WHERE `Type` = 'Regular Member' ORDER BY Status DESC";
$result = mysqli_query ($con, $sql);

$numrows = mysqli_num_rows ($result);
if ($numrows > 0) {
       for ($i = 0; $i < $numrows; $i++) {
           $row = mysqli_fetch_assoc ($result);
                echo $row['Username'] . " " . $row['Status'] . "|";
       }
	} else {
		echo "0 results";
	}

mysqli_close($con);

?>