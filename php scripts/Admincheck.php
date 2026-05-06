<?php
include_once 'functions.php';

if (!loggedin()) {
	header('location: http://solarwatergames.cf');
}
?>