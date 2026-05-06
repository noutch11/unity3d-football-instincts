<?php


$Username = $_POST['Username'];
$Status = $_POST['Status'];

include_once 'connect.php';

mysqli_query ($con,"UPDATE userinfotable SET Status = '".$Status."' WHERE `Username` = '".$Username."'");


mysqli_close($con);


?>