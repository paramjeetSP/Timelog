var popupStatus = 0;

//loading popup with jQuery magic!
function loadPopup() {
    //loads popup only if it is disabled
    if (popupStatus == 0) {
        $("#backgroundPopup").css({
            "opacity": "0.7"
        });
        $("#backgroundPopup").fadeIn(200);
        $("#popup").fadeIn(200);
        popupStatus = 1;
    }
}

//disabling popup with jQuery magic!
function disablePopup() {
       
    //disables popup only if it is enabled
    if (popupStatus == 1) { 
        $("#backgroundPopup").fadeOut(200);
        $("#popup").fadeOut(200);
        
        popupStatus = 0;
    }
}

 //centering popup
function centerPopup(width,height) {
    //request data for centering
    var windowWidth = 0, windowHeight = 0,ScrollTop=0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        windowWidth = window.innerWidth;
        windowHeight = window.innerHeight;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        windowWidth = document.documentElement.clientWidth;
        windowHeight = document.documentElement.clientHeight;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        windowWidth = document.body.clientWidth;
        windowHeight = document.body.clientHeight;
    }
    ScrollTop= window.pageYOffset || document.body.scrollTop || document.documentElement.scrollTop;

//    var windowWidth = document.documentElement.clientWidth;
//    var windowHeight = document.documentElement.clientHeight;
    var popupHeight = height;  //$("#popup").height();
    var popupWidth = width;  //$("#popup").width();
   
    //centering
    $("#popupSendMail").css({
        "position": "absolute",
        "top": (windowHeight / 2 - popupHeight / 2) + parseInt(ScrollTop),
        "left": windowWidth / 2 - popupWidth / 2,
        "right": windowWidth / 2 - popupWidth / 2,
        "height": "auto",
        "width": "auto"
    });
    //only need force for IE6

    $("#divUser").css({
        "position": "absolute",
        "top": (windowHeight / 2 - popupHeight / 2) + parseInt(ScrollTop),
        "left": windowWidth / 2 - popupWidth / 2,
        "right": windowWidth / 2 - popupWidth / 2,
        "height": "auto",
        "width": "auto"
    });

    $("#divGroup").css({
        "position": "absolute",
        "top": (windowHeight / 2 - popupHeight / 2) + parseInt(ScrollTop),
        "left": windowWidth / 2 - popupWidth / 2,
        "right": windowWidth / 2 - popupWidth / 2,
        "height": "auto",
        "width": "auto"
    });
   
    $("#backgroundPopup").css({
        "height": windowHeight
    });

}

