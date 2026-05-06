<?php
@ob_start();
session_start();

?>
<html>

<head>
<title>Admin Panel - Profile</title>
<link rel="stylesheet" type="text/css" href="styles.css">
</head>

<body>
<?php include_once '/home/u801994192/public_html/Global/connect.php';
include_once 'functions.php';
include_once 'title_bar.php';
include_once 'Admincheck.php';
	$my_id = $_SESSION ['user_id'];
	$user_query = mysqli_query ($con, "SELECT Username FROM userinfotable WHERE ID = '".$my_id."'");
	$row = mysqli_fetch_assoc($user_query);
	$Username = $row ['Username'];
?>


<h3>Profile:</h3>

<p>You are logged in as <b><i><?php echo $Username?></i></b></p>
<?php
echo "<a href='admin.php'>Administrative Options</a>";
?>

</body>

</html>