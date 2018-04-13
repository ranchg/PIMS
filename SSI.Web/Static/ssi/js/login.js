$(function () {
    var imgs = ['/Static/ssi/Images/login/header1.jpg', '/Static/ssi/Images/login/header2.jpg'];
    var index = 0;
    setInterval(function () {
        if (index < 1) {
            index = index + 1;
        } else {
            index = 0;
        }
        $('.login-page').css('backgroundImage', 'url(' + imgs[index] + ')');
    }, 3000)
});