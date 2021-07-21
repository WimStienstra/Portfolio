using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Text;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PuppeteerSharp;
using Portfolio;

namespace Portfolio.Pages
{
    public class ScraperModel : PageModel
    {
        AppConfiguration _config;
        public void OnGet(AppConfiguration config)
        {
            _config = config;
            scraper();
            //ScrapeWebsite();
        }

        public async void scraper()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            using (var page = await browser.NewPageAsync())
            {
                await page.SetViewportAsync(new ViewPortOptions() { Width = 1280, Height = 1200 });
                await page.GoToAsync("https://www.linkedin.com/");
                await page.SetCookieAsync(new CookieParam
                {
                    Name = "li_at",
                    Value = _config.LinkedInValue,
                });
                await page.GoToAsync("https://www.linkedin.com/in/wim-stienstra-932155145/");

                await page.WaitForSelectorAsync("#experience-section");
                var artist = await page.EvaluateExpressionAsync<string>("document.getElementById('experience-section').children[1].innerText");
                var test = artist.Split("\n");
                test = test.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                Console.WriteLine(artist);
                Console.ReadLine();
            }
        }


        private string Title { get; set; }
        private string Url { get; set; }
        private string siteUrl = "https://www.linkedin.com/in/wim-stienstra-932155145/";
        public string[] QueryTerms { get; } = { "Projecten", "Nature", "Pollution" };

        internal async void ScrapeWebsite()
        {
            Uri baseAddress = new Uri("https://www.linkedin.com");
            var cookieContainer = new CookieContainer();

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookieContainer.Add(baseAddress, new Cookie("li_at", "AQEDASMVV_YAFhqIAAABdsi-AfkAAAF5cERJaU4AKcmMBuBf0vqTX8UcedpGr_zs5n5e0JKbE59QzVLDqBSxAexR-ZxQBuj8kfbBGydt75035UkeQlHWhbiNntpU7m3vgaSKmzKa_z47F0lCfr5pAm_c"));
                var response = await client.GetStringAsync("/in/wim-stienstra-932155145");
                ParseHtml(response);
                //result.EnsureSuccessStatusCode();
            }

        }

        public void ParseHtml(string htmlData)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlData);

            var experiences = htmlDocument.DocumentNode.Descendants("#experience-section");

            foreach (var experience in experiences.ToList()[0].ChildNodes)
            {

            }

        }

        private void GetScrapeResults(IHtmlDocument document)
        {
            IEnumerable<IElement> articleLink = null;
            articleLink = document.All.Where(x =>
                x.ClassName == "education pp-section");
            if (articleLink.Any())
            {
                PrintResults(articleLink);
            }

            //foreach (var term in QueryTerms)
            //{
            //    articleLink = document.All.Where(x =>
            //        x.ClassName == "result-card personal-project" &&
            //        (x.ParentElement.InnerHtml.Contains(term) || x.ParentElement.InnerHtml.Contains(term.ToLower()))).Skip(1);

            //    //Overwriting articleLink above means we have to print it's result for all QueryTerms
            //    //Appending to a pre-declared IEnumerable (like a List), could mean taking this out of the main loop.
            //    if (articleLink.Any())
            //    {
            //        PrintResults(articleLink);
            //    }
            //}
        }

        public void PrintResults(IEnumerable<IElement> articleLink)
        {
            var test = "";
            //Every element needs to be cleaned and displayed
            foreach (var element in articleLink)
            {
                CleanUpResults(element);
                test += $"{Title} - {Url}{Environment.NewLine}";
                //test.AppendText($"{Title} - {Url}{Environment.NewLine}");
            }
        }

        private void CleanUpResults(IElement result)
        {
            string htmlResult = result.InnerHtml.ReplaceFirst("        <span class=\"field-content\"><div><a href=\"", @"https://www.oceannetworks.ca");
            htmlResult = htmlResult.ReplaceFirst("\">", "*");
            htmlResult = htmlResult.ReplaceFirst("</a></div>\n<div class=\"article-title-top\">", "-");
            htmlResult = htmlResult.ReplaceFirst("</div>\n<hr></span>  ", "");

            //Seperate the results into our class fields for use in PrintResults()
            SplitResults(htmlResult);
        }

        private void SplitResults(string htmlResult)
        {
            string[] splitResults = htmlResult.Split('*');
            Url = splitResults[0];
            Title = splitResults[1];
        }
    }
}
