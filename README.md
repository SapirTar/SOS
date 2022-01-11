# SOStock
This repository contains a stock trading web application. The website demonstrates buying stocks and investments management. 

![‏‏homescr](https://user-images.githubusercontent.com/83066973/148753245-795f752d-14d6-4266-8a77-516cbc23f854.PNG)

## Running the app
1. Clone this repo
2. Get a private key for each API used in this repo:
   - [Alpha Vantage](https://www.alphavantage.co/support/#api-key)
   - [Twitter](https://developer.twitter.com/en/docs/tutorials/step-by-step-guide-to-making-your-first-request-to-the-twitter-api-v2)
   - [Google Maps](https://developers.google.com/maps/documentation/javascript/overview)

3. Replace the ``XXX`` with your API key in these places:
    - Alpha Vantage:    
    ``SocksController.cs``
      ``` C#
      96. string QUERY_URL = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=" + symbol + "&apikey=XXX";
      97. string QUERY_URL2 = "https://www.alphavantage.co/query?function=SYMBOL_SEARCH&keywords=" + symbol + "&apikey=XXX";
      ```
    - Twitter:    
    ``TwitterAPIController.cs``
      ``` C#
      13. public static string _ConsumerKey = "XXX";
      14. public static string _ConsumerSecret = "XXX";
      15. public static string _AccessToken = "XXX";
      16. public static string _AccessTokenSecret = "XXX";
      ```
    - Google Maps:    
    ``Index.cshtml``
      ``` HTML
      50. <script async src="https://maps.googleapis.com/maps/api/js?key=XXX&callback=initMap"></script>
      ```
  

