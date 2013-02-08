﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace OSAE.UI.Controls
{
    /// <summary>
    /// Interaction logic for StateImage.xaml
    /// </summary>
    public partial class StateImage : UserControl
    {
        OSAE osae = new OSAE("GUI");
        public OSAEObject screenObject { get; set; }
        public Point Location;
        public DateTime LastUpdated;

        public string StateMatch;
        public string CurState;

        private string ObjectName;
        private string ObjectStateTime;
        private ImageManager imgMgr = new ImageManager();
        
        public StateImage(OSAEObject sObject)
        {
            InitializeComponent();

            screenObject = sObject;
            ObjectName = screenObject.Property("Object Name").Value;
            CurState = osae.GetObjectStateValue(ObjectName).Value;

            Image.Tag = ObjectName;
            Image.MouseLeftButtonUp += new MouseButtonEventHandler(State_Image_MouseLeftButtonUp);

            //ObjectStateTime = stateObject.LastUpd;

            string imgPath;
            foreach (ObjectProperty p in screenObject.Properties)
            {
                if (p.Value.ToLower() == CurState.ToLower())
                {
                    StateMatch = p.Name.Substring(0, p.Name.LastIndexOf(' '));
                }
            }
            
            string imgName = screenObject.Property(StateMatch + " Image").Value;
            OSAEImage img = imgMgr.GetImage(imgName);

            if (img.Data != null)
            {
                var imageStream = new MemoryStream(img.Data);
                var bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = imageStream;
                bitmapImage.EndInit();
                Image.Source = bitmapImage;
                Image.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                Image.Source = null;
                Image.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public void Update()
        {
            CurState = osae.GetObjectStateValue(ObjectName).Value;

            foreach (ObjectProperty p in screenObject.Properties)
            {
                if (p.Value.ToLower() == CurState.ToLower())
                {
                    StateMatch = p.Name.Substring(0, p.Name.LastIndexOf(' '));
                }
            }
            
            Location.X = Double.Parse(screenObject.Property(StateMatch + " X").Value);
            Location.Y = Double.Parse(screenObject.Property(StateMatch + " Y").Value);

            string imgName = screenObject.Property(StateMatch + " Image").Value;
            OSAEImage img = imgMgr.GetImage(imgName);

            if (img.Data != null)
            {
                var imageStream = new MemoryStream(img.Data);


                this.Dispatcher.Invoke((Action)(() =>
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = imageStream;
                    bitmapImage.EndInit();
                    Image.Source = bitmapImage;
                }));
            }
        }

        private void State_Image_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            bool iResults = false;

            if (CurState == "ON")
            {
                osae.MethodQueueAdd(ObjectName, "OFF", "", "");
                osae.ObjectStateSet(ObjectName, "OFF");
            }
            else
            {
                osae.MethodQueueAdd(ObjectName, "ON", "", "");
                osae.ObjectStateSet(ObjectName, "ON");
            }
            
        }

        
    }
}
