/// <reference path="../typings/kendo/kendo.all.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />

module DialogBox {

    export class Dialog {
        _btnConfirm: JQuery = $("<button/>", { id: 'RP_btnConfirm', style: 'margin-left:10px;', "class": 'k-button k-button-icontext k-primary mainbtn', text: "تایید", "data-bind": "vm: save" });
        _iconConfirm :JQuery = $("<span/>", { "class": 'k-icon k-i-tick' });
        _btnCancel: JQuery = $("<button/>", { id: 'RP_btnCancel', style: 'margin-left:2px;', "class": 'k-button k-button-icontext k-grid-cancel', text: "انصراف", "data-bind": "vm: cancel" });
        _iconCancel  :JQuery = $("<span/>", { "class": 'k-icon k-i-cancel' });
        _footer: JQuery = $("<div/>", { style: 'margin:2px 3px;position: relative;left: 1px;bottom:0px;' });
        _container   :JQuery = $("<div/>", { "data-role": "dialogBox" });
        _win: kendo.ui.Window = null;
        _width: any = null;
        _height: any = null;

        _initWindow(obj, closeCallBack) {

        }

        _setFooter(htmlObj, OkButton, cancelButton):JQuery {
            var tempJQ = this._footer;
            tempJQ.html(htmlObj);

            if ((OkButton && cancelButton) || (OkButton && cancelButton == undefined)) {
                //this._btnConfirm.append(this._iconConfirm);
                //this._btnCancel.append(this._iconCancel);
                tempJQ.append( this._btnCancel, this._btnConfirm);
            }
            else if (OkButton) {
                this._btnConfirm.append(this._iconConfirm);
                tempJQ.append(this._btnConfirm);
            }
            return tempJQ;
        }

        _formatingMessages(message:string) {
            var messageStruct;
            try {
                messageStruct = $.parseJSON(message);

                if (messageStruct.ModelState != "undefined") {
                    var lstProperties = [];
                    for (var prop in messageStruct.ModelState) {
                        lstProperties.push(prop);
                    }
                    messageStruct.Message = messageStruct.ModelState[lstProperties[0]].toString();
                }
                var messagePart = "<div>" + messageStruct.Message + "</div>";

                var seperator = messageStruct.Details ? '<hr class="seperator" />' : "";
                var detailsPart = messageStruct.Details ? "<div>" + messageStruct.Details + "</div>" : "";

                messageStruct = messagePart + seperator + detailsPart;
            } catch (e) {
                messageStruct = message;
            }
            return messageStruct;
            }

        _showError(message:string, isRTL:boolean, hasOkButton:boolean, hasCancelButton:boolean) {

            var parentTag:JQuery = $("#divMessage");

            var container:JQuery = this._container;

            container.addClass("k-block k-error-colored");

            container.addClass(isRTL ? "k-rtl" : "k-ltr");
            container.addClass(isRTL ? "right-align" : "left-align");

            container.html("<div>" + this._formatingMessages(message) + "</div>");

            if (hasOkButton || hasCancelButton) {
                container.append(this._setFooter(null, hasOkButton, hasCancelButton));

            }
            parentTag.html(<any>container);

            var win = container.kendoWindow({
                width: this._width,
                height: this._height,
                modal: true,
                actions: !(hasOkButton || hasCancelButton) ? ["Close"] : [],
                //  actions: ["Minimize", "Maximize", "Close"],
                title: "خطا",
                visible: false,
                resizable: false,
                //position:{ top:100,
                //      //  left:200 }
                close: function (e:kendo.ui.WindowEvent) {
                    this.destroy();
                }//,

                //       deactivate: function () {
                //           this.destroy();
                //       }
            }
                ).data("kendoWindow");

            win.center().open();

            this._btnCancel.click(function (e:JQueryEventObject) { win.destroy(); });
            this._btnConfirm.click(function (e: JQueryEventObject) {
                win.destroy();

            });
        }

        _showConfirm(message: string, url: string) {
            var parenTag:JQuery = $("#divMessage");

            var container: JQuery = this._container;
            container.addClass("k-rtl k-block k-success-colored");
            container.html("<text>" + message + "</text>");
            container.append(this._setFooter(null, true , null));

            parenTag.html(<any>container);

            var cnfrm = container.kendoWindow({
                //minWidth: "20%",
                //minHeight: "10%",
                width: this._width,
                height: this._height,
                modal: true,
                title: "تاییدیه",
                visible: false,
                resizable: true
            }
                ).data("kendoWindow");
            cnfrm.open().center();

            this._btnCancel.click(function (e: JQueryEventObject) { cnfrm.destroy(); });
            this._btnConfirm.click(function (e: JQueryEventObject) {
                cnfrm.destroy();
                window.location.href = url;
            });
        }

        _showDialog(contentUrl: string, title: string, confirmAction:string, sendigPlainObject:any, hasOkButton?:boolean, hasCancelButton?:boolean) {

            var parenTag = $("#divMessage");

            var container: JQuery= this._container;

            parenTag.append(container);
            container.addClass("k-block k-info-colored");

            var that = this;
           
            //var win = null;
            //
            //  if (this._win == null) {
            var win = container.kendoWindow({
                width: this._width,
                height: this._height,
                actions: !(hasOkButton && hasCancelButton) ? ["Close"] : [],
                modal: true,
                title: title,
                visible: false,
                resizable: true,
                scrollable: true,
                content: {
                    url: contentUrl,
                    data: sendigPlainObject
                },
                open: function (args) {
                    kendo.ui.progress(container, true);
                },
                //activate: function (args) {


                //},
                refresh: function (args) {
                   
                    kendo.ui.progress(container, false);
                    args.sender.element.append(that._setFooter(null, hasOkButton, hasCancelButton));

                },
                close: function () {
                    this.destroy();
                }
            }
                ).data("kendoWindow");

            //  }
            //  else
            // {
            //    win =this._win;
            // }

            win.center().open();

            this._btnCancel.click(function () {
                win.close();
                //  win.destroy(); 
            });

            this._win = win;

            this._btnConfirm.click(function () {
               
                if ($.isFunction(confirmAction)) {
                    eval("confirmAction()");
                    win.close();
                }
                else if (confirmAction !== "" && confirmAction != undefined && confirmAction != null) {
                    window.location.href = confirmAction;
                    win.close();
                }
                // win.close();

            });
            //if ($.isFunction(confirmAction))
            //{
            //    this._btnConfirm.click(function () {
            //       
            //        eval(confirmAction);
            //        win.close();
            //    });
            //}

            //else if (confirmAction !== "" && confirmAction != undefined && confirmAction != null ) {

            //    this._btnConfirm.click(function () {
            //       
            //        win.close();
            //        window.location.href = confirmAction;
            //    });
            //}
        }

        _showNotify(title:string, message:string, hasOkCancelButton:boolean):void {
            var parenTag = $("#divMessage");
            var container = this._container;
            container.addClass("k-block k-info-colored");
            container.html("<div'>" + message + "</div>");
            // container.append(this._setFooter(null, hasOkCancelButton));
            parenTag.append(container);
            var that = this;
            var win = container.kendoWindow({
                width: this._width,
                height: this._height,
                modal: true,
                title: title,
                visible: false,
                resizable: true,
                scrollable: true,
                activate: function (args) {

                    if (hasOkCancelButton) {
                        args.sender.element.append(that._setFooter(null, true , null));
                    }

                },
                close: function () {
                    this.destroy();
                }

            }
                ).data("kendoWindow");



            win.center().open();

            this._btnCancel.click(function () { win.destroy(); });

            this._win = win;

        }

        _showLookUp(title:string, containerName:string):void {
            var container:JQuery = $("#" + containerName);
            container.addClass("k-block k-info-colored");
            var win = container.kendoWindow({
                width: this._width,
                height: this._height,
                modal: true,
                title: title,
                visible: false,
                resizable: true,
                // open: function()
                // {
                //
                // var that = this;
                // that.refresh();
                //},
                //content: {
                //    //url: contentUrl,
                //    data: { /*path: dataSourceUrl, */bindingObjName: txtLookupName }
                //},
                close: function () {
                    //this.destroy();

                }

            }
                ).data("kendoWindow");
            //  var container = $("#lkp_" + lookupName + "_Div");
            // var win = $("#lkp_" + lookupName + "_Div").data("kendoWindow");
           
            win.center().open();
            // win = container;
            // win.data("kendoWindow").refresh();
            //win.data("kendoWindow").open();
            //win.data("kendoWindow").center();

            this._win = win;
        }

    }

    export function ShowError(message, width, height, isRTL, hasButtons?, justOkButton?) {
        var msgObj = new Dialog();
        var hasCancelButton: boolean = false;
        if (hasButtons) {

            if (justOkButton) {
                justOkButton = true;
                hasCancelButton = false;
            }
            else if (justOkButton === undefined) {
                justOkButton = true;
                hasCancelButton = true;
            }
        }
        else {
            justOkButton = false;
            hasCancelButton = false;
        }

        msgObj._width = (typeof width == "number" ? width : "40%");
        msgObj._height = (typeof height == "number" ? height : "60%");

        msgObj._showError(message, isRTL, justOkButton, hasCancelButton);
    }

    export function ShowConfirm(message:string, callBackUrl:string, width, height) {
        var msgObj = new Dialog();
        msgObj._width = (typeof width == "number" ? width : "20%");
        msgObj._height = (typeof height == "number" ? height : "15%");
        msgObj._showConfirm(message, callBackUrl);
    }

    export function ShowDialog(contentUrl, title, confirmAction, width, height, sendigPlainObject, hasButtons?, justOkButton?):Dialog {
        var dlgObj = new Dialog();
        var hasCancelButton: boolean = false;

        dlgObj._width = (typeof width == "number" ? width : "70%");

        dlgObj._height = (typeof height == "number" ? height : "80%");

        if (hasButtons || hasButtons === undefined) {

            if (justOkButton) {
                justOkButton = true;
                hasCancelButton = false;
            }
            else if (justOkButton === undefined) {
                justOkButton = true;
                hasCancelButton = true;
            }
        }
        else {
            justOkButton = false;
            hasCancelButton = false;
        }
        dlgObj._showDialog(contentUrl, title, confirmAction, sendigPlainObject, justOkButton, hasCancelButton);

        return dlgObj;
    }

    export function LookUp(title:string, bindingObjectName:string, width:any, height:any):Dialog {
        var lkpObj = new Dialog();

        lkpObj._width = (typeof width == "number" ? width : "40%");

        lkpObj._height = (typeof height == "number" ? height : "50%");

        lkpObj._showLookUp(title, bindingObjectName);

        return lkpObj;
    }

    export function ShowNotify(title:string, message:string, width?:any, height?:any, hasOkCancelButton?) {
       
        var notifyObj = new Dialog();

        notifyObj._width = (typeof width == "number" ? width : "30%");

        notifyObj._height = (typeof height == "number" ? height : "15%");

        hasOkCancelButton = (typeof hasOkCancelButton == "boolean" ? hasOkCancelButton : false);

        notifyObj._showNotify(title, message, hasOkCancelButton);

        return notifyObj;
    }

}

