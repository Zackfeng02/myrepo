"use strict";
/*app.js-Ze Feng 301210794 Feb 2023  */

//IIFE
(function () {
    function Start() {
        console.log("app started!");
        $("a.delete").on("click", function (event) {
            if (!confirm("Are you sure?")) {
                event.preventDefault();
                location.href = "/contact-list";
            }
        });
    }
    window.addEventListener("load", Start);
})();
//email validation
function EmailValidate(){
    let emailInput = document.getElementById("email").value;
    let error = document.getElementById("error");
    let emailReg =/^\w+@\w+\.\w+.*$/;
    if(! emailReg.test(emailInput)){
    
    error.innerHTML = "invalid email";
  
    }else{
      error.innerHTML = " ";
    }
  }
  //reset
  function ResetF(){
      let error = document.getElementById("error");
      error.innerHTML = " ";
  }
//set cookies of contact page
function CreateCookies(){
    let name = document.getElementById("name").value;
    let email = document.getElementById("email").value;
    let phone = document.getElementById("phone").value;
    let message = document.getElementById("message").value;
    document.cookie = "name="+encodeURIComponent(name)+";";
    document.cookie = "email="+encodeURIComponent(email)+";";
    document.cookie = "phone="+encodeURIComponent(phone)+";";
    document.cookie = "message="+encodeURIComponent(message)+";";

}
