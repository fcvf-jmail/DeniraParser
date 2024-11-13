#!/bin/sh

# Создаем файлы, если они не существуют
touch chatId.txt
touch .env
touch lastNearestDate.txt

# Запускаем Docker Compose с командой для сервиса denira-parser
docker-compose run --rm denira-parser

# После завершения интерактивного ввода запускаем контейнер в фоновом режиме
docker-compose run -d denira-parser