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
            PasswordTextbox.Text = GenerateKey();
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
            String cpu = GetProcessorId();
            String hdd = GetVolumeSerial("C");
            String mobo = GetMotherboardID();
            String SHA1 = Conversions.ToString(stringhash(cpu + hdd + mobo));
            String username = Interaction.Environ("username");
            return SHA1 + username;
        }

        public static string GetProcessorId()
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

        internal static string GetVolumeSerial(string string_6 = "C")
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
        internal static string GetMotherboardID()
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

        public static object stringhash(string str)
		{
			string result = "";
			try
			{
				UTF8Encoding uTF8Encoding = new UTF8Encoding();
				SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
				byte[] value = sHA1CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(str));
				str = BitConverter.ToString(value).Replace("-", "");
				result = str;
			}
			catch (Exception arg_35_0)
			{
				ProjectData.SetProjectError(arg_35_0);
				ProjectData.ClearProjectError();
			}
			return result;
		}

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }
    }
}
