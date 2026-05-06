<?php


$Username = $_POST['Username'];
$XP = $_POST['XP'];

include_once 'connect.php';

mysqli_query ($con,"UPDATE userinfotable SET XP = '".$XP."' WHERE `Username` = '".$Username."'");


mysqli_close($con);


?>