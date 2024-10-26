<?php
	class Initialize
	{
		public function Init($pass,$hwid)
		{
			include("include/db.inc.php");
			$apipass=$this->genApiPass();
			$stmt = $dbh->prepare("SELECT displayname FROM init WHERE pass=?");
			 

			$stmt->bind_param("s",$pass);
			if ($stmt->execute()) {
				$stmt->store_result();
				$stmt->bind_result($username);				
				$stmt->fetch();
				
				
					$stmt1=$dbh->prepare("INSERT INTO user (hwid,apipass,initkey,username) VALUES(?,?,?,?)");						
					$stmt1->bind_param("ssss",$hwid,$apipass,$pass,$username);
					if(!$stmt1->execute())
						return "FAIL";
					else
						return $dbh->insert_id.";".$apipass;

			}
			return "FAIL";
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
	}