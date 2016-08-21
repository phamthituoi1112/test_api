<?php 
	class Connect
	{	
		public function objectToArray($d) {
	        if (is_object($d)) {
	            $d = get_object_vars($d);
	        }

	        if (is_array($d)) {
	            return array_map(array($this, 'objectToArray'), $d);
	        }
	        else {
	            return $d;
	        }
	    }

	    public function CallService($user, $pass)
	    {
	    	$client = new SoapClient("http://localhost:7251/WebService.asmx?WSDL");

		    // Prepare SoapHeader parameters
		    //$sh_param = array('Token'=>$token);
		    //$headers = new SoapHeader('http://viettelpost.org/', 'ServiceAuthHeader', $sh_param);

		    // Prepare Soap Client
		    //$client->__setSoapHeaders(array($headers));
		
			
			$result = $client->Login(array(
			'username' => $user,
			'password' => $pass
			));
			
			$response_arr = $this->objectToArray($result);
			return $response_arr["LoginResult"];
	    }
	}

 ?>