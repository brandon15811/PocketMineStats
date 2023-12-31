# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /publish PocketMineStats.Web/PocketMineStats.Web.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
EXPOSE 80
CMD ["dotnet", "PocketMineStats.Web.dll"]