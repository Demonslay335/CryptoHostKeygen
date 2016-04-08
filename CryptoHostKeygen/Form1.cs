using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoHostKeygen
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            TwitterLink.Links.Add(0, TwitterLink.Text.Length, "https://twitter.com/demonslay335");
            PasswordTextbox.Text = GenerateKey();
        }

        private void TwitterLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        public static void Decrypt()
        {
            String key = GenerateKey();
            String rarPath = GetRarPath(key);

            Interaction.Shell(string.Concat(new string[]
            {
                "processor X -o+ -p",
               key,
                " ",
                rarPath,
                Interaction.Environ("userprofile"),
                "\\Desktop"
            }), AppWinStyle.Hide, false, -1);
        }

        public static String GetRarPath(String key)
        {
            String appData = Interaction.Environ("appdata");
            if(File.Exists(appData + "\\" + key))
            {
                return appData + "\\" + key;
            }
            else if(File.Exists(appData + "\\" + key + ".rar"))
            {
                return appData + "\\" + key + ".rar";
            }
            throw new FileNotFoundException("Encrypted data not found");
        }

        public static String GenerateKey()
        {
            String cpu = smethod_6();
            String hdd = smethod_7("C");
            String mobo = smethod_8();
            String SHA1 = Conversions.ToString(smethod_9(cpu + hdd + mobo));
            String username = Interaction.Environ("username");
            return SHA1 + username;
        }

        public static string smethod_6()
        {
            string result = "";
            try
            {
                string text = string.Empty;
                SelectQuery query = new SelectQuery("Win32_processor");
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
                try
                {
                    ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        text = managementObject["processorId"].ToString();
                    }
                }
                finally
                {
                    ManagementObjectCollection.ManagementObjectEnumerator enumerator = null;
                    if (enumerator != null)
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                result = text;
            }
            catch (Exception arg_63_0)
            {
                ProjectData.SetProjectError(arg_63_0);
                ProjectData.ClearProjectError();
            }
            return result;
        }

        internal static string smethod_7(string string_6 = "C")
        {
            string result = "";
            try
            {
                ManagementObject managementObject = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", string_6));
                managementObject.Get();
                result = managementObject["VolumeSerialNumber"].ToString();
            }
            catch (Exception arg_2A_0)
            {
                ProjectData.SetProjectError(arg_2A_0);
                ProjectData.ClearProjectError();
            }
            return result;
        }
        internal static string smethod_8()
        {
            string result = "";
            try
            {
                string text = string.Empty;
                SelectQuery query = new SelectQuery("Win32_BaseBoard");
                ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(query);
                try
                {
                    ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        ManagementObject managementObject = (ManagementObject)enumerator.Current;
                        text = managementObject["SerialNumber"].ToString();
                    }
                }
                finally
                {
                    ManagementObjectCollection.ManagementObjectEnumerator enumerator = null;
                    if (enumerator != null)
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                result = text;
            }
            catch (Exception arg_63_0)
            {
                ProjectData.SetProjectError(arg_63_0);
                ProjectData.ClearProjectError();
            }
            return result;
        }

        public static object smethod_9(string string_6)
        {
            object result ="";
            try
            {
                UTF8Encoding uTF8Encoding = new UTF8Encoding();
                SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
                byte[] value = sHA1CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(string_6));
                string_6 = BitConverter.ToString(value).Replace("-", "");
                result = string_6;
            }
            catch (Exception arg_37_0)
            {
                ProjectData.SetProjectError(arg_37_0);
                ProjectData.ClearProjectError();
            }
            return result;
        }

    }
}
