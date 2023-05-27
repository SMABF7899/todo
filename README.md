# TODO List
Before build project, check the following\
1- install sqlite3\
2- run this commands\
```dotnet tool install --global dotnet-ef```\
```dotnet-ef```\
```dotnet ef migrations add initialMigration --project TODO```\
```dotnet ef database update --project TODO```\
3- copy ```sample-env``` to ```.env``` and change your environment

For the new deployment, you need to backup the following files :\
```/app/database.db-wal``` and ```/app/database.db-shm```\
Of course, this is observed in the deployment script of the project

[Visit my website](https://smabf.ir)
