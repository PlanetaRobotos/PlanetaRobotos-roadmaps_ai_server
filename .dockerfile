FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["CourseAI.Api/CourseAI.Api.csproj", "CourseAI.Api/"]
RUN dotnet restore "CourseAI.Api/CourseAI.Api.csproj"

# Copy the rest of the code
COPY . .
WORKDIR "/src/CourseAI.Api"

# Build and publish
RUN dotnet build "CourseAI.Api.csproj" -c Release -o /app/build
RUN dotnet publish "CourseAI.Api.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5501
ENTRYPOINT ["dotnet", "CourseAI.Api.dll"]