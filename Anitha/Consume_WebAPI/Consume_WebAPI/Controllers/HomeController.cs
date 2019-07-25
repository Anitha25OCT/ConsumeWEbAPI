using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Consume_WebAPI.Models;
using System.Net.Http.Formatting;
using System.Net.Http;
namespace Consume_WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult GetMembers()
        {
           
            IEnumerable<MemberViewModel> members = null;
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("http://192.168.1.97:90/api/member");

                var resposeTask = client.GetAsync("member");
                resposeTask.Wait();
                var result = resposeTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<MemberViewModel>>();
                    readTask.Wait();
                    members = readTask.Result;
                }
                else
                {
                    //Error response received
                    members = Enumerable.Empty<MemberViewModel>();
                    ModelState.AddModelError(String.Empty, "Server error try after some time.");

                }
            }
                   
             

                return View(members);
               
            }
       [HttpPost]
        public ActionResult create(MemberViewModel member)
        {
          
            using (var client = new HttpClient())
            {
                
                const string baseUri = "http://192.168.1.97:90/api/member";
                client.BaseAddress = new Uri(baseUri);

            //    //HTTP POST
            //    //var postTask = client.PostAsJsonAsync<MemberViewModel>("student", member);
                var response=client.PostAsJsonAsync(baseUri,member).Result;
                //postTask.Wait();

            //    var result = postTask.Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetMembers");
                } else
                    Console.Write("Error");
            }

           ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(member);
        }
        //The PUT method
        [HttpPost]
       public ActionResult Edit(MemberViewModel member)
       {
           using (var client = new HttpClient())
           {
              
               const string baseuri = "http://192.168.1.97:90/api/member";
               client.BaseAddress = new Uri(baseuri);
               var response = client.PutAsJsonAsync<MemberViewModel>(baseuri + "/" + member.MemberID, member);
             
               response.Wait();
               var result = response.Result;
               if (result.IsSuccessStatusCode)
               {
                   return RedirectToAction("GetMembers");
               }
               else
                   Console.Write("Error");
           }
           return View(member);
       }

      
           
  [HttpGet]
        public ActionResult create()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
  [HttpGet]
  public ActionResult Edit(int id)
  {
      ViewBag.Message = "Your Edit Page";
       MemberViewModel student = null;
      //Session["ID"]= id;
     // return RedirectToAction("Edit", "Home");
          using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("http://192.168.1.97:90/api/member");
            //HTTP GET
            var responseTask = client.GetAsync("member?id=" + id.ToString());
            responseTask.Wait();

            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<MemberViewModel>();
                readTask.Wait();

                student = readTask.Result;
            }
        }

        return View(student);
    }
 
    [HttpGet]
  public ActionResult Delete(int id)
  {
      ViewBag.Message = "Your Delete page.";
      MemberViewModel member = null;
      using (var client = new HttpClient())
      {
          client.BaseAddress = new Uri("http://192.168.1.97:90/api/member");
          var responsetask = client.GetAsync("member?id=" + id.ToString());
          responsetask.Wait();
          var result = responsetask.Result;
          if (result.IsSuccessStatusCode)
          {
              var readtask = result.Content.ReadAsAsync<MemberViewModel>();
              readtask.Wait();
              member = readtask.Result;
          }
      }
  
      return View(member);
  }
        [HttpPost]
    public ActionResult Delete(MemberViewModel member)
    {
        using (var client = new HttpClient())
        {

            const string baseuri = "http://192.168.1.97:90/api/member";
            client.BaseAddress = new Uri(baseuri);
            var response = client.DeleteAsync(baseuri+"/"+member.MemberID);

            response.Wait();
            var result = response.Result;
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("GetMembers");
            }
            else
                Console.Write("Error");
        }
        return View();
    }
    }
}