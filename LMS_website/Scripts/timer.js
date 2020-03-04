var counter = 0;
var interID = 0;
function showAlert(timeinmilliseconds) {
    var i = timeinmilliseconds / 1000;
    counter = i;
    document.getElementById("chkControl1").style.visibility = 'hidden';
    document.getElementById('ButtonControl').style.visibility = 'hidden';

    //for (i = timeinmilliseconds / 1000; i > 0 ; i--){

       interID = setInterval(
        "callFun1();"
        , 1000);    
   // }

//    var t = setTimeout(callFun1, timeinmilliseconds);

};

function callFun1() {
    counter--;
    
    if (counter < 1) {
        document.getElementById('chkControl1').style.visibility = 'visible';
        //document.getElementById('TimerCountDown').style.visibility = 'hidden';
        document.getElementById('TimerCountDown').innerHTML = " --: &nbsp;&nbsp; NOW ! ";
        clearInterval(interID);
    }
    else {
        var min = parseInt(counter / 60);
        var sec = counter % 60;

 
        if (min > 0) {
            document.getElementById('TimerCountDown').innerHTML = " after --:&nbsp;&nbsp; " + min + " mins & " + sec + " secs";
        }
        else {
            document.getElementById('TimerCountDown').innerHTML = "after --:&nbsp;&nbsp;" + sec + " secs";
        }

    }
}


