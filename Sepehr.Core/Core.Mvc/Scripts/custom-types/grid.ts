const scrollPosittion = {
    top: "top",
    bottom: "bottom"
}
class CacheContent {
    info: CacheInfo;
    key:CacheKey
}
class CacheInfo {

    public AggregateResults: any = null;
    public Data: Array<any> = [];
    public Errors: any = null;
    public Total: number = 0 
}
class CacheKey {
    public sort: string;
    public page: number = 1;
    public pageSize: number;
    public group: string;
    public filter: string;

}