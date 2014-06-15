<?php

class DBConfig {

    private static $db_shards = Array(
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_0'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_1'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_2'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_3'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_4'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_5'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_6'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_7'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_8'),
        Array('host' => 'localhost', 'uid' => 'root', 'password' => '', 'db' => 'runner_9')
    );



    public static function GetDBConnectionInfo($shard_index){
        return self::$db_shards[$shard_index];
    }



    public static function GetShardsNumber(){
        return count(self::$db_shards);
    }
} 