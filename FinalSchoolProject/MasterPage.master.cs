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
            BindConsultPagesRepeater();
        }
    }

    #endregion

    #region page methods
    
    private void BindConsultPagesRepeater()
    {
        DataTable pages = ConsultPagesClass.GetAll();
        ConsultPagesRepeater.DataSource = pages;
        ConsultPagesRepeater.DataBind();
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
