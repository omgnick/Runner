ALTER TABLE `users` ADD COLUMN `jump_level` INT DEFAULT 0;
ALTER TABLE `users` ADD COLUMN `speed_level` INT DEFAULT 0;
ALTER TABLE `users` ADD COLUMN `hp_level` INT DEFAULT 0;
ALTER TABLE `users` ADD COLUMN `name` VARCHAR(16) DEFAULT 'PLAYER';