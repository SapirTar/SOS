using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TweetSharp;

namespace WebApp.Controllers
{
    public class TwitterAPIController : Controller
    {
       
        public static string _ConsumerKey = "<consumer-key>";
        public static string _ConsumerSecret = "<consumer-secret>";
        public static string _AccessToken = "<access-token>";
        public static string _AccessTokenSecret = "<access-token-secret>";

        protected TwitterSearchResult GetTwitterSearchResult()
        {

            var tweets_search = new SearchOptions { Q = "#stocks", Resulttype = TwitterSearchResultType.Popular };
            TwitterService twService = new TwitterService(_ConsumerKey, _ConsumerSecret, _AccessToken, _AccessTokenSecret);
            try
            { 
                TwitterSearchResult searchResult = twService.Search(tweets_search);
                return searchResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult Index()
        {
            var statuses = GetTwitterSearchResult();
            if (statuses != null)
            { 
            List<TwitterStatus> tweetList = statuses.Statuses.ToList();
            return View(tweetList);
            }
            return View("TwitterError");
        }

    }
}
