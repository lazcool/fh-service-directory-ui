function gtag$1(){dataLayer.push(arguments)}function loadAnalytics(e){window.dataLayer=window.dataLayer||[],gtag$1("js",new Date),addFormInteractionGa4Events();const o=getPiiSafePageView(e);gtag$1("config",e,{send_page_view:!1,page_path:o.page_path,page_location:o.page_location,page_referrer:o.referrer}),gtag$1("event","page_view",getPiiSafePageView(e))}function addFormInteractionGa4Events(e){const o=document.querySelectorAll("form");for(let t=0;t<o.length;++t){const i=o[t].id||"",n=o[t].name||null;let r=o[t].action||"";const s=r.split("?"),c=getPiiSafeQueryString(s[1]);null!=c&&(r=s[0]+c),o[t].addEventListener("submit",(o=>{gtag$1("event","form_submit",{form_id:i,form_name:n,form_destination:r,send_to:e})})),o[t].addEventListener("focus",(o=>{gtag$1("event","form_start",{form_id:i,form_name:n,form_destination:r,send_to:e})}))}}function getPiiSafePageView(e){const o={page_title:document.title,send_to:e};if(""===document.referrer)o.referrer="";else{const e=getPiiSafeQueryString(new URL(document.referrer).search);if(null==e)o.referrer=document.referrer;else{const t=document.referrer.split("?");o.referrer=t[0]+e}}const t=getPiiSafeQueryString(window.location.search);if(null==t)return o.page_location=window.location.href,o.page_path=window.location.pathname+window.location.search,o;const i=window.location.href.split("?");return o.page_location=i[0]+t,o.page_path=window.location.pathname+t,o}function getPiiSafeQueryString(e){const o=new URLSearchParams(e);let t=o.get("postcode");return null==t?null:(t=t.replace(/[a-zA-Z]+$/,""),o.set("postcode",t),o.delete("latitude"),o.delete("longitude"),"?"+o.toString())}var CONSENT_COOKIE_NAME="service_directory_cookies_policy",COOKIE_CATEGORIES={analytics:["_ga","_ga_"+window.GA_MEASUREMENT_ID],essential:["service_directory_cookies_policy"]},DEFAULT_COOKIE_CONSENT={analytics:!1};function Cookie(e,o,t){if(void 0===o)return getCookie(e);!1===o||null===o?deleteCookie(e):(void 0===t&&(t={days:30}),setCookie(e,o,t))}function getConsentCookie(){var e,o=getCookie(CONSENT_COOKIE_NAME);if(!o)return null;try{e=JSON.parse(o)}catch(e){return null}return e}function isValidConsentCookie(e){return e&&e.version>=window.GDS_CONSENT_COOKIE_VERSION}function setConsentCookie(e){var o=getConsentCookie();for(var t in o||(o=JSON.parse(JSON.stringify(DEFAULT_COOKIE_CONSENT))),e)o[t]=e[t];delete o.essential,o.version=window.GDS_CONSENT_COOKIE_VERSION,setCookie(CONSENT_COOKIE_NAME,JSON.stringify(o),{days:365}),resetCookies()}function resetCookies(){var e=getConsentCookie();for(var o in isValidConsentCookie(e)||(e=JSON.parse(JSON.stringify(DEFAULT_COOKIE_CONSENT))),e){if("version"===o)continue;if("essential"===o)continue;const t="analytics"===o&&e[o];if(gtag("config",window.GA_MEASUREMENT_ID,{send_page_view:t}),t&&loadAnalytics(window.GA_MEASUREMENT_ID),!e[o])COOKIE_CATEGORIES[o].forEach((function(e){Cookie(e,null)}))}}function userAllowsCookieCategory(e,o){if("essential"===e)return!0;try{return o[e]}catch(e){return console.error(e),!1}}function userAllowsCookie(e){if(e===CONSENT_COOKIE_NAME)return!0;var o=getConsentCookie();for(var t in isValidConsentCookie(o)||(o=DEFAULT_COOKIE_CONSENT),COOKIE_CATEGORIES){if("-1"!==COOKIE_CATEGORIES[t].indexOf(e))return userAllowsCookieCategory(t,o)}return!1}function getCookie(e){for(var o=e+"=",t=document.cookie.split(";"),i=0,n=t.length;i<n;i++){for(var r=t[i];" "===r.charAt(0);)r=r.substring(1,r.length);if(0===r.indexOf(o))return decodeURIComponent(r.substring(o.length))}return null}function setCookie(e,o,t){if(userAllowsCookie(e)){void 0===t&&(t={});var i=e+"="+o+"; path=/; SameSite=Strict";if(t.days){var n=new Date;n.setTime(n.getTime()+24*t.days*60*60*1e3),i=i+"; expires="+n.toUTCString()}"https:"===document.location.protocol&&(i+="; Secure"),document.cookie=i}}function deleteCookie(e){Cookie(e)&&(document.cookie=e+"=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/",document.cookie=e+"=;expires=Thu, 01 Jan 1970 00:00:00 GMT;domain="+window.location.hostname+";path=/",document.cookie=e+"=;expires=Thu, 01 Jan 1970 00:00:00 GMT;domain=."+window.location.hostname+";path=/")}function nodeListForEach(e,o){if(window.NodeList.prototype.forEach)return e.forEach(o);for(var t=0;t<e.length;t++)o.call(window,e[t],t,e)}var cookieBannerAcceptSelector=".js-cookie-banner-accept",cookieBannerRejectSelector=".js-cookie-banner-reject",cookieBannerHideButtonSelector=".js-cookie-banner-hide",cookieMessageSelector=".js-cookie-banner-message",cookieConfirmationAcceptSelector=".js-cookie-banner-confirmation-accept",cookieConfirmationRejectSelector=".js-cookie-banner-confirmation-reject";function CookieBanner(e){this.$module=e}function CookiesPage(e){this.$module=e}CookieBanner.prototype.init=function(){if(this.$cookieBanner=this.$module,this.$acceptButton=this.$module.querySelector(cookieBannerAcceptSelector),this.$rejectButton=this.$module.querySelector(cookieBannerRejectSelector),this.$cookieMessage=this.$module.querySelector(cookieMessageSelector),this.$cookieConfirmationAccept=this.$module.querySelector(cookieConfirmationAcceptSelector),this.$cookieConfirmationReject=this.$module.querySelector(cookieConfirmationRejectSelector),this.$cookieBannerHideButtons=this.$module.querySelectorAll(cookieBannerHideButtonSelector),this.$cookieBanner&&!this.onCookiesPage()){var e=getConsentCookie();e&&isValidConsentCookie(e)||(resetCookies(),this.$cookieBanner.removeAttribute("hidden")),this.$acceptButton.addEventListener("click",this.acceptCookies.bind(this)),this.$rejectButton.addEventListener("click",this.rejectCookies.bind(this)),nodeListForEach(this.$cookieBannerHideButtons,function(e){e.addEventListener("click",this.hideBanner.bind(this))}.bind(this))}},CookieBanner.prototype.hideBanner=function(){this.$cookieBanner.setAttribute("hidden",!0)},CookieBanner.prototype.acceptCookies=function(){setConsentCookie({analytics:!0}),this.$cookieMessage.setAttribute("hidden",!0),this.revealConfirmationMessage(this.$cookieConfirmationAccept)},CookieBanner.prototype.rejectCookies=function(){setConsentCookie({analytics:!1}),this.$cookieMessage.setAttribute("hidden",!0),this.revealConfirmationMessage(this.$cookieConfirmationReject)},CookieBanner.prototype.revealConfirmationMessage=function(e){e.removeAttribute("hidden"),e.getAttribute("tabindex")||(e.setAttribute("tabindex","-1"),e.addEventListener("blur",(function(){e.removeAttribute("tabindex")}))),e.focus()},CookieBanner.prototype.onCookiesPage=function(){return"/cookies/"===window.location.pathname},CookiesPage.prototype.init=function(){this.$cookiePage=this.$module,this.$cookiePage&&(this.$cookieForm=this.$cookiePage.querySelector(".js-cookies-page-form"),this.$cookieFormFieldsets=this.$cookieForm.querySelectorAll(".js-cookies-page-form-fieldset"),this.$successNotification=this.$cookiePage.querySelector(".js-cookies-page-success"),this.$successLink=this.$cookiePage.querySelector(".js-cookies-page-success-link"),nodeListForEach(this.$cookieFormFieldsets,function(e){this.showUserPreference(e,getConsentCookie())}.bind(this)),this.$cookieForm.addEventListener("submit",this.savePreferences.bind(this)))},CookiesPage.prototype.savePreferences=function(e){e.preventDefault();var o={};nodeListForEach(this.$cookieFormFieldsets,function(e){var t=this.getCookieType(e),i=e.querySelector("input[name="+t+"]:checked").value;o[t]="true"===i}.bind(this)),setConsentCookie(o),document.querySelector('[data-module="govuk-cookie-banner"]').setAttribute("hidden","true"),this.showSuccessNotification()},CookiesPage.prototype.showUserPreference=function(e,o){var t=this.getCookieType(e),i=!1;t&&o&&void 0!==o[t]&&(i=o[t]);var n=i?"true":"false";e.querySelector("input[name="+t+"][value="+n+"]").checked=!0},CookiesPage.prototype.showSuccessNotification=function(){this.$successNotification.removeAttribute("hidden");var e=!!document.referrer&&new URL(document.referrer).pathname;e&&e!==document.location.pathname?(this.$successLink.href=e,this.$successLink.removeAttribute("hidden")):this.$successLink.setAttribute("hidden","true"),this.$successNotification.getAttribute("tabindex")||this.$successNotification.setAttribute("tabindex","-1"),this.$successNotification.focus(),window.scrollTo(0,0)},CookiesPage.prototype.getCookieType=function(e){return e.id};var $cookieBanner=document.querySelector('[data-module="govuk-cookie-banner"]');new CookieBanner($cookieBanner).init();var userConsent=getConsentCookie();userConsent&&isValidConsentCookie(userConsent)&&userConsent.analytics&&loadAnalytics(window.GA_MEASUREMENT_ID);var $cookiesPage=document.querySelector('[data-module="app-cookies-page"]');new CookiesPage($cookiesPage).init();const backLinks=document.querySelectorAll(".app-back-link");backLinks.forEach((e=>{e.addEventListener("click",(()=>{window.history.back()}))}));const button=document.getElementById("open-close-filters");null==button||button.addEventListener("click",(function(e){const o=document.getElementById("filters");"none"===o.style.display?o.style.display="block":o.style.display="none"}));
//# sourceMappingURL=app.js.map
