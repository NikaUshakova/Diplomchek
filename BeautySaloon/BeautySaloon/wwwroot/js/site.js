/* Toggle between adding and removing the "responsive" class to topnav when the user clicks on the icon */
function myFunction() {
    var x = document.getElementById("myTopnav");
    if (x.className === "topnav") {
        x.className += " responsive";
    } else {
        x.className = "topnav";
    }
}

// // When the user scrolls the page, execute myFunction
// window.onscroll = function() { myFunction() };

// // Get the navbar
// var navbar = document.getElementById("navbar");

// // Get the offset position of the navbar
// var sticky = navbar.offsetTop;

// // Add the sticky class to the navbar when you reach its scroll position. Remove "sticky" when you leave the scroll position
// function myFunction() {
//     if (window.pageYOffset >= sticky) {
//         navbar.classList.add("sticky")
//     } else {
//         navbar.classList.remove("sticky");
//     }
function checkLogin() {
    var x = document.getElementById("Pass");
    var y = document.getElementById("ConfirmPass");
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

function setMin() {
    document.getElementById('datePicker').setAttribute("min", Date.Now());
}


// (function() {

//     $(".fa-search").click(function() {
//         $(".container, .input").toggleClass("active");
//         $("input[type='text']").focus();
//     });

// });