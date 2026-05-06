<?php

$Email = $_POST['Email'];
$Password = $_POST['Password'];

include_once 'connect.php';

$result = mysqli_query ($con,"SELECT * FROM userinfotable WHERE `Email` = '".$Email."'");
$numrows = mysqli_num_rows ($result);
if ($numrows == 0) {
	die ("Incorrect email");
} else {
    $Password = md5 ($Password);
	while ($row = mysqli_fetch_assoc ($result)) {
		$Status = $row ['Status'];
		$BanDate = $row ['BanDate'];
		if ($Password == $row['Password']) {
			if ($Status != "B") {
				echo ("Success");
			}
		else if ($Status == "B")
			die ("Your account has been banned by an administrator until $BanDate.");
		}
		else
			die ("Incorrect password");
	}
}
mysqli_close($con);
?>