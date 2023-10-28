FROM mcr.microsoft.com/dotnet/sdk:7.0.101-alpine3.17 AS build

WORKDIR /build

COPY ./Ferdinand.sln .
COPY ./tests/Ferdinand.Testing/Ferdinand.Testing.csproj ./tests/Ferdinand.Testing/Ferdinand.Testing.csproj
COPY ./tests/Ferdinand.Application.Tests.Integration/Ferdinand.Application.Tests.Integration.csproj ./tests/Ferdinand.Application.Tests.Integration/Ferdinand.Application.Tests.Integration.csproj
COPY ./tests/Ferdinand.Infrastructure.Tests.Integration/Ferdinand.Infrastructure.Tests.Integration.csproj ./tests/Ferdinand.Infrastructure.Tests.Integration/Ferdinand.Infrastructure.Tests.Integration.csproj
COPY ./tests/Ferdinand.Domain.Tests.Unit/Ferdinand.Domain.Tests.Unit.csproj ./tests/Ferdinand.Domain.Tests.Unit/Ferdinand.Domain.Tests.Unit.csproj
COPY ./src/Ferdinand.Extensions.Hosting/Ferdinand.Extensions.Hosting.csproj ./src/Ferdinand.Extensions.Hosting/Ferdinand.Extensions.Hosting.csproj
COPY ./src/Ferdinand.Domain.Primitives/Ferdinand.Domain.Primitives.csproj ./src/Ferdinand.Domain.Primitives/Ferdinand.Domain.Primitives.csproj  
COPY ./src/Ferdinand.Domain/Ferdinand.Domain.csproj ./src/Ferdinand.Domain/Ferdinand.Domain.csproj
COPY ./src/Ferdinand.Infrastructure/Ferdinand.Infrastructure.csproj ./src/Ferdinand.Infrastructure/Ferdinand.Infrastructure.csproj  
COPY ./src/Ferdinand.Application/Ferdinand.Application.csproj ./src/Ferdinand.Application/Ferdinand.Application.csproj  
COPY ./src/Ferdinand.DataMigrations/Ferdinand.DataMigrations.csproj ./src/Ferdinand.DataMigrations/Ferdinand.DataMigrations.csproj  
COPY ./src/Ferdinand.Api/Ferdinand.Api.csproj ./src/Ferdinand.Api/Ferdinand.Api.csproj
COPY ./src/Ferdinand.Jobs/Ferdinand.Jobs.csproj ./src/Ferdinand.Jobs/Ferdinand.Jobs.csproj

RUN dotnet restore

COPY . .

RUN dotnet build --no-restore -c "Release" \
    && dotnet publish --no-build -c "Release" -o /publish/Ferdinand.DataMigrations /build/src/Ferdinand.DataMigrations/ \
    && dotnet publish --no-build -c "Release" -o /publish/Ferdinand.Api /build/src/Ferdinand.Api/ \
    && dotnet publish --no-build -c "Release" -o /publish/Ferdinand.Jobs /build/src/Ferdinand.Jobs/

FROM mcr.microsoft.com/dotnet/aspnet:7.0.1-alpine3.17 AS release
WORKDIR /app
COPY --from=build /publish .

FROM release as release-api
WORKDIR /app/Ferdinand.Api
ENTRYPOINT [ "dotnet", "Ferdinand.Api.dll" ]

FROM release as release-jobs
WORKDIR /app/Ferdinand.Jobs
ENTRYPOINT [ "dotnet", "Ferdinand.Jobs.dll" ]

FROM release as release-migrations
WORKDIR /app/Ferdinand.DataMigrations
ENTRYPOINT [ "dotnet", "Ferdinand.DataMigrations.dll" ]

FROM build as tests
ENTRYPOINT [ "/bin/sh", "run_tests.sh" ]
