/* 
Selecting all the fields for validation
*/
let usernameField = document.getElementById("username");
let passwordField = document.getElementById("password");
let hints = document.getElementById("hints");
let submitButton = document.getElementById("submit");

// Disabling the submit button whent the page is loaded
submitButton.disabled = true;

/*
Event listeners (Dynamic validation)
*/
usernameField.addEventListener("input", () => { isAllOk(); isUsernameOk() });
passwordField.addEventListener("input", () => { isAllOk(); isPasswordOk() });

/* 
Validation of the fields
*/

function isUsernameOk() {
    return manageErrorMessages(isLengthCorrect(3, usernameField.value) && doesStartWithEnglishLetter(usernameField.value), "The username must be at least 3 characters and must start with a letter"); manageErrorMessages(isValidString(usernameField.value), "The username can contain only (0-9, a-z, A-Z, _, -) and must start with an English letter")
}

function isPasswordOk() {
    return manageErrorMessages(isPasswordValid(passwordField.value) && isValidString(), "The password must be length must be between 8 and 16 and must contain at least one digit, one uppecase and lowercase letter and must contain only (0-9, a-z, A-Z, _, -)")
}

/* 
Editing the errors displayed
*/

function manageErrorMessages(isCorrect, error) {
    var bullet = "*"

    // If the input isn't correct
    if (!(isCorrect) && !(hints.innerText.includes(error))) {
        hints.innerText += bullet + " " + error + "\n";
        return false;
    }

    // If the input is correct or unfilled
    else if (isCorrect) {
        hints.innerText = hints.innerText.replace(bullet + " " + error + "\n", "");
        return true;
    }
}

/*
Validation functions
*/

// Validating a given string using the provided min. length 
function isLengthCorrect(length, string) {
    return string.length >= length || string == "";
}

// Validating that the password is strong enough
function isPasswordValid(password) {
    let validPasswordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$/;
    return validPasswordPattern.test(password) || password == "";
}

function doesStartWithEnglishLetter(string) {
    let isLetterStartPattern = /^[a-zA-Z]/;
    return isLetterStartPattern.test(string) || string == "";
}

// Validting that the string has only legal characters (0-9, a-z, A-Z, _, -)
function isValidString(string) {
    let validStringPattern = /[0-9a-zA-Z_-]*$/;
    return validStringPattern.test(string) || string == "";
}

// Checking if at least one field is blank
function isAtLeastOneFieldBlank() {
    if (usernameField.value == "" ||
        passwordField.value == "") {
        return true;
    }
    return false;
}

// If the inputs are filled and passed all the validations than the submit buttom become enabled
function isAllOk() {
    if (isUsernameOk() &&
        isPasswordOk() &&
        !(isAtLeastOneFieldBlank())) {
        submitButton.disabled = false;
    }
    else {
        submitButton.disabled = true;
    }
}
