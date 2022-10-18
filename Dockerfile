#Stage 1
FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /src
COPY /ProfileManager/*.csproj ./
RUN dotnet restore
COPY /ProfileManager/ .
RUN dotnet build
RUN dotnet publish -c Release -o out

#Stage 2
FROM mcr.microsoft.com/dotnet/aspnet:latest AS runtime
WORKDIR /app
COPY --from=build /src/out .
EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT="Production"
# ENV Azure:Storage:ConnectionString=""
# ENV Azure:Storage:Container=""
# ENV Azure:Cosmos:ConnectionString=""
# ENV Azure:Cosmos:Database=""
# ENV Azure:Cosmos:Container=""
# ENV Azure:Cosmos:Partitionkey=""
# ENV Azure:ApplicationInsights:InstrumentationKey=""
# ENV Azure:Redis:ConnectionString=""
# ENV Azure:Redis:Prefix=""

ENTRYPOINT [ "dotnet", "ProfileManager.dll" ]
