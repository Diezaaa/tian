// Disabling the submit buttom first
document.getElementById('sumbitBtn').disabled = true;

function addNewMessage(msg, elem)
{
    if (!(elem.innerText.includes(msg))) {
        elem.innerHTML += msg + "</br>";
    }
}
function removeMessage(msg, elem)
{
    if (elem.innerText.includes(msg)) {
        elem.innerHTML = elem.innerHTML.replace(msg + "<br>", "");
    }
}
function checkOptions() {
    let isValid = true
    for (let i = 0; i < document.getElementsByClassName("slide").length; i++)
    {
        let option1 = document.getElementById(`slide${i}Option1`)
        let option2 = document.getElementById(`slide${i}Option2`)
        let option3 = document.getElementById(`slide${i}Option3`)
        let option4 = document.getElementById(`slide${i}Option4`)
        let correctOption = document.getElementById(`slide${i}CorrectAnswer`)
        let optionsError = document.getElementById(`slide${i}OptionsMessage`)
        if (option1.value.trim() !== "" ||
            option2.value.trim() !== "" ||
            option3.value.trim() !== "" ||
            option4.value.trim() !== "" ||
            correctOption.value.trim() !== "")
        {
            if (option1.value.trim() === "" ||
                option2.value.trim() === "" ||
                option3.value.trim() === "" ||
                option4.value.trim() === "")
            {
                addNewMessage("All four options must be filled", optionsError)
                isValid = false

            }
            else
            {
                removeMessage("All four options must be filled", optionsError)
            }
            if (parseInt(correctOption.value) < 1 || parseInt(correctOption.value) > 4)
            {
                addNewMessage("The correct answer must be between 1 and 4", optionsError)
                isValid = false

            }
            else
            {
                removeMessage("The correct answer must be between 1 and 4", optionsError)
            }
            if (correctOption.value.trim() === "")
            {
                addNewMessage("You must choose the correct option", optionsError)
                isValid = false

            }
            else {
                removeMessage("You must choose the correct option", optionsError)
            }
        }
        else
        {
            optionsError.innerText = "";
        }
    }
    return isValid
}

function checkMandatoryFields() {
    let isValid = true;
    let title = document.getElementById(`Title`)
    let order = document.getElementById(`Order`)
    let titleError = document.getElementById(`TitleError`)
    let orderError = document.getElementById("OrderError")
    if (title.value.trim() === "" ||
        order.value.trim() === ""
    ) {
        if (title.value.trim() == "") {
            addNewMessage("This field must be filled", titleError)
        }
        else {
            removeMessage("This field must be filled", titleError)
        }
        if (order.value.trim() == "") {
            addNewMessage("This field must be filled", orderError)
        }
        else {
            removeMessage("This field must be filled", orderError)
        }
        isValid = false
    }

    else 
    {
        titleError.innerHTML = ""
        orderError.innerHTML = ""
        isValid = true;

    }

    for (let i = 0; i < document.getElementsByClassName("slide").length; i++) {
        let content = document.getElementById(`slide${i}Content`)
        let contentError = document.getElementById(`slide${i}ContentError`)
        if (content.value.trim() === "" ||
            title.value.trim() === "" ||
            order.value.trim() === ""
        ) {
            if (content.value.trim() == "") {
                addNewMessage("This field must be filled", contentError)
            }
            else {
                removeMessage("This field must be filled", contentError)
            }
            if (title.value.trim() == "") {
                addNewMessage("This field must be filled", titleError)
            }
            else {
                removeMessage("This field must be filled", titleError)
            }
            if (order.value.trim() == "") {
                addNewMessage("This field must be filled", orderError)
            }
            else {
                removeMessage("This field must be filled", orderError)
            }
            isValid = false
        }
        else
        {
            titleError.innerHTML = ""
            orderError.innerHTML = ""
            contentError.innerHTML = ""
            let isValid = true;
        }
    }
    return isValid
}

function isThereSlide() {
    slidesError = document.getElementById("slidesError");
    if (document.getElementsByClassName("slide").length >= 1) {
        removeMessage("A lesson must contain at least one slide", slidesError)
        return true;
    }
    else {
        addNewMessage("A lesson must contain at least one slide", slidesError)
        return false
    }
}


document.getElementById("lessonForm").addEventListener("input", (event) => {
    let isValid = checkOptions() && checkMandatoryFields() && isThereSlide();
    if (isValid) {
        document.getElementById('sumbitBtn').disabled = false;
    }
})

document.getElementById('sumbitBtn').addEventListener("click", () => {
    document.getElementById('sumbitBtn').disabled = true;
    document.getElementById("lessonForm").submit();
});
