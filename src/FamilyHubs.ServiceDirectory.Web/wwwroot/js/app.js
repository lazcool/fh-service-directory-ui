// js components have been snaffled from https://github.com/alphagov/govuk-design-system/blob/main/src/javascripts/components/
import CookieBanner from './components/cookie-banner.js';
import { getConsentCookie, isValidConsentCookie } from './components/cookie-functions.js';
import Analytics from './components/analytics.js';
import CookiesPage from './components/cookies-page.js';
// Initialise cookie banner
var $cookieBanner = document.querySelector('[data-module="govuk-cookie-banner"]');
new CookieBanner($cookieBanner).init();
// Initialise analytics if consent is given
var userConsent = getConsentCookie();
if (userConsent && isValidConsentCookie(userConsent) && userConsent.analytics) {
    Analytics();
}
//todo: move this into scripts section on cookie page
// Initialise cookie page
var $cookiesPage = document.querySelector('[data-module="app-cookies-page"]');
new CookiesPage($cookiesPage).init();
const backLinks = document.querySelectorAll(".app-back-link");
backLinks.forEach((link) => {
    link.addEventListener("click", () => {
        window.history.back();
    });
});
const button = document.getElementById('open-close-filters');
button === null || button === void 0 ? void 0 : button.addEventListener('click', function handleClick(event) {
    //todo: update to ts 2?
    const filterButton = document.getElementById("filters");
    if (filterButton.style.display === "none") {
        filterButton.style.display = "block";
    }
    else {
        filterButton.style.display = "none";
    }
});

//# sourceMappingURL=data:application/json;charset=utf8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbImFwcC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSw4SEFBOEg7QUFROUgsT0FBTyxZQUFZLE1BQU0sK0JBQStCLENBQUE7QUFDeEQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLG9CQUFvQixFQUFFLE1BQU0sa0NBQWtDLENBQUE7QUFDekYsT0FBTyxTQUFTLE1BQU0sMkJBQTJCLENBQUE7QUFDakQsT0FBTyxXQUFXLE1BQU0sOEJBQThCLENBQUE7QUFFdEQsMkJBQTJCO0FBQzNCLElBQUksYUFBYSxHQUFHLFFBQVEsQ0FBQyxhQUFhLENBQUMscUNBQXFDLENBQUMsQ0FBQTtBQUNqRixJQUFJLFlBQVksQ0FBQyxhQUFhLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQTtBQUV0QywyQ0FBMkM7QUFDM0MsSUFBSSxXQUFXLEdBQUcsZ0JBQWdCLEVBQUUsQ0FBQTtBQUNwQyxJQUFJLFdBQVcsSUFBSSxvQkFBb0IsQ0FBQyxXQUFXLENBQUMsSUFBSSxXQUFXLENBQUMsU0FBUyxFQUFFO0lBQzNFLFNBQVMsRUFBRSxDQUFBO0NBQ2Q7QUFFRCxxREFBcUQ7QUFDckQseUJBQXlCO0FBQ3pCLElBQUksWUFBWSxHQUFHLFFBQVEsQ0FBQyxhQUFhLENBQUMsa0NBQWtDLENBQUMsQ0FBQTtBQUM3RSxJQUFJLFdBQVcsQ0FBQyxZQUFZLENBQUMsQ0FBQyxJQUFJLEVBQUUsQ0FBQTtBQUVwQyxNQUFNLFNBQVMsR0FBRyxRQUFRLENBQUMsZ0JBQWdCLENBQUMsZ0JBQWdCLENBQUMsQ0FBQztBQUM5RCxTQUFTLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBdUIsRUFBRSxFQUFFO0lBQzFDLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLEVBQUUsR0FBRyxFQUFFO1FBQ2hDLE1BQU0sQ0FBQyxPQUFPLENBQUMsSUFBSSxFQUFFLENBQUM7SUFDMUIsQ0FBQyxDQUFDLENBQUM7QUFDUCxDQUFDLENBQUMsQ0FBQztBQUVILE1BQU0sTUFBTSxHQUFHLFFBQVEsQ0FBQyxjQUFjLENBQUMsb0JBQW9CLENBQUMsQ0FBQztBQUM3RCxNQUFNLGFBQU4sTUFBTSx1QkFBTixNQUFNLENBQUUsZ0JBQWdCLENBQUMsT0FBTyxFQUFFLFNBQVMsV0FBVyxDQUFDLEtBQUs7SUFDeEQsdUJBQXVCO0lBQ3ZCLE1BQU0sWUFBWSxHQUFHLFFBQVEsQ0FBQyxjQUFjLENBQUMsU0FBUyxDQUEwQixDQUFDO0lBQ2pGLElBQUksWUFBWSxDQUFDLEtBQUssQ0FBQyxPQUFPLEtBQUssTUFBTSxFQUFFO1FBQ3ZDLFlBQVksQ0FBQyxLQUFLLENBQUMsT0FBTyxHQUFHLE9BQU8sQ0FBQztLQUN4QztTQUFNO1FBQ0gsWUFBWSxDQUFDLEtBQUssQ0FBQyxPQUFPLEdBQUcsTUFBTSxDQUFDO0tBQ3ZDO0FBQ0wsQ0FBQyxDQUFDLENBQUMiLCJmaWxlIjoiYXBwLmpzIiwic291cmNlc0NvbnRlbnQiOlsiLy8ganMgY29tcG9uZW50cyBoYXZlIGJlZW4gc25hZmZsZWQgZnJvbSBodHRwczovL2dpdGh1Yi5jb20vYWxwaGFnb3YvZ292dWstZGVzaWduLXN5c3RlbS9ibG9iL21haW4vc3JjL2phdmFzY3JpcHRzL2NvbXBvbmVudHMvXHJcblxyXG5kZWNsYXJlIGdsb2JhbCB7XHJcbiAgICBpbnRlcmZhY2UgV2luZG93IHtcclxuICAgICAgICBHRFNfQ09OU0VOVF9DT09LSUVfVkVSU0lPTjogYW55O1xyXG4gICAgfVxyXG59XHJcblxyXG5pbXBvcnQgQ29va2llQmFubmVyIGZyb20gJy4vY29tcG9uZW50cy9jb29raWUtYmFubmVyLmpzJ1xyXG5pbXBvcnQgeyBnZXRDb25zZW50Q29va2llLCBpc1ZhbGlkQ29uc2VudENvb2tpZSB9IGZyb20gJy4vY29tcG9uZW50cy9jb29raWUtZnVuY3Rpb25zLmpzJ1xyXG5pbXBvcnQgQW5hbHl0aWNzIGZyb20gJy4vY29tcG9uZW50cy9hbmFseXRpY3MuanMnXHJcbmltcG9ydCBDb29raWVzUGFnZSBmcm9tICcuL2NvbXBvbmVudHMvY29va2llcy1wYWdlLmpzJ1xyXG5cclxuLy8gSW5pdGlhbGlzZSBjb29raWUgYmFubmVyXHJcbnZhciAkY29va2llQmFubmVyID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignW2RhdGEtbW9kdWxlPVwiZ292dWstY29va2llLWJhbm5lclwiXScpXHJcbm5ldyBDb29raWVCYW5uZXIoJGNvb2tpZUJhbm5lcikuaW5pdCgpXHJcblxyXG4vLyBJbml0aWFsaXNlIGFuYWx5dGljcyBpZiBjb25zZW50IGlzIGdpdmVuXHJcbnZhciB1c2VyQ29uc2VudCA9IGdldENvbnNlbnRDb29raWUoKVxyXG5pZiAodXNlckNvbnNlbnQgJiYgaXNWYWxpZENvbnNlbnRDb29raWUodXNlckNvbnNlbnQpICYmIHVzZXJDb25zZW50LmFuYWx5dGljcykge1xyXG4gICAgQW5hbHl0aWNzKClcclxufVxyXG5cclxuLy90b2RvOiBtb3ZlIHRoaXMgaW50byBzY3JpcHRzIHNlY3Rpb24gb24gY29va2llIHBhZ2VcclxuLy8gSW5pdGlhbGlzZSBjb29raWUgcGFnZVxyXG52YXIgJGNvb2tpZXNQYWdlID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvcignW2RhdGEtbW9kdWxlPVwiYXBwLWNvb2tpZXMtcGFnZVwiXScpXHJcbm5ldyBDb29raWVzUGFnZSgkY29va2llc1BhZ2UpLmluaXQoKVxyXG5cclxuY29uc3QgYmFja0xpbmtzID0gZG9jdW1lbnQucXVlcnlTZWxlY3RvckFsbChcIi5hcHAtYmFjay1saW5rXCIpO1xyXG5iYWNrTGlua3MuZm9yRWFjaCgobGluazogSFRNTEFuY2hvckVsZW1lbnQpID0+IHtcclxuICAgIGxpbmsuYWRkRXZlbnRMaXN0ZW5lcihcImNsaWNrXCIsICgpID0+IHtcclxuICAgICAgICB3aW5kb3cuaGlzdG9yeS5iYWNrKCk7XHJcbiAgICB9KTtcclxufSk7XHJcblxyXG5jb25zdCBidXR0b24gPSBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnb3Blbi1jbG9zZS1maWx0ZXJzJyk7XHJcbmJ1dHRvbj8uYWRkRXZlbnRMaXN0ZW5lcignY2xpY2snLCBmdW5jdGlvbiBoYW5kbGVDbGljayhldmVudCkge1xyXG4gICAgLy90b2RvOiB1cGRhdGUgdG8gdHMgMj9cclxuICAgIGNvbnN0IGZpbHRlckJ1dHRvbiA9IGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKFwiZmlsdGVyc1wiKSBhcyBIVE1MRGl2RWxlbWVudCB8IG51bGw7XHJcbiAgICBpZiAoZmlsdGVyQnV0dG9uLnN0eWxlLmRpc3BsYXkgPT09IFwibm9uZVwiKSB7XHJcbiAgICAgICAgZmlsdGVyQnV0dG9uLnN0eWxlLmRpc3BsYXkgPSBcImJsb2NrXCI7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICAgIGZpbHRlckJ1dHRvbi5zdHlsZS5kaXNwbGF5ID0gXCJub25lXCI7XHJcbiAgICB9XHJcbn0pO1xyXG4iXX0=
