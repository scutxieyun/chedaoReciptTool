using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace CheDaoLoader
{
    public partial class fmConfigure : Form
    {
        String mCurProvince = null;
        String mCurCity = null;
        String mCurArea = null;
        String mAppCode = null;
        public String AppCode {
            get { return mAppCode; }
        }
        public fmConfigure()
        {
            InitializeComponent();
            AreaInfoInit();
        }

        private void cbProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            AreaInfoUpdate();
        }
        void AreaInfoInit() {
            cbProvince.Items.AddRange(CAreaInfo.GetProvinces());
        }

        void AreaInfoUpdate() {
            if (cbProvince.SelectedItem != null && cbProvince.SelectedItem.ToString() != mCurProvince) {
                mCurProvince = cbProvince.SelectedItem.ToString();
                mCurCity = null;
                mCurArea = null;
                this.cbArea.Items.Clear();
                this.cbCity.Items.Clear();
                this.cbCity.Items.AddRange(CAreaInfo.GetCities(mCurProvince));
                this.cbCity.SelectedIndex = 0;
                //mCurCity = cbCity.SelectedItem.ToString();
                return;
            }
            if (cbCity.SelectedItem != null && cbCity.SelectedItem.ToString() != mCurCity) {
                mCurCity = cbCity.SelectedItem.ToString();
                mCurArea = null;
                this.cbArea.Items.Clear();
                this.cbArea.Items.AddRange(CAreaInfo.GetAreas(mCurProvince, mCurCity));
                this.cbArea.SelectedIndex = 0;
                return;
            }
            if(cbArea.SelectedItem != null) mCurArea = cbArea.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String url = String.Format("{0:s}?p={1:s}&c={2:s}&a={3:s}&addr={4:s}&name={5:s}&o={6:s}",ConfigurationManager.AppSettings["service_url"] + "register",mCurProvince,mCurCity,mCurArea,tbAddr.Text,tbName.Text,tbOperator.Text);
            try
            {
                WebRequest req = WebRequest.Create((url));
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    Stream res_str = res.GetResponseStream();
                    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader reader = new StreamReader(res_str, encode);
                    mAppCode = reader.ReadToEnd().Trim();
                    if (mAppCode == "error")
                    {
                        MessageBox.Show("填写信息不完整，请检查数据");
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("请求失败，请联系技术支持 " + res.ToString());
                }
            }
            catch (Exception ex) {
                MessageBox.Show("连接服务失败，请检查网络或联系技术支持 " + ex.Message);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
