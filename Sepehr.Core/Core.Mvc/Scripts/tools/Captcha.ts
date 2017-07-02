
$("#btnCaptchaRefresh").click(() => {
    loadCaptcha();
});


function loadCaptcha() {

    var captchaUrl:string = (<any>window.location).origin + "/" + window.areaName + "/";

    captchaUrl += 'Captcha/GetCaptchaImage?guid=' + $("#hdnCaptchaGuid").val() + '&r=' + Math.random();


    var img:JQuery = $("<img />");
    var captchaContainer:JQuery  = $("#captchaContainer");
    var imgCaptchaLoading: JQuery = $("#imgCaptchaLoading");
    captchaContainer.hide()
    imgCaptchaLoading.show()
    img.attr('src', captchaUrl)
          .load( (data) => {
              imgCaptchaLoading.hide();
              captchaContainer.html(<any>img);
              captchaContainer.show();
        });
}; 