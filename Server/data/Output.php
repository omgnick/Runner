<?php

class Output {

    private static $data = Array();



    public static function Add($field, $data){
        self::$data[$field] = $data;
    }



    public static function Send(){
        print(json_encode(self::$data));
    }

} 