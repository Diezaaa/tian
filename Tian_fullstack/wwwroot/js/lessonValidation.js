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

function checkMandatoryFields()
{
    let isValid = true
    for (let i = 0; i < document.getElementsByClassName("slide").length; i++) {
        let content = document.getElementById(`slide${i}Content`)
        let title = document.getElementById(`Title`)
        let order = document.getElementById(`Order`)
        let titleError = document.getElementById(`TitleError`)
        let orderError = document.getElementById("OrderError")
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
        }
    }
    return isValid
}

document.getElementById("lessonForm").addEventListener("submit", (event) => {
    event.preventDefault()
    checkOptions()
    checkMandatoryFields()
    if (checkOptions() && checkMandatoryFields())
    {
        document.getElementById("lessonForm").submit()
    }
})