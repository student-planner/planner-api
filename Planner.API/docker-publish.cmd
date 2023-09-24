@echo off
dotnet publish -c Release
cd bin/Release/net7.0/publish
docker buildx build --platform linux/amd64 -t seljmov/planner-api:amd64 .
docker push seljmov/planner-api:amd64