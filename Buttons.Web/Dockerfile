FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Buttons.Web/Buttons.Web.csproj", "Buttons.Web/"]
RUN dotnet restore "Buttons.Web/Buttons.Web.csproj"
COPY . .
WORKDIR "/src/Buttons.Web"
RUN dotnet build "Buttons.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Buttons.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Buttons.Web.dll"]
