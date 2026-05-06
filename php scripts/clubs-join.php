<?php

$Name = $_POST['ClubName'];
$Member = $_POST['Member'];

include_once 'connect.php';

$checkname = mysqli_query ($con,"SELECT * FROM `clubs` WHERE `Name` = '".$Name."'");
$checkclubs = mysqli_query ($con,"SELECT * FROM `userinfotable` WHERE `Username` = '".$Member."'");
$numrows1 = mysqli_num_rows ($checkname);
$numrows2 = mysqli_num_rows ($checkclubs);

if ($numrows2 > 0 && $numrows1 > 0) {
	while ($row1 = mysqli_fetch_assoc ($checkclubs)) {
		if ($row1 ['Club'] != "None")
			echo "You cannot join a club as you are already a part of one.";
else if ($row1 ['Club'] == "None") {
	$updC = mysqli_query ($con, "UPDATE `userinfotable` SET `Club` = '".$Name."' WHERE `Username` = '".$Member."'");
	if ($updC) {
		while ($row2 = mysqli_fetch_assoc ($checkname) {
			$row2['Members'] += 1;
		}
		echo "Successfully join the club!";
	}
	else
		die ("Error creating a club: " . mysqli_error($con));
	}
 }
} else {
	echo "Unknown error.";
}

mysqli_close($con);

?>