# Learn about building .NET container images
# httpsgithub.comdotnetdotnet-dockerblobmainsamplesREADME.md
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY RampRage_WebAPI_ITStep/RampRage_WebAPI_ITStep.csproj RampRage_WebAPI_ITStep/
RUN dotnet restore RampRage_WebAPI_ITStep/RampRage_WebAPI_ITStep.csproj

# copy everything else and build app
COPY . .
WORKDIR /source/RampRage_WebAPI_ITStep
RUN dotnet publish -c Release -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
RUN apt-get update && apt-get install -y \
    wget \
    gnupg \
    lsb-release \
    && echo "deb http://apt.postgresql.org/pub/repos/apt/ $(lsb_release -c | awk '{print $2}')-pgdg main" > /etc/apt/sources.list.d/pgdg.list && \
    wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | apt-key add - && \
    apt-get update && \
    apt-get install -y postgresql-client-17 && \
    apt-get remove -y wget gnupg lsb-release && \
    apt-get clean
	
ENV CONNECTIONSTRING="Host=ep-raspy-pond-a2zjeqhy-pooler.eu-central-1.aws.neon.tech;Database=neondb;Username=neondb_owner;Password=npg_nuU9BGHNIcq2"
	
COPY --from=build /app .

ENTRYPOINT ["dotnet", "RampRage_WebAPI_ITStep.dll"]