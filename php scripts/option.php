<?php
@ob_start();
session_start();

include_once '/home/u801994192/public_html/Global/connect.php';
include_once 'functions.php';
include_once 'Admincheck.php';
$con = $con;
$u_id = $_GET['u_id'];
$u_status = $_GET['status'];
$u_type = $_GET['type'];
$BanDate = $_GET ['ban_date'];

if ($u_status == "A") {
	mysqli_query ($con, "UPDATE `userinfotable` SET `Status` = 'B', `BanDate` = '".$BanDate."' WHERE `ID` = '".$u_id."'");
	header ('location: admin.php?status=user');
} else if ($u_status == "B") {
	mysqli_query ($con, "UPDATE `userinfotable` SET `Status` = 'A', `BanDate` = '3000-01-01' WHERE `ID` = '".$u_id."'");
	header ('location: admin.php?status=user');
}

if ($u_type == "Regular") {
	mysqli_query ($con, "UPDATE `userinfotable` SET `Type` = 'Admin' WHERE `ID` = '".$u_id."'");
	header ('location: admin.php?type=user');
} else if ($u_type == "Admin") {
	mysqli_query ($con, "UPDATE `userinfotable` SET `Type` = 'Regular' WHERE `ID` = '".$u_id."'");
	header ('location: admin.php?type=user');
}

?>