# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем все остальные файлы
COPY . ./

# Собираем проект
RUN dotnet publish -c Release -o /app/publish

# Используем официальный образ .NET Runtime для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем собранное приложение из стадии build
COPY --from=build /app/publish .

# Указываем команду для запуска приложения
ENTRYPOINT ["dotnet", "DeniraParser.dll"]