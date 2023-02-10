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

        public string returnAcademicOrganization => AcademicOrganization;

        public string returnCourseDescription => CourseDescription;

        public string returnCourseId => CourseID;

        public string returnInstructorLastName => InstructorLastName;
    }
    public partial class Form1 : Form
    {
        private string[] _classes;
        private string[] _orgs;
        private List<NPC_Class> class_list = new List<NPC_Class>();
        private List<Visitor> visitor_list;
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
                // Old text file:
                _classes = System.IO.File.ReadAllLines(@"C:\Users\decyple\class_list.dat");
                _orgs    = System.IO.File.ReadAllLines(@"C:\Users\decyple\student_orgs.dat");
                
                // Read the new class list file:
                int i = 1;
                var classFile = System.IO.File.ReadAllLines(@"C:\Users\decyple\class_list_new.dat");
                foreach ( var course in classFile )
                {
                    var classDataColumn = course.Split('~');
                    NPC_Class tempClass = new NPC_Class(classDataColumn[0], classDataColumn[2], classDataColumn[3], classDataColumn[5]);
                    class_list.Add(tempClass);

                    statusBar.Text = @"Reading class " + i + @"/" + classFile.Length;
                    i++;
                }

                // Display how many classes were loaded:
                statusBar.Text = classFile.Length + @"classes loaded";
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
