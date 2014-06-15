<?php

require_once "data/Debug.php";
require_once 'data/Output.php';
require_once 'data/SocialConfig.php';

require_once 'db/DBConfig.php';
require_once 'db/DatabaseConnection.php';
require_once 'db/Shards.php';

require_once 'data/RequestProcessor.php';

require_once 'models/BaseModel.php';
require_once 'models/User.php';


Debug::ServerLog("Incoming request \n".var_export($_REQUEST, true));

$processor = new RequestProcessor();
$processor->ProcessRequest($_REQUEST);

Output::Send();


