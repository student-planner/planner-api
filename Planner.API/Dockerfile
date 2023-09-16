# Необходимо выполнить команду dotnet publish -c Release
# После этого в папке bin/Release/net7.0/publish появится папка publish, в которую необходимо скопировать Dockerfile
# После этого необходимо выполнить команду docker build -t planner-api .

FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /src

COPY . ./
ENV ASPNETCORE_URLS="http://+:5050"
EXPOSE 5050

ENTRYPOINT  ["dotnet", "/src/Planner.API.dll", "http://*:5050"]