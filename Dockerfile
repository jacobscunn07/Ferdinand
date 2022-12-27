FROM mcr.microsoft.com/dotnet/sdk:7.0.101-alpine3.17 AS build

WORKDIR /build

COPY ./Ferdinand.sln .
COPY ./src/Ferdinand.Domain.Primitives/Ferdinand.Domain.Primitives.csproj ./src/Ferdinand.Domain.Primitives/Ferdinand.Domain.Primitives.csproj  
COPY ./src/Ferdinand.Domain/Ferdinand.Domain.csproj ./src/Ferdinand.Domain/Ferdinand.Domain.csproj
COPY ./src/Ferdinand.Data/Ferdinand.Data.csproj ./src/Ferdinand.Data/Ferdinand.Data.csproj  
COPY ./src/Ferdinand.Application/Ferdinand.Application.csproj ./src/Ferdinand.Application/Ferdinand.Application.csproj  
COPY ./src/Ferdinand.Data.Migrations/Ferdinand.Data.Migrations.csproj ./src/Ferdinand.Data.Migrations/Ferdinand.Data.Migrations.csproj  
 
RUN dotnet restore

COPY . .

RUN dotnet build --no-restore -c "Release" \
    && dotnet publish --no-build -c "Release" -o /publish/Ferdinand.Data.Migrations /build/src/Ferdinand.Data.Migrations/

FROM mcr.microsoft.com/dotnet/aspnet:7.0.1-alpine3.17

WORKDIR /app

COPY --from=build /publish .

WORKDIR /app/Ferdinand.Data.Migrations

ENTRYPOINT [ "dotnet", "Ferdinand.Data.Migrations.dll" ]
