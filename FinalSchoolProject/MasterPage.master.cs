using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;

public partial class MasterPage : System.Web.UI.MasterPage
{
    #region page events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindAllConsultPagesRepeater();
            
            if(Session["Logged"] != null)
                BindMyConsultPagesRepeater();
        }
    }

    #endregion

    #region page methods
    
    private void BindAllConsultPagesRepeater()
    {
        DataTable pages = ConsultPagesClass.GetAll();
        AllConsultPagesRepeater.DataSource = pages;
        AllConsultPagesRepeater.DataBind();
    }

    private void BindMyConsultPagesRepeater()
    {
        UsersClass user = UsersClass.GetByID((int)Session["CurrentUserID"]);
        DataTable pages = user.GetMySubscibedPages();
        MyConsultPagesRepeater.DataSource = pages;
        MyConsultPagesRepeater.DataBind();
    }

    #endregion

    #region control events

    protected void LoginSubmitButton_Click(object sender, EventArgs e)
    {
        string username = loginInputUsername.Text;
        string password = loginInputPassword.Text;

        UsersService us = new UsersService();
        us.ValidateAndLogin(username, password);
    }

    protected void SignupSubmitButton_Click(object sender, EventArgs e)
    {
        string username = signupInputUsername.Text;
        string password = signupInputPassword.Text;
        string pass_confirm = signupInputPasswordConfirm.Text;
        string DOB = signupInputDOB.Text;

        UsersService us = new UsersService();
        us.ValidateAndSignup(username, password, pass_confirm, DOB);

    }

    protected void LogoutConfirmButton_Click(object sender, EventArgs e)
    {
        UsersService us = new UsersService();
        us.LogOutUser();
    }
    #endregion
}
