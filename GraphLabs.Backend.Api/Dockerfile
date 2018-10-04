FROM microsoft/dotnet:2.1-aspnetcore-runtime
COPY ./publish /publish
WORKDIR /publish
EXPOSE 5000/tcp 5001/tcp
ENV ASPNETCORE_URLS=http://*:5001;http://*:5000
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "GraphLabs.Backend.Api.dll"]