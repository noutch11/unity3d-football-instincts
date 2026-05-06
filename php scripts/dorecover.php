<?php
include_once '/home/u801994192/public_html/Global/connect.php';


$u_id = $_GET['u_001'];
$u_email = $_GET ['u_106'];

$sql = mysqli_query($con, "SELECT Password, Username FROM userinfotable WHERE ID='".$u_id."' && Email ='".$u_email."'");

$numrows = mysqli_num_rows($sql);

$row = mysqli_fetch_assoc ($sql);

if ($numrows == 0)
	echo "Unknown error.";
else {
	if (isset ($_POST ['setnewpass'])) {
		$Password = md5 ($_POST ['password']);
		$ConfirmPassword = md5 ($_POST ['confirmpassword']);
		if (empty($Password) || empty($ConfirmPassword)) {
			echo "One or more fields empty!";
	} else {
		if ($Password == $ConfirmPassword) {
			$ins = mysqli_query($con, "UPDATE userinfotable SET Password ='".$Password."' WHERE ID='".$u_id."' && Email ='".$u_email."'");
			if ($ins) {
				echo "The password for user '".$row['Username']."' has been changed successfully.";
				header ('Refresh: 3; URL=http://www.solarwatergames.cf');
			} else
				echo "Unknown error";
		} else if ($Password != $ConfirmPassword)
			echo "Passwords do not match!";
	}	
 }
}
mysqli_close($con);
?>
<html>
	<head>
	<title>Password Recovery</title>
	</head>
	<body>
  <center>
    <div>
      <!--form-->
      <form method="post" action = "<?php $_PHP_SELF ?>">
        <p id="errormessage"></p>
        <table align="center" width="30%" border="0">
		  <tr>
		  <td>Enter your new password:</td>
		  </tr>
          <tr>
            <td><input type="password" name="password" placeholder="New password" required /></td>
          </tr>
		  <tr>
		  <td>Confirm your new password:</td>
		  </tr>
		  <tr>
		  <td><input type="password" name="confirmpassword" placeholder="Confirm new password" required /></td>
		  </tr>
          <tr>
            <td><input type="submit" name="setnewpass" value="Set New Password"/></td>
          </tr>
        </table>
      </form>
      <!--form-->
</html>