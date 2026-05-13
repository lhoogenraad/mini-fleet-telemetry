import { sendTelemetry } from "../client/telemetryClient";

function random(min: number, max: number) {
	return Math.random() * (max - min) + min;
}

function createPayload(deviceId: string) {
	return {
		deviceId,
		timestamp: new Date().toISOString(),
		latitude: random(-90, 90),
		longitude: random(-180, 180),
		speed: random(0, 200),
		battery: random(20, 100),
		temperature: random(-10, 40),
	};
}

export function startDevice(deviceId: string, apiUrl: string, intervalMs: number) {
	console.log(`Starting device ${deviceId}`);

	const interval = setInterval(async () => {
		try {
			const payload = createPayload(deviceId);
			await sendTelemetry(apiUrl, payload);
			console.log(`${deviceId} → sent`);
		} catch (err) {
			console.error(`${deviceId} failed`, err);
		}
	}, intervalMs);

	return {
		stop: () => clearInterval(interval),
	};
}
