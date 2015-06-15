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
        Output::Add('permanent', Array('time' => time()));
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
            case 'update_user':
                $this->UpdatePlayerFromSave();
            break;
            case 'request_google_iab_payload':
                $this->ProcessRequestGoogleIABPayload();
            break;
            case 'google_iab_purchases':
                $this->ProcessGoogleIABPurchase();
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
        $create = false;
        if( $this->IsMobileUserRegistered() && $this->IsValidMobileAuthKey() ){
            $network_id = $this->data['network_id'];
        } else {
            $network_id = md5(uniqid($this->data['device_id']));
            $auth_key = md5($network_id.$soc_info['api_secret']);
            $create = true;
            Output::Add('auth_key', $auth_key);
        }

        Output::Add('user', User::FindOrRegister($network_id, $create)->GetOutputData());
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

        $result = $shard->SelectQuery(Array('`gold_collected`'), '`tournament_users`', ' where `network_id` = \''.
            $user->network_id.'\'', 1);

        $previous_result = 0;

        if($result){
            $result = $result->fetch_assoc();
            $previous_result = $result['gold_collected'];
        }

        if($previous_result < $this->data['gold']){
            $shard->InsertOrUpdate(Array('network_id' => $user->network_id,
                'gold_collected' => $this->data['gold'],
                'user_name' => $user->name
            ),
            'tournament_users');
        }

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



    private function UpdatePlayerFromSave(){
        $user = User::FindOrRegister($this->data['network_id']);
        $user->name = $this->data['name'];
        $user->gold = $this->data['gold'];
        $user->hp_level = $this->data['hp_level'];
        $user->jump_level = $this->data['jump_level'];
        $user->speed_level = $this->data['speed_level'];
        $user->last_update_time = $this->data['last_update_time'] + 1;
        $user->SaveToDatabase();
        Output::Add('user', $user->GetOutputData());
    }



    private function ProcessRequestGoogleIABPayload(){
        $user = User::FindOrRegister($this->data['network_id']);

        $data = $this->data;
        $product_id = $data['product_id'];
        $shard = Shards::SelectShardByUserId($user->network_id);

        $purchase = GoogleIABPurchase::FindByFields (
            Array('user_id' => $user->id,
                'product_id' => $product_id)
        );

        if($purchase == null){
            $payload = $user->network_id.'+0';
        } else {
            $payload = $user->network_id.'+'.($purchase->consume_index + 1);
        }

        Output::Add('google_iab_purchase_payload', Array('product_id' => $product_id, 'payload' => $payload));
    }



    private function ProcessGoogleIABPurchase(){
        $data = $this->data;
        $purchases = json_decode($data['purchases'], true);

        $processed_products = Array();

        foreach($purchases as $value_json) {

            $value = json_decode($value_json, true);
            $processed_products[] = $this->ConsumeGoogleIABPurchase($value, $data);
        }

        Output::add('google_iab_purchases_rewarded', $processed_products);
    }



    private function ConsumeGoogleIABPurchase($purchase_info, $data){
        Debug::ServerLog('Try Consume Google IAB Purchase!');
        $purchase_data = json_decode($purchase_info['purchase_data'], true);
        $product_id = $purchase_data['productId'];

        if($this->VerifyGoogleIABPurchase($purchase_info['purchase_data'], $purchase_info['purchase_signature'])){

            $payload = $purchase_info['payload'];
            $payload_items = explode('+', $payload);

            if(!$payload_items || count($payload_items) < 2) {
                Debug::ServerLog('Small Payload!');
                return Array('product_id' => $product_id, 'is_consumed' => false);
            }

            $payload_user_id = $payload_items[0];
            $payload_consume_index = $payload_items[1];

            //One google account may have different in game accounts, we have to reward the one that donated
            $user = User::FindOrRegister($payload_user_id);

            if($user == null) {
                Debug::ServerLog('User not Found');
                return Array('product_id' => $product_id, 'is_consumed' => false);
            }

            $shard = Shards::SelectShardByUserId($user->network_id);

            if($payload_user_id == $user->network_id){

                $goods_entry = ProductsManager::GetProductEntryById($product_id);

                if($goods_entry != null) {
                    Debug::ServerLog('Got good entry');
                    $purchase = GoogleIABPurchase::FindByFields(
                        Array('user_id' => $user->id,
                            'product_id' => $product_id));

                    Debug::ServerLog(var_export($purchase, true));

                    //If we didn't processed this request earlier
                    if( $purchase == null || $purchase->consume_index < $payload_consume_index ) {

                        if(ProductsManager::RewardProduct($product_id, $user)){

                            if($purchase == null)

                                $purchase = GoogleIABPurchase::create(
                                    Array(
                                        'user_id' => $user->id,
                                        'product_id' => $product_id,
                                        'consume_index' => $payload_consume_index
                                    ),
                                    $shard);

                            else
                                $purchase->consume_index = $payload_consume_index;

                            $purchase->SaveToDatabase();

                        } else{
                            Debug::ServerLog('Cant reward');

                            return Array('product_id' => $product_id, 'is_consumed' => false);
                        }
                    }
                    Debug::ServerLog('Already consumed');
                    return Array('product_id' => $product_id, 'is_consumed' => true);
                }
            }
        }
        Debug::ServerLog('Failed Verify Google IAB Purchase!');

        return Array('product_id' => $product_id, 'is_consumed' => false);
    }

    private function VerifyGoogleIABPurchase($signed_data, $signature)
    {
        //using PHP to create an RSA key
        $key = openssl_get_publickey('file://'.str_replace('\\', '/', getcwd()).'/certificates/android.pem');
        //$signature should be in binary format, but it comes as BASE64.
        //So, I'll convert it.
        $signature = base64_decode($signature);
        //using PHP's native support to verify the signature
        $result = openssl_verify(
            $signed_data,
            $signature,
            $key,
            OPENSSL_ALGO_SHA1);

        return $result === 1;
    }
}