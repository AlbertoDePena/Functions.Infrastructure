FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS installer-env

COPY ./Functions.Infrastructure /src/Functions.Infrastructure
COPY ./Demo /src/Demo
RUN cd /src/Demo && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

FROM mcr.microsoft.com/azure-functions/dotnet:3.0
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]