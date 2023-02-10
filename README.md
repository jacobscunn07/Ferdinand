# Ferdinand

## Prerequisites

Create a directory to be used for any sort of application data that will need to be persisted.
```shell
mkdir ~/.ferdinand
```

Add to your `.zhsrc` or equivalent file.
```shell
export FERDINAND_DOCKER_VOLUMES_ROOT=~/.ferdinand
```

## Data Access

### Entity Framework Core

#### Migrations

```shell
dotnet ef migrations add Initial \
-s src/Ferdinand.Data.Migrations \
-p src/Ferdinand.Data \
-o EntityFrameworkCore/Migrations
```
