#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ChatService.Web/ChatService.Web.csproj", "src/ChatService.Web/"]
COPY ["src/ChatService.Infrastructure/ChatService.Infrastructure.csproj", "src/ChatService.Infrastructure/"]
COPY ["src/ChatService.Domain/ChatService.Domain.csproj", "src/ChatService.Domain/"]
COPY ["src/ChatService.Application/ChatService.Application.csproj", "src/ChatService.Application/"]
RUN dotnet restore "src/ChatService.Web/ChatService.Web.csproj"
COPY . .
WORKDIR "/src/src/ChatService.Web"
RUN dotnet build "ChatService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatService.Web.dll"]