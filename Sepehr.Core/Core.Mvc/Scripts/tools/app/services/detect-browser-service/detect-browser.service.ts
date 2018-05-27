import { ServiceBase } from 'Areas/Core/Scripts/tools/app/services/service-base';
import { Injectable } from '@angular/core';
import { DeviceTypes } from 'Areas/Core/Scripts/tools/app/services/detect-browser-service/models/device-type';
import { AgentInfo } from 'Areas/Core/Scripts/tools/app/services/detect-browser-service/models/agent-type';

@Injectable()
export class DetectBrowserService extends ServiceBase {
    constructor() {
        super();
    }

    public detectBrowser() {

        var userAgent = window.navigator.userAgent;

        var browsers = new BrowserTypes();
        var devices = new DeviceTypes();
        var agentInfo = new AgentInfo();


        for (var key in browsers) {
            if (browsers[key].test(userAgent)) {
                agentInfo.browser = key;
                break;
            }
        }

        for (var key in devices) {
            if (devices[key].test(userAgent)) {
                agentInfo.device = key;
                break;
            }
        }
        return agentInfo;
    }
}


