using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class AdminController : Controller
    {
        private InsuranceDbContext dbContext;
        public AdminController()
        {
            dbContext = new InsuranceDbContext(); //initialize your DbContext
        }
        // GET: Admin
        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult GetAllCustomers()
        {
            var customers = dbContext.Customers.ToList();
            return View(customers);
        }

        //Action method to get all users
        public ActionResult GetAllUsers()
        {
            var users = dbContext.Customers.ToList(); //assuming users are stored in the same tables as customers
            return View(users);
        }

        public ActionResult PoliciesList()
        {
            var policies = dbContext.Policies.ToList();
            return View(policies);
        }

        public ActionResult Categories()
        {
            var categories = dbContext.Categories.ToList();
            return View(categories);
        }

        public ActionResult AllAppliedPolicies()
        {
            var appliedPolicies = dbContext.AppliedPolicies.ToList();
            return View(appliedPolicies);
        }


        [HttpPost]
        public ActionResult ApprovePolicy(int policyId)
        {
            var policy = dbContext.AppliedPolicies.Find(policyId);
            if(policy!=null && policy.StatusCode == PolicyStatus.Pending)
            {
                policy.StatusCode = PolicyStatus.Approved;
                dbContext.SaveChanges();
            }
            return RedirectToAction("AllAppliedPolicies");
        }

        public ActionResult DisapprovePolicy(int policyId)
        {
            var policy = dbContext.AppliedPolicies.Find(policyId);
            if(policy!=null && policy.StatusCode == PolicyStatus.Pending)
            {
                policy.StatusCode = PolicyStatus.Disapproved;
                dbContext.SaveChanges();
            }
            return RedirectToAction("AllAppliedPolicies");
        }

        public ActionResult ApprovedPolicies()
        {
            var approvedPolicies = dbContext.AppliedPolicies.Where(p => p.StatusCode == PolicyStatus.Approved).ToList();
            return View(approvedPolicies);
        }

        public ActionResult DisapprovedPolicies()
        {
            var disapprovedPolicies = dbContext.AppliedPolicies.Where(p => p.StatusCode == PolicyStatus.Disapproved).ToList();
            return View(disapprovedPolicies);
        }

        public ActionResult PendingPolicies()
        {
            var pendingPolicies = dbContext.AppliedPolicies.Where(p => p.StatusCode == PolicyStatus.Pending).ToList();
            return View(pendingPolicies);
        }

        public ActionResult Policy()
        {
            return View();
        }

        //YourController.cs

        public ActionResult Question()
        {
            var questions = dbContext.Questions.ToList();
            return View(questions); 
        }

        public ActionResult Reply(int id) 
        {
            var question = dbContext.Questions.Find(id);
            return View(question);
        }

        [HttpPost]
        public ActionResult Reply(Questions model)
        {
            if(ModelState.IsValid)
            {
                var existingQuestion = dbContext.Questions.Find(model.QuestionId);
                if(existingQuestion != null)
                {
                    existingQuestion.Answer = model.Answer;
                    dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            //if ModelState is not valid or if the question doesn't exist, return to the reply view
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveAnswer(int questionId, string answer)
        {
            var existingQuestion = dbContext.Questions.Find(questionId);
            if(existingQuestion != null)
            {
                existingQuestion.Answer = answer;
                dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, error = "Question not found" });
        }

    }
}