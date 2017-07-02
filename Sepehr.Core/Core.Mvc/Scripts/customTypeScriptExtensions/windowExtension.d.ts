interface Window {
    areaName: string;
    siteResetor: Function;
    gridSearchInstances: Array<gridInstanceInfo>;
    Math: any;
    active: boolean;
}
declare class gridInstanceInfo {
    instanc: ngSearchObj;
    id: string;
    active: boolean;
}
//interface Location {
//    origin:string
//}

interface Date {
    toGMTString(): string;
}


declare module kendo.data {
    var binders: any
   
}
declare function escape(encodedStr: string): string;


