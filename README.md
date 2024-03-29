# homepage 
*v2.1.0*<br><br>
Homemade homepage : )

Navigation website mainly implemented by Vue.js and .NET. This is the version 2.0.0 with a server program. Users can login and synchronize their added sites. Try me **[HERE](https://homepageweb20190914024555.azurewebsites.net/)**!
<br>All the background pictures are from [wallpapercave](https://wallpapercave.com/).

----------------------------------------

## What's new

+ Users can customize the background images of tags
+ The user interface was fine-tuned.

## Functions

### 1. Search
Provide 3 search engines: **Google**, **Bing** and **Baidu**. The search engine can be changed by *clicking on the search engine icon in front of the search box.* The first time open this page, the default search engine is **Google**. The selected search engine will be stored in **Local Storage**, and later everytime open this page, the search engine stored will be loaded. 

Of course, *enter the content you want to query in the input box* , *click the search icon* on the right or *hit enter* to search.


### 2. Shortcut
Shortcuts are *hidden* and *displayed when the mouse is over it*. There are 11 pre-stored shortcuts: **Youtube**, **Bilibili**, **Gmail**, **Google docs**, **Github**, **JD.com**, **TradeMe**, **Facebook**, **Twitter**, **Instagram** and **QQ mail**. You can add your own shortcuts site by *clicking the **+** button*, but *the background image of the custom shortcut can only be the icon for this website* (I wanted to use the default icon of the linked website but the effect is not so good because the icons of most websites are small and in low resolution). All the shortcuts can be removed by *clicking the red **x*** at the right top of the tags. 

The information of shortcuts will also be storaged in **Local Storage** so the next time you enter this website the shortcuts will be loaded.


### 3. Register, login and reset password
Users can *register their own account to synchronize their sites saved*. By clicking the **login button** on the right top of the main area, you can login, register or reset the password (forget password). The sites users added will be saved in the database, and when login at somewhere else, the sites can be synchronized. 

To create an account, **username**, **password** and **email address** are need. Email address will be used to *receive captcha* and *reset password*. The server will not save the emails directly but will save its hash value. 


### 4. Customize background images of tags
Users can *customize their background images of tags*. The uploaded images should be **under 25kb**, and the recommanded size should be around **210px * 130px**, which can have a better effect. 

All the uploaded images will be converted to Base64. If the user *does not login*, the images will be stored in **localstorage**. Or, the images will be *uploaded to server and stored in database*.

### 5. Others
+ The background images will be randomly changed every time you open this page or refresh it. 
+ The color of the breathing lights on both sides of the main area is randomly generated.
+ The placeholder content of the search box is randomly selected from the presets.
+ There are 3 hidden buttons (not really hard to find :P ).

------------------------------------------

## How to release and try

*Before you use it, there are some works to do*. 

1. **Configure the database connection string**. It is defined in ***Web.config*** in folder **homepageWeb**. Add your connection string in node **connectionStrings**. 
2. **Configure the email server settings**. An email SMTP service is needed to send captcha to users when they register new accounts or reset their password. Settings about this email service is defined in ***Web.config*** in folder **homepageWeb**. In node **EmailSettings/email** you need to set your email **username**, **password**, **host** and **prot** who provide the SMTP service. The preset host and port is for *Gmail*. If you need other emails, you need to replace them with your owns.
3. **Finally, release it to your server and use.**
------------------------------------------

## What's next
+ Support for automatic login.
+ Optimize the user experience of the user interface.

More features are being imagined...

-------------------------------------------
Try me at: https://homepageweb20190914024555.azurewebsites.net/

***Enjoy your journey with it!***


