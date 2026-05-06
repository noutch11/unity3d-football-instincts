<?php

$Username = $_POST['Username'];
$Email = $_POST['Email'];
$Password = $_POST['Password'];

include_once 'connect.php';

$checkusername = mysqli_query ($con,"SELECT * FROM userinfotable WHERE `Username` = '".$Username."'");
$checkemail = mysqli_query ($con,"SELECT * FROM userinfotable WHERE `Email` = '".$Email ."'");
$numrows1 = mysqli_num_rows ($checkusername);
$numrows2 = mysqli_num_rows ($checkemail);
if ($numrows1 == 0 && $numrows2 == 0) {
	$Password = md5 ($Password);
	$ins = mysqli_query ($con,"INSERT INTO `userinfotable` (`ID`, `Username`, `Email`, `Password`, `JoinDate`) VALUES ('', '".$Username."', '".$Email."', '".$Password."', '".date ("Y-m-d")."') ; ");
	if ($ins)
		echo "Success";
	else
		die ("Error: " . mysqli_error($con));
} else if ($numrows1 > 0 || $numrows2 > 0) {
	die ("This username or email already exists.");
}

mysqli_close($con);
?>