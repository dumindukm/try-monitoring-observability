#docker build -t monitoring-observability .

# docker tag mygateway localhost:5000/mygateway
# docker push localhost:5000/mygateway

# docker run -d -p 51080:80 --name api_gateway mygateway
# docker run -it --rm -p 26952:80 -p 44339:443 --name api_monitor monitoring-observability
# docker run -it --rm -p 51080:80 -p 51443:443 -v D:\Work\vs2019\kubernetes_sample\file_share:/app/file_share --name api_gateway mygateway
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY *.csproj ./
RUN dotnet restore


COPY . ./
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "monitoring-observability.dll"]