<?php


class User extends BaseModel {


    public static function FindOrRegister($network_id){

        $shard = Shards::SelectShardByUserId($network_id);
        $user = User::FindByFields(Array('network_id' => $network_id));

        if(!$user) {
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