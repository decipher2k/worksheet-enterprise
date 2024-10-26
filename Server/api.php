<?php

require_once("include/initialize.inc.php");
require_once("include/update.inc.php");

$apipass=$_GET["apipass"];

if($_GET["action"]=="init")
{
	if($apipass!=file_get_contents("apipass.conf"))
		die;
}
else
{
	$iduser=$_GET["id"];
	$apipass_user=getApiPass($iduser);
	if($apipass!=$apipass_user)
		die;
}




if($_GET["action"]=="init")
{
	$initialize=new Initialize();
	$pass=$_GET["pass"];
	$hwid=$_GET["hwid"];
	
	echo $initialize->Init($pass,$hwid);
}
else if($_GET["action"]=="query")
{	
	$update=new Update();
	$id=$_GET["id"];
	echo $update->query($id);	
}
else if($_GET["action"]=="update")
{
	
	$secret=$_GET["secret"];
	$id=$_GET["id"];
	$dateformat=$_GET["dateformat"];
	$update=new Update();
	echo $update->doUpdate($secret,$id,$dateformat);
}

function getApiPass($iduser)
{
	include ("include/db.inc.php");
	$stmt = $dbh->prepare("SELECT apipass FROM user WHERE ID=?");
			$stmt->bind_param("s",$iduser);
			if ($stmt->execute()) {
				$stmt->bind_result($apipass);
				if ($row = $stmt->fetch()) {
					return $apipass;	
				}
			}	
}			