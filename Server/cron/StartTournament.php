<?php
chdir('..');
require_once "data/Debug.php";
require_once 'db/DBConfig.php';
require_once 'db/DatabaseConnection.php';
require_once 'db/Shards.php';
require_once 'models/BaseModel.php';
require_once 'models/User.php';

$shard = Shards::GetShardByIndex(0);

$old_results = $shard->SelectQuery(Array('network_id'), 'tournament_users', $where = ' ORDER BY `gold_collected` DESC ', $limit = '5');

if($old_results !== false){
    $old_tournament = $shard->SelectQuery(Array('total_prize'), 'tournaments', $where = ' ORDER BY `end_time` DESC ', $limit = '1');

    if($old_tournament !== false) {
        $old_tournament = $old_tournament->fetch_assoc();
        $prize_users = Array();

        while($user = $old_results->fetch_assoc()){
            $prize_users[] = User::FindOrRegister($user['network_id']);
        }

        foreach($prize_users as $user){
            $user->gold += floor($old_tournament['total_prize'] / count($prize_users));
            $user->SaveToDatabase();
        }
    }
}

$shard->Query('TRUNCATE TABLE tournament_users');
$shard->Insert(Array('end_time' => time() + 600), 'tournaments');

