/// <reference path="../typings/jquery/jquery.d.ts" />
interface IValidator {
    //message: string;
    Validate: Function;
    model: any;
    element: JQuery;
    message: string;

}

class ValidatorBase implements IValidator {
    public message: string = "";
    public element: JQuery;
    model: any;

    public Validate(element): boolean {

        throw ("implements must be necessary");
    }
}

class RequiredValidator implements IValidator {
    message: string;
    element: JQuery;
    model: any;

    constructor() {
        this.message = "ورود اطلاعات الزامی است";
        this.element.addClass("required-validation");
        return this;
    }
    public Validate(): boolean {

        var text = this.element.val(),
            result = false;

        if (text && text.trim() != "") {
            result = true;
        }

        return result;

    }
}

class MaxLengthValidator implements IValidator {
    message: string;
    element: JQuery;
    model: any;
    len: number;
    constructor() {

        this.message = "طول رشته مجاز نیست";
    }

    Validate(): boolean {

        var text = this.element.val(),
            result = false;

        if (text.length < this.len) {
            result = true;
        }

        return result;

    }
}
class ValidationAttribute {
    public type: string = "";
    public params: Array<any> = [];
    public message: string = "";
}