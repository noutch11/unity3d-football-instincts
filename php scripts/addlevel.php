<?php


$Username = $_POST['Username'];
$Level = $_POST['Level'];

include_once 'connect.php'; 

mysqli_query ($con,"UPDATE userinfotable SET Level = '".$Level."' WHERE `Username` = '".$Username."'");


mysqli_close($con);


?>