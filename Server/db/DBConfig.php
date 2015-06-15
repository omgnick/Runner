<?php

class DBConfig {

    private static $db_shards = Array(
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_0'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_1'),
    );



    public static function GetDBConnectionInfo($shard_index){
        return self::$db_shards[$shard_index];
    }



    public static function GetShardsNumber(){
        return count(self::$db_shards);
    }
} 