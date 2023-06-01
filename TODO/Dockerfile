FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt install sqlite3 -y

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN ls -l
RUN apt-get update
RUN apt install sqlite3 -y
COPY ["TODO.csproj", "TODO/"]
RUN dotnet restore "TODO/TODO.csproj"
WORKDIR "/src/TODO"
COPY . .
RUN dotnet tool install --global dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet-ef
WORKDIR /src
RUN dotnet ef migrations add initialMigration --project TODO
RUN dotnet ef database update --project TODO
WORKDIR "/src/TODO"
RUN dotnet build "TODO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TODO.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN cp database.db /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TODO.dll"]
