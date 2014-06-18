<?php

class RequestProcessor {

    private $data;
    private $command;
    const SIGNATURE_SECRET = '12#$WKNssa&@asd><';


    public function ProcessRequest($data){
        $this->data = $data;

        if(!isset($data['command'])){
            Output::Add('error', 'No command recieved!');
            return;
        } else {
            $this->command = $data['command'];
        }

        if(!$this->CheckSignature())
            return;

        $this->ProcessCommand();
    }



    private function CheckSignature() {
        if(!isset($this->data['sig'])){
            Debug::ErrorLog('Signature not found. Incorrect request');
            return false;
        }

        $sig = RequestProcessor::SIGNATURE_SECRET;

        foreach($this->data as $key => $value)
            if($key != 'sig')
                $sig .= $key.$value;

        $sig = md5($sig);

        if($sig != $this->data['sig']){
            Debug::ErrorLog('Wrong signature! Client version '.$this->data['sig'].' Server version '.$sig);
            return false;
        } else
            Debug::ServerLog('Signature is correct! Client version '.$this->data['sig'].' Server version '.$sig);

        return true;
    }



    private function ProcessCommand() {
        Debug::ServerLog('Processing command '.$this->command);

        switch($this->command){
            case 'login':
                $this->Login();
                break;
            case 'regular_run_ended':
                $this->SaveRegularRunResults();
                break;
            case 'buy_jump':
                $this->BuyJump();
                break;
            case 'buy_speed':
                $this->BuySpeed();
                break;
            case 'buy_hp':
                $this->BuyHP();
                break;
            case 'tournament_run_ended':
                $this->SaveTournamentRunResults();
                break;
            case 'get_tournament_data':
                $this->OutputTournament();
                break;
            case 'change_name':
                $this->ChangeName();
                break;

        }
    }



    private function Login(){
        switch(SocialConfig::CURRENT_API){
            case SocialConfig::API_MOBILE:
                $this->MobileLogin();
                break;
        }
    }



    private function MobileLogin(){
        $soc_info = SocialConfig::GetCurrentAPIData();

        if( $this->IsMobileUserRegistered() && $this->IsValidMobileAuthKey() ){
            $network_id = $this->data['network_id'];
        } else {
            $network_id = md5(uniqid($this->data['device_id']));
            $auth_key = md5($network_id.$soc_info['api_secret']);

            Output::Add('auth_key', $auth_key);
        }

        Output::Add('user', User::FindOrRegister($network_id)->GetOutputData());
    }



    private function IsMobileUserRegistered(){
        return isset($this->data['auth_key']) && isset($this->data['network_id']);
    }



    private function IsValidMobileAuthKey(){
        $soc_info = SocialConfig::GetCurrentAPIData();
        return md5($this->data['network_id'].$soc_info['api_secret']) == $this->data['auth_key'];
    }



    private function SaveRegularRunResults(){
        $user = User::FindOrRegister($this->data['network_id']);
        $user->gold += $this->data['gold'];
        $user->SaveToDatabase();
        Output::Add('user', $user->GetOutputData());
    }



    private function SaveTournamentRunResults(){
        $user = User::FindOrRegister($this->data['network_id']);
        $shard = Shards::GetShardByIndex(0);
        $shard->InsertOrUpdate(Array('network_id' => $user->network_id,
            'gold_collected' => $this->data['gold'],
            'user_name' => $user->name
        ), 'tournament_users');

        $shard->Query('UPDATE `tournaments` SET `total_prize` = total_prize + '.$this->data['gold'].' ORDER BY end_time DESC LIMIT 1');
    }



    private function BuyJump(){
        $user = User::FindOrRegister($this->data['network_id']);

        if($user->jump_level < 3 && $user->gold >= 100 + 100 * $user->jump_level){
            $user->gold -= 100 + 100 * $user->jump_level;
            $user->jump_level++;
            $user->SaveToDatabase();
            Output::Add('user', $user->GetOutputData());
        }
    }



    private function BuySpeed(){
        $user = User::FindOrRegister($this->data['network_id']);

        if($user->speed_level < 3 && $user->gold >= 250 + 125 * $user->speed_level){
            $user->gold -= 250 + 125 * $user->speed_level;
            $user->speed_level++;
            $user->SaveToDatabase();
            Output::Add('user', $user->GetOutputData());
        }
    }



    private function BuyHp(){
        $user = User::FindOrRegister($this->data['network_id']);

        if($user->hp_level < 3 && $user->gold >= 400 + 200 * $user->hp_level){
            $user->gold -= 400 + 200 * $user->hp_level;
            $user->hp_level++;
            $user->SaveToDatabase();
            Output::Add('user', $user->GetOutputData());
        }
    }



    private function OutputTournament(){
        $shard = Shards::GetShardByIndex(0);
        $tournament_users = $shard->SelectQuery(Array('*'), 'tournament_users', $where = ' ORDER BY `gold_collected` DESC ', $limit = '5');
        $tournament = $shard->SelectQuery(Array('*'), 'tournaments', $where = ' ORDER BY `end_time` DESC ', $limit = '1');

        $prize_users = Array();

        while($user = $tournament_users->fetch_assoc()){
            $prize_users[] = $user;
        }

        Output::Add("tournament", $tournament->fetch_assoc());
        Output::Add("tournament_users", $prize_users);
    }



    private function ChangeName(){
        $user = User::FindOrRegister($this->data['network_id']);
        $user->name = $this->data['changed_name'];
        $user->SaveToDatabase();
        Output::Add('user', $user->GetOutputData());
    }

}