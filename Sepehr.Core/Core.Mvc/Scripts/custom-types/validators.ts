/// <reference path="../typings/jquery/jquery.d.ts" />
interface IValidator {
    //message: string;
    //widgetValidator: kendo.ui.Validator;
    Validate: Function;
}

class ValidatorBase implements IValidator {
    public message: string = "ورود اطلاعات الزامی است";
    public element: JQuery;

    public Validate(element): boolean {

        throw ("implements must be necessary");
    }
}

class RequiredValidator extends ValidatorBase {
    //private _message: string;

    //get message(): string {
    //    return this._message;
    //}
    //set message(val: string) {
    //    this._message = val;
    //}
    //public message: string = "";
    public element: JQuery;
    constructor(element: JQuery, message: string) {
        super();
        this.element = element.addClass("required-validation");
        //this.message = message;

    }

    public Validate(element): boolean {
        
        var text = element.val(),
            result = false;

        if (text && text.trim() != "") { result = true; }

        return result;

    }
}

class MaxLengthValidator extends ValidatorBase {
   
    len: number;
    constructor(len: number, message: string) {
        super();
        this.message = message;
        this.len = len;
       
    }

    Validate(element): boolean {
        
        var text = element.val(),
            result = false;

        if (text.length < this.len) { result = true; }

        return result;

    }
}
class ValidationAttribute {
    public type: string = "";
    public params: Array<any> = [];
    public message: string = "";
}