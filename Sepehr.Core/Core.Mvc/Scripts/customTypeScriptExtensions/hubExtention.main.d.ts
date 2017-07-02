
interface SignalR {
    siteVersionHub: HubProxy;
}
interface HubProxy {
    client: VersionClient;
    server: VersionServer;
}

interface VersionClient {
    reset: () => void;
}
interface VersionServer {
    check(): JQueryPromise<any> ;
}