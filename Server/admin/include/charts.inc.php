<?php

class Charts
{
	
	public function printByUser($zvalue)
	{
		$replace1=$this->printMonthlyUserChart($zvalue);
		$replace2=$this->printMonthlyUserTable($zvalue);
		return array_merge($replace1,$replace2);
	}
	
	public function printMonthlyUserTable($iduser)
	{
		
		include("include/db.inc.php");
		$startDay=date('01.m.Y');
		$numberDays=cal_days_in_month(CAL_GREGORIAN, date("m"), date("Y"));
		$chart = new LineChart(1024, 768);
		$serie1 = new XYDataSet();							
		$ret="<center><table class='table table-striped table-bordered' style='width:50%;'>
		<tr><td></td><td><b>Start</b></td><td><b>Ende</b></td><td><b>Dauer (minuten)</b></td></tr>";
		for($i=0;$i<$numberDays;$i++)
		{
			$slices=Array();
			$min=strtotime("+".$i." days", strtotime($startDay));
			$max=strtotime("+".($i+1)." days", strtotime($startDay));	
			$stmt = $dbh->prepare("SELECT timestamp FROM timestamp WHERE timestamp>=? AND timestamp<=? AND iduser=?");
			$stmt->bind_param("iii",$min, $max,$iduser);
			
				if ($stmt->execute()) {
					$stmt->bind_result($timestamp);
					$timestamps=Array();
					$ges=Array();
					$ges[$min]=0;
					$sliceCount=1;
					$slices[$sliceCount]["count"]=0;					
					
					while ( $stmt->fetch()) {							
						$ges[$min]++;						
						if(isset($timestamp_prev) && $timestamp-$timestamp_prev>400)
						{
							$slices[$sliceCount]["end"]=$timestamp_prev;					
							$sliceCount++;							
							$slices[$sliceCount]["start"]=$timestamp;												
						}
						else
						{
							$slices[$sliceCount]["start"]=$timestamp;												
						}
						$slices[$sliceCount]["count"]++;
						$timestamp_prev=$timestamp;
					}										
				}
				$ret.="<tr><td colspan='4'><b>".date("d/m/Y",$min)."</b></td></tr>";
				foreach($slices as $sliceNum => $slice)
				{					
					if(isset($slices[$sliceNum]["end"]))						
						$ret.="<tr><td></td><td>".date("h:i",$slices[$sliceNum]["start"])."</td><td>".date("h:i",$slices[$sliceNum]["end"])."</td><td>".$slices[$sliceNum]["count"]."</td></tr>";
					else
						if(isset($slices[$sliceNum]["start"]))
							$ret.="<tr><td></td><td>".date("h:i",$slices[$sliceNum]["start"])."</td><td>".date("h:i",$slices[$sliceNum]["start"]+300)."</td><td>5</td></tr>";						
				}
		}				
		
		$ret.="</table></center>";		
		
		$replacement["<!-- {{USER_TABLE}} -->"]=$ret;
		return $replacement;
		
		
	}
	
	public function printMonthlyUserChart($iduser)
	{
		include("include/db.inc.php");
		include "libchart/classes/libchart.php";
		$startDay=date('01.m.Y');
		$numberDays=cal_days_in_month(CAL_GREGORIAN, date("m"), date("Y"));
		$chart = new LineChart(1024, 768);
		$serie1 = new XYDataSet();							
	
		for($i=0;$i<$numberDays;$i++)
		{
			$min=strtotime("+".$i." days", strtotime($startDay));
			$max=strtotime("+".($i+1)." days", strtotime($startDay));	
			$stmt = $dbh->prepare("SELECT timestamp FROM timestamp WHERE timestamp>=? AND timestamp<=? AND iduser=?");
			$stmt->bind_param("iii",$min, $max,$iduser);
			
				if ($stmt->execute()) {
					$stmt->bind_result($timestamp);
					$timestamps=Array();
					$ges=Array();
					$ges[$min]=0;
					while ( $stmt->fetch()) {	
						$ges[$min]++;						
					}
					
					$serie1->addPoint(new Point($min, $ges[$min]));
				}
		}				
		
				$dataSet = new XYSeriesDataSet();
	$dataSet->addSerie("Minutes per day", $serie1);
	$chart->setTitle("Timesheet");
	
	
	$chart->setDataSet($dataSet);
	$filename=$this->genApiPass().".png";
	$img='<img src="'.$filename.'"></img>';
	$chart->render($filename);		
		$replacement["<!-- {{IMG_MONTHLY}} -->"]=$img;
		return $replacement;
	}
	
	public function printUserList()
	{
		include("include/db.inc.php");
		
		$stmt = $dbh->prepare("SELECT ID, username FROM user WHERE deleted=0");								
				if ($stmt->execute()) {
					$stmt->bind_result($iduser, $username);
					$ret='Username:<br>

<select name="mvalue">';

					while ( $stmt->fetch()) {	
						$ret.="<option value='".$iduser."'>".$username."</option>";
					}
					$ret.="</select>";
				}
		$replace["<!-- {{USERS_LB}} -->"]=$ret;
		return $replace;
	}
	
	public function displayWeekly()
	{
		$day = date('w');
		$week_start = date('m/d/Y', strtotime('-'.$day.' days'));	
		$numberDays=7;
		
		return $this->printOverview($week_start,$numberDays);
	}
	
	public function displayMonthly()
	{		
		$startDay=date('01.m.Y');
		$numberDays=cal_days_in_month(CAL_GREGORIAN, date("m"), date("Y"));
		return $this->printOverview($startDay,$numberDays);
	}
	
	public function printOverview($startDay,$numberDays)
	{
		include("include/db.inc.php");
		include "libchart/classes/libchart.php";

	$chart = new LineChart(1024, 768);
	$serie1 = new XYDataSet();							
	
		for($i=0;$i<$numberDays;$i++)
		{
			$min=strtotime("+".$i." days", strtotime($startDay));
			$max=strtotime("+".($i+1)." days", strtotime($startDay));	
			    
				
				$stmt = $dbh->prepare("SELECT timestamp, iduser FROM timestamp WHERE timestamp>=? AND timestamp<=?");
				$stmt->bind_param("ii",$min, $max);
				
				if ($stmt->execute()) {
					$stmt->bind_result($timestamp, $iduser);
					$timestamps=Array();
					$ges=Array();
					
					while ( $stmt->fetch()) {		
						if(!isset($timestamp[$iduser]))						
							$timestamps[$iduser]=0;
						$timestamps[$iduser]++;
					}
					

					$alloverall=0;
					foreach($timestamps as $timestamp1)
					{						
							$alloverall+=$timestamp1;
					}
					
					if(sizeof($timestamps)>0)
					{
						$durchschnitt=intval($alloverall/sizeof($timestamps));
						$serie1->addPoint(new Point(date("m.d.Y",strtotime("+".$i." days", strtotime($startDay))), $durchschnitt));
					}
					else
					{						        		  
						$serie1->addPoint(new Point(date("m.d.Y",strtotime("+".$i." days", strtotime($startDay))), 0));
					}
				}
		}
			
		$dataSet = new XYSeriesDataSet();
	$dataSet->addSerie("Average of all employees", $serie1);
	$chart->setTitle("Timesheet");
	
	
	$chart->setDataSet($dataSet);
	$filename=$this->genApiPass().".png";
	$chart->render($filename);		
		$replacement["{{IMG}}"]=$filename;
		return $replacement;
	}
	
	function startOfDay($day)
	{
		return strtotime($day);
	}
	
	function endOfDay($day)
	{
		return strtotime('+1 day', $day);		
	}
	
	public function printUserSelection($value)
	{
		$userSelection=file_get_contents("templates/userchartselection.tpl");
		$replacement["<!-- {{CHARTUSERSEL}} -->"]=$userSelection;
		return $replacement;
	}
	
			private function genApiPass($length=10)
		{
			$characters = '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
			$charactersLength = strlen($characters);
			$randomString = '';
			for ($i = 0; $i < $length; $i++) {
				$randomString .= $characters[rand(0, $charactersLength - 1)];
			}
			return $randomString;
		}
		
		public function exportTimesheetData()
		{
			unlink(addslashes(__DIR__)."/../export/timestamps.csv");
			unlink(addslashes(__DIR__)."/../export/users.csv");
			include("include/db.inc.php");
			$stmt = $dbh->prepare("SELECT * INTO OUTFILE '".addslashes(__DIR__)."/../export/users.csv"."' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\r\n' FROM user");						
			if ( false===$stmt ) {
  // and since all the following operations need a valid/ready statement object
  // it doesn't make sense to go on
  // you might want to use a more sophisticated mechanism than die()
  // but's it's only an example
  die('prepare() failed: ' . htmlspecialchars($dbh->error));
}
			
			if ( false===$stmt->execute() ) {
  die('execute() failed: ' . htmlspecialchars($stmt->error));
}
			$stmt = $dbh->prepare("SELECT * INTO OUTFILE '".addslashes(__DIR__)."/../export/timestamps.csv"."' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\r\n' FROM timestamp");						
			$stmt->execute();
			$tempzipname=$this->genPass();
				$zipname = "Timesheet_export_".$tempzipname.".zip";
    $zip = new 	ZipArchive();
    if(!$zip->open(dirname(__FILE__). "/../".$zipname, ZipArchive::CREATE))
	{
		echo "err";
		die;
	}
    if ($handle = opendir(__DIR__."/../export/")) {
      while (false !== ($entry = readdir($handle))) {
        if ($entry != "." && $entry != ".." && !strstr($entry,'.php')) {
			$zip->addFile(realpath(opendir(__DIR__)."/../export/".$entry),$entry);
			
        }
      }
      closedir($handle);
	  
    }
	else echo "err";
	
    $zip->close();

    header('Content-Type: application/zip');
    header("Content-Disposition: attachment; filename='Timesheet_export_".$tempzipname.".zip'");
    header('Content-Length: ' . filesize("Timesheet_export_".$tempzipname.".zip"));
    header("Location: Timesheet_export_".$tempzipname.".zip");
			
			

		
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
}

