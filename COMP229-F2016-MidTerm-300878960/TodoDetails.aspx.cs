using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
// using statements that are required to connect to EF DB
using COMP229_F2016_MidTerm_300878960.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace COMP229_F2016_MidTerm_300878960
{
    public partial class TodoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetTodo();
            }
        }

        //GetTodo method
        protected void GetTodo() {
            // populate the form with existing data from db
            int temporaryTodoID = Convert.ToInt32(Request.QueryString["temporaryTodoID"]);

            // connect to the EF DB
            using (TodoContext db = new TodoContext())
            {
                // populate a Todo object instance with the TodoID from 
                // the URL parameter
                Todo updatedTodo = (from todo in db.Todos
                                          where todo.TodoID == temporaryTodoID
                                          select todo).FirstOrDefault();

                // map the Todo properties to the form control
                if (updatedTodo != null)
                {
                    TodoNameTextBox.Text = updatedTodo.TodoDescription;
                    TodoNotesTextBox.Text = updatedTodo.TodoNotes;
                    CompletedTextBox.Text = Convert.ToString(updatedTodo.Completed);
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to the TodoList.aspx page
            Response.Redirect("TodoList.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to conect to the server
            using (TodoContext db = new TodoContext())
            {
                // use the student model to create a new student object and 
                // save a new record
                Todo newTodo = new Todo();
                int temporaryTodoID = 0;
                //Our URL has a TodoID in it
                if (Request.QueryString.Count > 0) {
                    // get the id from the URL
                    temporaryTodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

                    //Get the curreent Todo from the db
                    newTodo = (from queryTodo in db.Todos
                                  where queryTodo.TodoID == temporaryTodoID
                                  select queryTodo).FirstOrDefault();
                }
                // add form data to the new Todo record
                newTodo.TodoDescription = TodoNameTextBox.Text;
                newTodo.TodoNotes = TodoNotesTextBox.Text;
                newTodo.Completed = Convert.ToBoolean(CompletedTextBox.Text);

                // use LINQ to ADO.NET to add / insert new Todo into the db
                if (temporaryTodoID == 0)
                {
                    db.Todos.Add(newTodo);
                }

                // save our changes - also updates and inserts
                db.SaveChanges();

                //Redirect back to the updated TodoList.aspx page
                Response.Redirect("TodoList.aspx");
            }
        }
    }
}