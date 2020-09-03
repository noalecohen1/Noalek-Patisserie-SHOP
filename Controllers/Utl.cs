using Patisserie.Data;
using Patisserie.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Patisserie.Controllers
{
    public static class Utl
    {
        private static PatisserieContext db = new PatisserieContext();
        private const int AMOUNT_OF_PREFERENCES = 3;

        public struct ColorRGB
        {
            public byte R;
            public byte G;
            public byte B;

            public ColorRGB(Color value)
            {
                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }

            public static implicit operator Color(ColorRGB rgb)
            {
                Color c = Color.FromArgb(rgb.R, rgb.G, rgb.B);
                return c;
            }

            public static explicit operator ColorRGB(Color c)
            {
                return new ColorRGB(c);
            }

            public static string ByteToHexString(byte B)
            {
                StringBuilder Result = new StringBuilder(2);
                string HexAlphabet = "0123456789ABCDEF";

                Result.Append(HexAlphabet[(int)(B >> 4)]);
                Result.Append(HexAlphabet[(int)(B & 0xF)]);

                return Result.ToString();
            }

            public override string ToString()
            {
                return ByteToHexString(this.R) + ByteToHexString(this.G) + ByteToHexString(this.B);
            }
        }

        public static bool IsAdmin(HttpSessionStateBase Session)
        {
            if (Session["accountID"] == null) return false;
            
            int accountID = (int)Session["accountID"];
            Account account = db.Accounts.FirstOrDefault(a => a.AccountID == accountID);
            return account.IsModerator;
        }

        public static bool IsLoggedIn(HttpSessionStateBase Session)
        {
            return (Session["accountID"] != null);
        }

        public static List<Product> PopulateProducts(List<int> IDs)
        {
            List<Product> result = new List<Product>();

            foreach(int id in IDs)
            {
                Product p = db.Products.Find(id);
                result.Add(p);
            }

            return result;
        }

        public static List<int> Preferences(int? accountID, int productID)
        {
            List<int> result = new List<int>();

            if (accountID != null && accountID != -1)
            {
                try
                {
                    var ohQueryable = db.OrdersHistories.Where(o => o.AccountID == accountID && o.ProductID != productID);

                    var sortedProducts = CountOrders(ohQueryable).OrderByDescending(key => key.Value)
                        .ToDictionary(pair => pair.Key, pair => pair.Value).Select(p => p.Key).ToList();

                    AddMaxFourProducts(result, sortedProducts, productID);
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Bad DB query");
                }
            }

            if (result.Count < AMOUNT_OF_PREFERENCES)
            {
                try
                {
                    IQueryable<Product> productsByPopularityQuery = db.Products
                        .OrderByDescending(p => p.PopularityRate);
                    //db.Products.OrderByDescending(p => p.PopularityRate)
                        //.Select(p => p.ProductID);
                    var productsByPopularity = CastToList(productsByPopularityQuery.Select(p => p.ProductID)); //Changed to p=>p.ProductID instead of p=>p.PopolarityRate
                    AddMaxFourProducts(result, productsByPopularity, productID);
                }
                catch(InvalidOperationException)
                {
                    Console.WriteLine("Bad DB query");
                }
            }

            return result;
        }

        private static Dictionary<int, int> CountOrders(IQueryable<OrdersHistory> ohQueryable)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            var oh = ohQueryable.ToList();
            var ordersGroupByProduct = oh.GroupBy(p => p.ProductID);

            foreach (var productGroup in ordersGroupByProduct)
            {
                result.Add(productGroup.ToList()[0].ProductID, 0);
                foreach (var p in productGroup)
                {
                    result[p.ProductID] += p.Amount;
                }
            }

            return result;
        }

        private static void AddMaxFourProducts(List<int> ids, IList<int> productsList, int productID)
        {
            foreach (var p in productsList)
            {
                if (ids.Count >= AMOUNT_OF_PREFERENCES)
                    break;

                if (!ids.Contains(p) && p != productID)
                    ids.Add(p);
            }
        }

        public static List<object> CreateCart(HttpSessionStateBase Session, List<Order> accountOrders)
        {
            List<object> cart = new List<object>();
            if (Session == null || Session["accountID"] == null) return cart;

            // { id: 1, name: 'product', description: 'description', price: 10, amount: 1, image: 'images/img_1.png' },
            foreach (var order in accountOrders)
            {
                Product product = db.Products.Where(p => p.ProductID == order.ProductID).ToList()[0];

                cart.Add(new {
                    id = order.ProductID,
                    name = product.Name,
                    description = product.Description,
                    price = product.Price,
                    amount = order.Amount,
                    image = product.ImagePath
                });
            }

            return cart;
        }

        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            ColorRGB rgb;

            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);

            return rgb;
        }
        public static MvcHtmlString AntiForgeryTokenForAjaxPost(this HtmlHelper helper)
        {
            var antiForgeryInputTag = helper.AntiForgeryToken().ToString();
            // Above gets the following: <input name="__RequestVerificationToken" type="hidden" value="PnQE7R0MIBBAzC7SqtVvwrJpGbRvPgzWHo5dSyoSaZoabRjf9pCyzjujYBU_qKDJmwIOiPRDwBV1TNVdXFVgzAvN9_l2yt9-nf4Owif0qIDz7WRAmydVPIm6_pmJAI--wvvFQO7g0VvoFArFtAR2v6Ch1wmXCZ89v0-lNOGZLZc1" />
            var removedStart = antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            var tokenValue = removedStart.Replace(@""" />", "");
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue)
                throw new InvalidOperationException("Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            return new MvcHtmlString(tokenValue);
        }

        public static IList<T> CastToList<T>(IQueryable<T> source)
        {
            return new List<T>(source.Cast<T>());
        }

        public static string GetAbsoluteUrlofImg(string fileName)
        {
            string relativeUrl = "~/Media/Images/" + fileName;
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}