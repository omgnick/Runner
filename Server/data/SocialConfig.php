<?php

class SocialConfig {
    const API_MOBILE = 'mobile';
    const CURRENT_API = SocialConfig::API_MOBILE;



    private static $api_data = Array(
        SocialConfig::API_MOBILE => Array(
            'api_secret' => 'asskdm2381*@#&*amhaskMWKA'
        )
    );



    public static function GetAPIData($api){
        return self::$api_data[$api];
    }



    public static function GetCurrentAPIData(){
        return self::GetAPIData(self::CURRENT_API);
    }
}