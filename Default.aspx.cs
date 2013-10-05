using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;

namespace DailyPic
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private System.Drawing.Image FormatImage(HtmlInputFile file, string savePath,string Imgsize)
        {
            //width,height,size
            int fix = Convert.ToInt32(Imgsize);
            int nHeight = 0;

            int nWidth = 0;

            int maxSize = 3000;

            //if (!(type == "jpg" || type == "bmp" || type == "gif" || type == "jpeg" || type == "png")) { throw new Exception("type error"); } //validate formatter
            int size = (int)((float)file.PostedFile.ContentLength / (float)1024); //byte exchange kb 

            if (size > maxSize) { throw new Exception("img size:" + size.ToString() + "KB,Maximum image size is 200KB! "); }

            if (System.IO.File.Exists(savePath)) { System.IO.File.Delete(savePath); }
            using (System.Drawing.Image Cimage = System.Drawing.Image.FromStream(file.PostedFile.InputStream))
            {
                if (Cimage.Width > nWidth || Cimage.Height > nHeight)
                {
                    if (Cimage.Width >= Cimage.Height)
                    {
                        nWidth = fix;
                        nHeight = Convert.ToInt32((Convert.ToDouble(nWidth) / Convert.ToDouble(Cimage.Width)) * Convert.ToDouble(Cimage.Height));
                    }
                    else
                    {
                        nHeight = fix;
                        nWidth = Convert.ToInt32((Convert.ToDouble(nHeight) / Convert.ToDouble(Cimage.Height)) * Convert.ToDouble(Cimage.Width));
                    }
                    Bitmap newimage = new Bitmap(Cimage, nWidth, nHeight);
                    System.Drawing.Imaging.ImageCodecInfo jpegEncoder = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().Where(ks => ks.MimeType == "image/jpeg").FirstOrDefault();
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                    System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, 100L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    newimage.Save(Server.MapPath(savePath), jpegEncoder, myEncoderParameters);
                    return Cimage;
                }
                else { Cimage.Save(Server.MapPath(savePath));
                return Cimage;
                } 
                
            }
            Response.Write("add sucessful.");
        }
        public static System.Drawing.Imaging.ImageFormat GetImageFormat(System.Drawing.Image img)
        {  
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
                return System.Drawing.Imaging.ImageFormat.Png;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
                return System.Drawing.Imaging.ImageFormat.Bmp;
          
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Emf))
                return System.Drawing.Imaging.ImageFormat.Emf;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Exif))
                return System.Drawing.Imaging.ImageFormat.Exif;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                return System.Drawing.Imaging.ImageFormat.Gif;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Icon))
                return System.Drawing.Imaging.ImageFormat.Icon;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.MemoryBmp))
                return System.Drawing.Imaging.ImageFormat.MemoryBmp;
            if (img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Tiff))
                return System.Drawing.Imaging.ImageFormat.Tiff;
            else
                return System.Drawing.Imaging.ImageFormat.Wmf;
        }
        public string ImageToBase64(System.Drawing.Image image)
        {

            MemoryStream ms = new MemoryStream();

            System.Drawing.Imaging.ImageFormat format = GetImageFormat(image);
            Response.Write(format.ToString());
            // 將圖片轉成 byte[]
            image.Save(ms, format);
            byte[] imageBytes = ms.ToArray();

            // 將 byte[] 轉 base64
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection Conn2 = new SqlConnection(WebConfigurationManager.ConnectionStrings["DailyPicConnectionString"].ConnectionString);
            SqlCommand Validate = new SqlCommand("select TOP 1 DPDate from DailyPicture ORDER BY DPDate DESC", Conn2);
            Conn2.Open();
            string Val = Validate.ExecuteNonQuery().ToString();
            Conn2.Close();
            Response.Write(Val);
            //防止重複上傳

            if (file1.PostedFile.FileName.ToString() != "" )
            {
                //縮圖並儲存
                string filename = file1.PostedFile.FileName;
                string extension = System.IO.Path.GetExtension(filename);
                string newFileName = Guid.NewGuid().ToString() + extension;
                FormatImage(file1, "~/photo/" + newFileName,"400");
                string fileUrl = "~/photo/" + newFileName;
                var img = System.Drawing.Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\photo\" +newFileName);
                string ImgBase64 = ImageToBase64(img);
               SqlCommand cmdtop1 = new SqlCommand("select TOP 1 DPID from DailyPicture ORDER BY DPID DESC", Conn2);

                try
                {
                    Conn2.Open();
                    string strDPID = null;
                    object obj = cmdtop1.ExecuteScalar();
                    var dateNo = DateTime.Now.ToString("yyMM");
                    if (obj != null)
                    {
                        string dr = cmdtop1.ExecuteScalar().ToString();
                        int drpluse = Convert.ToInt32(dr.Substring(6, 4));
                        drpluse++;
                        string GFNO = drpluse.ToString();
                        string showGF = GFNO.PadLeft(4, '0');
                        strDPID = "DP" + dateNo + showGF.ToString();
                    }
                    else
                    {
                        strDPID = "DP" + dateNo + "0001";
                    }
                    string addRecord = "INSERT INTO DailyPicture (DPID,DPDate,DPPicPath,DPPic,DPTitle,DPNote)values('" + strDPID + "','" + DateTime.Now.ToShortDateString() + "','" + newFileName + "','"+ImgBase64+"','"+pictitle.Value+"','"+piccontent.Value+"')";
                    SqlCommand cmd = new SqlCommand(addRecord, Conn2);
                    cmd.ExecuteNonQuery();
                    Conn2.Close();
                }
                catch (Exception ex)
                { Response.Write("<b>Error Message----  </b>" + ex.ToString() + "<HR/>"); }
            }

            else
            {
                Response.Write("請檢查是否有選擇照片");
            }
        }
    }
}