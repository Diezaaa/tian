/* 
Selecting all the fields for validation
*/
let emailField = document.getElementById("email");
let usernameField = document.getElementById("username");
let firstNameField = document.getElementById("first_name");
let surnameField = document.getElementById("surname");
let phoneNumberField = document.getElementById("phoneNumber")
let birthDayField = document.getElementById("birth_day");
let passwordField = document.getElementById("password");
let hints = document.getElementById("hints");
let submitButton = document.getElementById("submit");

// Disabling the submit button whent the page is loaded
submitButton.disabled = true;

/*
Event listeners (Dynamic validation)
*/
emailField.addEventListener("input", () => { isAllOk(); isEmailOk() });
usernameField.addEventListener("input", () => { isAllOk(); isUsernameOk() });
firstNameField.addEventListener("input", () => { isAllOk(); isFirstNameOk() });
surnameField.addEventListener("input", () => { isAllOk(); isSurNameOk() });
phoneNumberField.addEventListener("input", () => { isAllOk(); isPhoneNumberOk() });
birthDayField.addEventListener("input", () => { isAllOk(); isBirthdayOk()})
passwordField.addEventListener("input", () => { isAllOk(); isPasswordOk() });


/* 
Validation of the fields
*/
function isEmailOk() {
    return manageErrorMessages(isEmailCorrect(emailField.value), "The email is not valid")
}

function isUsernameOk() {
    return manageErrorMessages(isLengthCorrect(3, usernameField.value) && doesStartWithEnglishLetter(usernameField.value), "The username must be at least 3 characters and must start with a letter"); manageErrorMessages(isValidString(usernameField.value), "The username can contain only (0-9, a-z, A-Z, _, -) and must start with an English letter")
}

function isFirstNameOk() {
    return manageErrorMessages(isLengthCorrect(2, firstNameField.value), "The first name must be at least 2 characters")
}

function isSurNameOk() {
    return manageErrorMessages(isLengthCorrect(2, surnameField.value), "The surname must be at least 2 characters")
}

function isPhoneNumberOk() {
    return manageErrorMessages(isValidPhoneNumber(phoneNumberField.value), "The phone number must follow the international format with a + in the start and must not have hyphens(e.g +9720559934099)")
} 

function isBirthdayOk() {
    return manageErrorMessages(isValidBirthDay(birthDayField.value), "The birthday must not be earlier than 1900 year, and you must be 18 years old")
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

// Validting the email using regExp
function isEmailCorrect(email) {
    let emailPattern = /^\S+@\S+\.\S+$/;
    return emailPattern.test(email) || email == "";
}

// Validating a given string using the provided min. length 
function isLengthCorrect(length, string) {
    return string.length >= length || string == "";
}

// Validting that the string has only legal characters (0-9, a-z, A-Z, _, -)
function isValidString(string) {
    let validStringPattern = /[0-9a-zA-Z_-]*$/;
    return validStringPattern.test(string) || string == "";
}

function doesStartWithEnglishLetter(string) {
    let isLetterStartPattern = /^[a-zA-Z]/;
    return isLetterStartPattern.test(string) || string == "";
}

// Validating if a string is in international format
function isValidPhoneNumber(phoneNumber) {
    let validPhoneNumerPattern = /^\+(?:[0-9] ?){6,14}[0-9]$/;
    return validPhoneNumerPattern.test(phoneNumber) || phoneNumber == "";
}

// Validating if the birthday isn't earlier than 1900 and not late than (now - 18 years)
function isValidBirthDay(birthdayDateString) {
    let birthDayDate = new Date(birthdayDateString);
    const minDate = new Date("1900-1-1");
    const maxDate = new Date();
    maxDate.setFullYear(maxDate.getFullYear() - 18);
    return (birthDayDate >= minDate && birthDayDate <= maxDate);
}

// Validating that the password is strong enough
function isPasswordValid(password) {
    let validPasswordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$/;
    return validPasswordPattern.test(password) || password == "";
}

// Checking if at least one field is blank
function isAtLeastOneFieldBlank() {
    if (emailField.value == "" ||
        usernameField.value == "" ||
        firstNameField.value == "" ||
        surnameField.value == "" ||
        phoneNumberField.value == "" ||
        passwordField.value == "") {
        return true;
    }
    return false;
}

// If the inputs are filled and passed all the validations than the submit buttom become enabled
function isAllOk() {
    if (isEmailOk() &&
        isUsernameOk() &&
        isFirstNameOk() &&
        isSurNameOk() &&
        isPhoneNumberOk() &&
        isBirthdayOk() &&
        isPasswordOk() &&
        !(isAtLeastOneFieldBlank())) {
        submitButton.disabled = false;
    }
    else {
        submitButton.disabled = true;
    }
}
