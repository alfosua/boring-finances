FROM mcr.microsoft.com/dotnet/sdk:5.0 AS builder
COPY . /source
WORKDIR /source
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
COPY --from=builder /publish /app
WORKDIR /app
ENTRYPOINT ["dotnet", "BoringSoftware.Finances.WebApi.dll"]