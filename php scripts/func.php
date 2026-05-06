<?php
@ob_start();
session_start();

    $session_name = 'sec_session_id';   // Set a custom session name
    $secure = true;
    // This stops JavaScript being able to access the session id.
    $httponly = false;

    if ((ini_set('session.cookie_secure', 1) === FALSE) || (ini_set('session.cookie_lifetime', 12*60*60) === FALSE) || (ini_set('session.cookie_httponly', 1) === FALSE) || (ini_set('session.use_only_cookies', 1) === FALSE)) {
        header("Location: ../error.php?err=Could not initiate a safe session (ini_set)");
        exit();
    }  
    // Gets current cookies params.
    $cookieParams = session_get_cookie_params();
    session_set_cookie_params(12*60*60,
        "/", 
       	$cookieParams["domain"], 
        true,
        false);
    // Sets the session name to the one set above.
	
    session_name($session_name); 
    session_start();            // Start the PHP session 
//    session_regenerate_id(true);    // regenerated the session, delete the old one.
  

if ($_SESSION['timeout'] + 1 * 60 < time()) {
    // session_destroy();
  } else {
     // session ok
  }
?>