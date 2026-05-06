<?php


$Username = $_POST['Username'];
$Type = $_POST['Type'];

include_once 'connect.php';

mysqli_query ($con,"UPDATE userinfotable SET Type = '".$Type."' WHERE `Username` = '".$Username."'");


mysqli_close($con);


?>