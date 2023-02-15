

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5001

ENV ASPNETCORE_URLS=http://+:5001
ENV TZ=America/Sao_Paulo

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Template.Api/Template.Api.csproj", "src/Template.Api/"]
RUN dotnet restore "src/Template.Api/Template.Api.csproj"
COPY . .
WORKDIR "/src/src/Template.Api"
RUN dotnet build "Template.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

WORKDIR /
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN date

# Run application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Template.Api.dll"]
