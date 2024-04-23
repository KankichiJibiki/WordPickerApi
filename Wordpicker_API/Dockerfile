# RUNTIME image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
RUN mkdir /app/creds
COPY wordpicker-419405-e5e458ecb99b.json /app/creds/
ENV GOOGLE_APPLICATION_CREDENTIALS=/app/creds/wordpicker-419405-e5e458ecb99b.json
EXPOSE 81 443
ENV TZ='Asia/Tokyo'

# Use .NET Core SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Wordpicker_API.csproj Wordpicker_API/
WORKDIR /src/Wordpicker_API
RUN dotnet restore Wordpicker_API.csproj
COPY . .
RUN dotnet build Wordpicker_API.csproj -c Release -o /app/build

# Publish image
FROM build AS publish
RUN dotnet publish Wordpicker_API.csproj -c Release -o /app/publish 

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install -y nginx
#COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 5200
ENTRYPOINT ["dotnet", "Wordpicker_API.dll"]