CREATE TABLE `google_iab_purchases` (
	`id` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
	`user_id` INT(10) UNSIGNED NOT NULL,
	`product_id` VARCHAR(50) NOT NULL COLLATE 'utf8_bin',
	`consume_index` INT(10) UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY (`id`),
	INDEX `user_id` (`user_id`)
)
COLLATE='utf8_bin'
ENGINE=InnoDB
;