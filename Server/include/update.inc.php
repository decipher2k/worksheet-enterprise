<?php

class Update
{
		public function query($id)
		{
					include("include/db.inc.php");
					$key=$this->genKey();			
					$stmt1=$dbh->prepare("UPDATE user SET currKey=? WHERE ID=?");
					$stmt1->bind_param("si",$key,$id);
					$stmt1->execute();
					
						return $key;
					
			
		}
		
		public function doUpdate($secret,$id,$dateformat)
		{
			include("include/db.inc.php");
			$hwid=$this->getHWID($id);
			$currentkey=$this->getcurrentkey($id);
			
			echo "1";
			$hash=Hashing::genHash($hwid,$dateformat,$currentkey);
			echo "2";
			if(trim($hash)==trim($secret))
			{
				
				$stmt=$dbh->prepare("INSERT INTO timestamp(iduser, timestamp) VALUES(?, ?)");
				 
					$mid=$id;
					$timestamp=time();
					$stmt->bind_param("is",$mid,$timestamp);					
				
					if(!$stmt->execute())
						return "FAIL";
					else
						return "SUCCESS";
				
			}
			else
			{
				return "WRONG HASH:" . $hash;
			}
			return "SUCCESS";
		}
		
		private function getHWID($id)
		{
			include("include/db.inc.php");
			$stmt = $dbh->prepare("SELECT hwid FROM user WHERE ID=?");	
			
			$stmt->bind_param("i",$id);
			
			if ($stmt->execute()) {
				$stmt->bind_result($hwid);
				if ($stmt->fetch()) {
					return $hwid;
				}
			}
			return false;
		}
		private function getcurrentkey($id)
		{
			include("include/db.inc.php");
			$stmt = $dbh->prepare("SELECT currKey FROM user WHERE ID=?");	
			
			$stmt->bind_param("i",$id);
			
			if ($stmt->execute()) {
				$stmt->bind_result($hwid);
				if ($stmt->fetch()) {
					return $hwid;
				}
			}
			return false;
		}
		
		private function genKey($length=10)
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

class Hashing
		{
			public static function genHash($hwid, $dateformat,$currentkey)
			{
				$date=Hashing::genDateString($dateformat);
				$secret=$hwid."///".$date;				
				$xored=Hashing::mXor($currentkey,$secret);
				return Hashing::base64Encode($xored);
			}
			
			private static function genDateString($dateformat)
			{/*
				if($dateformat=="de")
					return date("d.m.Y");
				else*/
					return date("n/j/Y");
			}
			
			private static function mXor($key, $string)
			{				
				// Our plaintext/ciphertext
				$text = $string;

				// Our output text
				$outText = '';

				// Iterate through each character
				for($i=0; $i<strlen($text); )
				{
					for($j=0; ($j<strlen($key) && $i<strlen($text)); $j++,$i++)
					{
						$outText .= $text[$i] ^ $key[$j];
						//echo 'i=' . $i . ', ' . 'j=' . $j . ', ' . $outText{$i} . '<br />'; // For debugging
					}
				}
				return $outText;
			}
			
			private static function base64Encode($s1)
			{
				return base64_encode($s1);
			}
		}
