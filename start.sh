#!/bin/sh

# Создаем файлы, если они не существуют
touch chatId.txt
touch .env
touch lastNearestDate.txt

# Проверяем, запущен ли контейнер denira-parser
CONTAINER_ID=$(docker ps -q -f name=denira-parser)

# Если контейнер запущен, останавливаем и удаляем его
if [ -n "$CONTAINER_ID" ]; then
  echo "Контейнер denira-parser уже запущен с ID $CONTAINER_ID. Останавливаем его..."
  docker stop "$CONTAINER_ID"
  docker rm "$CONTAINER_ID"
fi

# Запускаем Docker Compose с командой для сервиса denira-parser
docker-compose run --rm denira-parser

# После завершения интерактивного ввода запускаем контейнер в фоновом режиме
docker-compose run -d denira-parser
