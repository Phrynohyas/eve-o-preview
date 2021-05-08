using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace PreviewToy
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.richTextBoxDescription.Rtf =
@"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033{\fonttbl{\f0\fnil\fcharset0 Arial;}{\f1\fnil\fcharset0 Calibri;}{\f2\fnil\fcharset2 Symbol;}}
{\*\generator Riched20 6.3.9600}\viewkind4\uc1 
\pard\sa200\sl276\slmult1\qc\b\fs28\lang9 EVE-O Preview\par

\pard\sa200\sl276\slmult1\b0\fs24 EVE-O Preview is a multi-client management tool which displays miniature windows, or previews, of your client(s) and allows you to switch between them easily. Due to the nature of the window tracking (Windows DLLs), Linux and Mac are not supported.\par
\i Requirements:\line\i0 Windows Vista/7/8/8.1\line Windows Aero\par
\i Key Features:\par

\pard{\pntext\f2\'B7\tab}{\*\pn\pnlvlblt\pnf2\pnindent0{\pntxtb\'B7}}\fi-360\li720\sa200\sl276\slmult1\i0 Client Previews (windowed and borderless)\par
{\pntext\f2\'B7\tab}Resizable previews (independent or syncronized)\par
{\pntext\f2\'B7\tab}Options for 'always on top', 'hide when EVE client not active', 'hide active client preview', and 'zoom on hover'\par
{\pntext\f2\'B7\tab}EVE Client position tracking (CCP FoxFour)\par

\pard\sa200\sl276\slmult1\i Current Maintainer(s):\i0\line CCP FoxFour\line Makari Aeron\par
\i Original Creator:\line\i0 StinkRay\par
\i Devs:\line\i0 StinkRay, CCP FoxFour, Makari Aeron\par
\i Special Thanks:\i0\line Takagamu\f1\fs22\par
}";
        }

        //StinkRay
        private void labelCopyright_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {       
            string url = "https://forums.eveonline.com/default.aspx?g=posts&t=246157";
            ProcessStartInfo sInfo = new ProcessStartInfo(new Uri(url).AbsoluteUri);
            Process.Start(sInfo);
        }

        //New Thread
        private void labelCompanyName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://forums.eveonline.com/default.aspx?g=posts&m=5264866";
            ProcessStartInfo sInfo = new ProcessStartInfo(new Uri(url).AbsoluteUri);
            Process.Start(sInfo);
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
