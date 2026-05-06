<?php

	if (isset($_GET['email']) && isset($_GET['recovery'])){

		$gEmail = $_GET['email'];
		$gRecovery = $_GET['recovery'];

		include_once 'connect.php';
		$result =  mysqli_query ($con,"SELECT * FROM ForgotPass WHERE `email` = '".$gEmail."'");                   
		while ($row = mysqli_fetch_assoc ($result)) {        
			$rEmail = $row['email'];
			$rRecovery = $row['recovery'];
			if (($rEmail  == $gEmail) && ($rRecovery == $gRecovery)){
				mysqli_query ($con,"DELETE FROM ForgotPass WHERE recovery = '".$gRecovery."' AND email = '".$gEmail."'");
        $Recovery = true;
				if (isset ($_POST ['SubmitPass'])) {
					if ((!empty($_POST['pass1']) || !empty($_POST['pass2'])) && ($_POST['pass1'] == $_POST['pass2'])){
						 mysqli_query ($con,"UPDATE userinfotable WHERE Email = '".$gEmail."' SET Password = '".md5($_POST['pass1'])."'" );
						$passErr = "Successfully changed password";
						 header ('Refresh: 3; URL=http://solarwatergames.cf');
					}
					elseif (empty($_POST['pass1']) || empty($_POST['pass2'])){
        		$passErr = "One or more fields are empty.";
					}
					else {
        		$passErr = "Passwords does not match";
					}
    		}
      } else {
        $Recovery = false;
			}
		}
		mysqli_close($con);
  } else {
		$Recovery = false;
	}
?>
<html>
  <head>
    <title>Reset password</title>
  </head>
  <body>

<!doctype html>
<html class="no-js" lang="en">
  <head>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <title>New Password - Solar Water Games</title>
    <link rel="icon" href="../assets/images/favicon.ico" type="image/icon"> 

    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Luckiest+Guy|Bitter:700|Open+Sans:400,600,600italic">
    <link rel="stylesheet" href="../assets/stylesheets/style-min.css">
  </head>
  <body>

    <header id="main-header">
      <div class="row">
        <div class="small-11 large-6 columns"><a href="../index.php"><img src="../assets/images/logo.png" alt="gamedevs" class="logo"></a></div>
        <nav id="main-nav" class="small-16 large-10 columns">
          <ul class="main-menu">
            <li><a href="../index.php">Home</a></li>
            <li><a href="../about.php">About</a></li>
            <li><a href="../games.php">Games</a></li>
            <li><a href="../forums" target="_blank">Forums</a></li>
            <li><a href="../contact.php">Contact</a></li>
          </ul>
          <ul class="mobile-menu"></ul><!--Mobile Toggle-->
        </nav>
      </div>
    </header>

    <section class="inner-slider">
      <div style="background: url('../assets/images/placeholders/slider_1_small.jpg')"></div>
      <div style="background: url('../assets/images/placeholders/slider_2.jpg')"></div>
    </section>

    <div class="content-top"></div>

    <section id="main-content" <?php if ($Recovery == false) { echo ' style="display:none" ';} else { echo ' '; }?> >

      <div class="row">
        <div class="small-16 columns">
          <h1>New Password</h1>
          <p>Please input your new password which will be the password associated with your account.</p>
          <div class="big-sep"></div>
        </div>
      </div>

      <div class="row" >
        <div class="small-16 large-16 columns">
          <form method="post" action = "<?php htmlspecialchars($_SERVER["PHP_SELF"]);?>">
            <p class="error-text" id="login-errormessage"><?php echo $passErr; ?></p>
            <label>
              <input type="password" placeholder="New Password" name="pass1">
              <input type="password" placeholder="New Password" name="pass2">
            </label>
            <button type="submit" class="lime-button" style="float:right;" name="SubmitPass">Set New Password</button>
          </form>
        </div><!--//columns-->
      </div><!--//row-->

    </section>
    
    <p class="error-text" id="login-errormessage"><?php if ($Recovery == false) {echo "Link has expired"; } ?><p>
    <footer class="main-footer">
      <div class="row">
        <!--social-icons-->
        <div class="small-16 medium-6 columns">
          <ul class="inline-list social-icons">
            <li>
              <a href="#"><!--Twitter Link-->
                <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 56.693 56.693"><path d="M28.348 5.157c-13.6 0-24.625 11.027-24.625 24.625 0 13.6 11.025 24.623 24.625 24.623S52.97 43.382 52.97 29.782c0-13.598-11.023-24.625-24.622-24.625zm12.404 19.66c.013.266.018.533.018.803 0 8.2-6.242 17.656-17.656 17.656-3.504 0-6.767-1.027-9.513-2.787.487.056.98.085 1.48.085 2.91 0 5.585-.992 7.708-2.656-2.715-.052-5.006-1.847-5.796-4.312.378.074.767.11 1.167.11.565 0 1.113-.073 1.634-.216-2.84-.57-4.98-3.08-4.98-6.084 0-.027 0-.053.002-.08.836.465 1.793.744 2.81.777-1.665-1.115-2.76-3.012-2.76-5.166 0-1.138.306-2.205.84-3.12 3.06 3.753 7.634 6.224 12.792 6.482-.106-.453-.16-.928-.16-1.414 0-3.426 2.777-6.205 6.205-6.205 1.785 0 3.397.754 4.53 1.96 1.413-.278 2.74-.796 3.94-1.507-.465 1.45-1.448 2.666-2.73 3.433 1.257-.15 2.453-.485 3.565-.978-.83 1.247-1.883 2.34-3.096 3.215z"/></svg>
              </a>
            </li>
            <li>
              <a href="#"><!--Facebook Link-->
                <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 56.693 56.693"><path d="M28.347 5.157c-13.6 0-24.625 11.027-24.625 24.625 0 13.6 11.025 24.623 24.625 24.623s24.625-11.023 24.625-24.623c0-13.598-11.026-24.625-24.625-24.625zm6.517 24.522H30.6v15.206h-6.32V29.68h-3.006v-5.37h3.006v-3.48c0-2.49 1.182-6.376 6.38-6.376l4.68.018v5.215h-3.4c-.554 0-1.34.277-1.34 1.46v3.164h4.82l-.556 5.37z"/></svg>
              </a>
            </li>
            <li>
              <a href="#"><!--Google Plus Link-->
                <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 56.6934 56.6934"><path d="M28.068 4.155c-13.6 0-24.625 11.023-24.625 24.623s11.025 24.625 24.625 24.625 24.625-11.025 24.625-24.625S41.67 4.155 28.068 4.155zm2.33 31.095c-2.55 3.59-7.674 4.64-11.67 3.1-4.013-1.527-6.854-5.764-6.516-10.084.09-5.285 4.948-9.914 10.232-9.737 2.533-.12 4.913.983 6.853 2.53-.828.94-1.685 1.847-2.6 2.695-2.332-1.612-5.648-2.072-7.98-.21-3.335 2.306-3.487 7.753-.28 10.236 3.12 2.832 9.018 1.426 9.88-2.91-1.954-.028-3.913 0-5.868-.062-.005-1.166-.01-2.332-.005-3.497 3.267-.01 6.534-.014 9.806.01.196 2.743-.166 5.663-1.85 7.93zm14.088-5.02c-.975.01-1.95.015-2.93.024-.01.98-.014 1.955-.02 2.93H38.62c-.01-.975-.01-1.95-.02-2.925l-2.93-.03v-2.914c.976-.01 1.95-.015 2.93-.02.005-.98.015-1.954.025-2.93h2.914c.005.976.01 1.955.02 2.93.974.01 1.954.01 2.93.02v2.914z"/></svg>
              </a>
            </li>
            <li>
              <a href="#"><!--YouTube Link-->
                <svg xmlns="http://www.w3.org/2000/svg" width="35" height="35" viewBox="0 0 56.693 56.693"><path d="M17.833 31.853h1.783v8.857h1.723v-8.857h1.78v-1.508h-5.287M28.413 24.493c.234 0 .42-.062.557-.19.137-.13.207-.308.207-.532v-4.59c0-.183-.07-.333-.21-.444-.142-.115-.325-.172-.554-.172-.21 0-.38.057-.512.172-.13.11-.194.262-.194.445v4.59c0 .23.06.41.184.534.12.127.297.19.523.19zM32.212 32.97c-.238 0-.473.06-.705.182-.23.12-.45.3-.654.533v-3.34h-1.545V40.71h1.545v-.586c.2.236.418.408.652.52.232.11.5.166.8.166.452 0 .802-.143 1.038-.432.24-.29.36-.705.36-1.246v-4.244c0-.627-.126-1.104-.384-1.428-.255-.326-.624-.49-1.108-.49zm-.084 5.95c0 .247-.045.42-.133.528-.088.11-.225.162-.412.162-.13 0-.25-.03-.37-.082-.116-.053-.24-.146-.36-.27v-4.764c.104-.107.21-.186.314-.234.105-.053.215-.076.324-.076.206 0 .366.066.478.197.107.136.16.33.16.59v3.95zM26.628 38.874c-.143.164-.3.3-.473.408-.172.107-.316.16-.426.16-.146 0-.25-.04-.315-.12-.062-.08-.096-.212-.096-.392v-5.867H23.79v6.395c0 .457.09.793.268 1.025.182.227.445.34.8.34.286 0 .583-.078.888-.242.305-.165.598-.4.88-.708v.838h1.53v-7.646h-1.53v5.81z"/><path d="M28.347 5.155c-13.6 0-24.625 11.025-24.625 24.625 0 13.602 11.025 24.625 24.625 24.625S52.972 43.382 52.972 29.78c0-13.6-11.026-24.625-24.625-24.625zm3.978 12.162h1.72v6.46c0 .2.038.343.11.43.07.09.188.138.35.138.125 0 .285-.06.48-.178.19-.12.37-.27.53-.457v-6.393h1.722v8.424h-1.723v-.93c-.314.343-.645.606-.99.784-.342.178-.674.27-.998.27-.398 0-.697-.127-.9-.38-.2-.247-.3-.622-.3-1.128v-7.04zm-6.39 1.926c0-.65.23-1.17.693-1.56.465-.384 1.088-.58 1.87-.58.712 0 1.294.206 1.75.612.454.406.68.934.68 1.578v4.35c0 .723-.222 1.287-.665 1.695-.45.408-1.062.613-1.844.613-.753 0-1.356-.21-1.808-.63-.45-.426-.678-.996-.678-1.71v-4.367zm-4.688-4.92l1.258 4.562h.123l1.197-4.562h1.97l-2.255 6.682v4.737h-1.938v-4.526l-2.307-6.893h1.952zm22.54 24.033c0 3.047-2.472 5.52-5.52 5.52H19.093c-3.05 0-5.52-2.473-5.52-5.52v-4.438c0-3.05 2.47-5.52 5.52-5.52h19.176c3.047 0 5.518 2.47 5.518 5.52v4.438z"/><path d="M36.827 32.874c-.686 0-1.24.207-1.674.627-.432.417-.65.96-.65 1.618v3.438c0 .738.2 1.316.592 1.734.393.42.932.63 1.617.63.762 0 1.334-.196 1.715-.59.387-.4.576-.99.576-1.774v-.393H37.43v.348c0 .452-.052.743-.15.874s-.278.197-.532.197c-.244 0-.416-.075-.518-.23-.1-.157-.148-.435-.148-.84v-1.438h2.922V35.12c0-.724-.186-1.278-.562-1.667-.377-.386-.916-.58-1.615-.58zm.604 3.008h-1.35v-.773c0-.32.05-.554.157-.687.107-.143.28-.21.525-.21.23 0 .404.067.508.21.105.133.16.365.16.686v.772z"/></svg>
              </a>
            </li>
          </ul>
        </div>
        <!--//social-icons-->
        <!--logo-->
        <div class="small-16 medium-4 columns">
          <a href="../index.php"><img src="../assets/images/logo.png" alt="Solarwater Games Logo" class="footer-logo"/></a>
        </div>
        <!--//logo-->
        <!--copyright-->
        <div class="small-16 medium-6 columns">
          <p class="copyright">
            Copyright &copy; Solarwater Games <span class="date"></span>. All Rights Reserved.
          </p>
        </div>
        <!--//copyright-->
      </div>
    </footer>

    <script src="../assets/js/app-min.js"></script>
    <script src="../assets/js/functions.js"></script>
  </body>

</html>
