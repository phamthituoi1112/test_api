<?php 
	require_once 'connect.class.php';

	$connect = new Connect();

	$result = $connect->CallService("tuoipt", "123");

	print_r($result);
?>