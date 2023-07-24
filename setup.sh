#!/bin/bash

RED='\033[1;31m'
GREEN='\033[1;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}[local] rm /home/gitlab-runner/Backups/backup-$CI_PIPELINE_ID.sql${NC}"
rm /home/gitlab-runner/Backups/backup-$CI_PIPELINE_ID.db
set -e
echo -e "${YELLOW}[local] buildDate=`date +"%Y-%m-%d %H:%M:%S "`+0330${NC}"
buildDate=`date +"%Y-%m-%d %H:%M:%S "`+0330
echo -e "${YELLOW}[local] docker exec -i TODO-List sqlite3 database.db  \".backup backup-$CI_PIPELINE_ID.db\"${NC}"
docker exec -i TODO-List sqlite3 database.db ".backup backup-$CI_PIPELINE_ID.db"
echo -e "${YELLOW}[local] docker cp TODO-List:/app/backup-$CI_PIPELINE_ID.db /home/gitlab-runner/Backups/${NC}"
docker cp TODO-List:/app/backup-$CI_PIPELINE_ID.db /home/gitlab-runner/Backups/
echo -e "${YELLOW}[local] docker stop TODO-List${NC}"
docker stop TODO-List
echo -e "${YELLOW}[local] cd TODO${NC}"
cd TODO
echo -e "${YELLOW}[local] docker build --rm -t todo-list:latest .${NC}"
docker build --rm -t todo-list:latest .
echo -e "${YELLOW}[local] docker rm -f -v TODO-List${NC}"
docker rm -f -v TODO-List
echo -e "${YELLOW}[local] docker run --name TODO-List -d --network-alias todo-list -p 5000:5000 -p 7011:7011 --network net_c34-122-0 -e ASPNETCORE_HTTP_PORT=https:+7011 -e ASPNETCORE_URLS=http://+:5000 todo-list:latest${NC}"
docker run --name TODO-List -d --network-alias todo-list -p 5000:5000 -p 7011:7011 --network net_c34-122-0 -e ASPNETCORE_HTTP_PORT=https:+7011 -e ASPNETCORE_URLS=http://+:5000 todo-list:latest
sleep 5
echo -e "${YELLOW}[local] cd ..${NC}"
cd ..
echo -e "${YELLOW}[local] docker cp $ENV TODO-List:/app/.env${NC}"
docker cp $ENV TODO-List:/app/.env
echo -e "${YELLOW}[local] docker exec -i TODO-List sed -i "s/BUILD_DATE=.*/BUILD_DATE=$buildDate/g" /app/.env${NC}"
docker exec -i TODO-List sed -i "s/BUILD_DATE=.*/BUILD_DATE=$buildDate/g" /app/.env
echo -e "${YELLOW}[local] docker cp /home/gitlab-runner/Backups/backup-$CI_PIPELINE_ID.db TODO-List:/app/${NC}"
docker cp /home/gitlab-runner/Backups/backup-$CI_PIPELINE_ID.db TODO-List:/app/
echo -e "${YELLOW}[local] docker exec -i TODO-List sqlite3 database.db  \".restore backup-$CI_PIPELINE_ID.db\"${NC}"
docker exec -i TODO-List sqlite3 database.db ".restore backup-$CI_PIPELINE_ID.db"
echo -e "${YELLOW}[local] docker restart TODO-List${NC}"
docker restart TODO-List
sleep 5
i=1
while [[ ${result} != ${buildDate} ]]; do
    echo -e "${YELLOW}Try "$i" of 20 ...${NC}"
    sleep 10
    echo -e "${YELLOW}[local] curl --max-time 30 -s http://127.0.0.1:5000/buildDate${NC}"
    result=`curl --max-time 30 -s http://127.0.0.1:5000/buildDate`
    echo -e "${YELLOW}Expected Result: ${buildDate}${NC}"
    echo -e "${YELLOW}Current  Result: ${result}${NC}"
    ((i=i+1))
    if [ $i == 20 ]; then
        echo -e "${RED}Error in Deploy${NC}"
        exit 1
    fi
done
echo -e "${GREEN}Deployed Successfully${NC}"
