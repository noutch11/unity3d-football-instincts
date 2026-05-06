<?php


$Username = $_POST['Username'];
$Coins = $_POST['Coins'];

include_once 'connect.php';

mysqli_query ($con,"UPDATE userinfotable SET Coins = '".$Coins."' WHERE `Username` = '".$Username."'");


mysqli_close($con);


?>