<?php

include_once 'connect.php';
 
$sql = "SELECT Username, Level FROM userinfotable ORDER BY Level DESC LIMIT 10";
$result = mysqli_query ($con, $sql);

$numrows = mysqli_num_rows ($result);
if ($numrows > 0) {
        echo "<table><tr><th>Username</th><th>Level</th></tr>";
	//output data
	while ($row = mysqli_fetch_assoc ($result)) {
		//echo ($row['Username'] . " " . $row['Level'] . "<br><br>");
                echo "<tr><td>" . $row['Username'] . "</td><td>" . $row['Level'] . "</td></tr>";
	 }
         echo "</table>";
	} else {
		echo "0 results";
	}

mysqli_close($con);

?>