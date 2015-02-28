window.onload = function () {
    var user = task1.GetCookie("userName");
    if (user.length >= 2) {
        task1.LoginHtml(user);
    }
};

var task1 = task1 || {}

task1.LogoutHtml = function () {
    var loginForm = document.getElementById("LoginForm");
    var logInfo = document.getElementById("loginInfo");
    logInfo.innerHTML = '';
    var innerHtml = '<span>Login:</span><br /><input type="text" id="login" placeholder="Enter your login" value="" />';
    innerHtml += '<br /><span>Password:</span><br /><input type="text" id="pass" placeholder="Your passwrd" value="" />';
    innerHtml += '<br /><input type="button" value="Login" onclick="task1.Login()" />';
    loginForm.innerHTML = innerHtml;
}

task1.LoginHtml = function (userName) {
    var logInfo = document.getElementById("loginInfo");
    var loginForm = document.getElementById("LoginForm");
    var innerHtml = '<h2 class="hellow">Hellow</h2><h2 class="username">' + userName + '</h2><input type="button" value="Logout" onclick="task1.Logout()" />';
    loginForm.innerHTML = innerHtml;
    task1.CreateButton("loginInfo");
    document.cookie = "userName=" + userName;
}

task1.Login = function () {
    var login = document.getElementById("login").value;
    var pass = document.getElementById("pass").value;
    var req = new XMLHttpRequest();
    var params = ['login=' + login, 'pass=' + pass];
    req.open('POST', 'getajax.aspx', true, login, pass);
    req.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    req.onreadystatechange = function () {
        if (req.readyState == 4) {
            var state = task1.GetCookie('status');
            var userName = task1.GetCookie('userName');
            if (state === 'Login Success') {
                task1.LoginHtml(userName);
            }
            else {
                var innerHtml = '<span>An error occurred when registering:</span><br /><span>' + state + '</span>';
                logInfo.innerHTML = innerHtml;
            }
        }
    }
    req.send("login=" + login + "&pass=" + pass);
}

task1.Logout = function () {
    var req = new XMLHttpRequest();
    req.open('POST', 'logout.aspx', true);
    req.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    req.onreadystatechange = function () {
        if (req.readyState == 4) {
            task1.LogoutHtml();
        }
    }
    req.send(null);
}

task1.CreateButton = function (divId) {
    var div = document.getElementById(divId);
    var innerHtml = '<input type="button" value="GetJson" onclick="task1.GetJson()" />';
    div.innerHTML = innerHtml;
}

task1.GetCookie = function (name) {
    var cookie = " " + document.cookie;
    var search = " " + name + "=";
    var setStr = null;
    var offset = 0;
    var end = 0;
    if (cookie.length > 0) {
        offset = cookie.indexOf(search);
        if (offset != -1) {
            offset += search.length;
            end = cookie.indexOf(";", offset)
            if (end == -1) {
                end = cookie.length;
            }
            setStr = unescape(cookie.substring(offset, end));
        }
    }
    return (setStr);
}

task1.SetCookie = function (name, value, options) {
    options = options || {};
    var expires = options.expires;
    if (typeof expires == "number" && expires) {
        var d = new Date();
        d.setTime(d.getTime() + expires * 1000);
        expires = options.expires = d;
    }
    if (expires && expires.toUTCString) {
        options.expires = expires.toUTCString();
    }
    value = encodeURIComponent(value);
    var updatedCookie = name + "=" + value;
    for (var propName in options) {
        updatedCookie += "; " + propName;
        var propValue = options[propName];
        if (propValue !== true) {
            updatedCookie += "=" + propValue;
        }
    }
    document.cookie = updatedCookie;
}

task1.GetJson = function () {
    var req = new XMLHttpRequest();
    req.open('GET', 'Content/contact.json', true);
    req.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    req.onreadystatechange = function () {
        if (req.readyState == 4) {
            if (req.status == 200) {
                var div = document.getElementById('jsonDiv');
                var json = JSON.parse(req.responseText);
                div.innerHTML = task1.JsonTree(json);
            }

        }
    }
    req.send(null);
}

task1.JsonTree = function (object) {
    var json = "<ul>";
    for (prop in object) {
        var value = object[prop];
        switch (typeof (value)) {
            case "object":
                if (value == null) break;
                var token = Math.random().toString().substr(2);
                json += "<li style='list-style-type:circle;'><a href='#" + token + "' data-toggle='collapse'>" + prop + "</a><div id='" + token + "' class='collapse'>" + task1.JsonTree(value) + "</div></li>";
                break;
            default:
                json += "<li>" + prop + ":" + value + "</li>";
        }
    }
    return json + "</ul>";
};