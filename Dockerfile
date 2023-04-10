FROM mcr.microsoft.com/dotnet/sdk:7.0.101-alpine3.17 AS build

WORKDIR /build

COPY ./Ferdinand.sln .
COPY ./tests/Ferdinand.Application.Tests.Integration/Ferdinand.Application.Tests.Integration.csproj ./tests/Ferdinand.Application.Tests.Integration/Ferdinand.Application.Tests.Integration.csproj
COPY ./tests/Ferdinand.Data.Tests.Integration/Ferdinand.Data.Tests.Integration.csproj ./tests/Ferdinand.Data.Tests.Integration/Ferdinand.Data.Tests.Integration.csproj
COPY ./tests/Ferdinand.Domain.Tests.Unit/Ferdinand.Domain.Tests.Unit.csproj ./tests/Ferdinand.Domain.Tests.Unit/Ferdinand.Domain.Tests.Unit.csproj
COPY ./src/Ferdinand.Domain.Primitives/Ferdinand.Domain.Primitives.csproj ./src/Ferdinand.Domain.Primitives/Ferdinand.Domain.Primitives.csproj  
COPY ./src/Ferdinand.Domain/Ferdinand.Domain.csproj ./src/Ferdinand.Domain/Ferdinand.Domain.csproj
COPY ./src/Ferdinand.Data/Ferdinand.Data.csproj ./src/Ferdinand.Data/Ferdinand.Data.csproj  
COPY ./src/Ferdinand.Application/Ferdinand.Application.csproj ./src/Ferdinand.Application/Ferdinand.Application.csproj  
COPY ./src/Ferdinand.Data.Migrations/Ferdinand.Data.Migrations.csproj ./src/Ferdinand.Data.Migrations/Ferdinand.Data.Migrations.csproj  
COPY ./src/Ferdinand.Api/Ferdinand.Api.csproj ./src/Ferdinand.Api/Ferdinand.Api.csproj

RUN dotnet restore

COPY . .

RUN dotnet build --no-restore -c "Release" \
    && dotnet publish --no-build -c "Release" -o /publish/Ferdinand.Data.Migrations /build/src/Ferdinand.Data.Migrations/ \
    && dotnet publish --no-build -c "Release" -o /publish/Ferdinand.Api /build/src/Ferdinand.Api/

FROM mcr.microsoft.com/dotnet/aspnet:7.0.1-alpine3.17 AS release
WORKDIR /app
COPY --from=build /publish .
COPY --from=build /build/run.sh .

FROM release as release-api
WORKDIR /app/Ferdinand.Api
ENTRYPOINT [ "dotnet", "Ferdinand.Api.dll" ]

FROM release as release-migrations
WORKDIR /app/Ferdinand.Data.Migrations
ENTRYPOINT [ "dotnet", "Ferdinand.Data.Migrations.dll" ]

FROM build as tests
ENTRYPOINT [ "/bin/sh", "run_tests.sh" ]
