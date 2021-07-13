using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using ConversorMoedas.Data;
using ConversorMoedas.Models;
using Newtonsoft.Json;

namespace ConversorMoedas.Controllers
{
    public class TransactionsController : Controller
    {
        private ConversorMoedasContext db = new ConversorMoedasContext();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private string Baseurl = "http://api.exchangeratesapi.io/v1";

        // GET: Transactions
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.ListCoin = ListCoin();
            return View();
        }

        // POST: Transactions/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTransaction,IdUser,OriginCoin,OriginValue,DestinyCoin,ConversionRate")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    APITransaction apiTransaction = new APITransaction();
                    using (var client = new HttpClient())
                    {
                        string param = string.Format("latest?access_key={0}&base={1}&symbols={2}", transaction.IdUser, transaction.OriginCoin, transaction.DestinyCoin);
                        // @"latest?access_key=a18073edda514b096af23d9bf8f6c228&base=EUR&symbols=BRL"; 
                        //Passing service base url
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Clear();
                        //Define request data format
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //Sending request to find web api REST service resource using HttpClient
                        var responseTask = client.GetAsync(param);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        //Checking the response is successful or not which is sent using HttpClient 
                        if (result.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            var TransResponse = result.Content.ReadAsStringAsync().Result;
                            //Deserializing the response recieved from web api and storing into the Employee list
                            apiTransaction = JsonConvert.DeserializeObject<APITransaction>(TransResponse);

                            if (apiTransaction.Success == true)
                            {
                                decimal rate = Convert.ToDecimal(apiTransaction.Rates[transaction.DestinyCoin].ToString());

                                transaction.ConversionRate = rate;
                                transaction.DateTimeUTC = DateTime.Now;
                                db.Transactions.Add(transaction);
                                db.SaveChanges();

                                return RedirectToAction("Details", new { id = transaction.IdTransaction });
                            }
                            else
                            {
                                APITransactionError apiTransactionError = JsonConvert.DeserializeObject<APITransactionError>(TransResponse);
                                TempData["Error"] = "Error: " + apiTransactionError.Error.code+ "  -  " +   apiTransactionError.Error.type; 
                                Logger.Info(TempData["Error"].ToString(), "Error API Controller: Create");
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Error: " + result.StatusCode + "  -  " +  result.RequestMessage;
                            Logger.Info(TempData["Error"].ToString(), "Error API Controller: Create");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Erro Controller: Create");
                }


                return RedirectToAction("Create");
            }

            return View(transaction);
        }

        

        // POST: Transactions/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTransaction,IdUser,OriginCoin,OriginValue,DestinyCoin,ConversionRate,DateTimeUTC")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

          

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Enum Coins list
        /// </summary>
        public enum Coins { BRL, EUR, JPY, USD }

        /// <summary>
        /// Bind list coins
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListCoin()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "", Value = "" });

            var values = Enum.GetValues(typeof(Coins));
            foreach (var r in values)
            {
                list.Add(new SelectListItem { Text = r.ToString(), Value = r.ToString() });
            }

            return list;
        }
    }
}
