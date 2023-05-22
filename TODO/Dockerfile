FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TODO.csproj", "TODO/"]
RUN dotnet restore "TODO/TODO.csproj"
WORKDIR "/src/TODO"
COPY . .
RUN dotnet tool install --global dotnet-ef
RUN /bin/bash
RUN dotnet-ef
RUN export PATH="$PATH:$HOME/.dotnet/tools/"
RUN dotnet ef migrations add initialMigration --project TODO
RUN dotnet ef database update --project TODO
RUN dotnet build "TODO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TODO.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TODO.dll"]

RUN apt-get update
RUN apt install sqlite3 -y
