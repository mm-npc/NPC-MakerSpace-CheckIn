using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPC_MakerSpace_CheckIn
{
    class Visitor
    {
        
    }

    class Class
    {
        // AcadOrg,
        // ClassNbr,
        // CourseDescription,
        // CourseID,
        // Last,
        // CourseType,
        // MeetingDays,
        // StartTime,
        // EndTime,
        // FacilID,
        // StartDate,
        // EndDate,
        // EnrlStat,
        // CapEnrl,
        // TotEnrl,
        // WaitTot,
        // Session,
        // Location,
        // Mode
        private String AcadOrg           = null;
        private String ClassNbr          = null;
        private String CourseDescription = null;
        private String CourseID          = null;
        private String Last              = null;
        private String CourseType        = null;
        private String MeetingDay        = null;
        private String StartTime         = null;
        private String EndTime           = null;
        private String FacilID           = null;
        private String StartDate         = null;
        private String EndDate           = null;
        private String EnrlStat          = null;
        private String CapEnrl           = null;
        private String TotEnrl           = null;
        private String WaitTot           = null;
        private String Session           = null;
        private String Location          = null;
        private String Mode              = null;
    }
    public partial class Form1 : Form
    {
        private string[] _classes;
        private string[] _orgs;
        enum Reasons : ushort
        {
            Null = 0,
            Consultation = 1,
            PersonalProject = 2,
            Class = 3,
            StudentOrgProject = 4,
            Workshop = 5,
            Commission = 6,
            MAX = 7
        }
        
        // Convert the enum to a string variable:
        private string ReasonToString( int index )
        {
            switch (index)
            {
                case 0:
                    return "";
                case 1:
                    return "Consultation";
                case 2:
                    return "Personal Project";
                case 3:
                    return "Class";
                case 4:
                    return "Student Org Project";
                case 5:
                    return "Workshop";
                case 6:
                    return "Commission";
            }

            return "";
        }
    
        public Form1()
        {
            InitializeComponent();

            ReadFiles();
            
            load_cbReason();
        }

        // Read the files for the classes and the orgs into global variables:
        private void ReadFiles()
        {
            try
            {
                _classes = System.IO.File.ReadAllLines(@"C:\Users\decyple\class_list.dat");
                _orgs    = System.IO.File.ReadAllLines(@"C:\Users\decyple\student_orgs.dat");
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error reading files..." + Environment.NewLine + Environment.NewLine + e.Message);
            }
        }

        private void load_classes()
        {
            
        }

        private void load_orgs()
        {
            
        }
        
        // Loop through each of the reasons, convert them to a string, then add them to the check box:
        private void load_cbReason()
        {
            for (int i = 0; i < (int)Reasons.MAX - 1; i++)
            {
                cbReason.Items.Add(ReasonToString(i));
            }
        }

        // Add the btnSearch click functionality and also error proofing:
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(tbSearch.Text))
                    MessageBox.Show(@"Please enter a search term...");
                else
                {
                    string[] result = { "" };
                    if (cbReason.SelectedIndex == (int)Reasons.Class)
                    {
                        // Query the class list using the search term and find all entries that match: 
                        result = Array.FindAll(_classes, element => element.ToLower().Contains(tbSearch.Text.ToLower()));
                    }
                    else if (cbReason.SelectedIndex == (int)Reasons.StudentOrgProject)
                    {
                        // Query the orgs list using the search term and find all entries that match: 
                        result = Array.FindAll(_orgs, element => element.ToLower().Contains(tbSearch.Text.ToLower()));
                    }

                    // Clear the list to start afresh:
                    lbClassList.Items.Clear();

                    // If we found classes, add them. Otherwise, notify the user no entries were found:
                    if (result.Length != 0)
                        lbClassList.Items.AddRange(result.ToArray<object>());
                    else
                        lbClassList.Items.Add(@"No results found...");
                }
            }
            catch (ArgumentNullException e2)
            {
                MessageBox.Show(@"Error reading files..." + Environment.NewLine + Environment.NewLine + e2.Message);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbInOut_CheckedChanged(object sender, EventArgs e)
        {
            if (cbInOut.Checked)
                cbInOut.Text = @"In";
            else
                cbInOut.Text = @"Out";
        }

        // General form reset command:
        private void ResetForm()
        {
            tbID.Text = "";
            cbReason.SelectedIndex = (int)Reasons.Null;
            tbEstHours.Text = "";
            cbInOut.Checked = false;
            cbInOut.Text = @"In/Out";
            gbSearch.Visible = false;
        }
        private void cbReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the list and search box to start afresh:
            lbClassList.Items.Clear();
            tbSearch.Clear();
            
            switch (cbReason.SelectedIndex)
            {
                case (int)Reasons.Null:                     // Blank
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;

                    break;                
                case (int)Reasons.Consultation:             // Consultation
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;

                    // Set the default hours for this category:
                    tbEstHours.Text = 2.ToString();
                    
                    break;
                case (int)Reasons.PersonalProject:         // Personal Project
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;

                    // Set the default hours for this category:
                    tbEstHours.Text = 2.ToString();
                    break;
                case (int)Reasons.Class:                    // Class
                    // Enable the search functionality to search the class list:
                    gbSearch.Enabled = true;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 1.ToString();

                    break;
                case (int)Reasons.StudentOrgProject:     // Student Org Project
                    // Enable the search functionality to search the org list:
                    gbSearch.Enabled = true;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 2.ToString();
                    
                    // No need to search a small list so directly inject the orgs:
                    lbClassList.Items.AddRange(_orgs);
                    
                    break;
                case (int)Reasons.Workshop:                 // Workshop
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 4.ToString();
                    
                    break;
                case (int)Reasons.Commission:               // Commission
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 3.ToString();
                    
                    break;
            }
        }

        // This function will write the data to a data file. Later there will be several files written. This includes a
        // txt log file, excel spreadsheet, and others that have yet to be determined:
        private void WriteData()
        {
            MessageBox.Show(@"[TODO] Write data...");
        }

        // This function will check several factors to verify the data enter is correct and in the form needed to be
        // stored later:
        private void DataValidation()
        {
            MessageBox.Show(@"[TODO] Validate data...");
        }

        // Finish by validating the forms fully filled-out, the data is written, and then reset:
        private void btnComplete_Click(object sender, EventArgs e)
        {
            // Validate all fields are populated:
            DataValidation();
            
            // Write the data to the file:
            WriteData();
            
            // Reset the form to the default values:
            ResetForm();
        }

        // This provides the same functionality as pressing the search button by entering the enter key:
        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)13)
                btnSearch.PerformClick();
        }
    }
}
