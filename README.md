# Fleet Backend

> Shipping app.
> =

# Getting Started

## Setup database

1. Install [__Docker__](https://docs.docker.com/engine/install/)
2. Run `docker compose -f docker-compose.yaml up`
3. Use local connection string
   - `Server=localhost,1434;Database=FleetDev;User Id=sa;Password=MyPassword123!;Encrypt=false;MultipleActiveResultSets=true`

## Run the app

- `dotnet run src/Fleet.Api/Fleet.Api.csproj`

Or using Docker:

- `docker build -t fleet-backend .`
- `docker run -d -p 7277:80 --name fleet-backend fleet-backend`


- `gcloud auth login`
- `gcloud config set project gen-lang-client-0693639934`
