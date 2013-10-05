using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DailyPic
{
    public partial class calander : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["DailyPicConnectionString"].ConnectionString);
            SqlCommand Validate = new SqlCommand("select DPPic, DPDate, Day(DPDate) AS DD from DailyPicture ORDER BY DPDate DESC", Conn);
            SqlDataReader dr = null;
            SqlDataAdapter da = new SqlDataAdapter(Validate);
            DataTable dt = new DataTable();
            da.Fill(dt);
            string BaseImg = null;
            string dno = null;
            try
            {
                //List<System.Web.UI.HtmlControls.HtmlImage> daylist = new List<System.Web.UI.HtmlControls.HtmlImage>();
                //daylist.Add(day1);
                Conn.Open();
                foreach (DataRow DRdt in dt.Rows)
                {
                    dno = DRdt["DD"].ToString();
                    BaseImg = DRdt["DPPic"].ToString();
                    if (dno == "1")
                    { day1.Src = "data:image/" + System.Drawing.Imaging.ImageFormat.Png + ";base64," + BaseImg;}
                    if (dno == "2")
                    { day2.Src = "data:image/" + System.Drawing.Imaging.ImageFormat.Png + ";base64," + BaseImg; }
                    if (dno == "3")
                    { day3.Src = "data:image/" + System.Drawing.Imaging.ImageFormat.Png + ";base64," + BaseImg; }
                    if (dno == "4")
                    { day4.Src = "data:image/" + System.Drawing.Imaging.ImageFormat.Png + ";base64," + BaseImg; }
                    if (dno == "5")
                    { day5.Src = "data:image/" + System.Drawing.Imaging.ImageFormat.Png + ";base64," + BaseImg; }
                    if (dno == "6")
                    { day6.Src = "data:image/" + System.Drawing.Imaging.ImageFormat.Png + ";base64," + BaseImg; }
                }
                Conn.Close();
            }
           catch (Exception ex)
        { Response.Write("<b>Error Message----  </b>" + ex.ToString() + "<HR/>"); }
        }
    }
}