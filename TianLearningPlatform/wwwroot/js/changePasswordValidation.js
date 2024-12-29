// Selecting password fields
let passwordField = document.getElementById("currentPassword")
let newPasswordField = document.getElementById("newPassword");
let hints = document.getElementById("hints");
let submitButton = document.getElementById("submit");


// Adding event listener to the field
passwordField.addEventListener("input", () => { isAllOk();});
newPasswordField.addEventListener("input", () => { isAllOk(); isNewPasswordOk() });

// Disabling the submit button whent the page is loaded
submitButton.disabled = true;


/* 
Validation of the fields
*/
function isNewPasswordOk() {
    return manageErrorMessages(isPasswordValid(newPasswordField.value) && isValidString(), "The password must be length must be between 8 and 16 and must contain at least one digit, one uppecase and lowercase letter and must contain only (0-9, a-z, A-Z, _, -)")
}

function isCurrentPassword() {
    return manageErrorMessages()

}

// Managing displaying error
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

// Validating that the password is strong enough
function isPasswordValid(password) {
    let validPasswordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,16}$/;
    return validPasswordPattern.test(password) || password == "";
}

// Validting that the string has only legal characters (0-9, a-z, A-Z, _, -)
function isValidString(string) {
    let validStringPattern = /[0-9a-zA-Z_-]*$/;
    return validStringPattern.test(string) || string == "";
}

// Checking if at least one field is blank
function isAtLeastOneFieldBlank() {
    if (passwordField.value == "" ||
        newPasswordField.value == "") {
        return true;
    }
    return false;
}



// If the inputs are filled and passed all the validations than the submit buttom become enabled
function isAllOk() {
    if (isNewPasswordOk() &&
        !(isAtLeastOneFieldBlank())) {
        submitButton.disabled = false;
    }
    else {
        submitButton.disabled = true;
    }
}
