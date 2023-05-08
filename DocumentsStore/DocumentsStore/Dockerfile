FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DocumentsStore/DocumentsStore.csproj", "DocumentsStore/"]
RUN dotnet restore "DocumentsStore/DocumentsStore.csproj"
COPY . .
WORKDIR "/src/DocumentsStore"
RUN dotnet build "DocumentsStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DocumentsStore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentsStore.dll"]
