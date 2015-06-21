$(window).scroll(function () {
    if ($(this).scrollTop() > 1) {
        $('div:first').addClass("sticky");
        $('div.navbar-header').addClass("stickyHeader");
        $('div.navbar-collapse').addClass("stickyMenu");
    }
    else {
        $('div:first').removeClass("sticky");
        $('div.navbar-header').removeClass("stickyHeader");
        $('div.navbar-collapse').removeClass("stickyMenu");
    }
});

$("#modal_trigger").leanModal({ top: 200, overlay: 0.6, closeButton: ".modal_close" });

$(function () {
    // Calling Login Form
    $("#login_form").click(function () {
        $(".social_login").hide();
        $(".user_login").show();
        return false;
    });

    // Calling Register Form
    $("#register_form").click(function () {
        $(".social_login").hide();
        $(".user_register").show();
        $(".header_title").text('Register');
        return false;
    });

    // Going back to Social Forms
    $(".back_btn").click(function () {
        $(".user_login").hide();
        $(".user_register").hide();
        $(".social_login").show();
        $(".header_title").text('Login');
        return false;
    });

    // Going back to Social Forms
    $(".btn_red").click(function () {
        validate();
        return false;
    });

    $('.aaf').on("click",function(){
  var usersid =  $(this).attr("id");
  //post code
})
})

function validate()
{
    var emailAdress = document.getElementById('EmailAdress');
    var password = document.getElementById('Password');

    if(emailAdress.value == "")
    {

    }
    

}
