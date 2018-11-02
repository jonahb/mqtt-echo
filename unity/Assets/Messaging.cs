using System;
using System.Collections.Generic;
using UnityEngine;

using Encoding = System.Text.Encoding;

using JSON = SimpleJSON.JSON;
using JSONNode = SimpleJSON.JSONNode;
using JSONObject = SimpleJSON.JSONObject;

using MqttClient = uPLibrary.Networking.M2Mqtt.MqttClient;
using MqttMsgBase = uPLibrary.Networking.M2Mqtt.Messages.MqttMsgBase;
using MqttMsgPublishEventArgs = uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs;

public class Messaging : MonoBehaviour
{
    private MqttClient mqttClient;
    Queue<byte[]> messageQueue = new Queue<byte[]>();

    public string mqttTopic = "moey";
    public string mqttBrokerHost = "test.mosquitto.org";

    #region Unity Events

    private void Start() {
        mqttClient = new MqttClient(mqttBrokerHost);
        mqttClient.MqttMsgPublishReceived += HandleClientMqttMsgPublishReceived;

        String clientId = Guid.NewGuid().ToString();
        mqttClient.Connect(clientId);

        string[] topics = { mqttTopic };
        byte[] qualityOfServiceLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
        mqttClient.Subscribe(topics, qualityOfServiceLevels);
    }

    private void OnDestroy() {
        if (mqttClient != null) {
            mqttClient.Disconnect();
        }
    }

    private void Update() {
        while (messageQueue.Count > 0) {
            DispatchMessage(messageQueue.Dequeue());
        }
    }

    #endregion

    #region Sending

    public void SendCompletedMission() {
        JSONObject json = new JSONObject();
        json["type"] = "completed-mission";
        SendMessage(json);
    }

    #endregion

    #region Receiving

    protected void ReceivedStartMission(Network network, int mission, bool autoPlay, int numberOfPlayers) {
        Debug.LogFormat("Network: {0}", network);
        Debug.LogFormat("Mission: {0}", mission);
        Debug.LogFormat("Auto-play: {0}", autoPlay);
        Debug.LogFormat("Number of players: {0}", numberOfPlayers);
    }

    protected void ReceivedSetNetwork(Network network) {
        Debug.LogFormat("Network: {0}", network);
    }

    protected void ReceivedEndMission() {
    }

    #endregion

    private void HandleClientMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) {
        messageQueue.Enqueue(e.Message);
    }

    private void DispatchMessage(byte[] message) {
        JSONNode json = JSON.Parse(Encoding.UTF8.GetString(message));
        Network network;

        if (json["type"] == null) {
            Debug.LogWarning("Invalid message: no type");
            return;
        }

        Debug.LogFormat("Received: {0}", (string)json["type"]);

        switch ((string)json["type"]) {
            case "start-mission":
                network = ParseNetwork(json["network"].AsObject);
                int mission = (int)json["mission"].AsLong;
                bool autoPlay = json["autoPlay"].AsBool;
                int numberOfPlayers = (int)json["numberOfPlayers"].AsLong;
                ReceivedStartMission(network, mission, autoPlay, numberOfPlayers);
                break;
            
            case "set-network":
                network = ParseNetwork(json["network"].AsObject);
                ReceivedSetNetwork(network);
                break;
            
            case "end-mission":
                ReceivedEndMission();
                break;
            
            default:
                Debug.LogWarningFormat("Discarded: {0}", json["type"]);
                break;
        }
    }

    private void SendMessage(JSONNode json) {
        byte[] message = Encoding.UTF8.GetBytes(json.ToString());
        mqttClient.Publish(mqttTopic, message, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }

    private static Network ParseNetwork(JSONObject json) {
        Network network;
        network.bandwidth = (int)json["bandwidth"].AsLong;
        network.latency = (int)json["latency"].AsLong;
        network.reliability = (int)json["reliability"].AsLong;
        network.security = (int)json["security"].AsLong;
        return network;
    }
}
