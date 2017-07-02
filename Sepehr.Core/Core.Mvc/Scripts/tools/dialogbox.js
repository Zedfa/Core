var DialogBox;
(function (DialogBox) {
    var Dialog = (function () {
        function Dialog() {
            this._btnConfirm = $("<button/>", { id: 'RP_btnConfirm', style: 'margin-left:10px;', "class": 'k-button k-button-icontext k-primary mainbtn', text: "تایید", "data-bind": "vm: save" });
            this._iconConfirm = $("<span/>", { "class": 'k-icon k-i-tick' });
            this._btnCancel = $("<button/>", { id: 'RP_btnCancel', style: 'margin-left:2px;', "class": 'k-button k-button-icontext k-grid-cancel', text: "انصراف", "data-bind": "vm: cancel" });
            this._iconCancel = $("<span/>", { "class": 'k-icon k-i-cancel' });
            this._footer = $("<div/>", { style: 'margin:2px 3px;position: relative;left: 1px;bottom:0px;' });
            this._container = $("<div/>", { "data-role": "dialogBox" });
            this._win = null;
            this._width = null;
            this._height = null;
        }
        Dialog.prototype._initWindow = function (obj, closeCallBack) {
        };
        Dialog.prototype._setFooter = function (htmlObj, OkButton, cancelButton) {
            var tempJQ = this._footer;
            tempJQ.html(htmlObj);
            if ((OkButton && cancelButton) || (OkButton && cancelButton == undefined)) {
                tempJQ.append(this._btnCancel, this._btnConfirm);
            }
            else if (OkButton) {
                this._btnConfirm.append(this._iconConfirm);
                tempJQ.append(this._btnConfirm);
            }
            return tempJQ;
        };
        Dialog.prototype._formatingMessages = function (message) {
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
            }
            catch (e) {
                messageStruct = message;
            }
            return messageStruct;
        };
        Dialog.prototype._showError = function (message, isRTL, hasOkButton, hasCancelButton) {
            var parentTag = $("#divMessage");
            var container = this._container;
            container.addClass("k-block k-error-colored");
            container.addClass(isRTL ? "k-rtl" : "k-ltr");
            container.addClass(isRTL ? "right-align" : "left-align");
            container.html("<div>" + this._formatingMessages(message) + "</div>");
            if (hasOkButton || hasCancelButton) {
                container.append(this._setFooter(null, hasOkButton, hasCancelButton));
            }
            parentTag.html(container);
            var win = container.kendoWindow({
                width: this._width,
                height: this._height,
                modal: true,
                actions: !(hasOkButton || hasCancelButton) ? ["Close"] : [],
                title: "خطا",
                visible: false,
                resizable: false,
                close: function (e) {
                    this.destroy();
                }
            }).data("kendoWindow");
            win.center().open();
            this._btnCancel.click(function (e) { win.destroy(); });
            this._btnConfirm.click(function (e) {
                win.destroy();
            });
        };
        Dialog.prototype._showConfirm = function (message, url) {
            var parenTag = $("#divMessage");
            var container = this._container;
            container.addClass("k-rtl k-block k-success-colored");
            container.html("<text>" + message + "</text>");
            container.append(this._setFooter(null, true, null));
            parenTag.html(container);
            var cnfrm = container.kendoWindow({
                width: this._width,
                height: this._height,
                modal: true,
                title: "تاییدیه",
                visible: false,
                resizable: true
            }).data("kendoWindow");
            cnfrm.open().center();
            this._btnCancel.click(function (e) { cnfrm.destroy(); });
            this._btnConfirm.click(function (e) {
                cnfrm.destroy();
                window.location.href = url;
            });
        };
        Dialog.prototype._showDialog = function (contentUrl, title, confirmAction, sendigPlainObject, hasOkButton, hasCancelButton) {
            var parenTag = $("#divMessage");
            var container = this._container;
            parenTag.append(container);
            container.addClass("k-block k-info-colored");
            var that = this;
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
                refresh: function (args) {
                    kendo.ui.progress(container, false);
                    args.sender.element.append(that._setFooter(null, hasOkButton, hasCancelButton));
                },
                close: function () {
                    this.destroy();
                }
            }).data("kendoWindow");
            win.center().open();
            this._btnCancel.click(function () {
                win.close();
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
            });
        };
        Dialog.prototype._showNotify = function (title, message, hasOkCancelButton) {
            var parenTag = $("#divMessage");
            var container = this._container;
            container.addClass("k-block k-info-colored");
            container.html("<div'>" + message + "</div>");
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
                        args.sender.element.append(that._setFooter(null, true, null));
                    }
                },
                close: function () {
                    this.destroy();
                }
            }).data("kendoWindow");
            win.center().open();
            this._btnCancel.click(function () { win.destroy(); });
            this._win = win;
        };
        Dialog.prototype._showLookUp = function (title, containerName) {
            var container = $("#" + containerName);
            container.addClass("k-block k-info-colored");
            var win = container.kendoWindow({
                width: this._width,
                height: this._height,
                modal: true,
                title: title,
                visible: false,
                resizable: true,
                close: function () {
                }
            }).data("kendoWindow");
            win.center().open();
            this._win = win;
        };
        return Dialog;
    }());
    DialogBox.Dialog = Dialog;
    function ShowError(message, width, height, isRTL, hasButtons, justOkButton) {
        var msgObj = new Dialog();
        var hasCancelButton = false;
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
    DialogBox.ShowError = ShowError;
    function ShowConfirm(message, callBackUrl, width, height) {
        var msgObj = new Dialog();
        msgObj._width = (typeof width == "number" ? width : "20%");
        msgObj._height = (typeof height == "number" ? height : "15%");
        msgObj._showConfirm(message, callBackUrl);
    }
    DialogBox.ShowConfirm = ShowConfirm;
    function ShowDialog(contentUrl, title, confirmAction, width, height, sendigPlainObject, hasButtons, justOkButton) {
        var dlgObj = new Dialog();
        var hasCancelButton = false;
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
    DialogBox.ShowDialog = ShowDialog;
    function LookUp(title, bindingObjectName, width, height) {
        var lkpObj = new Dialog();
        lkpObj._width = (typeof width == "number" ? width : "40%");
        lkpObj._height = (typeof height == "number" ? height : "50%");
        lkpObj._showLookUp(title, bindingObjectName);
        return lkpObj;
    }
    DialogBox.LookUp = LookUp;
    function ShowNotify(title, message, width, height, hasOkCancelButton) {
        var notifyObj = new Dialog();
        notifyObj._width = (typeof width == "number" ? width : "30%");
        notifyObj._height = (typeof height == "number" ? height : "15%");
        hasOkCancelButton = (typeof hasOkCancelButton == "boolean" ? hasOkCancelButton : false);
        notifyObj._showNotify(title, message, hasOkCancelButton);
        return notifyObj;
    }
    DialogBox.ShowNotify = ShowNotify;
})(DialogBox || (DialogBox = {}));
