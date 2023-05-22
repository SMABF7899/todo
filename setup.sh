#!/bin/bash

RED='\033[1;31m'
GREEN='\033[1;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

set -e
echo -e "${YELLOW}[local] cd TODO${NC}"
cd TODO
echo -e "${YELLOW}[local] docker build --rm -t todo-list:latest .${NC}"
docker build --rm -t todo-list:latest .
echo -e "${YELLOW}[local] docker rm -f -v TODO-List${NC}"
docker rm -f -v TODO-List
echo -e "${YELLOW}[local] docker run --name TODO-List -d --network-alias todo-list -p 5000:5000 -p 7011:7011 --network net_c34-122-0 -e ASPNETCORE_HTTP_PORT=https:+7011 -e ASPNETCORE_URLS=http://+:5000 todo-list:latest${NC}"
docker run --name TODO-List -d --network-alias todo-list -p 5000:5000 -p 7011:7011 --network net_c34-122-0 -e ASPNETCORE_HTTP_PORT=https:+7011 -e ASPNETCORE_URLS=http://+:5000 todo-list:latest
sleep 5
echo -e "${YELLOW}[local] curl --max-time 30 -s http://127.0.0.1:5000${NC}"
curl --max-time 30 -s http://127.0.0.1:5000
