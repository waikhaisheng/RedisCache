Install Package:
https://github.com/microsoftarchive/redis/releases
****************************************************************
Port: 6379
****************************************************************
CMD command: visit https://redis.io/commands
>redis-cli
>ping
>clear
>set <Key> <Value>
>get <Key>
>expire <Key> <second>
>keys *
>keys k*
>config get *
>config set requirepass "<anyname>"
>auth "<anyname>"
>info
>client list
>monitor
****************************************************************
Install NuGet:
-StackExchange.Redis
-Microsoft.Extensions.Caching.StackExchangeRedis

Test: Nuget:
-Microsoft.Extensions.Configuration
-Microsoft.Extensions.DependencyInjection
-Microsoft.Extensions.Configuration.Json
****************************************************************
Redis Desktop Manager:
https://github.com/qishibo/AnotherRedisDesktopManager
****************************************************************
>docker run --name my-redis -p 5002:6369 -d redis
>docker ps -a
>docker exec -it my-redis sh
>dbsize
>select 0
>scan 0
>docker stop namecontainer
>docker rm namecontainer
>docker images
>exit
>KEYS *
>GET <keyname>