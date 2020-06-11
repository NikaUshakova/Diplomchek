/* Toggle between adding and removing the "responsive" class to topnav when the user clicks on the icon */
function topnavHamburger() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}


function checkLogin() {
    var x = document.getElementById("Pass");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}

function checkRegister() {
    var x = document.getElementById("Pass");
    var y = document.getElementById("ConfirmPass");
    if (x.type === "password" || y.type === "password") {
        x.type = "text";
        y.type = "text";
    } else {
        x.type = "password";
        y.type = "password";
    }

}




// (function() {

//     $(".fa-search").click(function() {
//         $(".container, .input").toggleClass("active");
//         $("input[type='text']").focus();
//     });

// });