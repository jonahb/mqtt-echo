# MQTT Examples

These examples show how JavaScript, Unity, and TouchDesigner can exchange
reliable messages over MQTT. There are echo servers for Unity and TouchDesigner
and a command-line client for Node.js.

## Client (Node.js)

The JavaScript client sends messages to the echo server and displays the
responses. To run, install Node.js. Then open a console, navigate to the root
of this repo, and:

```
cd nodejs
npm install
node index.js
```

Type a message and press "return." Nothing is echoed because no echo servers
are running.

## Echo Server (Unity)

1. Open the `unity` folder in Unity
2. Press play
3. Type a message into the console where the client is running

The client prints the echo, and the message is logged to the Unity console.

## Echo Server (TouchDesigner)

1. Open `touch_designer/mqtt-echo.toe` in TouchDesigner
2. Type a message into the console where the client is running

The client prints the echo, and the message appears in the Table DAT in
TouchDesigner.
