using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PreviewToy
{
    public partial class Preview : Form
    {
        private IntPtr m_hThumbnail;
        public IntPtr sourceWindow;
        private DwmApi.DWM_THUMBNAIL_PROPERTIES m_ThumbnailProperties;
        private bool has_been_set_up = false;
        private PreviewToyHandler spawner;

        public Preview(IntPtr sourceWindow, String title, PreviewToyHandler spawner, Size size) 
        {
            has_been_set_up = false;

            this.sourceWindow = sourceWindow;
            this.spawner = spawner; 

            InitializeComponent(); 
            SetUp();

            this.Text = title;

            has_been_set_up = true;

        }

        protected override void OnResize(EventArgs e)
        {
            RefreshPreview();
            base.OnResize(e);
            if (has_been_set_up)
            {
                this.spawner.syncronize_preview_size(this.Size);
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.OnMove(e);
            this.spawner.register_preview_position(this.Handle, this.Location);
        }

        protected void RefreshPreview()
        {
            if (has_been_set_up)
            {
                m_ThumbnailProperties.rcDestination = new DwmApi.RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);
                DwmApi.DwmUpdateThumbnailProperties(m_hThumbnail, m_ThumbnailProperties);
            }
        }

        public void SetUp()
        {
            m_hThumbnail = DwmApi.DwmRegisterThumbnail(this.Handle, sourceWindow);

            m_ThumbnailProperties = new DwmApi.DWM_THUMBNAIL_PROPERTIES();
            m_ThumbnailProperties.dwFlags = DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_VISIBLE
                + DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_OPACITY
                + DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_RECTDESTINATION
                + DwmApi.DWM_THUMBNAIL_PROPERTIES.DWM_TNP_SOURCECLIENTAREAONLY;
            m_ThumbnailProperties.opacity = 255;
            m_ThumbnailProperties.fVisible = true;
            m_ThumbnailProperties.fSourceClientAreaOnly = true;
            m_ThumbnailProperties.rcDestination = new DwmApi.RECT(0, 0, ClientRectangle.Right, ClientRectangle.Bottom);
            
            DwmApi.DwmUpdateThumbnailProperties(m_hThumbnail, m_ThumbnailProperties);
            
        }

        private void Preview_Load(object sender, EventArgs e)
        {

        }

        public void bring_client_to_foreground()
        {
            DwmApi.SetForegroundWindow(sourceWindow);
            int style = DwmApi.GetWindowLong(sourceWindow, DwmApi.GWL_STYLE);
            if ((style & DwmApi.WS_MAXIMIZE) == DwmApi.WS_MAXIMIZE)
            {
                //It's maximized
            }
            else if ((style & DwmApi.WS_MINIMIZE) == DwmApi.WS_MINIMIZE)
            {
                DwmApi.ShowWindowAsync(sourceWindow, DwmApi.SW_SHOWNORMAL);
            }
        }

        private void render_area_Click(object sender, EventArgs e)
        {
            bring_client_to_foreground();
        }

        public void set_render_area_size(Size size)
        {
            this.Size = size;
        }

    }
}