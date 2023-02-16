using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace NPC_MakerSpace_CheckIn
{
    class Visitor
    {
        private string id;              // Student ID (prefered), DL#, Name, etc...
        private string reason;          // From the list of reasons
        private string reason_detail;   // Only applies for the class(department, course number, description, instructor name) and the student organization(org name)
        private string time;            // Epoch time
        private int    est_hours;       // Estimated time being in the space

        public Visitor(string id, string reason, string reasonDetail, string time, int estHours)
        {
            this.id = id ?? throw new ArgumentNullException(nameof(id));
            this.reason = reason ?? throw new ArgumentNullException(nameof(reason));
            reason_detail = reasonDetail ?? throw new ArgumentNullException(nameof(reasonDetail));
            this.time = time ?? throw new ArgumentNullException(nameof(time));
            est_hours = estHours;
        }

        public string Id
        {
            get => id;
            set => id = value;
        }

        public string Reason
        {
            get => reason;
            set => reason = value;
        }

        public string ReasonDetail
        {
            get => reason_detail;
            set => reason_detail = value;
        }

        public string Time
        {
            get => time;
            set => time = value;
        }

        public int EstHours
        {
            get => est_hours;
            set => est_hours = value;
        }
    }

    class NPC_Class
    {
        private String  AcademicOrganization;       // Department
        private int     ClassNumber = 0;            // 
        private String  CourseDescription;          // 
        private String  CourseID;                   // 
        private float   CreditHours = 0.0f;         // (Not listed/named in the header)
        private String  InstructorLastName;         // 
        private String  CourseType;                 // [ Blended, IndepStudy, Internship, Lab, Online, Tradition, Tradtnl, Web-Enhanc ]
        private String  MeetingDay;                 // 
        private String  StartTime;                  // 
        private String  EndTime;                    // 
        private String  FacilityID;                 // 
        private String  StartDate;                  // 
        private String  EndDate;                    // 
        private Boolean EnrolmentStatus = false;    // [ Open, Closed ] Course is open/closed for this semester
        private int     CapEnrolment = 0;           // Total Students that can enrol for the class
        private int     TotalEnrolled = 0;          // Total Students Enrolled for the class
        private int     WaitTotal = 0;              // Total Students Waiting for the class
        private String  Session;                    // [ REG, 8W1, 8W2 ] Regular 16 Weeks, 8-Weeks First, 8-Weeks Last
        private String  Location;                   // [ BUSINESS, HIGHSCHOOL, HSTECH, NPCC, ONLINE, STUDNTHOME ]
        private String  Mode;                       // UNKNOWN

        // Partial Constructor with pertinent data only:
        public NPC_Class(string academicOrganization, string courseDescription, string courseId, string instructorLastName)
        {
            AcademicOrganization = academicOrganization;
            CourseDescription = courseDescription;
            CourseID = courseId;
            InstructorLastName = instructorLastName;
        }

        // Full constructor:
        public NPC_Class( string academicOrganization, 
                          int classNumber, 
                          string courseDescription, 
                          string courseId, 
                          float creditHours, 
                          string instructorLastName, 
                          string courseType, 
                          string meetingDay, 
                          string startTime, 
                          string endTime, 
                          string facilityId, 
                          string startDate, 
                          string endDate, 
                          bool enrolmentStatus, 
                          int capEnrolment, 
                          int totalEnrolled, 
                          int waitTotal, 
                          string session, 
                          string location, 
                          string mode)
        {
            AcademicOrganization = academicOrganization;
            ClassNumber = classNumber;
            CourseDescription = courseDescription;
            CourseID = courseId;
            CreditHours = creditHours;
            InstructorLastName = instructorLastName;
            CourseType = courseType;
            MeetingDay = meetingDay;
            StartTime = startTime;
            EndTime = endTime;
            FacilityID = facilityId;
            StartDate = startDate;
            EndDate = endDate;
            EnrolmentStatus = enrolmentStatus;
            CapEnrolment = capEnrolment;
            TotalEnrolled = totalEnrolled;
            WaitTotal = waitTotal;
            Session = session;
            Location = location;
            Mode = mode;
        }

        public string getAcademicOrganization => AcademicOrganization;

        public string getCourseDescription => CourseDescription;

        public string getCourseId => CourseID;

        public string getInstructorLastName => InstructorLastName;
    }
    
    public partial class Form1 : Form
    {
        private string[] _orgs;
        private List<NPC_Class> class_list = new List<NPC_Class>();
        private List<Visitor> visitor_list = new List<Visitor>();
        private List<Visitor> visitor_in_space_list = new List<Visitor>();
        private List<NPC_Class> classQryResult = new List<NPC_Class>();

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

            // Logout Scheduler:
            {
                // // Define when to run the action in seconds. USED FOR TESTING:
                // DateTime midnight = DateTime.Now.AddSeconds(20);
                
                // Define when to run the action (midnight):
                DateTime midnight = DateTime.Now.AddDays(1);
                
                // Get the milliseconds till the action needs to be executed:
                double millisecondsTillMidnight = midnight.Subtract(DateTime.Now).TotalMilliseconds;
                
                // Create a timer set for midnight:
                Timer checkForTime = new Timer(millisecondsTillMidnight);
                
                // Set the event handler to run based on the timer:
                checkForTime.Elapsed += new ElapsedEventHandler(Scheduler);
                
                // Enable the timer:
                checkForTime.Enabled = true;
            }
        }

        // Read the files for the classes and the orgs into global variables:
        private void ReadFiles()
        {
            try
            {
                // Open the Student Organization File:
                _orgs    = System.IO.File.ReadAllLines(@"C:\Users\decyple\student_orgs.dat");
                
                // Read the new class list file:
                int i = 1;
                var classFile = System.IO.File.ReadAllLines(@"C:\Users\decyple\class_list_new.dat");
                foreach ( var course in classFile )
                {
                    // Split the line via the tilda delimiter:
                    var classDataColumn = course.Split('~');
                    
                    // Instantiate the new NPC_Class object:
                    NPC_Class tempClass = new NPC_Class(classDataColumn[0], classDataColumn[2], classDataColumn[3], classDataColumn[5]);
                    
                    // Add the class to the list:
                    class_list.Add(tempClass);

                    // Update the statusbar as to the progress:
                    statusBar.Text = @"Reading class " + i + @"/" + classFile.Length;
                    i++;
                }

                // Display how many classes were loaded:
                statusBar.Text = classFile.Length + @" classes loaded";
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error reading files..." + Environment.NewLine + Environment.NewLine + e.Message);
            }
        }

        // Loop through each of the reasons, convert them to a string, then add them to the check box:
        private void load_cbReason()
        {
            for (int i = 0; i < (int)Reasons.MAX - 1; i++)
            {
                cbReason.Items.Add(ReasonToString(i));
            }
        }

        private string FormatListBoxEntry(string courseDescription, string instructorLastName)
        {
            // 30 characters wide:
            while(courseDescription.Length < 30)
                courseDescription += ' ';
            return courseDescription + @" - " + instructorLastName;
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
                    // Clear the list to start afresh:
                    lbClassList.Items.Clear();

                    // If we are searching for a class:
                    if (cbReason.SelectedIndex == (int)Reasons.Class)
                    {
                        // Query the class list using the search term and find all entries that match: 
                        classQryResult = class_list.FindAll(element => element.getCourseDescription.ToLower().Contains(tbSearch.Text.ToLower()));
                        
                        // If we found classes, add them. Otherwise, notify the user no entries were found:
                        if (classQryResult.Count != 0)
                            foreach (var classElement in classQryResult)
                                lbClassList.Items.Add(FormatListBoxEntry(classElement.getCourseDescription, classElement.getInstructorLastName));
                        else
                            lbClassList.Items.Add(@"No results found...");
                    }
                    // If we are searching for a Student Organization:
                    else if (cbReason.SelectedIndex == (int)Reasons.StudentOrgProject)
                    {
                        // Query the orgs list using the search term and find all entries that match: 
                        string[] orgsQryResult = Array.FindAll(_orgs, element => element.ToLower().Contains(tbSearch.Text.ToLower()));
                        
                        // If we found classes, add them. Otherwise, notify the user no entries were found:
                        if (orgsQryResult.Length != 0)
                            lbClassList.Items.AddRange(orgsQryResult.ToArray<object>());
                        else
                            lbClassList.Items.Add(@"No results found...");
                    }
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
        }
        
        private void cbReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the list and search box to start afresh:
            lbClassList.Items.Clear();
            tbSearch.Clear();
            
            switch (cbReason.SelectedIndex)
            {
                case (int)Reasons.Null:                                                 // Blank
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;

                    break;                
                case (int)Reasons.Consultation:                                         // Consultation
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;

                    // Set the default hours for this category:
                    tbEstHours.Text = 2.ToString();
                    
                    break;
                case (int)Reasons.PersonalProject:                                      // Personal Project
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;

                    // Set the default hours for this category:
                    tbEstHours.Text = 2.ToString();
                    
                    break;
                case (int)Reasons.Class:                                                // Class
                    // Enable the search functionality to search the class list:
                    gbSearch.Enabled = true;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 1.ToString();

                    break;
                case (int)Reasons.StudentOrgProject:                                    // Student Org Project
                    // Enable the search functionality to search the org list:
                    gbSearch.Enabled = true;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 2.ToString();
                    
                    // No need to search a small list so directly inject the orgs:
                    lbClassList.Items.AddRange(_orgs);
                    
                    break;
                case (int)Reasons.Workshop:                                             // Workshop
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 4.ToString();
                    
                    break;
                case (int)Reasons.Commission:                                           // Commission
                    // Search isn't needed for this reason:
                    gbSearch.Enabled = false;
                    
                    // Set the default hours for this category:
                    tbEstHours.Text = 3.ToString();
                    
                    break;
            }
        }

        // This function will write the data to a data file. Later there will be several files written. This includes a
        // txt log file, excel spreadsheet, and others that have yet to be determined:
        private void WriteData(Visitor v)
        {
            // Set the path to the file:
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Format the text for output:
            string writeText = v.Id + '~' + v.Reason + '~' + v.ReasonDetail + '~' + v.Time + '~' + v.EstHours + Environment.NewLine;
            
            // Write the text to a new file named "NPC_MakerSpace_Check_In_Out.dat".
            File.AppendAllText(Path.Combine(docPath, "NPC_MakerSpace_Check_In_Out.dat"), writeText);
        }

        // This function will check several factors to verify the data enter is correct and in the form needed to be
        // stored later:
        private bool DataValidation(Visitor v)
        {
            MessageBox.Show(@"[TODO] Validate data...");
            
            // Check data...
            
            return true;
        }

        // Finish by validating the forms fully filled-out, the data is written, and then reset:
        private void btnComplete_Click(object sender, EventArgs e)
        {
            // Get the epoch timestamp:
            var unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

            string reason_detail = "";
            // Get the class:
            if (cbReason.SelectedIndex == (int)Reasons.Class)
            {
                reason_detail = classQryResult[lbClassList.SelectedIndex].getCourseDescription;
            }
            // Get the Student Organization:
            else if (cbReason.SelectedIndex == (int)Reasons.StudentOrgProject)
            {
                reason_detail = lbClassList.Items[lbClassList.SelectedIndex].ToString();
            }

            // Create the visitor class object:
            Visitor visitor = new Visitor(tbID.Text, cbReason.Items[cbReason.SelectedIndex].ToString(), reason_detail, unixTimestamp.ToString(), int.Parse(tbEstHours.Text));
            
            // Validate all fields are populated:
            if (DataValidation(visitor))
            {
                // Write the data to the file:
                WriteData(visitor);
                
                // Add the visitor to the check-out list:
                Button tempBTN = new Button();
                tempBTN.Text = tbID.Text;
                tempBTN.BackgroundImage = Image.FromFile(@"C:\Users\decyple\RiderProjects\NPC-MakerSpace-CheckIn\user64_v2a.png");
                tempBTN.BackgroundImageLayout = ImageLayout.Zoom;
                int width = 108; 
                int height = 108;
                tempBTN.Size = new Size(width, height);
                tempBTN.TextAlign = ContentAlignment.BottomCenter;
                tempBTN.TextImageRelation = TextImageRelation.ImageAboveText;
                tempBTN.Parent = this;
                tempBTN.Click += new EventHandler(UserCheckOut);
                flpCheckOutList.Controls.Add(tempBTN);
                
                // Add the visitor to the list:
                visitor_in_space_list.Add(visitor);
            
                // Reset the form to the default values:
                ResetForm();
            }
        }

        //private void userCheckOut(string userID)
        private void UserCheckOut(object sender, EventArgs e)
        {
            // Find the visitor in the list by Querying the visitor_in_space_list list: 
            Visitor visitorQryResult = visitor_in_space_list.Find(element => element.Id.ToLower().Contains((sender as Button).Text.ToLower()));
            
            // Check the visitor out by removing them from the list:
            if (visitorQryResult == null)
                // Cant find the user:
                MessageBox.Show(@"We got problems...");
            else
            {
                // Remove the visitor from the list:
                visitor_in_space_list.Remove(visitorQryResult);

                // Remove the button by disposing of it:
                (sender as Button).Dispose();
            }
        }

        // This provides the same functionality as pressing the search button by entering the enter key:
        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)13)
                btnSearch.PerformClick();
        }

        private void onScreenKeyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
        }

        private void GenerateReport()
        {
            MessageBox.Show(@"REPORT!");
        }

        private void LogOutAll()
        {
            MessageBox.Show(@"LOG OUT ALL!");
            
            // Check for any visitors still logged in:
            
            // Check their default hours:
            
            // Logout the visitors the default amount of hours after login:
        }
        
        void Scheduler(object sender, ElapsedEventArgs e)
        {
            // Log out all user that are still logged in.
            LogOutAll();
            
            // Generate the daily report:
            GenerateReport();
        }
    }
}
