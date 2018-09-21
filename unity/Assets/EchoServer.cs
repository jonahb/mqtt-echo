using System;
using UnityEngine;

using Encoding = System.Text.Encoding;

using JSON = SimpleJSON.JSON;
using JSONNode = SimpleJSON.JSONNode;
using JSONObject = SimpleJSON.JSONObject;

using MqttClient = uPLibrary.Networking.M2Mqtt.MqttClient;
using MqttMsgBase = uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase;
using MqttMsgPublishEventArgs = uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs;

public class EchoServer : MonoBehaviour {
	private MqttClient mqttClient;

	public string topic = "moey";
	public string echoTopic = "moey_echo";
	public string mqttBrokerHost = "test.mosquitto.org";

	void Start() {
		mqttClient = new MqttClient(mqttBrokerHost);
		mqttClient.MqttMsgPublishReceived += HandleClientMqttMsgPublishReceived;

		String clientId = Guid.NewGuid().ToString();
		mqttClient.Connect(clientId);

		string[] topics = { topic };
		byte[] qualityOfServiceLevels = { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE };
		mqttClient.Subscribe(topics, qualityOfServiceLevels);
	}

	void HandleClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {
		JSONNode obj = JSON.Parse(Encoding.UTF8.GetString(e.Message));
		string message = obj["message"];
		Debug.Log(message);

		JSONNode echoObj = new JSONObject();
		echoObj["message"] = message;
		mqttClient.Publish(echoTopic, Encoding.UTF8.GetBytes(echoObj.ToString()));
	}
}
