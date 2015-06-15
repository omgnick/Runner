<?php
chdir('..');

if(!file_exists('certificates'))
    mkdir('certificates');

$android_public_key = 'MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnn3gnWtau+ecW0kpaA2S2cdDtQ88WmSzBmHuZ4Y92obvGCjdXrVmQdkj5Ro2S3yiq6jd700BEZxG+pjZuaIlWZucTcJycf/UMR6d1COUVygCurk22pztemWUHGroHKKhxd7brLJRK2wnoCEaOGEiGP0YDNXZIoEmfN7wDfxtRGFk+q7ckUYru754LaOYgWPcinNUO7DCfo4KdHYerSebby3dVBc+9npePM7wMYi5C642VZjgBhND+Oety7wyYbh8S/XGRdYdKny8I7qx7DCWs+Fs0cPrvNIHOfMrxLuhsQKyYoE69cexbXSeIR+/HtERpH43hX+RsIQg4vfdlc1AMQIDAQAB';

if($android_public_key != null){
    $key =	"-----BEGIN PUBLIC KEY-----\n".
        chunk_split($android_public_key, 64,"\n").
        '-----END PUBLIC KEY-----';

    $stream = fopen('certificates/android.pem', 'w+');
    fwrite($stream, $key);
    fclose($stream);

    echo 'generated ';
}
