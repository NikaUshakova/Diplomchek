// /* When the user scrolls down, hide the navbar. When the user scrolls up, show the navbar */
// var prevScrollpos = window.pageYOffset;
// window.onscroll = function() {
//     var currentScrollPos = window.pageYOffset;
//     if (prevScrollpos > currentScrollPos) {
//         document.getElementById("navbar").style.top = "150px";
//     } else {
//         document.getElementById("navbar").style.top = "0px";
//     }
//     prevScrollpos = currentScrollPos;
// }


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
    }
    else {
        x.type = "password";       
    } 
}

function checkRegister() {
    var x = document.getElementById("Pass");
    var y = document.getElementById("ConfirmPass");
    if (x.type === "password" || y.type === "password") {
        x.type = "text";
        y.type = "text";
    }
    else {
        x.type = "password";
        y.type = "password";
    }
   
}