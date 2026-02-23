FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JournalChallenge.Presentation/JournalChallenge.Presentation.csproj", "JournalChallenge.Presentation/"]
COPY ["JournalChallenge.Application/JournalChallenge.Application.csproj", "JournalChallenge.Application/"]
COPY ["JournalChallenge.Application.Core/JournalChallenge.Application.Core.csproj", "JournalChallenge.Application.Core/"]
COPY ["JournalChallenge.Domain/JournalChallenge.Domain.csproj", "JournalChallenge.Domain/"]
COPY ["JournalChallenge.Infrastructure/JournalChallenge.Infrastructure.csproj", "JournalChallenge.Infrastructure/"]
COPY ["JournalChallenge.Infrastructure.Core/JournalChallenge.Infrastructure.Core.csproj", "JournalChallenge.Infrastructure.Core/"]
RUN dotnet restore "JournalChallenge.Presentation/JournalChallenge.Presentation.csproj"
COPY . .
WORKDIR "/src/JournalChallenge.Presentation"
RUN dotnet build "JournalChallenge.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JournalChallenge.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Generate EF Migration Bundle
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet ef migrations bundle --project ../JournalChallenge.Infrastructure --startup-project . -o /app/publish/efbundle && chmod +x /app/publish/efbundle

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JournalChallenge.Presentation.dll"]
