FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Innowise.Clinic.Scheduling.Api/Innowise.Clinic.Scheduling.Api.csproj", "Innowise.Clinic.Scheduling.Api/"]
COPY ["Innowise.Clinic.Scheduling.Services/Innowise.Clinic.Scheduling.Services.csproj", "Innowise.Clinic.Scheduling.Services/"]
COPY ["Innowise.Clinic.Scheduling.Persistence/Innowise.Clinic.Scheduling.Persistence.csproj", "Innowise.Clinic.Scheduling.Persistence/"]
COPY ["Innowise.Clinic.Scheduling.Api/NuGet.Config", "./"]
RUN dotnet restore "Innowise.Clinic.Scheduling.Api/Innowise.Clinic.Scheduling.Api.csproj"
COPY . .
WORKDIR "/src/Innowise.Clinic.Scheduling.Api"
RUN dotnet build "Innowise.Clinic.Scheduling.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innowise.Clinic.Scheduling.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innowise.Clinic.Scheduling.Api.dll"]
