// Selecting fields
let emailField = document.getElementById("email");
let usernameField = document.getElementById("username");
let firstNameField = document.getElementById("first_name");
let surnameField = document.getElementById("surname");
let phoneNumberField = document.getElementById("phoneNumber")
let birthDayField = document.getElementById("birth_day");
let passwordField = document.getElementById("password");
let hints = document.getElementById("hints");
let submitButton = document.getElementById("submit");

// Disable the submit button whent the page is loaded
submitButton.disabled = true;

// Setting the max date for birthday as current date
birthDayField.setAttribute("max", (new Date).toISOString().substring(0, 10));

/*
Event listeners
*/
emailField.addEventListener("input", () => { isEmailCorrect(); isAllValid() })
usernameField.addEventListener("input", () => { isShort(usernameField, 3, "* The username must be at least 3 characters long.\n"); isAllValid(); });
firstNameField.addEventListener("input", () => { isShort(firstNameField, 3, "* The first name must be at least 3 characters long.\n"); isAllValid(); });
surnameField.addEventListener("input", () => { isShort(surnameField, 3, "* The surname must be at least 3 characters long.\n"); isAllValid(); });
phoneNumberField.addEventListener("input", () => { isPhoneNumberCorrect(); isAllValid() })
passwordField.addEventListener("input", () => { isShort(passwordField, 8, "* The password must be at least 8 characters long.\n"); isPasswordStrong(); isValidCharactersInPassword(); isAllValid(); })

/*
Edit the hints paragraph
 */
function addNewHint(hint) {
    if (!(hints.innerText.includes(hint))) {
        hints.innerText += hint;
    }
}
function removeHint(hint) {
    if (hints.innerText.includes(hint)) {
        hints.innerText = hints.innerText.replace(hint, "");
    }
}

/*
Validation functions
 */

// Validates email using regex, returns true if the email is valid, otherwise returns false
function isEmailCorrect() {
    const regex = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    if (!(regex.test(emailField.value)) && emailField.value.length > 0) {
        addNewHint("* This email is invalid\n");
        return true;
    }
    else if (emailField.value.length === 0) {
        removeHint("* This email is invalid\n")
        return false
    }
    else {
        removeHint("* This email is invalid\n");
        return true;
    }
}

// Checks if something is long enough
function isShort(field, minLength, hint) {
    if ((field.value.length < minLength)
        && (field.value.length !== 0)) {
        addNewHint(hint)
        return false;
    }
    else if (field.value.length === 0) {
        removeHint(hint)
        return false
    }
    else {
        removeHint(hint)
        return true;
    }
}

// Check if the number is in the correct format
function isPhoneNumberCorrect() {
    // Regular expression pattern for phone number validation
    const phonePattern = /^\+(\d{1,3})\s?\d{4,14}$/;
    if (phoneNumberField.value.length === 0) {
        removeHint("* The number must be entered in an international format (without spaces or hypenes), e.g.: +972 054 992 4098.\n")
        return false
    }
    else if (!(phonePattern.test(phoneNumberField.value)) && (phoneNumberField !== 0)) {
        addNewHint("* The number must be entered in an international format (without spaces or hypenes), e.g.: +972 054 992 4098.\n");
        return false;
    }
    else {
        removeHint("* The number must be entered in an international format (without spaces or hypenes), e.g.: +972 054 992 4098.\n");
        return true;
    }
}

// Cheks if the passowrd meets all the requirments
function isValidCharactersInPassword()
{
    // Cheks if the passowrd is built from valid characters (0-9, A-Z. a-z, _, -)
    const validPasswordRegExp = /^[A-Za-z0-9_-]+$/

    if (!validPasswordRegExp.test(passwordField.value) && passwordField.value.length >0 ) {
        addNewHint("* The password must be built from valid chatacters only (0-9, A-Z. a-z, _, -).")
        return true;
    }
    else if (passwordField.value.length === 0)
    {
        removeHint("* The password must be built from valid chatacters only (0-9, A-Z. a-z, _, -).")
        return false
    }
    else {
        removeHint("* The password must be built from valid chatacters only (0-9, A-Z. a-z, _, -).")
        return true;
    }
}

function isPasswordStrong() {
    const strongPasswordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/

    if (!(strongPasswordPattern.test(passwordField.value)) && passwordField.value.length > 0) {
        addNewHint("* The password must include at least one digit, one lowercase letter, and one uppercase letter.")
        return false
    }
    else if (passwordField.value.length === 0) {
        removeHint("* The password must include at least one digit, one lowercase letter, and one uppercase letter.")
        return false
    }
    else {
        removeHint("* The password must include at least one digit, one lowercase letter, and one uppercase letter.")
        return true
    }

}
// Cheks if all fields are filled properly, if they are filled prorperly the submit button becomes clickable
function isAllValid() {
    if (isEmailCorrect() &&
        isShort(usernameField, 3, "* The username must be at least 3 characters long.\n") &&
        isShort(firstNameField, 3, "* The first name must be at least 3 characters long.\n") &&
        isShort(surnameField, 3, "* The surname must be at least 3 characters long.\n") &&
        isPhoneNumberCorrect() &&
        isShort(passwordField, 6, "* The password must be at least 6 characters long.\n") &&
        isValidCharactersInPassword() &&
        isPasswordStrong()
    ) {
        submitButton.disabled = false;
    }
    else {
        submitButton.disabled = true;
    }
}