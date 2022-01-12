# SOStock
This repository contains a stock trading web application. The website demonstrates buying stocks and investments management. 

![‏‏homescr](https://user-images.githubusercontent.com/83066973/148753245-795f752d-14d6-4266-8a77-516cbc23f854.PNG)

## Running the app
1. Clone this repo

2. Get a private key for each API used in this repo:
   - [Alpha Vantage](https://www.alphavantage.co/support/#api-key)
   - [Twitter](https://developer.twitter.com/en/docs/tutorials/step-by-step-guide-to-making-your-first-request-to-the-twitter-api-v2)
   - [Google Maps](https://developers.google.com/maps/documentation/javascript/overview)
   
3. Open the files in Visual Studio IDE. [Click to download the community version.](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=17)

4. Replace the ``<place-holders>`` with your API key in these places:
    - Alpha Vantage:    
    ``SocksController.cs``
      ``` C#
      33.  const string _AlphaVantageKey="<api-key>";
      ```
    - Twitter:    
    ``TwitterAPIController.cs``
      ``` C#
      13. const string _ConsumerKey = "<consumer-key>";
      14. const string _ConsumerSecret = "<consumer-secret>";
      15. const string _AccessToken = "<access-token>";
      16. const string _AccessTokenSecret = "<access-token-secret>";
      ```
    - Google Maps:    
    ``Index.cshtml``
         ``` HTML
         50. <script async src="https://maps.googleapis.com/maps/api/js?key=<google-api-key>&callback=initMap"></script>
         ```
  

