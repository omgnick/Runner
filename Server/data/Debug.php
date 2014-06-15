<?php

class Debug {

    private static $server_log = 'logs/server.log';
    private static $error_log = 'logs/error.log';



    private static function Log($message, $file) {
        $f = fopen($file, "a");
        $message = '['.date("d.m.Y H:i:s") .'] '. $message . "\n";
        fwrite($f, $message);
        fclose($f);
    }



    public static function ServerLog($message) {
        self::Log($message, self::$server_log);
    }



    public static function ErrorLog($message) {
        self::Log($message, self::$error_log);
    }


} 