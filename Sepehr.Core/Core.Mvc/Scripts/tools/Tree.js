var TreeView;
(function (TreeView) {
    var TrVw = (function () {
        function TrVw() {
            this._id = null;
            this._treeObj = null;
            this._kendoObj = null;
        }
        TrVw.prototype._initTree = function () {
            this._id = this._treeObj.id;
            this._kendoObj = $("#" + this._id).data('kendoTreeView');
        };
        TrVw.prototype._refresh = function () {
            this._initTree();
            this._kendoObj.dataSource.transport.cache._store = {};
            this._kendoObj.dataSource.read();
        };
        return TrVw;
    }());
    function refresh(treeView) {
        var tree = new TrVw();
        tree._treeObj = treeView;
        tree._refresh();
    }
    TreeView.refresh = refresh;
    function error(args) {
        var dataSource = args.sender;
        var data = args.sender._data;
        $.each(data, function (index, model) {
            if (model.isNew() || model.dirty) {
                dataSource.cancelChanges();
            }
        });
    }
    TreeView.error = error;
})(TreeView || (TreeView = {}));
