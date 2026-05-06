<?php
@ob_start();
session_start();
?>
<html>

<head>
<title>Admin Panel</title>
<link rel="stylesheet" type="text/css" href="styles.css">
</head>

<body>
<?php 
include_once '/home/u801994192/public_html/Global/connect.php';
include_once 'functions.php'; 
include_once 'title_bar.php';
?>
<h3>Index</h3>
<?php
if (!loggedin()) {
echo "<p>Index for the Admin Panel. If you are an Admin for Soccer Instincts you may <a href='login.php'>login</a>. Otherwise, please leave this site.</p>";
} else if (loggedin()) {
echo "<p>Welcome back to the Admin Panel. Visit the <a href='profile.php'>'profile'</a> tab for administrative options.</p>";
}
?>
<img src="https://cdn0.iconfinder.com/data/icons/i_love_icons__by_svengraph-d2yk60n/512/lock.png"/>
</body>

</html>