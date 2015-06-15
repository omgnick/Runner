<?php

class DatabaseConnection {

    public $connection;
    public $connection_info;


    public function __construct($connection_info){
        $this->connection = new mysqli($connection_info['host'], $connection_info['uid'], $connection_info['password'],
                                $connection_info['db']);

        $this->connection_info = $connection_info;

        if ($this->connection->connect_errno) {
            Debug::ErrorLog('Connect failed: '.$this->connection->connect_error."\n"
                .var_export($connection_info, true)."\n" );
            return false;
        }

        return $this->connection;
    }



    public function __destruct(){
        $this->connection->close();
    }



    public function Query($sql){
        Debug::ServerLog("SQL: ".$sql);
        return $this->connection->query($sql);
    }



    public function SelectQuery($fields, $table, $where = '', $limit = ''){
        $sql = '';

        foreach($fields as $field)
            if(strlen($sql) == 0)
                $sql = 'SELECT '.$field;
            else
                $sql .= ','.$field;

        $sql .= ' FROM '.$table;

        if($where)
            $sql .= ' '.$where;

        if($limit)
            $sql .= ' LIMIT '.$limit;

        return $this->Query($sql);
    }



    public function SelectAllByFields($fields, $table, $limit = 1) {
        $first = true;

        foreach($fields as $field => $value){
            if($first){
                $where = ' WHERE `'.$field."`='".$this->connection->real_escape_string($value)."'";
                $first = false;
            } else {
                $where .= ' AND `'.$field."`='".$this->connection->real_escape_string($value)."'";
            }
        }

        $sql = 'SELECT * FROM '.$table.$where;

        if($limit)
            $sql .= ' LIMIT '.$limit;

        return $this->Query($sql);
    }



    public function InsertOrUpdate($fields, $table) {
        $sql = 'INSERT INTO '.$table;
        $sql_fields = '';
        $sql_values = '';
        $sql_update = '';
        $first = true;

        foreach($fields as $key => $value) {
            if($first) {
                $sql_fields = ' (`'.$key.'`';
                $sql_values = " VALUES ('".$this->connection->real_escape_string($value)."'";
                $sql_update = ' ON DUPLICATE KEY UPDATE `'.$key."`='".$this->connection->real_escape_string($value)."'";
                $first = false;
            } else {
                $sql_fields .= ',`'.$key.'`';
                $sql_values .= ",'".$this->connection->real_escape_string($value)."'";
                $sql_update .= ", `".$key."`='".$this->connection->real_escape_string($value)."'";
            }
        }

        $sql .= $sql_fields.')'.$sql_values.')'.$sql_update;

        $this->Query($sql);
        return $this->connection->insert_id;
    }



    public function Insert($fields, $table){
        $sql = 'INSERT INTO '.$table;
        $sql_fields = '';
        $sql_values = '';
        $first = true;

        foreach($fields as $key => $value) {
            if($first) {
                $sql_fields = ' (`'.$key.'`';
                $sql_values = " VALUES ('".$this->connection->real_escape_string($value)."'";
                $first = false;
            } else {
                $sql_fields .= ',`'.$key.'`';
                $sql_values .= ",'".$this->connection->real_escape_string($value)."'";
            }
        }

        $sql .= $sql_fields.')'.$sql_values.')';
        $this->Query($sql);
        return $this->connection->insert_id;
    }
}