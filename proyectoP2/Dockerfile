#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["proyectoP2/proyectoP2.csproj", "proyectoP2/"]
COPY ["ClasesVotafacil/ClasesVotafacil.csproj", "ClasesVotafacil/"]
RUN dotnet restore "proyectoP2/proyectoP2.csproj"
COPY . .
WORKDIR "/src/proyectoP2"
RUN dotnet build "proyectoP2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "proyectoP2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "proyectoP2.dll"]