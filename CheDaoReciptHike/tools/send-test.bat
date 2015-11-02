ECHO ¡°Test Start "
FOR /L %%G IN (1,1,1000) DO \opt\nc\nc 127.0.0.1 3344 -w 1 < sample.dat