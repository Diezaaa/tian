let btn = document.getElementById("menu");
let menu = document.getElementById("menu_slider");
let openStyles = 'backdrop-filter: blur(10px);background-color: rgba(215, 220, 227, 0.48); height: calc(100vh - 68.25);position:absolute;width: 90%; max-width: 800px; margin-right:auto;margin-top:15px;margin-left:auto;    border-radius: 20px; display: flex;    transform: translateX(-50%);    left: 50%;';

btn.addEventListener("click", function(){
    if (menu.classList.contains("closed")){
        window.scrollTo(0, 0);
        menu.style.cssText = openStyles
        menu.classList.remove("closed")
        document.body.classList.add("remove-scrolling");
    }
    else{
        menu.style.cssText = 'width: 0;'
        menu.classList.add("closed")
        document.body.classList.remove("remove-scrolling");
    }
});
