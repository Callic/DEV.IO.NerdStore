#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Services/NSR.Carrinho.API/NSR.Carrinho.API.csproj", "src/Services/NSR.Carrinho.API/"]
RUN dotnet restore "src/Services/NSR.Carrinho.API/NSR.Carrinho.API.csproj"
COPY . .
WORKDIR "/src/src/Services/NSR.Carrinho.API"
RUN dotnet build "NSR.Carrinho.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NSR.Carrinho.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NSR.Carrinho.API.dll"]