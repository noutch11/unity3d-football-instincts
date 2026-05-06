<?php
@ob_start();
session_start();

?>
<html>

<head>
<title>Admin Panel - Options</title>
<link rel="stylesheet" type="text/css" href="styles.css">
</head>

<body>
<?php include_once '/home/u801994192/public_html/Global/connect.php';
include_once 'functions.php';
include_once 'title_bar.php';
include_once 'Admincheck.php';
	?>
<h3>Options:</h3>

<p style="font-family:Courier;">
<a href ='admin.php?status=user'>User Settings</a> |
<a href ='admin.php?type=user'>Member Privelages Settings</a> | 
<a href ='admin.php?banned=user'>Banned Users List</a>
</p>

<p>
<?php
if (isset ($_GET['status']) && !empty ($_GET['status'])) {
?>
<p style ="font-family:Lucida Sans Unicode;"><font color="red">Format for ban date: <strong>YYYY-MM-DD</strong></font>. <font color ="blue">Maximum date is 3000-01-01 (Enter this for a permanent ban)</font>.</p>
<!--Table for showing options to ban users-->
<table style ="border: 1px solid black;">
<tr>
<td style ="border: 1px solid green;"><u><font size ="6" style ="font-weight:bold;">User ID</font></u></td>
<td width ='100px' style ="border: 1px solid orange;"><p><u><font size ="6" style ="font-weight:bold;">Users</font></u></p></td>
<td style ="border: 1px solid red;"><p><u><font size ="6" style ="font-weight:bold;">Options</font></u></p></td>
</tr> 
<?php
$list_query = mysqli_query($con, "SELECT ID, Username, Status, BanDate FROM userinfotable");
while ($row = mysqli_fetch_assoc($list_query)) {
	$u_id = $row['ID'];
	$u_username = $row['Username'];
	$u_status = $row['Status'];
        $BanDate = $row ['BanDate'];

?>
<tr>
<td style ="border: 1px solid green; font-family: Comic Sans MS; color:#006600;"><?php echo $u_id ?></td>
<td style ="border: 1px solid orange;"><p style ="font-family: Comic Sans MS; color:#663300;"><?php echo $u_username;?></p></td>
<td style ="border: 1px solid red;"> 
<?php
if (isset ($_POST['SetBanDate'])) {
$BanDate = $_POST ['BanDate'];
}
if ($u_status == "A"){

echo "<a href='option.php?u_id=$u_id&status=$u_status&ban_date=$BanDate'><button type='button' onclick= 'alert(\"$u_username was banned until: $BanDate.\")'>Ban this user</button></a>";

} else {
	echo "<a href='option.php?u_id=$u_id&status=$u_status'><button type='button' onclick= 'alert(\"$u_username was unbanned.\")'>Unban this user</button></a>";
        
}
?>
</td>
</tr>
<?php
}
?>
</table>
<form method = "post" action ="<?php $_PHP_SELF ?>">
        Ban Date:   <input type ="date" name="BanDate" max="3000-01-01" value ="<?=$BanDate?>" />   <input type ="submit" name ="SetBanDate" value ="Set" />
</form> 
<?php
}
else if (isset ($_GET['type']) && !empty ($_GET['type'])) {
?>
<!-- Table for showing Admins-->
<p style ="font-size:25; font-family:Lucida Sans Unicode;"><u>Administrators</u></p>
<table style ="border: 1px solid black;">
<tr>
<td style ="border: 1px solid green;"><u><font size ="6" style ="font-weight:bold;">User ID</font></u></td>
<td width ='100px' style ="border: 1px solid orange;"><u><font size ="6" style ="font-weight:bold;">Admins</font></u></td>
<td style ="border: 1px solid red;"><u><font size ="6" style ="font-weight:bold;">Options</font></u></td>
</tr>
<?php
$list_query = mysqli_query($con, "SELECT ID, Username, Type FROM userinfotable WHERE Type ='Admin'");
while ($row = mysqli_fetch_assoc($list_query)) {
	$u_id = $row['ID'];
	$u_username = $row['Username'];
	$u_type = $row['Type'];

?>
<tr>
<td style ="border: 1px solid green; font-family: Comic Sans MS; color:#006600;"><?php echo $u_id ?></td>
<td style ="border: 1px solid orange;"><p style ="font-family: Comic Sans MS; color:#663300;"><?php echo $u_username?></p></td>
<td style ="border: 1px solid red;">
<?php
	echo "<a href ='option.php?u_id=$u_id&type=$u_type'><button type ='button' onclick= 'alert(\"$u_username has been removed as an Admin.\")'>Remove this user as an Administrator</button></a>";
?>
</td></tr>
<?php
}
?>
</table>
<!-- Table for showing Regular users-->
<p style ="font-size:25; font-family:Lucida Sans Unicode;"><u>Regular Members</u></p>
<table style ="border: 1px solid black;">
<tr>
<td style ="border: 1px solid green;"><u><font size ="6" style ="font-weight:bold;">User ID</font></u></td>
<td width ='100px' style ="border: 1px solid orange;"><u><font size ="6" style ="font-weight:bold;">Users</font></u></td>
<td style ="border: 1px solid red;"><u><font size ="6" style ="font-weight:bold;">Options</font></u></td>
</tr>
<?php
$list_query = mysqli_query($con, "SELECT ID, Username, Type FROM userinfotable WHERE Type = 'Regular'");
while ($row = mysqli_fetch_assoc($list_query)) {
	$u_id = $row['ID'];
	$u_username = $row['Username'];
	$u_type = $row['Type'];

?>
<tr>
<td style ="border: 1px solid green; font-family: Comic Sans MS; color:#006600;"><?php echo $u_id ?></td>
<td style ="border: 1px solid orange;"><p style ="font-family: Comic Sans MS; color:#663300;"><?php echo $u_username?></p></td>
<td style ="border: 1px solid red;">
<?php
	echo "<a href ='option.php?u_id=$u_id&type=$u_type'><button type ='button' onclick= 'alert(\"$u_username has been made an Admin.\")'>Make this user an Administrator</button></a>";
?>
</td></tr>
<?php
}
?>
</table>
<?php
}
else if (isset ($_GET['banned']) && !empty ($_GET['banned'])) { ?>
<!-- Table for showing banned users-->
<table style="border: 1px solid black;">
<tr>
<td style ="border: 1px solid green;"><u><font size ="6" style ="font-weight:bold;">User ID</font></u></td>
<td width ='100px' style ="border: 1px solid orange;"><u><font size ="6" style ="font-weight:bold;">Users</font></u></td>
<td style ="border: 1px solid red;"><u><font size ="6" style ="font-weight:bold;">Banned Until</font></u></td>
</tr>
<?php
$list_query = mysqli_query($con, "SELECT ID, Username, BanDate FROM userinfotable WHERE `Status` = 'B'");
if (mysqli_num_rows ($list_query)> 0){
while ($row = mysqli_fetch_assoc($list_query)) {
	$u_id = $row['ID'];
	$u_username = $row['Username'];
        $BanDate = $row ['BanDate'];
?>
<tr>
<td style ="border: 1px solid green; font-family: Comic Sans MS; color:#006600;"><?php echo $u_id ?></td>
<td style ="border: 1px solid orange;"><p style ="font-family: Comic Sans MS; color:#663300;"><?php echo $u_username?></p></td>
<td style ="border: 1px solid red; font-family: Comic Sans MS; color:#660000;"><?php echo $BanDate ?></td>
</tr>
<?php
}
?> <form method = "post" action ="<?php $_PHP_SELF ?>">
<input type="submit" name="ClearBanList" value ="Clear List" />
</form>
<?php 
} else {
?>
</table>
<?php
echo "<p style='font-family:Comic Sans MS; font-size:30;'>Currently no banned users. Yay!</p>";
} ?>
<?php
if (isset($_POST['ClearBanList'])) {
	mysqli_query ($con, "UPDATE `userinfotable` SET `Status` = 'A', `BanDate` = '3000-01-01'");
        echo ("<font color ='red'> *</font> List of banned users was cleared.");
        header('location:admin.php?banned=user');
}
}
else {
	echo "Select an option above";
}
?>
</p>
</body>

</html>