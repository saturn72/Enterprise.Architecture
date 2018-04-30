FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Programmer.Server.sln ./
COPY ../Components/PnpServiceFake/PnpServiceFake.csproj ../Components/PnpServiceFake/
COPY Core/EventBus/EventBus.csproj Core/EventBus/
COPY Core/RabbitMqEventBus/RabbitMqEventBus.csproj Core/RabbitMqEventBus/
COPY Object.Extensions/Object.Extensions.csproj Object.Extensions/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/../Components/PnpServiceFake
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PnpServiceFake.dll"]
