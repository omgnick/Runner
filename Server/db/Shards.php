<?php


class Shards {

    private static $connections = Array();
    public static $current_shard;



    public static function GetShardByIndex($index){
        if(!isset(self::$connections[$index])){
            self::$connections[$index] = new DatabaseConnection(DBConfig::GetDBConnectionInfo($index));
        }

        return self::$connections[$index];
    }



    public static function SelectShard($index) {
        self::$current_shard = self::GetShardByIndex($index);
        return self::$current_shard;
    }



    public static function SelectShardByUserId($user_id){
        $length = strlen($user_id);
        $value = 0;

        for($i = 0; $i < $length; $i++){
            $value += ord($user_id[$i]);
        }

        $shard_index = $value % DBConfig::GetShardsNumber();
        self::$current_shard = self::GetShardByIndex($shard_index);

        Debug::ServerLog("Selected shard ".self::$current_shard->connection_info['db']." for $user_id");

        return self::$current_shard;
    }






} 