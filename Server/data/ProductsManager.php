<?php

class ProductsManager {

    private static $products = Array(
        'gold500' => Array(
            'id' => 'gold500',
            'reward' => '500',
            'reward_field' => 'gold'
        ),
        'gold1500' => Array(
            'id' => 'gold1500',
            'reward' => '1500',
            'reward_field' => 'gold'
        ),
        'gold3000' => Array(
            'id' => 'gold3000',
            'reward' => '3000',
            'reward_field' => 'gold'
        )
    );



    public static function GetProductEntryById($product_id) {
        return isset(self::$products[$product_id]) ? self::$products[$product_id] : null;
    }



    public static function RewardProduct($product_id, $user){
        $product = self::$products[$product_id];
        $field = $product['reward_field'];
        $amount = $product['reward'];

        $user->$field += $amount;
        $user->SaveToDatabase();
        Output::Add('user', $user->GetOutputData());
        return true;
    }
}