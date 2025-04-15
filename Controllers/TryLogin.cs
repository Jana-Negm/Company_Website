using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Project1.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;


namespace Project1.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
          return View();
        }

    }
}





// -------------------------------------------------------------------------




//        protected void ValidateUser(object sender, EventArgs e)
//        {
//            int userId = 0;
//            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
//            using (SqlConnection con = new SqlConnection(conn))
//            {
//                using (SqlCommand cmd = new SqlCommand("Validate_User"))
//                {
//                    Employee emp = new Employee();
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@UserName", emp.UserName);
//                    cmd.Parameters.AddWithValue("@UserPassword", emp.UserPassword);
//                    cmd.Connection = con;
//                    con.Open();
//                    userId = Convert.ToInt32(cmd.ExecuteScalar());
//                    con.Close();
//                }
//                switch (userId)
//                {
//                    case -1:
//                        Login1.FailureText = "Username and/or password is incorrect.";
//                        break;
//                    case -2:
//                        Login1.FailureText = "Account has not been activated.";
//                        break;
//                    default:
//                        FormsAuthentication.RedirectFromLoginPage(Login1.UserName, Login1.RememberMeSet);
//                        break;
//                }
//            }
//        }

//    }
//}














