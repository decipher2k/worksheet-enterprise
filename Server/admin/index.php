<?php
	session_start();
	$_SESSION["internal"]="INTERNAL";
	require_once(realpath(dirname(__FILE__))."/include/db.inc.php");
	require_once(realpath(dirname(__FILE__))."/include/template.inc.php");
	
	if(isset($_POST["Login"]) && $_POST["Login"]=="Login")
	{		
		if($_POST["password"]==file_get_contents("adminarea.conf"))
		{
			$_SESSION["auth"]=true;			
		}
	}
	
	$template=new Template();
	
	
	
	if(!isset($_GET["page"]))
		$page="";
	else
		$page=$_GET["page"];
	
	if(!isset($_GET["action"]))
		$action="";
	else
		$action=$_GET["action"];

	if(!isset($_POST["mvalue"]))
		$zvalue="";
	else
		$zvalue=$_POST["mvalue"];	
	
	
	
	if(isset($_SESSION["auth"]) && $_SESSION["auth"]==true)
		echo $template->processAction($action, $page, $zvalue);
	else
		echo file_get_contents("templates/login.tpl");