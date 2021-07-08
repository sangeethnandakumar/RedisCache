# ASP.NET Core API With RedisCache
Redis Cache implementation in Docker

## Installing Redis With Docker
### Download Docker Image
`docker pull redis`
### Start The Container
`docker run --name local-redis -p 6379:6739 -d redis`
### Verify Image
`docker images`
### Verify Logs
`docker logs -f local-redis`
### Get Inside With Bash
`docker exec -it local-redis /bin/bash`
### Run a ping test on RedisCLI
`redis-cli`
`ping`
You will get `PONG` as response if everything is working
