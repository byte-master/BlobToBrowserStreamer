using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication13
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string storageConnectionString = TextBox1.Text;
            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(TextBox2.Text);

                string Dir_path = Server.MapPath("~/tmp//");
                string name = TextBox3.Text;
                string newname = DateTimeOffset.Now.UtcTicks.ToString();

                CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(name);
                    string path = (Dir_path + newname);
                    blockBlob.DownloadToFile(path, FileMode.Create);
                 //Response.WriteFile(path);

                FileInfo file = new FileInfo(path);

                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();

                Response.AddHeader("Content-Disposition", "attachment; filename="+ newname);
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "text/plain";
                Response.Flush();
                Response.WriteFile(file.FullName);
                try
                {

                
                Response.TransmitFile(path);
                Response.End();
                File.Delete(path);
                }
                catch(Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string storageConnectionString = TextBox1.Text;
            CloudStorageAccount storageAccount;

            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(TextBox2.Text);

                string Dir_path = Server.MapPath("~/tmp//");
                string name = TextBox3.Text;
                string newname = DateTimeOffset.Now.UtcTicks.ToString();

                CloudBlockBlob blockBlob = cloudBlobContainer.GetBlockBlobReference(name);
                string path = (Dir_path + newname);
                blockBlob.DownloadToFile(path, FileMode.Create);
               

                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + name + "\"");
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.WriteFile(Dir_path + newname);
                HttpContext.Current.Response.Flush();
                File.Delete(Dir_path + newname);
                HttpContext.Current.Response.End();
            }
        }
    }
}
