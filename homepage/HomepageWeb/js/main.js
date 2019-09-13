// ***********************************************************************************

// globle parts:

// data:

var defaultSites = {
            "Youtube": {url: "https://www.youtube.com", imgPath: "icos/youtube.ico",},
            "Bilibili": {url: "https://www.bilibili.com/", imgPath: "icos/bilibili.ico",},
            "Gmail": {url: "https://mail.google.com", imgPath: "icos/Gmail.ico",},
            "Google docs": {url: "https://www.google.com/intl/zh-CN/docs/about/", imgPath: "icos/Google docs.ico",},
            "Github": {url: "https://github.com/", imgPath: "icos/Github.ico",},
            "JD.com": {url: "https://www.jd.com/", imgPath: "icos/JD.com.ico",},
            "TradeMe": {url: "https://www.trademe.co.nz/", imgPath: "icos/TradeMe.ico",},
            "Facebook": {url: "https://www.facebook.com/", imgPath: "icos/Facebook.ico",},
            "Twitter": {url: "https://twitter.com/", imgPath: "icos/Twitter.ico",},
            "Instagram": {url: "https://www.instagram.com/", imgPath: "icos/Instagram.ico",},
            "QQ mail": {url: "https://mail.qq.com/", imgPath: "icos/QQ mail.ico",},
        };

// functions:

Vue.prototype.getLoginAs = function() {
    return document.cookie.split("=")[1];
};

Vue.prototype.givenSiteInDefault = function(site) {
    for (key in defaultSites) {
        var defaultSite = defaultSites[key];
        if(key.toLowerCase() == site.name.toLowerCase() 
            && (defaultSite.url.toLowerCase().indexOf(site.url.split("//")[1].toLowerCase()) > -1 
            || site.url.toLowerCase().indexOf(defaultSite.url.split("//")[1].toLowerCase()) > -1)) {
            return key;
        }
    }
    return null;
};

Vue.prototype.bgUrl = function(){
    var index = Math.floor(Math.random()*44);
    var url  = "url('bg/"+index+".jpg')";
    $("#bg").css("backgroundImage", url);
    return url;
};

Vue.prototype.randomColor = function(){
    var r,g,b;

    // way1: limit the amount of green and blue

    // r = Math.floor(Math.random()*255);
    // g = Math.floor((0.4 + Math.random() / 2) * 255);
    // b = Math.floor((0.3 + Math.random() / 5) * 255);
    

    // way2: completely random, high saturation and brightness

    r = Math.floor(Math.random()*255);
    g = Math.floor(Math.random()*255);
    b = Math.floor(Math.random()*255);
    var sa = Math.min.apply(Math,[r,g,b]);
    r -= sa;
    g -= sa;
    b -= sa;
    var br = 255 - Math.max.apply(Math,[r,g,b]);
    r += br;
    g += br;
    b += br; 

	// weak purple: purple is not so suitable for these background images
    if (Math.abs(b-r) < 100) {
    	return this.randomColor();
    }

    return "rgba(" + [r,g,b,0.8] + ")";
};


// focus and blur effects for inputs:

Vue.prototype.focusEffect = function(event) {
    var id = event.target.id;
    $("#" + id).css("height", "2.5em");
    $("#" + id).css("font-size", "1.2em");
    $("#" + id + "Label").css("font-size", "0.9em");
};

Vue.prototype.blurEffect = function(event) {
    var id = event.target.id;
    $("#" + id).css("height", "1.7em");
    $("#" + id).css("font-size", "1em");
    $("#" + id + "Label").css("font-size", "1.3em"); 
};


// get captchar by email:

Vue.prototype.getCaptcha = function(that, dom, second) {
    var self = this;
    var type = ""; 
    var email = "";

    if (dom.attr("name") == "getForgetCaptcha") {
        type = "forget";
        email = $("#forgetEmail").val();
    } else if (dom.attr("name") == "getRegisterCaptcha") {
        type = "register";
        email = $("#registerEmail").val();
    } else {
        alert("Error!");
        return;
    }

    dom.attr("disabled", true);

    if(second == 60) {
        if (that.emailOk) {
            // ajax to get captcha:
            $.ajax({
                url: "./GetCaptchaEmail.ashx",
                type: "get",
                data: {"email": email, "type": type},
                success: function(data) {
                    if (data === "1") {
                        alert("An error occurred, please check your email address or try later.");
                    } else if (data === "0") {
                        alert("An email containing the captcha has sent to the email you entered, please check the email.");
                    } else {
                        alert("An error occurred, please try later.");
                    }
                },
                error: function() {
                    alert("An error occurred, please try later.");
                },  
            });
        }
    }

    if(second != 0) {
        that.buttonText = "Retry (" + second + "s)";
        setTimeout(function() {
            self.getCaptcha(that, dom, --second);
        }, 1000);
    } else {
        dom.removeAttr("disabled");
        that.buttonText = "Get";
    }
};

//  get captcha image:
Vue.prototype.changeImageCaptcha = function() {
    $("#captchaPic").attr("src", "./GetCaptchaImage.ashx?d=" + new Date());
};

// windowLayer commen functions:

Vue.prototype.closeWindow = function(type, that) {
    $('#' + type + "Form")[0].reset();
    $('#cover').fadeOut('normal');
    $('#' + type + "Wrap").fadeOut('fast');
    $('.formInputs').css("box-shadow", "");
    that.info = "";
};


// check functions: 
Vue.prototype.checkUsername = function(that, dom) {
    var text = dom.val();

    dom.css("box-shadow", "0 0 10px red");
    that.usernameOk = false;

    if(text.length == 0) {
        that.info = "Please enter your username."
    } else if(text.length > 50) {
        that.info = "Length not satisfied (<50)."
    } else {
        that.info = "";
        that.usernameOk = true;
        dom.css("box-shadow", "");
    }
}

Vue.prototype.checkNewUsername = function(that, dom) {
    var text = dom.val();
    that.usernameOk = false;

    if(text.length == 0) {
        that.info = "Please enter your username."
        dom.css("box-shadow", "0 0 10px red");
    } else if(text.length > 50) {
        that.info = "Length not satisfied (<50)."
        dom.css("box-shadow", "0 0 10px red");
    } else {
        $.ajax({
            url: "./CheckExist.ashx",
            type: "post",
            data: {"username": text},
            success: function(data) {
                if (data === "0") {
                    that.info = "";
                    that.usernameOk = true;
                    dom.css("box-shadow", "");
                } else if (data == "1") {
                    that.info = "This username has been used.";
                    dom.css("box-shadow", "0 0 10px red");
                } else {
                    alert("An error occurred, please try later.");
                    dom.css("box-shadow", "0 0 10px red");
                }
            },
            error: function() {
                alert("An error occurred, please try later.");
                dom.css("box-shadow", "0 0 10px red");
            },  
        });
    }
};

Vue.prototype.checkPassword = function(that, dom) {
    var text = dom.val();
    var regNumber = /\d+/;
    var regLetter = /[a-zA-Z]+/;

    dom.css("box-shadow", "0 0 10px red");
    that.passwordOk = false;

    if (text.length == 0) {
        that.info = "Please enter your password.";
    } else if (text.length < 6 || text.length > 20) {
        that.info = "Length not satisfied (6-20).";
    } else if (!regNumber.test(text) || !regLetter.test(text)) {
        that.info = "Formate not satisfied.";
    } else {
        that.passwordOk = true;
        dom.css("box-shadow", "");
    }
};

Vue.prototype.checkRepeat = function(that, dom, target) {
    var text = dom.val();
    var targetText = target.val();

    var regNumber = /\d+/;
    var regLetter = /[a-zA-Z]+/;

    dom.css("box-shadow", "0 0 10px red");
    that.repeat = false;

    if (text == "") {
        that.info = "Please repeat your password."
    } else if (text != targetText) {
        that.info = "Not the same with the password.";
    } else if (text.length < 6 || text.length > 20) {
        that.info = "Length not satisfied (6-20).";
    } else if (text.length == 0) {
        that.info = "Please enter your password.";
    } else if (!regNumber.test(text) || !regLetter.test(text)) {
        that.info = "Formate not satisfied.";
    } else {
        that.repeatOk = true;
        dom.css("box-shadow", "");
    }
};

Vue.prototype.checkEmail = function(that, dom) {
    var text = dom.val();
    var reg = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;

    dom.css("box-shadow", "0 0 10px red");
    that.emailOk = false;

    if (text == "") {
        that.info = "Please enter your Email.";
    } else if (!reg.test(text)) {
        that.info = "Formate not satisfied.";
    } else {
        that.emailOk = true;
        dom.css("box-shadow", "");
    }
};


Vue.prototype.checkCaptcha = function(that, dom) {
    var text = dom.val();

    dom.css("box-shadow", "0 0 10px red");
    that.captchaOk = false;

    if (text == "") {
        that.info = "Please enter the captcha.";
    } else if (text.length != 5) {
        that.info = "Formate not satisfied.";
    } else {
        that.captchaOk = true;
        dom.css("box-shadow", "");
    }
};

//  check if all the inputs are okay in format
Vue.prototype.checkSatisfied = function(that, buttonDom) {
    if (that.satisfied()) {
        that.info = "Great!";
        buttonDom.removeAttr("disabled");
        buttonDom.addClass("thisSubmitterAbled");
        buttonDom.removeClass("thisSubmitterDisabled");
    } else {
        buttonDom.attr("disabled", true);
        buttonDom.addClass("thisSubmitterDisabled");
        buttonDom.removeClass("thisSubmitterAbled");
    }
};


// after login: 
Vue.prototype.loginSucceed = function(username) {
    $("#login").hide();
    $("#user").show();
    $("#showedUsername").text(username);
    localStorage.setItem("loginAs", "user");
    this.loadUserSites();
};


// data synchronization: 
Vue.prototype.loadUserSites = function() {
    var that = this;
    $.ajax({
        url: "./loadUserSites.ashx",
        type: "get",
        success: function(data) {
            var list = eval("(" + data + ")");
            tagsVue.userSites = [];

            // add data to userSites of tagsVue
            for (var index in list) {
                var obj = list[index];
                var newObj = {"name": obj.siteName, "url": obj.siteUrl, "imgPath": "icos/default.ico"};
                var defaultSite = that.givenSiteInDefault(newObj);
                if (defaultSite != null) {
                    newObj.imgPath = defaultSites[defaultSite].imgPath;
                }
                tagsVue.userSites.push(newObj);
            }
        },
        error: function() {
            alert("synchromize failed, please try later.");
        },
        complete: function() {
            tagsVue.loadSites();
        }
    });
};

// ***********************************************************************************

// tags:

var tagsVue = new Vue({
    el: "#tags",
    data: {
        userSites: [],
    },
    methods: {
        loadSites(){
            var savedLoginAs = localStorage.getItem("loginAs");
            if (savedLoginAs == null) {
                for (site in defaultSites) {
                    var tagObj = {"name": site, "url": defaultSites[site].url, "imgPath": defaultSites[site].imgPath};
                    this.userSites.push(tagObj);
                }
                
                localStorage.setItem("loginAs", "visitor");
                localStorage.setItem("visitor", JSON.stringify(this.userSites));
            } 

            if (savedLoginAs == "visitor") {
                var savedSites = localStorage.getItem("visitor");
                this.userSites = JSON.parse(savedSites);
            }
        },

        openTag(url){
            window.open(url,"_blank");
        },

        deleteTag(event){
            // stop bubble
            var e = (event) ? event : window.event;
            if (window.event) {
                e.cancelBubble = true;
            }
            else {
                e.stopPropagation();
            }

            var r = confirm("Delete this tag?")
            if(r){               
                var target = event.target.parentNode.firstElementChild.innerHTML;
                var siteName, siteUrl;

                //  remove from page
                for (var i=0; i<this.userSites.length; i++) {
                    if(this.userSites[i].name == target) {
                        siteName = this.userSites[i].name;
                        siteUrl = this.userSites[i].url;
                        this.userSites.splice(i, 1);
                        break;
                    }
                }

                //  save or synchromize:
                if (this.getLoginAs() == "visitor" || this.getLoginAs() == null) {
                    localStorage.setItem("visitor",JSON.stringify(this.userSites));
                } else {
                    this.deleteUserSite(siteName, siteUrl);
                }
            }
        },
        addNewTag(e){
            //  get url:
            var url = prompt("Enter the URL:");
            if(url != null && url != "") {
                //  check if this site has existed:
                for(i in this.userSites) {
                    var site = this.userSites[i];
                    if(site.url.toLowerCase() == url.toLowerCase()) {
                        alert("Sorry, this URL has existed");
                        return;
                    }
                }

                // predict name:
                var prediction = "";
                var urlParts = url.split("//");
                if(urlParts.length == 2){
                    urlParts = urlParts[1];
                } else {
                    urlParts = urlParts[0];
                    url = "http://" + url;
                }
                var mainUrl = urlParts.split("/")[0];
                var mainUrlParts = mainUrl.split(".");
                if(mainUrlParts[0] == "www"){
                    prediction = mainUrlParts[1];
                } else {
                    prediction = mainUrlParts[0];
                }

                //  get name:
                var name = prompt("Enter the name:", prediction);
                if(name != null && name != "") {
                    for(i in this.userSites) {
                        var site = this.userSites[i];
                        if(site.name.toLowerCase() == name.toLowerCase()) {
                            alert("Sorry, this name has existed");
                            return;
                        }
                    }


                    var newTagObj = {
                        name: name,
                        url: url,
                        imgPath: "icos/default.ico",
                    };

                    //  check if the added is in default sites:
                    var defaultSite = this.givenSiteInDefault(newTagObj);
                    if (defaultSite != null) {
                        newTagObj.imgPath = defaultSites[defaultSite].imgPath;
                    }
                    
                    this.userSites.push(newTagObj);

                    //  save or synchromize:
                    if (this.getLoginAs() == null) {
                        localStorage.setItem("visitor",JSON.stringify(this.userSites));
                    } else {
                        this.addUserSite(name, url);
                    }
                
                } else {
                    alert("Please enter a name")
                }


            } else {
                alert("Please enter a URL");
            }
        },

        addUserSite(siteName, siteUrl) {
            $.ajax({
                url: "./AddUserSite.ashx",
                type: "post",
                data: {
                    "sitename": siteName,
                    "siteUrl": siteUrl,
                },
                success: function(data) {
                    if (data === "0") {
                    } else if (data === "2") {
                        alert("Please login first.");
                        loginArea.logout();
                    } else if (data === "1") {
                        alert("This site site is already in the server. Error code: 1");
                    } else if (data === "-1") {
                        alert("synchromize failed. Error code: -1. Please try later.");
                    } else {
                        alert("synchromize failed. Error code: " + data + ". Please try later.");
                    }

                },
                error: function() {
                    alert("synchromize failed. Error code: unknown. Please try later.");
                },
            });
        },

        deleteUserSite(siteName, siteUrl) {
            if (siteName == "" || siteUrl == "") {
                return;
            }

            $.ajax({
                url: "./DeleteUserSite.ashx",
                type: "get",
                data: {
                    "sitename": siteName,
                    "siteUrl": siteUrl,
                },
                success: function(data) {
                    if (data === "0") {
                    } else if (data === "2") {
                        alert("Please login first.");
                        loginArea.logout();
                    } else if (data === "1") {
                        alert("This site site does not exist in the server. Error code: 1");
                    } else if (data === "-1") {
                        alert("synchromize failed. Error code: -1. Please try later.");
                    } else {
                        alert("synchromize failed. Error code: " + data + ". Please try later.");
                    }
                },
                error: function() {
                    alert("synchromize failed. Error code: unknown. Please try later.");
                },
            });
        },


        //  this is a reserved function: can be used to add all the sites of this user

        // addUserSites() {
        //     var sitesJsonObj = {};
        //     for (var i=0; i<this.userSites.length; i++) {
        //         var siteObj = this.userSites[i];
        //         sitesJsonObj[siteObj.name] = siteObj.url;
        //     }

        //     $.ajax({
        //         url: "./AddUserSites.ashx",
        //         data: { "sites": JSON.stringify(sitesJsonObj) },
        //         type: "get",
        //         success: function(data) {
        //             console.log("upload return: " + data);

        //         },
        //         error: function() {
        //             console.log("error");
        //         }
        //     });
        // },

    },
});


// ***********************************************************************************

// login button click:

var loginArea = new Vue({
	el: "#loginArea",
	methods: {
		showLoginWrap(){
            this.changeImageCaptcha();
            $('#cover').fadeIn("fast");
            $('#loginWrap').fadeIn("normal");
		},

        logout() {
            localStorage.setItem("loginAs", "visitor");
            $("#afterLoginWrap").hide();
            $("#user").hide();
            $("#login").show();
            $.get("./Logout.ashx");
            tagsVue.loadSites();
        },

        toggleAfterLoginWrap() {
            $("#afterLoginWrap").fadeToggle();
        },

        checkLogin() {
            var that = this;
            $.ajax({
                url: "./CheckLogin.ashx",
                type: "get",
                async: false,
                success: function(data) {
                    if (data === "0") {
                        var username = that.getLoginAs();
                        that.loginSucceed(username);
                    } else {
                        if (localStorage.getItem("loginAs") != null) {
                            localStorage.setItem("loginAs", "visitor");
                        }
                        tagsVue.loadSites();
                    }
                },
            });
        },
	},
    mounted: function() {
        $("#user").hide();
        this.checkLogin();
    },
});


// ***********************************************************************************

// about login: 

new Vue({
    el: "#loginWrap",
    data: {
        info:"",

        usernameOk: false,
        passwordOk: false,
        captchaOk: false,

        // When login is successfully submitted but failed, this flag will be set to false. 
        // If password or username is modified, this flag will be set to true again.
        flag: true,             

        satisfied() {
            return this.usernameOk && this.passwordOk && this.captchaOk && this.flag;
        },
    },
    methods: {
        checkLoginUsername() {
            this.checkUsername(this, $("#loginUsername"));
            this.flag = true;
        },

        checkLoginPassword() {
            this.checkPassword(this, $("#loginPassword"));
            this.flag = true;
        },

        checkLoginCaptcha() {
            this.checkCaptcha(this, $("#loginCaptcha"));
            this.flag = true;
        },

        checkLoginSatisfied() {
            this.checkSatisfied(this, $("#loginSubmitter"));
        },

        toRegister() {
            this.info = "";
            $('#loginWrap').fadeOut("fast");
            $('#registerWrap').fadeIn("normal");
            $('#loginForm')[0].reset();
            $('.formInputs').css("box-shadow", "");
        },

        toForgetPassword() {
            this.info = "";
            $('#loginWrap').fadeOut("fast");
            $('#forgetWrap').fadeIn("normal");
            $('#loginForm')[0].reset();
            $('.formInputs').css("box-shadow", "");
        },

        login() {
            var username = $("#loginUsername").val();
            var password = $("#loginPassword").val();
            var captcha = $("#loginCaptcha").val();

            if (this.satisfied()) {
                var that = this;

                $.ajax({
                    url: "./Login.ashx",
                    type: "post",
                    data: {
                        "username": username,
                        "password": password,
                        "captcha": captcha
                    },
                    success: function(data) {
                        if (data === "-1") {
                            that.info = "Invalid username.";
                            that.usernameOk = false;
                            $("#forgetUsername").css("box-shadow", "0 0 10px red");
                        } else if (data === "-2") {
                            that.info = "Invalid password.";
                            that.passwordOk = false;
                            $("#forgetEmail").css("box-shadow", "0 0 10px red");
                        } else if (data === "-3") {
                            that.info = "Incorrect captcha.";
                            that.captchaOk = false;
                            $("#forgetPassword").css("box-shadow", "0 0 10px red");
                        } else if (data === "1") {
                            that.info = "Incorrect username or password.";
                            that.flag = false;
                            $("#forgetUsername").css("box-shadow", "0 0 10px red");
                        } else if (data === "0") {
                            that.info = "Login succeed!";
                            that.loginSucceed(username);
                            setTimeout(function() {
                                that.closeLoginWrap();
                            }, 2000);
                        } else {
                            that.info = "";
                            alert("An error occurred, please try later.");
                        }

                        that.changeImageCaptcha();
                    },
                    error: function() {
                        alert("An error occurred, please try later.");
                        that.changeImageCaptcha();
                    },  
                });
            }
        },

        closeLoginWrap() {
            this.closeWindow("login", this);
        },
    },
});


// ***********************************************************************************

// about forget password: 

new Vue({
    el: "#forgetWrap",
    data: {
        info: "",
        buttonText: "Get",

        usernameOk: false,
        emailOk: false,
        passwordOk: false,
        repeatOk: false,
        captchaOk: false,

        satisfied() {
            return this.usernameOk && this.passwordOk && this.repeatOk && this.emailOk && this.captchaOk;
        },
    },
    methods: {
        checkForgetUsername() {
            this.checkUsername(this, $("#forgetUsername"));
        },

        checkForgetEmail() {
            this.checkEmail(this, $("#forgetEmail"));
        },

        checkForgetPassword() {
            this.checkPassword(this, $("#forgetPassword"));
            if ($("#forgetRepeat").val().length != 0) {
                this.checkRepeat(this, $("#forgetRepeat"), $("#forgetPassword"));
            }
        },

        checkForgetRepeat() {
            this.checkRepeat(this, $("#forgetRepeat"), $("#forgetPassword"));
        },

        checkForgetCaptcha() {
            this.checkCaptcha(this, $("#forgetCaptcha"));
        },

        checkForgetSatisfied() {
            this.checkSatisfied(this, $("#forgetSubmitter"));
        },

        closeForgetWrap() {
            this.closeWindow("forget", this);
        },

        toLogin() {
            this.info = "";
            $('#forgetWrap').fadeOut("fast");
            $('#loginWrap').fadeIn("normal");
            $('#forgetForm')[0].reset();
            $('.formInputs').css("box-shadow", "");
        },

        getForgetCaptcha() {
            this.checkForgetEmail();
            if (this.emailOk) {
                this.getCaptcha(this, $("#getForgetCaptcha"), 60);
            }
            
        },

        changePassword() {
            var username = $("#forgetUsername").val();
            var email = $("#forgetEmail").val();
            var password = $("#forgetPassword").val();
            var repeat = $("#forgetRepeat").val();
            var captcha = $("#forgetCaptcha").val();

            if (this.satisfied()) {
                var that = this;

                $.ajax({
                    url: "./ForgetPassword.ashx",
                    type: "post",
                    data: {
                        "username": username,
                        "password": password,
                        "repeat": repeat,
                        "email": email,
                        "captcha": captcha
                    },
                    success: function(data) {
                        if (data === "-1") {
                            that.info = "Invalid username.";
                            that.usernameOk = false;
                            $("#forgetUsername").css("box-shadow", "0 0 10px red");
                        } else if (data === "-2") {
                            that.info = "Invalid email address.";
                            that.emailOk = false;
                            $("#forgetEmail").css("box-shadow", "0 0 10px red");
                        } else if (data === "-3") {
                            that.info = "Invalid password.";
                            that.passwordOk = false;
                            $("#forgetPassword").css("box-shadow", "0 0 10px red");
                        } else if (data === "-4") {
                            that.info = "Invalid repeat password.";
                            that.repeatOk = false;
                            $("#forgetRepeat").css("box-shadow", "0 0 10px red");
                        } else if (data === "-5") {
                            that.info = "Invalid captcha, please try again.";
                            that.captchaOk = false;
                            $("#forgetCaptcha").css("box-shadow", "0 0 10px red");
                        } else if (data === "1") {
                            that.info = "Username is not existed.";
                            that.usernameOk = false;
                            $("#forgetUsername").css("box-shadow", "0 0 10px red");
                        } else if (data === "2") {
                            that.info = "Email is not matched.";
                            that.emailOk = false;
                            $("#forgetEmail").css("box-shadow", "0 0 10px red");
                        }else if (data === "0") {
                            that.info = "Succeed! Jumping to login...";
                            setTimeout(function() {
                                that.toLogin();
                            }, 2000);
                        } else {
                            that.info = "";
                            alert("An error occurred, please try later.");
                        }
                    },
                    error: function() {
                        alert("An error occurred, please try later.");
                    },  
                });
            }
        },
    },
    mounted: function() {
        this.checkForgetSatisfied();
    }
});


// ***********************************************************************************

// about register: 

new Vue({
    el: "#registerWrap",
    data: {
        info: "",
        buttonText: "Get",

        usernameOk: false,
        passwordOk: false,
        repeatOk: false,
        emailOk: false,
        captchaOk: false,

        satisfied() {
            return this.usernameOk && this.passwordOk && this.repeatOk && this.emailOk && this.captchaOk;
        },
    },
    methods: {

        checkRegisterUsername() {
            this.checkNewUsername(this, $("#registerUsername"));
        },

        checkRegisterEmail() {
            this.checkEmail(this, $("#registerEmail"));
        },

        checkRegisterPassword() {
            this.checkPassword(this, $("#registerPassword"));
            if ($("#registerRepeat").val().length != 0) {
                this.checkRepeat(this, $("#registerRepeat"), $("#registerPassword"));  
            }
        },

        checkRegisterRepeat() {
            this.checkRepeat(this, $("#registerRepeat"), $("#registerPassword"));
        },

        checkRegisterCaptcha() {
            this.checkCaptcha(this, $("#registerCaptcha"));
        },

        checkRegisterSatisfied() {
            this.checkSatisfied(this, $("#registerSubmitter"));
        },

        closeRegisterWrap() {
            this.closeWindow("register", this);
        },

        toLogin() {
            this.info = "";
            $('#registerWrap').fadeOut("fast");
            $('#loginWrap').fadeIn("normal");
            $('#registerForm')[0].reset();
            $('.formInputs').css("box-shadow", "");
        },

        getRegisterCaptcha() {
            this.checkRegisterEmail();
            if (this.emailOk) {
                this.getCaptcha(this, $("#getRegisterCaptcha"), 60);
            }
        },

        register() {
            var username = $("#registerUsername").val();
            var password = $("#registerPassword").val();
            var repeat = $("#registerRepeat").val();
            var email = $("#registerEmail").val();
            var captcha = $("#registerCaptcha").val();

            if (this.satisfied) {
                var that = this;

                $.ajax({
                    url: "./Register.ashx",
                    type: "get",
                    data: {
                        "username": username,
                        "password": password,
                        "repeat": repeat,
                        "email": email,
                        "captcha": captcha
                    },
                    success: function(data) {
                        if (data === "-1") {
                            that.info = "Invalid username.";
                            that.usernameOk = false;
                            $("#registerUsername").css("box-shadow", "0 0 10px red");
                        } else if (data === "-2") {
                            that.info = "Invalid password.";
                            that.passwordOk = false;
                            $("#registerPassword").css("box-shadow", "0 0 10px red");
                        } else if (data === "-3") {
                            that.info = "Invalid repeat password.";
                            that.repeatOk = false;
                            $("#registerRepeat").css("box-shadow", "0 0 10px red");
                        } else if (data === "-4") {
                            that.info = "Invalid email address.";
                            that.emailOk = false;
                            $("#registerEmail").css("box-shadow", "0 0 10px red");
                        } else if (data === "-5") {
                            that.info = "Invalid captcha, please try again.";
                            that.captchaOk = false;
                            $("#registerCaptcha").css("box-shadow", "0 0 10px red");
                        } else if (data === "1") {
                            that.info = "This username has existed.";
                            that.usernameOk = false;
                            $("#registerUsername").css("box-shadow", "0 0 10px red");
                        } else if (data === "0") {
                            that.info = "Succeed! Jumping to login...";
                            setTimeout(function() {
                                that.toLogin();
                            }, 2000);
                        } else {
                            that.info = "";
                            alert("An error occurred, please try later.");
                        }

                    },
                    error: function() {
                        alert("An error occurred, please try later.");
                    },  
                });
            } else {
                this.info = "Please Please fill in all inputs."
            }
        },
    },
    mounted: function() {
        this.checkRegisterSatisfied();
    },
});

// ***********************************************************************************
// effect functions

new Vue({
    el: "#hiddenButtons",
    data: {
        hideTags: true,
    },
    methods: {
        changeImg(){
            var div = document.getElementById("bg");
            div.style.backgroundImage = this.bgUrl();
        },

        fixTags(){
            this.hideTags = !this.hideTags;
            if(this.hideTags){
                document.getElementById("tags").removeAttribute("style");
            } else {
                document.getElementById("tags").style.opacity=1;
            }
            
        },
    },
});

new Vue({
    el: "#mainBG",
    data: {
        lightColor:"",
    },
    methods:{
        findKeyframesRule(name){
            var ss = document.styleSheets;
            for(var i=0; i<ss.length; i++){
                for(var j=0; j<ss[i].cssRules.length; j++){
                    var rule = ss[i].cssRules[j];
                    if(rule.name == name && (rule.tyle == window.CSSRule.KEYFRAMES_RULE) || rule.style == window.CSSRule.WEBKIT_KEYFRAMES_RULE){
                        return rule;
                    }
                }
            }
            return null;
        },
        breathStyleRefresh(){
            this.lightColor = this.randomColor();
            var keyframes = this.findKeyframesRule("breath");
            keyframes.deleteRule("0%");
            keyframes.deleteRule("50%");
            keyframes.deleteRule("100%");

            var newRule1 = "{" + 
                "-webkit-box-shadow: 0 0 0px " + this.lightColor +";" +
                "-moz-box-shadow: 0 0 0px " + this.lightColor +";" +
                "ms-box-shadow: 0 0 0px " + this.lightColor +";" +
                "-o-box-shadow: 0 0 0px " + this.lightColor +";" +
                "box-shadow: 0 0 0px " + this.lightColor +";" +
            "}";
            var newRule2 = "{" + 
                "-webkit-box-shadow: 0 0 30px " + this.lightColor +";" +
                "-moz-box-shadow: 0 0 30px " + this.lightColor +";" +
                "ms-box-shadow: 0 0 30px " + this.lightColor +";" +
                "-o-box-shadow: 0 0 30px " + this.lightColor +";" +
                "box-shadow: 0 0 30px " + this.lightColor +";" +
            "}";

            keyframes.appendRule("from" + newRule1);
            keyframes.appendRule("50%" + newRule2);
            keyframes.appendRule("to" + newRule1);
        },
    },
    mounted: function(){
        this.breathStyleRefresh();
        setInterval(this.breathStyleRefresh, 6000);
    },
});

new Vue({
    el: "#bg",
    methods: {
        styleObject: function(){
            return {
                "background-image": this.bgUrl(),
                "background-repeat": " no-repeat",
                "background-position": "center",
                "background-size": "cover",
                "position": "fixed",
                "height": "100%",
                "width": "100%",
                "z-index": "-100",
            };
        },
    }
});


// ***********************************************************************************
// search

new Vue({
    el: '#search',
    data: {
        searchEngines: [
            { name: 'Google'},
            { name: 'Bing'},
            { name: 'Baidu'}
        ],
        picked: "Google",
        text: "",
    },
    methods: {
        changeEngine(){
            var switcher = document.getElementById("switcher");
            var path = "pic/" + this.picked + ".png";
            switcher.style.backgroundImage = 'url(' + path + ')';
            localStorage.setItem("engine",this.picked);
        },
        search(){
            var url;
            if(this.picked == "Google"){
                url = "https://www.google.com.hk/search?&sourceid=chrome&ie=UTF-8&q=" + encodeURIComponent(this.text);
            } else if(this.picked == "Bing"){
                url = "https://www.bing.com/search?ensearch=1&q=" + encodeURIComponent(this.text);
            } else if(this.picked == "Baidu"){
                url = "https://www.baidu.com/s?wd=" + encodeURIComponent(this.text);
            }
            window.open(url,"_blank");
            this.randomHolder();
        },
        randomHolder(){
            var holders = [ 
                " Search here ...",
                " What do u want to know ?",
                " Get something new here !",
                " What are u wondering ?",
                " I can answer your question ~",
                " Ask me anything ."
            ];
            var index = Math.floor(Math.random()*holders.length);
            return holders[index];
        },
        loadEngine(){
                var savedEngine = localStorage.getItem("engine");
                if(savedEngine == "Google" || savedEngine == "Bing" || savedEngine == "Baidu") {
                    this.picked = savedEngine;
                }
        },
        focus() {
            document.getElementById("search").setAttribute('style', "background : rgba(255, 255, 255, 0.95); box-shadow: 0 0 20px rgba(245, 247, 240, 0.9);");
        },
        blur() {
            document.getElementById("search").removeAttribute("style");
        }
    },
    mounted: function(){
        this.loadEngine();
        this.changeEngine();
    }
});



