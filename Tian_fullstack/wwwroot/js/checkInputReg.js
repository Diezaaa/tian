// Selecting fields
let emailField = document.getElementById("email");
let usernameField = document.getElementById("username");
let firstNameField = document.getElementById("first_name");
let surnameField = document.getElementById("surname");
let countryField = document.getElementById("country_code");
let numberField = document.getElementById("number");
let birthDayField = document.getElementById("birth_day");
let passwordField = document.getElementById("password");
let hints = document.getElementById("hints");
let submitButton = document.getElementById("submit");

// Disables submit button until all the input is correct
submitButton.disabled = true;

// Setting the max date for birthday as current date
birthDayField.setAttribute("max", (new Date).toISOString().substring(0, 10));

/*
Event listeners
*/
emailField.addEventListener("input", () => {isEmailCorrect(); isAllValid()})
usernameField.addEventListener("input", () => {isShort(usernameField, 3, "* The username must be at least 3 characters long.\n"); isAllValid();});
firstNameField.addEventListener("input", () => {isShort(firstNameField, 3, "* The first name must be at least 3 characters long.\n"); isAllValid();});
surnameField.addEventListener("input", () => {isShort(surnameField, 3, "* The surname must be at least 3 characters long.\n"); isAllValid();});
countryField.addEventListener("input", () => {isPhoneNumberCorrect(); isAllValid()})
numberField.addEventListener("input", () => {isPhoneNumberCorrect(); isAllValid()})
passwordField.addEventListener("input", () => {isShort(passwordField, 6, "* The password must be at least 6 characters long.\n"); isAllValid();})

/*
Edit the hints paragraph
 */
function addNewHint(hint)
{
    if (!(hints.innerText.includes(hint)))
    {
        hints.innerText += hint;
    }
}
function removeHint(hint)
{
    if (hints.innerText.includes(hint))
    {
        hints.innerText = hints.innerText.replace(hint, "");
    }
}

/*
Validation functions
 */

// Validates email using regex, returns true if the email is valid, otherwise returns false
function isEmailCorrect()
{
    const regex = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    if (!(regex.test(emailField.value)) && emailField.value.length > 0)
    {
        addNewHint("* This email is invalid\n");
    }
    else if (emailField.value.length === 0)
    {
        return false
    }
    else
    {
        removeHint("* This email is invalid\n");
        return true;
    }
}

function isShort(field, minLength, hint)
{
    if ((field.value.length < minLength)
    && (field.value.length !== 0))
    {
        addNewHint(hint)
        return false;
    }
    else if (field.value.length === 0)
    {
        return false
    }
    else
    {
        removeHint(hint)
        return true;
    }
}

function isPhoneNumberCorrect()
{
    // Regular expression pattern for phone number validation
    const phonePattern =  /^\+?[0-9]{1,4}?[-.\s]?\(?[0-9]{1,3}?\)?[-.\s]?[0-9]{1,4}[-.\s]?[0-9]{1,4}[-.\s]?[0-9]{1,9}$/;

    if (!(phonePattern.test(countryField.value + numberField.value)) && (countryField.value.length + numberField.value.length !== 0))
    {
        addNewHint("* This phone number is invalid\n");
        return false;
    }
    else if(countryField.value.length + numberField.value.length === 0)
    {
        return false
    }
    else
    {
        removeHint("* This phone number is invalid\n");
        return true;
    }
}

function isAllValid()
{
    if (isEmailCorrect() &&
        isShort(usernameField, 3, "* The username must be at least 3 characters long.\n") &&
        isShort(firstNameField, 3, "* The first name must be at least 3 characters long.\n") &&
        isPhoneNumberCorrect() &&
        isShort(surnameField, 3, "* The surname must be at least 3 characters long.\n") &&
        isShort(passwordField, 6, "* The password must be at least 6 characters long.\n")
        )
    {
        submitButton.disabled = false;
    }
    else{
        submitButton.disabled = true;
    }
}