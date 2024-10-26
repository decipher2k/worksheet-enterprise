<?php

class Distri
{
	public function setApiPass($value)
	{
		file_put_contents("../apipass.conf",$value);
		$replacement["<!-- {{RESULT}} -->"]="API password has been set.";
		return $replacement;
	}
	
	public function setHostname($value)
	{
		file_put_contents("../hostname.conf",$value);
		$replacement["<!-- {{RESULT}} -->"]="Hostname has been set.";
		return $replacement;
	}
	
	public function generateDistri($displayname)
	{
		require_once("include/distrigen.inc.php");
		$ret=generateDistri($displayname);
		
		$replacement["<!-- {{RESULT}} -->"]="The initalization key is: ".$ret["pass"]."<br>
		<a href='".$ret["file"]."'>Download Distribution</a>";
		return $replacement;		
	}
}
