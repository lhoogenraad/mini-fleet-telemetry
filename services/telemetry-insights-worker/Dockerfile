FROM golang:1.25-alpine AS build

WORKDIR /app

COPY . .

RUN go mod tidy
RUN go build -o insights-worker .

FROM alpine:latest

WORKDIR /app

COPY --from=build /app/insights-worker .

EXPOSE 50051

CMD ["./insights-worker"]
