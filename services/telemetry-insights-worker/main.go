package main

import (
	"context"
	"database/sql"
	"log"
	"net"

	pb "telemetry-insights-worker/proto"

	_ "github.com/lib/pq"
	"google.golang.org/grpc"
)


type server struct {
	pb.UnimplementedDeviceInsightsServer
	db *sql.DB
}

func (s *server) GetInsights(
	ctx context.Context,
	req *pb.InsightsRequest,
) (*pb.InsightsResponse, error) {

	rows, err := s.db.Query(`
	SELECT
	device_id,
	AVG(speed) as avg_speed,
	AVG(temperature) as avg_temperature,
	AVG(battery) as avg_battery,
	COUNT(*) as total_events
	FROM telemetry_raw
	GROUP BY device_id
	ORDER BY device_id
	`)

	if err != nil {
		return nil, err
	}

	defer rows.Close()

	response := &pb.InsightsResponse{}

	for rows.Next() {
		var insight pb.DeviceInsight

		var totalEvents int64

		err := rows.Scan(
			&insight.DeviceId,
			&insight.AvgSpeed,
			&insight.AvgTemperature,
			&insight.AvgBattery,
			&totalEvents,
		)

		insight.TotalEvents = int32(totalEvents)

		if err != nil {
			return nil, err
		}

		response.Devices = append(response.Devices, &insight)
	}

	log.Printf("Returned %d device insights", len(response.Devices))

	return response, nil
}

func main() {
	connStr := "host=postgres port=5432 user=postgres password=postgres dbname=telemetry sslmode=disable"

	db, err := sql.Open("postgres", connStr)

	if err != nil {
		log.Fatal(err)
	}

	lis, err := net.Listen("tcp", ":50051")

	if err != nil {
		log.Fatal(err)
	}

	grpcServer := grpc.NewServer()

	pb.RegisterDeviceInsightsServer(
		grpcServer,
		&server{db: db},
	)

	log.Println("Insights gRPC server running on :50051")

	if err := grpcServer.Serve(lis); err != nil {
		log.Fatal(err)
	}
}
