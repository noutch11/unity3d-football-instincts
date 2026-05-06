<?php


$Username = $_POST['Username'];
$LevelUpPrice = $_POST['LevelUpPrice'];

include_once 'connect.php'; 

mysqli_query ($con,"UPDATE userinfotable SET LevelUpPrice = '".$LevelUpPrice."' WHERE `Username` = '".$Username."'");


mysqli_close($con);


?>