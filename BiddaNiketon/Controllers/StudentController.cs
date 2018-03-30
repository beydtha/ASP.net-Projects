using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BiddaNiketon.Models;
using BiddaNiketon.DAL;
using PagedList; 

namespace BiddaNiketon.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: /Student/
        public ActionResult Index(string sortOrder,string currentFilter,  string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder; 
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DataSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString; 
            var students = from s in db.Students select s; 
            if(!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(x => x.FirstMidName.Contains(searchString) || x.LastName.Contains(searchString)); 
            }

            switch(sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date" :
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default :
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            
            return View(students.ToPagedList(pageNumber,pageSize));
        }

        // GET: /Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex*/)
            {
                ModelState.AddModelError("", "পরিবর্তনগুলি সংরক্ষণ করতে অক্ষম আবার চেষ্টা করুন, এবং যদি সমস্যাটি বজায় থাকে, তাহলে আপনার সিস্টেম প্রশাসক দেখুন।"); 
            }


            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var studentToUpdate = db.Students.Find(id);
            if(TryUpdateModel(studentToUpdate, "", new string[]{"LastName", "FirstMIdName", "EnrollmentDate"}))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(DataException /* DEX*/)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator."); 
                }
            }
            return View(studentToUpdate);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete Failed. Try again and if the problem persist, talk your system adminitrator"; 
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();
                return RedirectToAction("Index"); 

            }
            catch(DataException /* dex*/)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true});
            }

            return RedirectToAction("Index");       
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
