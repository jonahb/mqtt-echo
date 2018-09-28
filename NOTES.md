## Setup

### MacOS

1. Install Homebrew
2. `brew install mosquitto`

### Windows

Follow the "Quick Windows Install" instructions
[here](http://www.steves-internet-guide.com/install-mosquitto-broker/).

## Command-line Tools

To send a message using the mosquitto command-line tool:

```
mosquitto_pub -h test.mosquitto.org -t moey -m '{"message": "hiya"}'
```

## Local Broker

1. In TouchDesigner, change the "Network Address" parameter of the MQTT Client
   DAT to `tcp://localhost:1883`. Change the "Active" parameter to "Off" and
   back to "On".

2. In Unity, inspect the object named `EchoServer`. Change the property "MQTT
   Broker Host" to `localhost`. Restart the scene.

3. In `nodejs/index.js`, change the value of `mqttBrokerUrl` to
   `mqtt://localhost`. Restart the client.

Now the components will communicate via the local broker. To test the local
broker with the mosquitto command-line tools, change the value of the `-h`
option.
