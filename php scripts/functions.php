<?php
@ob_start();
session_start();
include_once '/home/u801994192/public_html/Global/connect.php'; $con = $con;
function loggedin() {
	if (isset($_SESSION['user_id']) && !empty ($_SESSION['user_id'])) {
		return true;
	} else {
		return false;
	}
}
if (loggedin()) {
	$my_id = $_SESSION ['user_id'];
	$user_query = mysqli_query ($con, "SELECT Username FROM userinfotable WHERE ID = '".$my_id."'");
	$row = mysqli_fetch_assoc($user_query);
	$Username = $row ['Username'];
}
?>