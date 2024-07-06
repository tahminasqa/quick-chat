#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 8090
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Quick.Chat/Server/Quick.Chat.Server.csproj", "Quick.Chat/Server/"]
COPY ["Quick.Chat/Client/Quick.Chat.Client.csproj", "Quick.Chat/Client/"]
COPY ["Quick.Chat/Shared/Quick.Chat.Shared.csproj", "Quick.Chat/Shared/"]
RUN dotnet restore "./Quick.Chat/Server/Quick.Chat.Server.csproj"
COPY . .
WORKDIR "/src/Quick.Chat/Server"
RUN dotnet build "./Quick.Chat.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Quick.Chat.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Quick.Chat.Server.dll"]