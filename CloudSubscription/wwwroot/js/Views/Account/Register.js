$(function () {

    $("#RegisterForm").submit(function (e) {
        e.preventDefault();

        if (ValidateForm()) {
            /*Show()*/
            $.post($(this).attr("action"),
                $(this).serialize(),
                function (data) {
                    if (!data.valid) {
                        $('#btnFailModal').click();
                        $('#FailMessage').text(data.message);
                    }
                    else {
                        $('#btnSuccessModal').click();
                        $('#successMessage').text(data.message);
                    }
                    Hide()
                });
        }
    });


    // Configure MinSize -- default is 5
    PasswordValidator.minSize = 8;
    // Configure MaxSize -- default is 15
    PasswordValidator.maxSize = 12;

    // whether you want to validate on prohibited characters
    PasswordValidator.prohibitedConfigured = true;

    PasswordValidator.setup('password1', 'verify1');

    /*Show();*/
})
$('#EmailAddress').focusout(function (e) {

    if (validateEmail($("#EmailAddress").val())) {
        $('#errorEmailAddress').css('border-color', '');
    }
    else {
        $('#EmailAddress').css('border-color', 'red');
        $('#EmailAddress').focus();
        $('#errorEmailAddress').text('Invalid Email Address Format')
        return
    }

    $.ajax({
        type: 'POST',
        url: '/Account/CheckEmailAddressExist',
        dataType: 'json',
        data: { EmailAddress: $("#EmailAddress").val() },
        beforeSend: function () {
            Show();
        },
        success: function (data) {
           
            if (data.message != '') {
                $("#txtEmailAddress").val('')
                $("#txtEmailAddress").css('border-color', 'red')
                $("#errorEmailAddress").text(data.message);
                $('#Username').val('');
                $('#txtEmailAddress').focus();
            }
            else {
                $("#txtEmailAddress").css('border-color', '')
                $("#errorEmailAddress").text('');
                $('#Username').attr('readonly', true);
            }
        },
        complete: function () {
            Hide();
        },
        error: function (ex) {
            Hide();
        }
    });
})
function validateEmail(email) {
    const re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
$('#ContactNumber').focusout(function (e) {

    if (validateEmail($("#txtEmailAddress").val())) {
        $('#errorEmailAddress').css('border-color', '');
    }
    else {
        $('#txtEmailAddress').css('border-color', 'red');
        $('#txtEmailAddress').focus();
        $('#errorEmailAddress').text('Invalid Email Address Format')
        return
    }



    $.ajax({
        type: 'POST',
        url: '/Employee/CheckEmailAddressExist',
        dataType: 'json',
        headers: { 'RequestVerificationToken': token },
        data: { EmailAddress: $("#txtEmailAddress").val() },
        beforeSend: function () {
            Show();
        },
        success: function (data) {
            debugger;
            if (data.message != '') {
                debugger;
                $("#txtEmailAddress").val('')
                $("#txtEmailAddress").css('border-color', 'red')
                $("#errorEmailAddress").text(data.message);
                $('#Username').val('');
                $('#txtEmailAddress').focus();
            }
            else {
                $("#txtEmailAddress").css('border-color', '')
                $("#errorEmailAddress").text('');
                $('#Username').attr('readonly', true);
            }
        },
        complete: function () {
            Hide();
        },
        error: function (ex) {
            Hide();
        }
    });
})
$("#ContactNumber").keypress(function (e) {
    //if the letter is not digit then display error and don't type anything
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        //display error message
        // $("#errmsg").html("Digits Only").show().fadeOut("slow");
        $('#errorCellPhone').text('Only number are allowed');
        $("#ContactNumber").css('border-color', 'red');
        return false;
    }
    else {
        $('#errorCellPhone').text('');
        $("#ContactNumber").css('border-color', '');
    }
});
function CheckUserExist() {
    Show()
    var token = $('#RequestVerificationToken').val();
    $.ajax({
        type: 'POST',
        url: '@Url.Action("CheckUserExist")',
        headers: { 'RequestVerificationToken': token },
        dataType: 'json',
        data: { username: $("#Username").val() },

        success: function (data) {
            if (data.message != '') {
                $("#Username").val('')
                $("#Username").css('border-color', 'red')
                $("#divError").show();
                $("#eusername").text(data.message);
            }
            else {
                $("#Username").css('border-color', '')
                $("#divError").hide();
                $("#eusername").text('')
            }
            Hide()
        },
        error: function (ex) {
            Hide()
        }
    });
}
$("#password1").keypress(function (e) {
    //if the letter is not digit then display error and don't type anything
    if (e.which == 35 || e.which == 61 || e.which == 38 || e.which == 35) {
        //display error message
        // $("#errmsg").html("Digits Only").show().fadeOut("slow");
        $('#password1').text('');
        $('#ePassword1').text(e.key + ' charactor is not allowed');
        $("#password1").css('border-color', 'red');
        return false;
    }
    else {
        $('#ePassword1').text('');
        $("#password1").css('border-color', '');
    }
});
function PasswordTogggle() {
    var x = document.getElementById("myInput");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}
function Show() {
    $('#busyModal').modal('show');
    //$('#busyModal').modal('show').on('shown', function () {
    //    $('body').css('overflow', 'hidden');
    //}).on('hidden', function () {
    //    $('body').css('overflow', 'auto');
    //});
}
function Hide() {
    $('#busyModal').modal('hide');

}
function ValidateForm() {

   var  valid = true
    if ($('#FirstName').val() == "") {
        $('#FirstName').css('border-color', 'red');
        $('#FirstName').focus();
        $('#errorFirstName').text('First Name Required');
        valid = false;
    }
    else {
        $('#FirstName').css('border-color', '');
        $('#errorFirstName').text('');
    }
    if ($('#LastName').val() == "") {
        $('#LastName').css('border-color', 'red');
        $('#LastName').focus();
        $('#errorLastName').text('Last Name Required');
        valid = false;
    }
    else {
        $('#LastName').css('border-color', '');
        $('#errorLastName').text('');
    }

    if ($('#ContactNumber').val() == "") {
        $('#ContactNumber').css('border-color', 'red');
        $('#ContactNumber').focus();
        $('#errorCellPhone').text('Contact Number Required');
        valid = false;
    }
    else {
        $('#ContactNumber').css('border-color', '');
        $('#errorCellPhone').text('');
    }
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
    if($('#verify1').val() == "") {
        $('#verify1').css('border-color', 'red');
        $('#errorConfirmPassword').text('Confirm Password Required');
        valid = false;

    }
    
    else {
        if ($('#password1').val() != $('#verify1').val()) {
            $('#verify1').css('border-color', 'red');
            $('#errorConfirmPassword').text('Password must match');
            valid = false;
        }
        else {
            $('#verify1').css('border-color', '');
            $('#errorConfirmPassword').text(' ');
        }
       
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
function CloseModalSuccess() {
    window.location.href = '/Account/Login';
}
function CloseModalFail() {
    $('#FailMessage').modal('hide');
}