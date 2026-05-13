// src/client/telemetryClient.ts

export async function sendTelemetry(apiUrl: string, payload: any) {
	await fetch(`${apiUrl}/telemetry`, {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(payload),
	});
}
