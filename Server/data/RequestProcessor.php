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
}