FROM mcr.microsoft.com/dotnet/core/sdk:2.2
COPY / /publish
WORKDIR /publish/GraphLabs.Backend.Api
EXPOSE 5000/tcp 5001/tcp
ENV ASPNETCORE_URLS=http://*:5001;http://*:5000
ENV ASPNETCORE_ENVIRONMENT=Development
RUN dotnet dev-certs https
RUN dotnet build
CMD ["dotnet", "run"]