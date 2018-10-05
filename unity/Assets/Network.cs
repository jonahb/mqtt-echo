using System;

public struct Network {
    public int bandwidth;
    public int latency;
    public int reliability;
    public int security;

    public override string ToString() {
        return String.Format("Network ({0}, {1}, {2}, {3})", bandwidth, latency, reliability, security);
    }
}
