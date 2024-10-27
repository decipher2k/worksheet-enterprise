<?php

require_once("include/dashboard.inc.php");
require_once("include/distri.inc.php");
require_once("include/charts.inc.php");
require_once("include/users.inc.php");

class Template
{
	private $pages=Array();
		
	function __construct()
	{
		$this->pages["index"]["template"]="index.tpl";
		$this->pages["index"]["area"]="Dashboard Overview";
		$this->pages["dashboard"]["template"]="dashboard.tpl";
		$this->pages["dashboard"]["area"]="Dashboard Overview";
		$this->pages["distrisettings"]["template"]="distrisettings.tpl";
		$this->pages["distrisettings"]["area"]="Distribution Settings";
		$this->pages["distrigen"]["template"]="distrigen.tpl";
		$this->pages["distrigen"]["area"]="Distribution Generator";
		$this->pages["chartoverview"]["template"]="chartoverview.tpl";
		$this->pages["chartoverview"]["area"]="Chart: Overview";
		$this->pages["chartuser"]["template"]="chartuser.tpl";
		$this->pages["chartuser"]["area"]="Chart: User";
		$this->pages["chartweekly"]["template"]="chartweekly.tpl";
		$this->pages["chartweekly"]["area"]="Chart: Weekly";
		$this->pages["chartmonthly"]["template"]="chartmonthly.tpl";
		$this->pages["chartmonthly"]["area"]="Chart: Monthly";
		$this->pages["chartexport"]["template"]="chartexport.tpl";
		$this->pages["chartexport"]["area"]="Chart: Export";
		$this->pages["userclients"]["template"]="userclients.tpl";
		$this->pages["userclients"]["area"]="Users: Clients";
		$this->pages["useradmins"]["template"]="useradmins.tpl";				
		$this->pages["useradmins"]["area"]="Users: Admins";				
	}
	
	
	function processAction($action,$page,$value)
	{
		$dashboard=new Dashboard();
		$distri=new Distri();
		$charts=new Charts();
		$users=new Users();
		
		if(!isset($page) || $page=="")
			$page="chartmonthly";	

		$tpl_idx=file_get_contents("templates/index.tpl");
		$tpl_idx=str_replace("<!-- {{AREA}} -->",$this->pages[$page]["area"],$tpl_idx);
		$tpl_pge=file_get_contents("templates/".$page.".tpl");		
		
		if(true)
		{

			if(isset($action) && $action=="init")
			{			
				include("include/db.inc.php");

				$stmt = $dbh->prepare("SELECT displayname FROM init WHERE pass=?");
				$stmt->bind_param("s",$_GET["init"]);			
				$stmt->execute();
				$result = $stmt->get_result();
				$user= $result->fetch_array();

				$stmt = $dbh->prepare("DELETE FROM init WHERE pass=?");
				$stmt->bind_param("s",$_GET["init"]);			
				$stmt->execute();

				if($user[0]!=NULL && $user[0]!="")
				{
					$stmt = $dbh->prepare("INSERT INTO user (hwid,currKey,apipass,initkey,username,deleted) VALUES (?,?,?,?,?,?)");
					$n=0;
					$nu="";
					$stmt->bind_param("sssssi",$_GET["hwid"],$nu,$_GET["apipass"],$_GET["init"],$user[0]	,$n);			
					$stmt->execute();
				}
				else
				{
					return "FAIL";
				}
				return "OK";
			}

			if($page=="distrisettings")
			{
				if($action=="set_apipass")
					$replacement=$distri->setApiPass($value);
				else if($action=="set_hostname")
					$replacement=$distri->setHostname($value);
			}
			else if($page=="chartuser")
			{
				$replacement=$charts->printUserList();
				
				if($action=="print_by_user")
				{
					$replacement1=$charts->printByUser($value);
					$replacement=array_merge($replacement,$replacement1);
				}				
			}
			else if($page=="chartexport")
			{
				if($action=="export")
					$replacement=$charts->exportTimesheetData();
			}
			else if($page=="userclients")
			{
				$replacement=$users->printUserTable();
				if($action=="delete_client")
				{
					$replacement1=$users->deleteClient($value);				
					$replacement=array_merge($replacement,$replacement1);				
				}
			}
			else if($page=="useradmins")
			{
				if($action=="set_admin_pass")
					$replacement=$users->setAdminPass($value);				
			}
			else if($page=="distrigen")
			{
				if($action=="gen_distri")
				{
					$replacement=$distri->generateDistri($value);
				}
			}
			else if($page=="chartoverview")
			{
				$replacement=$charts->printOverview();
			}
			else if($page=="chartweekly")
			{
				$replacement=$charts->displayWeekly();
			}
			else if($page=="chartmonthly")
			{
				$replacement=$charts->displayMonthly();
			}
			if(isset($replacement))
			{
				foreach($replacement as $token => $actionresult )
				{					
					$tpl_pge=str_replace($token,$actionresult,$tpl_pge);
				}		
			}
		}	
		
		
		$combined=str_replace("<!-- {{CONTENT}} -->",$tpl_pge,$tpl_idx);
		return $combined;
	}
}