
(function(){

    function Start()
    {
        console.log("app started!");
        //use AJAX to read JSON file
        let XHR = new XMLHttpRequest();
        //create the request
        XHR.open("GET","../data/data.json");
        //send the request
        XHR.send();
        //create an event listener/handler
        XHR.addEventListener("readystatechange",function()
        {
            if (XHR.readyState == 4 && XHR.status == 200)
            {
                console.log("JSON DATE:");
                console.log("==============");
                console.log(XHR.responseText);
            }

        });
        
    }
    window.addEventListener("load", Start);
})();