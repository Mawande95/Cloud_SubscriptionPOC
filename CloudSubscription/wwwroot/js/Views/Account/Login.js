$(function () {
    $('#divDisability').css('display', 'none');
    $("#LoginForm").submit(function (e) {
        e.preventDefault();

        if (ValidateForm()) {
            Show()
            $.post($(this).attr("action"),
                $(this).serialize(),
                function (data) {
                    debugger;
                    if (!data.valid) {
                        $('#errorMessage').css('display', 'block');
                        $('#errorMessage').html('<p>' + data.message + '</p>');
                    }
                    else {
                        window.location.href = '/Dashboard/Index';
                    }
                    Hide()
                });
        }
    });


})
function Show() {
    $('#busyModal').modal('show').on('shown', function () {
        $('body').css('overflow', 'hidden');
    }).on('hidden', function () {
        $('body').css('overflow', 'auto');
    });
}
function Hide() {
    $('#busyModal').modal('hide');

}
function ValidateForm() {

    var valid = true

    if ($('#EmailAddress').val() == "") {
        $('#EmailAddress').css('border-color', 'red');
        $('#EmailAddress').focus();
        $('#errorEmailAddress').text('Email Address Required');
        valid = false;
    }
    else {
        $('#EmailAddress').css('border-color', '');
        $('#errorEmailAddress').text('');
    }
    if ($('#password1').val() == "") {
        $('#password1').css('border-color', 'red');
        $('#password1').focus();
        $('#ePassword1').text('Password Required');
        valid = false;
    }
    else {
        $('#password1').css('border-color', '');
        $('#ePassword1').text('');
    }

    return valid;
}
$(".toggle-password").click(function () {
    $(this).toggleClass("fa-eye fa-eye-slash");
    var input = $($(this).attr("toggle"));
    if (input.attr("type") == "password") {
        input.attr("type", "text");
    } else {
        input.attr("type", "password");
    }
});