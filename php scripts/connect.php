<?php
$con = mysqli_connect ("files.000webhost.com","id4924295_fidb","soccer11", "id4924295_fiaccounts") or ("Cannot connect!" . mysqli_error($con));
if (!$con)
	die('Could not connect: ' . mysqli_error($con));
mysqli_select_db ($con, "id4924295_fiaccounts") or die ("Could not load the database: " . mysqli_error($con));
?>