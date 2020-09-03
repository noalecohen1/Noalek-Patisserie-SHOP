

//Facebook API
const token = "EAAtBXy0BCwYBAL0dLLmn3yAnLhJnvoFPRfSyFUd9ZAHlAEhHiEWOBqxZCPZB6pYfkiLS5is8qX3ZBw2ap6OyzYX3ajeHm05s4Hxe1eyTCsrrXWj3aFH2m4eBMQUegKEoRZBva7jL6Yict3hj3WoYV5HHVtCqpqqtJ7KIJWH11VwZDZD";

//Facebook API - Post Status
function postAStatus() {
    var status = document.getElementById('postTxt').value;

    FB.api(
        '/102057014963217/feed',
        'POST',
        {
            "message": status,
            "access_token": token
        },
        function (response) {
            console.log(response);
        }
    );
}

//Facebook API - Boost product (Post Image+Name)
function boostAProduct(status, photoURL) {
    FB.api(
        '/102057014963217/photos',
        'POST',
        {
            "message": status,
            "url": photoURL,
            "access_token": token
        },
        function (response) {
            console.log(response);
        }
    );
}

window.fbAsyncInit = function () {
    FB.init({
        appId: '320536829357291',
        cookie: true,
        xfbml: true,
        version: 'v8.0'
    });

    FB.AppEvents.logPageView();
};

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "https://connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));
