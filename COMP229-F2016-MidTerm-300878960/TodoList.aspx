<%@ Page Title="Todo List" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TodoList.aspx.cs" Inherits="COMP229_F2016_MidTerm_300878960.TodoList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="TodoGridView" runat="server" AutoGenerateColumns="false"
        CssClass="table table-bordered table-striped table-hover" DataKeyNames="TodoID"
        OnRowDeleting="TodoGridView_RowDeleting" AllowPaging="true" PageSize="3"
        OnPageIndexChanging="TodoGridView_PageIndexChanging" AllowSorting="true"
        OnSorting="TodoGridView_Sorting" OnRowDataBound="TodoGridView_RowDataBound"
        PagerStyle-CssClass="pagination-ys">
        <Columns>
            <asp:BoundField DataField="TodoDescription" HeaderText="Description" Visible="true" SortExpression="TodoDescription" />
            <asp:BoundField DataField="TodoNotes" HeaderText="Notes" Visible="true" SortExpression="TodoNotes" />
            <asp:BoundField DataField="Completed" HeaderText="Completed" Visible="true" SortExpression="Completed" />

            <asp:HyperLinkField HeaderText="Edit" Text="<i class='fa fa-pencil-square-o fa-lg'></i> Edit"
                NavigateUrl="TodoDetails.aspx.cs" ControlStyle-CssClass="btn btn-primary btn-sm"
                runat="server" DataNavigateUrlFields="TodoID"
                DataNavigateUrlFormatString="TodoDetails.aspx?TodoID={0}" />

            <asp:CommandField HeaderText="Delete" DeleteText="<i class='fa fa-trash-o fa-lg'></i> Delete"
                ShowDeleteButton="true" ButtonType="Link" ControlStyle-CssClass="btn btn-danger btn-sm" />
        </Columns>
    </asp:GridView>
</asp:Content>
