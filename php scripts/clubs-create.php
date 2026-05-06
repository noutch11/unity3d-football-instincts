<?php

$Name = $_POST['ClubName'];
$Abbreviation = $_POST['Abbreviation'];
$Owner = $_POST['Owner'];

include_once 'connect.php'; 

$checkname = mysqli_query ($con,"SELECT * FROM `clubs` WHERE `Name` = '".$Name."'");
$checkclubs = mysqli_query ($con,"SELECT * FROM `userinfotable` WHERE `Username` = '".$Owner."'");
$numrows1 = mysqli_num_rows ($checkname);
$numrows2 = mysqli_num_rows ($checkclubs);

if ($numrows2 > 0) {
	while ($row = mysqli_fetch_assoc ($checkclubs)) {
		if ($row ['Club'] != "None")
			echo "You cannot create a club as you are already a part of one.";
else if ($row ['Club'] == "None") {
if ($numrows1 == 0) {
	$ins = mysqli_query ($con,"INSERT INTO `clubs` (`ID`, `Name`, `Abbreviation`, `Owner`, `Members`) VALUES ('', '".$Name."', '".$Abbreviation."', '".$Owner."', 1) ; ");
	if ($ins) {
		mysqli_query ($con, "UPDATE `userinfotable` SET `Club` = '".$Name."' WHERE `Username` = '".$Owner."'");
		echo "Successfully created your club!";
	}
	else
		die ("Error creating a club: " . mysqli_error($con));
} else if ($numrows1 > 0) {
	die ("This club name already exists.");
   }
  }
 }
} else {
	echo "Unknown error.";
}
mysqli_close($con);

?>