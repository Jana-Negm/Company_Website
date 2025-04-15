using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly Context _context;

        public EmployeesController(Context context)
        {
            _context = context;
        }

        // GET: Employees
        public IActionResult Index()
        {

            return View();



        }


        //public IActionResult Details()

        //{
        //    return View();
        //}
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'Context.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeeIndex");
        }
        public async Task<IActionResult> DeleteEmployee()
        {
            var eids = GetEmployeeIDs();
            ViewBag.eids = eids;



            await _context.SaveChangesAsync();
            return View();


        }

        public IActionResult Select()
        {
            return View();
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details()
        {
            var emp = HttpContext.Session.GetInt32("EID");
            Employee emp1 = await _context.Employees.FindAsync(emp);
            
            //var employee = await _context.Employees.FindAsync();
            //if (employee == null)
            //{
            //    return NotFound();
            //}

            return View(emp1);
        }



        // GET: Employees/Create

        public async Task<IActionResult> EmployeeIndex()
        {
            var Context = new Context();
            var employees = await Context.Employees.Where(e => e.EID != null).ToListAsync();
            return View(employees);
        }
        public ActionResult CreateEmployee()
        {
            var eids = GetEmployeeIDs();

            ViewBag.eids = eids;

            return View();
        }

        private List<int> GetEmployeeIDs()
        {
            // Retrieve the list of employee IDs from the database.
            return _context.Employees.Select(e => e.EID).ToList();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EID,UserName,UserPassword,MID,FirstName,LastName")] Employee employee)
        {
            employee.Administrator = false;
            _context.Add(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeeIndex");

            //ViewData["MID"] = new SelectList(_context.Employees, "EID", "EID", employee.MID);
            //return View(employee);
        }

        //// GET: Employees/Edit/5
        //public async Task<IActionResult> Edit1(int? id)
        //{
        //    var eids = GetEmployeeIDs();

        //    ViewBag.eids = eids;
        //    if (id == null || _context.Employees == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees.FindAsync(id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }
        //    //ViewData["MID"] = new SelectList(_context.Employees, "EID", "EID", employee.MID);
        //    return View();
        //}
        public async Task<IActionResult> Edit(Employee em)
        {
            //var employee = await _context.Employees.FindAsync(id);
            em.Administrator = false;
            if (ModelState.IsValid)
            {
                try
                {
                    
                    //employee.Administrator = false;
                    _context.Update(em);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(em.EID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(EmployeeIndex));
            }
             return View();
        }

        //// POST: Employees/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit1(int id, [Bind("EID,UserName,UserPassword,FirstName,LastName,MID")] Employee employee)
        //{
        //    var eids = GetEmployeeIDs();

        //    ViewBag.eids = eids;
        //    if (id != employee.EID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            employee.Administrator = false;
        //            _context.Update(employee);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeeExists(employee.EID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MID"] = new SelectList(_context.Employees, "EID", "EID", employee.MID);
        //    return View(employee);
        //}




        // GET: Employees/Delete/5
        //public async Task<IActionResult> DeleteEmployee(int? id)
        //{
        //    if (id == null || _context.Employees == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _context.Employees
        //        .Include(e => e.MIDNavigation)
        //        .FirstOrDefaultAsync(m => m.EID == id);
        //    if (employee == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(employee);
        //}







        // POST: Employees/Delete/5




        public async Task<IActionResult> Edit1(int id)
        {
            var eids = GetEmployeeIDs();

            ViewBag.eids = eids;
            var employee = await _context.Employees.FindAsync(id);

           
            if (id != employee.EID)
            {
                return NotFound();
            }

           
            //ViewData["MID"] = new SelectList(_context.Employees, "EID", "EID", employee.MID);
            return View(employee);

        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        public IActionResult DeleteConfirmed()
        {
            return View();
        }




        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EID == id)).GetValueOrDefault();
        }


        public IActionResult SessionCon()
        {
            var lol = HttpContext.Session.GetInt32("EID");
            return View(lol);
        }

    }
}
