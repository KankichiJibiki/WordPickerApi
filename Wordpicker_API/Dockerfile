# Use .NET Core SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
# Set working directory
WORKDIR /app

# Copy .csproj file
COPY *.csproj ./
RUN dotnet restore

# Copy the authentication file into the container
RUN mkdir /app/creds
COPY wordpicker-419405-e5e458ecb99b.json /app/creds/

# Set environment variable pointing to the authentication file
ENV GOOGLE_APPLICATION_CREDENTIALS=/app/creds/wordpicker-419405-e5e458ecb99b.json

# Copy the remaining source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out


# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
# Set working directory
WORKDIR /app
# Copy the published app from build environment
COPY --from=build-env /app/out .

# Install NGINX and configure
FROM nginx:latest AS nginx
# Copy your NGINX configuration to the container
COPY nginx.conf /etc/nginx/nginx.conf

# Final stage
FROM runtime AS final
# Expose NGINX port
EXPOSE 80
# Continue with your ENTRYPOINT
ENTRYPOINT ["dotnet", "Wordpicker_API.dll"]