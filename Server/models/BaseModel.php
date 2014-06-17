<?php

class BaseModel {
    private $data = Array();
    private $changed_fields = Array();



    public function __set($name, $value){

        if(isset($this->data[$name]) && $this->data[$name] != $value)
            $this->changed_fields[$name] = $value;

        $this->data[$name] = $value;
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
        if(count($this->changed_fields) > 0){
            $data = $this->changed_fields;
            $data['id'] = $this->id;
            Shards::$current_shard->InsertOrUpdate($data, static::GetTableName());
        }
    }



    public static function FindByFields($fields){
        $shard = Shards::$current_shard;
        $result = $shard->SelectAllByFields($fields, static::GetTableName());

        if(!$result)
            return null;

        $result = $result->fetch_assoc();
        $instance = new static();
        $instance->SetData($result);

        return $instance;
    }



    public function SetField($name, $value){
        if(isset($this->data[$name]) && $this->data[$name] != $value)
            $this->changed_fields[$name] = $value;

        $data[$name] = $value;
    }



    public function SetData($data){
        $this->data = $data;
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