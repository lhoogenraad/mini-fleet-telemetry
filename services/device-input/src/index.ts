import { config } from "./config/config";
import { startDevice } from "./simulator/deviceSimulator";

console.log("device-input service started");

const runningDevices: any[] = [];

process.on("SIGINT", shutdown);
process.on("SIGTERM", shutdown);

function shutdown() {
	console.log("Shutting down device simulator...");
	process.exit(0);
}

// spawn fleet
for (let i = 0; i < config.deviceCount; i++) {
	const deviceId = `device-${i + 1}`;

	const interval = config.baseIntervalMs + Math.random() * 1000;

	const handle = startDevice(deviceId, config.apiUrl, interval);

	runningDevices.push(handle);
}
