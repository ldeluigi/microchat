FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["UserService.Web/UserService.Web.csproj", "UserService.Web/"]
COPY ["UserService.Infrastructure/UserService.Infrastructure.csproj", "UserService.Infrastructure/"]
COPY ["UserService.Application/UserService.Application.csproj", "UserService.Application/"]
COPY ["UserService.Domain/UserService.Domain.csproj", "UserService.Domain/"]
RUN dotnet restore "UserService.Web/UserService.Web.csproj"
COPY . .
WORKDIR "/src/UserService.Web"
RUN dotnet build "UserService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Web.dll"]