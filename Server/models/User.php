<?php


class User extends BaseModel {


    public static function FindOrRegister($network_id, $create = false){

        $shard = Shards::SelectShardByUserId($network_id);
        $user = User::FindByFields(Array('network_id' => $network_id));


        if($create) {
            $user = User::Create(Array(
                    'network_id' => $network_id,
                    'gold' => 0
                ), $shard
            );
        }

        return $user;
    }



    public static function GetTableName(){
        return 'users';
    }

} 