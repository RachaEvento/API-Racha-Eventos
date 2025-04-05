FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrganizadorEventos/OrganizadorEventos.csproj", "OrganizadorEventos/"]
RUN dotnet restore "OrganizadorEventos/OrganizadorEventos.csproj"
COPY . .
WORKDIR "/src/OrganizadorEventos"
RUN dotnet build "OrganizadorEventos.csproj" -c Release -o /app/build

# Etapa 3: Publish
FROM build AS publish
RUN dotnet publish "OrganizadorEventos.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 4: Final (Runtime)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN adduser --disabled-password --gecos "" appuser
USER appuser

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "OrganizadorEventos.dll"]
