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
    public partial class TodoList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // if loading the page for the first time
            // populate the student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "TodoID"; // default sort column
                Session["SortDirection"] = "ASC";

                // Get the student data
                this.GetTodo();
            }
        }

        //GetTodo method
        protected void GetTodo()
        {
            //using the TodoContext
            using (TodoContext db = new TodoContext())
            {
                string SortString = Session["SortColumn"].ToString() + " " +
                    Session["SortDirection"].ToString();

                // query the Student Table using EF and LINQ
                var todo = (from allTodo in db.Todos
                            select allTodo);

                // bind the result to the Students GridView
                TodoGridView.DataSource = todo.AsQueryable().OrderBy(SortString).ToList();
                TodoGridView.DataBind();
            }

        }

        protected void TodoGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // store which row was clicked
            int selectedRow = e.RowIndex;

            // get the selected StudentID using the Grid's DataKey collection
            int TodoID = Convert.ToInt32(TodoGridView.DataKeys[selectedRow].Values["TodoID"]);

            // use EF and LINQ to find the selected student in the DB and remove it
            using (TodoContext db = new TodoContext())
            {
                // create object ot the student clas and store the query inside of it
                Todo deletedTodo = (from todoRecords in db.Todos
                                    where todoRecords.TodoID == TodoID
                                    select todoRecords).FirstOrDefault();

                // remove the selected student from the db
                db.Todos.Remove(deletedTodo);

                // save my changes back to the db
                db.SaveChanges();

                // refresh the grid
                this.GetTodo();
            }
        }

        protected void TodoGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Set the new page number
            TodoGridView.PageIndex = e.NewPageIndex;

            // refresh the Gridview
            this.GetTodo();
        }

        protected void TodoGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            // refresh the GridView
            this.GetTodo();

            // toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
        }

        protected void TodoGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Check if IsPostback
            if (IsPostBack) {
                //Check if header row has been clicked
                if (e.Row.RowType == DataControlRowType.Header) {
                    LinkButton linkButton = new LinkButton();

                    //Iterating through the TodoGridView all data using its length
                    for (int index = 0; index < TodoGridView.Columns.Count - 1; index++) {
                        if (TodoGridView.Columns[index].SortExpression == Session["SortColumn"].ToString()) {
                            if (Session["SortDirection"].ToString() == "ASC") {
                                linkButton.Text = " <i class='fa fa-caret-up fa-lg'></i>";
                            } else {
                                linkButton.Text = " <i class='fa fa-caret-down fa-lg'></i>";
                            }
                            //Add this linkButton to the TodoGridView control
                            e.Row.Cells[index].Controls.Add(linkButton);
                        }
                    }//End of for loop
                }//End of secondary level if statement
            }//End of outermost if(IsPostBack)
        }//end of TodoGridView_RowDataBound method

        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // set the new Page size
            TodoGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            // refresh the GridView
            this.GetTodo();
        }//End of PageSizeDropDownList_SelectedIndexChanged method
    }// end of partial class
}// end of namespace