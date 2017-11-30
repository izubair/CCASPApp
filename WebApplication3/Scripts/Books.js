function GetBooks()
{
    // If we already have a bearer token, set the Authorization header.
 ///   var token = sessionStorage.getItem(tokenKey);
 //   var headers = {};
//    if (token) {
//        headers.Authorization = 'Bearer ' + token;
 //   }

    $.ajax({
        type: 'GET',
        url: 'http://localhost:8090/api/Books/GetBooks',
     //   headers: headers
    }).done(function (data) {
        //self.result(data);
        var dataStr = JSON.stringify(data);
        //var paraStr = $('p:second').text();
        alert(paraStr);        
       
       
        //return JSON.stringify(data);
    }).fail(showError);
}

function showError()
{
    alert("Error in AJAX call to get books!");
}


