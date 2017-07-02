
module TreeView {

    class TrVw {
        _id = null;
        _treeObj:any = null;
        _kendoObj = null;

        _initTree() {

             this._id = this._treeObj.id;

             this._kendoObj = $("#" + this._id).data('kendoTreeView');

         }

       _refresh(){

            this._initTree();

            this._kendoObj.dataSource.transport.cache._store = {};

            this._kendoObj.dataSource.read();

         }

   }
   
    export function refresh(treeView) {

        var tree = new TrVw();

        tree._treeObj = treeView;

        tree._refresh();
    }

    export function error(args:any) {
       
        var dataSource = args.sender;
        var data = args.sender._data;
        $.each(data, (index, model) => {
            if (model.isNew() || model.dirty) {
                dataSource.cancelChanges();
            }
        });
    }
}





 