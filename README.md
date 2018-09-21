# MQTT Examples

This directory contains:

1. TouchDesigner MQTT echo server
2. Unity MQTT echo server
3. Node.js MQTT client

## Setup

Mosquitto is an open-source MQTT broker and command-line tools for sending and
receiving MQTT messages. Ensure you have Homebrew installed, then:

```
brew install mosquitto
```

## Echo Server (Touch Designer)

Open `touch_designer/mqtt-echo.toe` in TouchDesigner. The MQTT Client Dat will
connect to a test MQTT server on the Internet (test.mosquitto.org).

On the command line, run:

```
mosquitto_pub -h test.mosquitto.org -t moey -m '{"message": "hiya"}'
```

Note that "hiya" appears in the Table DAT in TouchDesigner.

## Echo Server (Unity)

Open the `unity` folder in Unity. Press play. On the command line, run:

```
mosquitto_pub -h test.mosquitto.org -t moey -m '{"message": "hiya"}'
```

Note that "hiya" appears on the Unity console.

## Client (Node.js)

The JavaScript client sends messages to the echo server via MQTT and displays
the echo responses. To run, ensure you have Node.js installed, then:

```
cd nodejs
npm install
node index.js
```

Type a message and press "return." The message is displayed in TouchDesigner
and/or Unity.  TouchDesigner and/or Unity echo the message, and the echoes are
printed on the console.

## Local MQTT Broker

To this point, we have been using a test MQTT broker on the Internet. To run a
local broker:

1. On the command line, run `mosquitto`.

2. In TouchDesigner, change the "Network Address" parameter of the MQTT Client
   DAT to `tcp://localhost:1883`. Change the "Active" parameter to "Off" and
   back to "On".

3. In Unity, inspect the object named `EchoServer`. Change the property "MQTT
   Broker Host" to `localhost`. Restart the scene.

4. In `nodejs/index.js`, change the value of `mqttBrokerUrl` to
   `mqtt://localhost:1883`. Restart the client.

Now the components will communicate via the local broker. To test the local
broker with the mosquitto command-line tools, change the value of the `-h`
option.
