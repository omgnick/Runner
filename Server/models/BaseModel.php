<?php

class BaseModel {
    private $data = Array();
    private $changed_fields = Array();



    public function __set($name, $value){

        if(isset($this->data[$name]) && $this->data[$name] != $value)
            $this->changed_fields[$name] = $value;

        $data[$name] = $value;
    }



    public function __get($name){
        return $this->data[$name];
    }



    public static function GetTableName(){
        return '';
    }



    public function GetOutputData(){
        return $this->data;
    }



    public function SaveToDatabase(){
        if(count($this->changed_fields) > 0)
            Shards::$current_shard->InsertOrUpdate($this->changed_fields, static::GetTableName());
    }



    public static function FindByFields($fields){
        $shard = Shards::$current_shard;
        $result = $shard->SelectAllByFields($fields, static::GetTableName());

        if(!$result)
            return null;

        $result = $result->fetch_assoc();
        $instance = new static();

        foreach($result as $key => $value)
            $instance[$key] = $value;

        return $instance;
    }



    public static function Create($fields, $shard){
        $id = $shard->Insert($fields, static::GetTableName());

        $instance = new static();
        $fields['id'] = $id;
        $instance->SetFields($fields);

        return $instance;
    }



    public function SetFields($fields){
        $this->data = $fields;
    }
}