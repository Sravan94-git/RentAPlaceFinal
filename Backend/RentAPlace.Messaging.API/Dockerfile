FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Backend/RentAPlace.Messaging.API/RentAPlace.Messaging.API.csproj", "Backend/RentAPlace.Messaging.API/"]
RUN dotnet restore "Backend/RentAPlace.Messaging.API/RentAPlace.Messaging.API.csproj"
COPY . .
RUN dotnet publish "Backend/RentAPlace.Messaging.API/RentAPlace.Messaging.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RentAPlace.Messaging.API.dll"]
