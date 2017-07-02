var Helpers;
(function (Helpers) {
    function MapObjects(sourceObjs, func) {
        var result = [];
        for (var i = 0; i < sourceObjs.length; i++) {
            result.push(func(sourceObjs[i]));
        }
        return result;
    }
    Helpers.MapObjects = MapObjects;
    function sortJSON(data, key) {
        return data.sort(function (a, b) {
            var x = a[key];
            var y = b[key];
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
        });
    }
    Helpers.sortJSON = sortJSON;
})(Helpers || (Helpers = {}));
