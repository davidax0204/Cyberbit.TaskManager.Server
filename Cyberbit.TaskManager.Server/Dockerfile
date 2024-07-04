#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Cyberbit.TaskManager.Server/Cyberbit.TaskManager.Server.csproj", "Cyberbit.TaskManager.Server/"]
RUN dotnet restore "Cyberbit.TaskManager.Server/Cyberbit.TaskManager.Server.csproj"
COPY . .
WORKDIR "/src/Cyberbit.TaskManager.Server"
RUN dotnet build "Cyberbit.TaskManager.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cyberbit.TaskManager.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cyberbit.TaskManager.Server.dll"]