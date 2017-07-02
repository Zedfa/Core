module Helpers {
    export function MapObjects<T, U>(sourceObjs: T[] , func: (x: T) =>  U ): U[] {
        var result: U[] = [];
        for (var i = 0; i < sourceObjs.length; i++) {
            result.push(func(sourceObjs[i]));
        }
        return result;
    }

    export function sortJSON<T>(data: T[], key: string): T[] {
        return data.sort((a, b) => {
            var x = a[key];
            var y = b[key];
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
       });
    }

}