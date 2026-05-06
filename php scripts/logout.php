<?php
@ob_start();
session_start();









include_once '/home/u801994192/public_html/Global/connect.php';
include_once 'functions.php';

session_destroy();
header('location: index.php');




?>