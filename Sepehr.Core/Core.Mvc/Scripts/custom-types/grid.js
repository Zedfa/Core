var scrollPosittion = {
    top: "top",
    bottom: "bottom"
};
var CacheContent = (function () {
    function CacheContent() {
    }
    return CacheContent;
}());
var CacheInfo = (function () {
    function CacheInfo() {
        this.AggregateResults = null;
        this.Data = [];
        this.Errors = null;
        this.Total = 0;
    }
    return CacheInfo;
}());
var CacheKey = (function () {
    function CacheKey() {
        this.page = 1;
    }
    return CacheKey;
}());
