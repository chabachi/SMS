const togglePassword_As1 = document.querySelector("#togglePassword_As1");
const password_As1 = document.querySelector("#Input_CurrentPassword");

togglePassword_As1.addEventListener("click", function () {
    // toggle the type attribute
    const type = password_As1.getAttribute("type") === "password" ? "text" : "password";
    password_As1.setAttribute("type", type);
    // toggle the icon
    this.classList.toggle("bi-eye");

});

const togglePassword_As2 = document.querySelector("#togglePassword_As2");
const password_As2 = document.querySelector("#Input_NewPassword");

togglePassword_As2.addEventListener("click", function () {
    // toggle the type attribute
    const type = password_As2.getAttribute("type") === "password" ? "text" : "password";
    password_As2.setAttribute("type", type);
    // toggle the icon
    this.classList.toggle("bi-eye");

});

const togglePassword_As3 = document.querySelector("#togglePassword_As3");
const password_As3 = document.querySelector("#Input_ConfirmNewPassword");

togglePassword_As3.addEventListener("click", function () {
    // toggle the type attribute
    const type = password_As3.getAttribute("type") === "password" ? "text" : "password";
    password_As3.setAttribute("type", type);
    // toggle the icon
    this.classList.toggle("bi-eye");

});