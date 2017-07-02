$("#btnCaptchaRefresh").click(function () {
    loadCaptcha();
});
function loadCaptcha() {
    var captchaUrl = window.location.origin + "/" + window.areaName + "/";
    captchaUrl += 'Captcha/GetCaptchaImage?guid=' + $("#hdnCaptchaGuid").val() + '&r=' + Math.random();
    var img = $("<img />");
    var captchaContainer = $("#captchaContainer");
    var imgCaptchaLoading = $("#imgCaptchaLoading");
    captchaContainer.hide();
    imgCaptchaLoading.show();
    img.attr('src', captchaUrl)
        .load(function (data) {
        imgCaptchaLoading.hide();
        captchaContainer.html(img);
        captchaContainer.show();
    });
}
;
