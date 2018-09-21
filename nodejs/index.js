const mqtt = require("mqtt");
const readline = require("readline");

const topic = "moey";
const topicEcho = "moey_echo";
const mqttBrokerUrl = "mqtt://test.mosquitto.org";

const mqttClient  = mqtt.connect(mqttBrokerUrl);

mqttClient.on("connect", () => {
  mqttClient.subscribe(topicEcho);
});

const cli = readline.createInterface({
  input: process.stdin,
  output: process.stdout
});

mqttClient.on("message", (topic, payload) => {
  // mqtt assumes utf-8 and delivers payload as string
  const obj = JSON.parse(payload);
  console.log("> %s", obj["message"]);
});

cli.on("line", line => {
  const obj = { message: line };
  const payload = Buffer.from(JSON.stringify(obj), "utf8");
  mqttClient.publish(topic, payload);
});

