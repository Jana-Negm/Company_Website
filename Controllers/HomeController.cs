using Microsoft.AspNetCore.Mvc;
using Project1.Models;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Security.Cryptography;
using System.Net;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;


namespace Project1.Controllers;
public class HomeController : Controller
{
    private readonly Context _context;



    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        //_context = context;

    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CreateEmplpoyee(Employee model)
    {
        return View("CreateEmployee","Employee");
    }


    public async Task<IActionResult> Logged(Employee model)
    {
        // Starting a session
        //var session = HttpContext.Session;

        // Changing the variables to string
            var userName = model.UserName.ToString();
            var userPassword = model.UserPassword.ToString();

        //UserRole variable to use in the layout shared page 
        string UserRole;

        // A way for the program to read the data from the user and store it in the session
        var bytesusername = Encoding.UTF8.GetBytes(userName);
        var bytespassword = Encoding.UTF8.GetBytes(userPassword);



        HttpContext.Session.Set("UserName", bytesusername);
        HttpContext.Session.Set("UserPassword", bytespassword);



        // Connecting to the database
        var Context = new Context();

        // Searching through the database for a match
        var user = Context.Employees.FirstOrDefault(u => u.UserName == userName);
        var upass = Context.Employees.FirstOrDefault(u => u.UserPassword == userPassword);

       if (user != null && user.UserName == userName && upass != null && upass.UserPassword == userPassword)
        {
            // Find the employee in the database with the specified username
            var employee = Context.Employees.FirstOrDefault(e => e.UserName == userName);
            var EID = employee.EID;

            var FirstName = employee.FirstName;
            var LastName = employee.LastName;
            var Administrator = employee.Administrator;
            var MID = employee.MID;


            // If the employee is found, get their EID
            if (employee != null)
            {
                if (employee.MID == null && employee.Administrator == false)
                {

                    // Set the EID in the session
                    HttpContext.Session.SetInt32("EID", EID);

                    // Redirect to the tasks page for the employee
                    return RedirectToAction("DisplayTask");
                }
                else if (employee.MID != null && employee.Administrator == false)
                {

                    // Create a temporary variable to store the manager status.
                    var isManager = TempData["IsManager"] ?? false;

                    // Store the temporary variable in TempData.
                    TempData["IsManager"] = isManager;
                    TempData["IsManager"] = true;
                    //is manager
                    UserRole = "Manager";
                    
                    return View("ManagerPage");
                }

                else if (employee.Administrator == true)
                {
                    var isAdmin = TempData["IsAdmin"] ?? false;
                    //HttpContext.Session.Set<bool>("Administrator", true);
                    HttpContext.Session.SetInt32("Administrator", 1) ;

                    // Store the temporary variable in TempData.
                    TempData["IsAdmin"] = isAdmin;
                    TempData["IsAdmin"] = true;

                    //is admin
                    UserRole = "Admin";
                    return RedirectToAction("AdminPage");
                }
                else 
                {
                    return View("LoginFailed");
                }
            }
            else
            {
                // The employee was not found, so redirect to the login failed page
                return View("LoginFailed");
            }

        }
        else
        {
            return View("LoginFailed");
        }
    }




    public IActionResult ManagerPage()
    {
        return View();

    }
    public IActionResult AdminPage()
    {
        return View("AdminPage");

    }

    public IActionResult ResetPass()
    {
            return View("ResetPassword");
      
    }

    public IActionResult ResetPassword(Employee model)
    {
        // Starting a session
        //var session = HttpContext.Session;

        // Changing the variables to string
        var userName = model.UserName.ToString();
        var userPassword = model.UserPassword.ToString();
        var userConfirmPass = model?.ConfirmPassword?.ToString();

        // A way for the program to read the data from the user and store it in the session
        var bytesusername = Encoding.UTF8.GetBytes(userName);
        var bytespassword = Encoding.UTF8.GetBytes(userPassword);
        var bytesconfirmpass = Encoding.UTF8.GetBytes(userConfirmPass);

        // HttpContext.Session.Set("UserName", bytesusername);
        HttpContext.Session.Set("UserName", bytesusername);
        HttpContext.Session.Set("UserPassword", bytespassword);
        HttpContext.Session.Set("ConfirmPassword", bytesconfirmpass);

        // Connecting to the database
        var Context = new Context();

        // Searching through the database for a match
        var user = Context.Employees.FirstOrDefault(u => u.UserName == userName);

        if (user != null && user.UserName == userName && userPassword != null && user.UserPassword != userConfirmPass && userPassword==userConfirmPass)
        {
            // Update the employee's password in the database with the confirm password
            user.UserPassword = userConfirmPass;
            Context.Update(user);
            Context.SaveChanges();

            // Redirect to the success page
            return RedirectToAction("ResetPasswordSuccess");
        }
        else
        {
            return View("LoginFailed");
        }
    }

    public IActionResult ResetPasswordSuccess()
    {
        return View();
    }

    ////if EID is NULL
    //public async Task<IActionResult> DisplayTask()
    //{
    //    var EID = HttpContext.Session.GetInt32("EID");

    //    if (EID != null)
    //    {
    //        var Context = new Context();
    //        var tasks = await Context.Task_.Where(t => t.EID == EID).ToListAsync();
    //        return View(tasks);
    //    }

    //    // Handle the case when EID is null
    //    return RedirectToAction("Privacy");
    //}



    public async Task<IActionResult> DisplayTask()
    {
        var EID = HttpContext.Session.GetInt32("EID");
            var Context = new Context();
            var tasks = await Context.Tasks.Where(t => t.EID == EID).ToListAsync();
            return View(tasks);

    }




    public IActionResult Privacy()
    {
        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult EmployeeList()
    {
        String connectionString = "Server=.;Database=DB;Trusted_Connection=True;";
        SqlConnection conn = new SqlConnection(connectionString);
        String sql = "SELECT * FROM Employee";
        using (conn)
        {

            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Employee em = new Employee();


                em.EID = Convert.ToInt32(rdr["EID"]);

                em.UserName = (string)rdr["UserName"];

                em.UserPassword = (string)rdr["UserPassword"];

                em.FirstName = (string)(rdr["FirstName"]);

                em.LastName = (string)(rdr["LastName"]);

                em.Administrator = (bool)rdr["Administrator"];

                em.MID = (int)(rdr["EID"]);

                int count = 0;
                count = count + 1;

                // if i need to add an attribute to a table in sql will it be reflected ? 

                //if (em. == "Active")
                //{
                //    lstem.Add(em); 
                //}


            }
        }

        return View();
    }



    public IActionResult Logout()
    {

        // End the session
        HttpContext.Session.Clear();

        // Redirect to the login page
        return RedirectToAction("Index");
    }


}