FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChatService.Web/ChatService.Web.csproj", "ChatService.Web/"]
COPY ["ChatService.Infrastructure/ChatService.Infrastructure.csproj", "ChatService.Infrastructure/"]
COPY ["ChatService.Application/ChatService.Application.csproj", "ChatService.Application/"]
COPY ["ChatService.Domain/ChatService.Domain.csproj", "ChatService.Domain/"]
RUN dotnet restore "ChatService.Web/ChatService.Web.csproj"
COPY . .
WORKDIR "/src/ChatService.Web"
RUN dotnet build "ChatService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatService.Web.dll"]