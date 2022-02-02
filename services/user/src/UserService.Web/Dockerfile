#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UserService.Web/UserService.Web.csproj", "src/UserService.Web/"]
COPY ["src/UserService.Infrastructure/UserService.Infrastructure.csproj", "src/UserService.Infrastructure/"]
COPY ["src/UserService.Application/UserService.Application.csproj", "src/UserService.Application/"]
COPY ["src/UserService.Domain/UserService.Domain.csproj", "src/UserService.Domain/"]
RUN dotnet restore "src/UserService.Web/UserService.Web.csproj"
COPY . .
WORKDIR "/src/src/UserService.Web"
RUN dotnet build "UserService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Web.dll"]