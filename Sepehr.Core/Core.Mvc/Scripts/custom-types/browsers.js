var BrowserTypes = (function () {
    function BrowserTypes() {
        this.chrome = /chrome/i;
        this.safari = /safari/i;
        this.firefox = /firefox/i;
        this.ie = /internet explorer/i;
        this.opera = /Opera Mini/i;
    }
    return BrowserTypes;
}());
var DeviceTypes = (function () {
    function DeviceTypes() {
        this.windows = /IEMobile/i;
        this.android = /Android/i;
        this.blackBerry = /BlackBerry/i;
        this.ios = /iPhone|iPad|iPod/i;
    }
    return DeviceTypes;
}());
var AgentInfo = (function () {
    function AgentInfo() {
        this.device = undefined;
        this.browser = undefined;
    }
    return AgentInfo;
}());
