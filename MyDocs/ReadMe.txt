https://docs.microsoft.com/en-us/windows/wsl/install
username:owner
password:123456
https://redis.io/docs/getting-started/installation/install-redis-on-windows/
curl -fsSL https://packages.redis.io/gpg | sudo gpg --dearmor -o /usr/share/keyrings/redis-archive-keyring.gpg

echo "deb [signed-by=/usr/share/keyrings/redis-archive-keyring.gpg] https://packages.redis.io/deb $(lsb_release -cs) main" | sudo tee /etc/apt/sources.list.d/redis.list

sudo apt-get update
sudo apt-get install redis
sudo service redis-server start
redis-cli 
127.0.0.1:6379> ping
PONG
sudo service redis-server stop