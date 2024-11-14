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

# Проверяем, существует ли образ denira-parser:latest, и удаляем его, если он есть
IMAGE_ID=$(docker images -q denira-parser:latest)
if [ -n "$IMAGE_ID" ]; then
  echo "Образ denira-parser:latest найден. Удаляем его..."
  docker image rm -f "$IMAGE_ID"
else
  echo "Образ denira-parser:latest не найден, пропускаем удаление."
fi

# Запускаем Docker Compose с командой для сервиса denira-parser
docker-compose run --rm denira-parser

# После завершения интерактивного ввода запускаем контейнер в фоновом режиме
docker-compose run -d denira-parser