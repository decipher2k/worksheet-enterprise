<?php

class Users
{
	public function setAdminPass($value)
	{
		file_put_contents("adminarea.conf",$value);
		$replacement["<!-- {{RESULT}} -->"]="The admin password has been changed.";
		return $replacement;
	}
	
	public function deleteUser($iduser)
	{
		include("include/db.inc.php");
		$stmt = $dbh->prepare("UPDATE user SET deleted=1 WHERE ID=?");
		$stmt->bind_param("i",$iduser);			
		$stmt->execute();
		$ret="The user ".$iduser." has been deleted.";
		$replace["<!-- {{RESULT}} -->"]=$ret;
		return $replace;
	}
	
	public function printUserTable()
	{
		include("include/db.inc.php");
		$stmt = $dbh->prepare("SELECT ID, username from user where deleted=0");
		$stmt->execute();			
		$stmt->bind_result($iduser,$username);
		$ret="<select name='mvalue'>";
		while ( $stmt->fetch()) {							
			$ret.="<option value='".$iduser."'>".$username."</option>";
		}
		$ret.="</select>";
		$replace["<!-- {{USERLIST}}-->"]=$ret;
		return $replace;
	}
}