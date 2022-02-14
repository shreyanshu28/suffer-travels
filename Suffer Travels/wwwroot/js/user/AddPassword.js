const getOtp = document.getElementById("GetOtp");

$(document).ready(() => {
    $('#GetOtp').click((event) => {
        $.ajax({
            type: "POST",
            url: '@Url.Action("SendOtp", "User")',
            data: {
                email: $('#email').val(),
            },
            error: function (result) {
                $('#OtpMsg').val(() => "Otp not sent");
                //alert("There is a Problem, Try Again!");
            },
            success: function (result) {
                console.log(result);
                if (result.status == true) {
                    $('#OtpMsg').val(() => "Otp sent");
                    getOtp.classList.remove("text-danger");
                    getOtp.classList.add("text-success");
                }
                else {
                    alert(result.message);
                }
            }
        });
    });
});