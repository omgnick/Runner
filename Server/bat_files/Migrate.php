<?php
error_reporting(E_ALL);
chdir("../db");

require_once '../data/Debug.php';
require_once 'DBConfig.php';
require_once 'DatabaseConnection.php';
require_once 'Shards.php';

$migrations_table_name = 'migrations';

//$names = preg_replace("/.*\//", '', $files);
chdir('migrations');
$files = glob("*.sql");

$shards_number = DBConfig::GetShardsNumber();

for($i = 0; $i < $shards_number; $i++){

    $shard = Shards::GetShardByIndex($i);
    $executed_migrations = Array();
    $old_migrations = Array();

    //Fetching already executed migrations
    $result = $shard->Query("SELECT * FROM $migrations_table_name");

    if($result){
        while($item = $result->fetch_assoc())
            $old_migrations[] = $item['name'];
    }

    //Executing new migrations
    foreach($files as $file){
        if(!in_array($file, $old_migrations)){
            echo 'Database: '.$shard->connection_info['db'].' Migration: '.$file."\n";

            $f = fopen($file, 'r');
            $sql = fread($f, filesize($file));
            fclose($f);

            if(!$shard->connection->multi_query($sql))
                echo "Migration failed.\n\nError #".$shard->connection->errno.":".$shard->connection->error."\n\n";
            else{
                do {
                    if ($result = $shard->connection->store_result())
                        $result->free();
                } while ($shard->connection->next_result());

                $executed_migrations[] = $file;
            }
        }
    }


    //Refreshing migrations table
    if(count($executed_migrations) > 0){
        $sql = '';

        foreach($executed_migrations as $migration){
            if(strlen($sql) == 0)
                $sql = "INSERT INTO $migrations_table_name (`name`) VALUES ('$migration')";
            else
                $sql .= ",('$migration')";
        }

        if(!$shard->Query($sql))
            echo "Can't save migrations results.\n\nError #".$shard->connection->errno.":"
                .$shard->connection->error."\n\n";
    }
}