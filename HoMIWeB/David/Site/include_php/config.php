<?PHP
//------------------------------------------------------------------------------------------
//Connexion � la base de donn�e
//------------------------------------------------------------------------------------------

$serveur="localhost";
$user="homiweb";
$password="homiweb";
$dbname="homiweb";
$connexion = mysql_connect("$serveur","$user","$password");
if (!$connexion) {echo "impossible de se connecter au serveur, r�essaye plus tard...";exit;}
$db = mysql_select_db("$dbname", $connexion);
if (!$db) {echo "Impossible de s�lectionner cette base de donn�es !!!";exit;}
?>
