<?php


if($_SESSION["internal"]!="INTERNAL")
	die;

function generateDistri($displayname)
{
	
$pass=genPass();


$hostname=file_get_contents("../hostname.conf");
$apipass=file_get_contents("../apipass.conf");
$tempzipname=genPass();


	insertPass($pass,$displayname);
	$tempname=dirname(__FILE__). "/..//temp/".genPass();
	mkdir($tempname);
	copy(dirname(__FILE__). "/../dist/Timesheet.exe",$tempname."/Timesheet.exe");
	copy(dirname(__FILE__). "/../dist/ManagedWifi.dll",$tempname."/ManagedWifi.dll");
	file_put_contents($tempname."/hostname.txt",$hostname);
	file_put_contents($tempname."/apipass.txt",$apipass);
	
	$zipname = "Timesheet_".$tempzipname.".zip";
    $zip = new 	ZipArchive();
    if(!$zip->open(dirname(__FILE__). "/../".$zipname, ZipArchive::CREATE))
	{
		echo "err";
		die;
	}
    if ($handle = opendir($tempname)) {
      while (false !== ($entry = readdir($handle))) {
        if ($entry != "." && $entry != ".." && !strstr($entry,'.php')) {
			$zip->addFile(realpath($tempname."/".$entry),$entry);
			
        }
      }
      closedir($handle);
	  
    }
	else echo "err";
	
    $zip->close();

	/*

    header('Content-Type: application/zip');
    header("Content-Disposition: attachment; filename='Timesheet_".$tempzipname.".zip'");
    header('Content-Length: ' . filesize("Timesheet_".$tempzipname.".zip"));
    header("Location: Timesheet_".$tempzipname.".zip");
*/


	$ret=Array();
	$ret["pass"]=$pass;
	$ret["file"]="Timesheet_".$tempzipname.".zip";
	//unlink("Timesheet_".$tempzipname.".zip");
	return $ret;
}



function insertPass($pass,$displayname)
{	
include("include/db.inc.php");
	$stmt1=$dbh->prepare("INSERT INTO init (pass,displayname) VALUES(?,?)");
	if(!$stmt1)
				{
					return false;
				}
				else
				{
					$stmt1->bind_param("ss",$pass,$displayname);
					if(!$stmt1->execute())
						return false;
					else
						return true;
				}
}

		 function genPass($length=10)
		{
			$characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
			$charactersLength = strlen($characters);
			$randomString = '';
			for ($i = 0; $i < $length; $i++) {
				$randomString .= $characters[rand(0, $charactersLength - 1)];
			}
			return $randomString;
		}