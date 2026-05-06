<?php
@ob_start();
session_start();

include_once '/home/u801994192/public_html/Global/connect.php'; $con = $con;
include_once 'functions.php';
include_once 'title_bar.php';
?>
<html>

<head>
<title>Admin Panel - Login</title>
<link rel="stylesheet" type="text/css" href="styles.css">
</head>


<body>


<h3>Login Here:</h3>

<form method = 'post' action = "<?php echo htmlspecialchars($_SERVER["PHP_SELF"]); ?>">
<?php
if (loggedin()) {
	header('location: profile.php');
}
if (isset ($_POST ['LoginButton'])) {  
	$Username = $_POST ['Username'];
	$Password = md5 ($_POST ['Password']);
	if (empty($Username) || empty($Password)) {
		echo "<p>One or more fields empty!</p>";
	} else {
		$check_login = mysqli_query ($con, "SELECT * FROM `userinfotable` WHERE `Username` = '".$Username."'");
		if (mysqli_num_rows($check_login) == 1) {
			while ($row = mysqli_fetch_assoc($check_login)) {
				if ($row ['Type'] == "Admin") {
			if ($Password == $row ['Password']) {

				$user_id = $row ['ID'];
				$_SESSION ['user_id'] = $user_id;
				header ('location: index.php');
			}
			else
				echo "<p>Incorrect password</p>";
				} else {
					echo "<p>You are not an administrator of the game. You are being redirected.</p>";
					header ('Refresh: 3; URL=http://www.solarwatergames.cf');
				}
			}
		} else {
			echo "<p>Incorrect username</p>";
		}
	}
}
?>
Username: <br/>
<input type ='text' name ='Username' />
<br/><br/>
Password: <br/>
<input type ='password' name ='Password' />
<br/><br/>
<input type='submit' name ='LoginButton' value = 'Login' />
</form>


</body>

</html>