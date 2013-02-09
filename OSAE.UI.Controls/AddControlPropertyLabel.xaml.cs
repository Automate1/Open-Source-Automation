﻿
namespace OSAE.UI.Controls
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaction logic for AddControlPropertyLabel.xaml
    /// </summary>
    public partial class AddControlPropertyLabel : UserControl
    {
        private string currentScreen;

        public AddControlPropertyLabel(string screen)
        {
            InitializeComponent();
            currentScreen = screen;
            LoadObjects();
            LoadColors();
        }

        /// <summary>
        /// Load the screens from the DB into the combo box
        /// </summary>
        private void LoadObjects()
        {
            DataSet dataSet = OSAESql.RunSQL("SELECT object_name FROM osae_v_object order by object_name");
            objectComboBox.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void LoadColors()
        {
            //create a generic list of strings
            //Dim colors As New List(Of String)()
            //get the color names from the Known color enum
            string[] colorNames = Enum.GetNames(typeof(KnownColor));
            //iterate thru each string in the colorNames array
            foreach (string colorName in colorNames)
            {
                //cast the colorName into a KnownColor
                //KnownColor. knownColor;// = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                //check if the knownColor variable is a System color
                //if (knownColor > knownColor.)
                //{
                    //add it to our list
                    foreColorComboBox.Items.Add(colorName);
                    backColorComboBox.Items.Add(colorName);
                //}
            }
            //return the color list
            foreColorComboBox.Text = "Black";
            backColorComboBox.Text = "White";

        }

        private void objectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataSet dataSet = OSAESql.RunSQL("select property_name from osae_v_object_property where object_name='" + (sender as ComboBox).SelectedValue.ToString() + "'");
            propertyComboBox.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        private void txtFont_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.Forms.FontDialog dlgFont = null;
                dlgFont = new System.Windows.Forms.FontDialog();

                //dlgFont.Font = set your font here

                if (dlgFont.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFont.Text = dlgFont.Font.FontFamily.Name;
                    txtSize.Text = dlgFont.Font.Size.ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string sName = "Screen - " + currentScreen + " - " + objectComboBox.Text + ": " + propertyComboBox.Text;
            OSAEObjectManager.ObjectAdd(sName, sName, "CONTROL PROPERTY LABEL", "", currentScreen, true);
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Font Name", txtFont.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Font Size", txtSize.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Fore Color", foreColorComboBox.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Back Color", backColorComboBox.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Object Name", objectComboBox.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Property Name", propertyComboBox.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Prefix", txtPrefix.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Suffix", txtSuffix.Text, "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "X", "100", "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Y", "100", "GUI");
            OSAEObjectPopertyManager.ObjectPropertySet(sName, "Zorder", "1", "GUI");

            OSAEScreenControlManager.ScreenObjectAdd(currentScreen, objectComboBox.Text, sName);


            NotifyParentFinished();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NotifyParentFinished();
        }

        /// <summary>
        /// Let the hosting contol know that we are done
        /// </summary>
        /// <remarks>At present it tells the parent to close, this could later be altered to have a event that fires to
        /// the parent allowing them to decide what to do when the control is finished. If the control is being hosted in
        /// an element host this will have no affect as the parent is the element host and not the form.</remarks>
        private void NotifyParentFinished()
        {
            // Get the window hosting us so we can ask it to close
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}
