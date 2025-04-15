using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project1;
using Project1.Models;

namespace Project1.Controllers
{
    public class TasksController : Controller
    {
        private readonly Context _context;

        public TasksController(Context context)
        {
            _context = context;
        }

        // Original 
        //GET: Tasks
        public async Task<IActionResult> TaskIndex()
        {
            var context = _context.Tasks.Include(t => t.EIDNavigation);
            return View(await context.ToListAsync());
        }

        // GET: Tasks
        //public async Task<IActionResult> Task_Index()
        //{
        //    var tasks = await _context.Task_.ToListAsync();
        //    return View(tasks);
        //}

        // GET: Tasks/Details/5
        public async Task<IActionResult> TaskDetails(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks
                .Include(t => t.EIDNavigation)
                .FirstOrDefaultAsync(m => m.TID == id);
            if (tasks == null)
            {
                return NotFound();
            }

            return View(tasks);
        }

        // GET: Tasks/Create


        /*
         working but trying the new one
         public IActionResult CreateTask()
        {
            ViewData["EID"] = new SelectList(_context.Employees, "EID", "EID");
            return View();
        }
        */

        //added
        public IActionResult CreateTask()
        {
            ViewBag.EID = new SelectList(_context.Employees, "EID", "EID");
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([Bind("TID,TaskName,TaskStatus,Comments,StartDate,EndDate,Deadline,AssignedDate,TaskPriority,EID")] Models.Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TaskIndex));
            }
            ViewData["EID"] = new SelectList(_context.Employees, "EID", "EID", tasks.EID);
            return View(tasks);



        }

        // GET: Tasks/Edit/5
       
        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        /*
        public async Task<IActionResult> EditTask(int id)
        {
            var eids = GetEmployeeIDs();

            ViewBag.eids = eids;
            var tasks = await _context.Tasks.FindAsync(id);


            if (id != tasks.EID)
            {
                return NotFound();
            }


            //ViewData["MID"] = new SelectList(_context.Employees, "EID", "EID", employee.MID);
            return View(tasks);

        }
        private List<int> GetEmployeeIDs()
        {
            // Retrieve the list of employee IDs from the database.
            return _context.Employees.Select(e => e.EID).ToList();
        }
        */

        public async Task<IActionResult> EditTask(int id)
        {
            var tids = GetTaskIDs();

            ViewBag.tids = tids;
            var tasks = await _context.Tasks.FindAsync(id);


            if (id != tasks.TID)
            {
                return NotFound();
            }


            //ViewData["MID"] = new SelectList(_context.Employees, "EID", "EID", employee.MID);
            return View(tasks);

        }
        private List<int> GetTaskIDs()
        {
            // Retrieve the list of employee IDs from the database.
            return _context.Tasks.Select(t => t.TID).ToList();
        }













        public async Task<IActionResult> Update(Tasks t)
        {
            var tids = GetTaskIDs();

            ViewBag.tids = tids;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(t);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(t.TID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TaskIndex));
            }
            //ViewData["EID"] = new SelectList(_context.Employees, "EID", "EID", t.EID);
            return View(t);
        }













        // GET: Tasks/Delete/5
     
        
        public async Task<IActionResult> DeleteTask(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .Include(t => t.EIDNavigation)
                .FirstOrDefaultAsync(m => m.TID == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'Context.Tasks'  is null.");
            }
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TaskIndex));
        }
     


















        private bool TaskExists(int id)
        {
            return (_context.Tasks?.Any(e => e.TID == id)).GetValueOrDefault();
        }
    }
}
