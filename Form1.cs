using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using ContentAlignment = System.Drawing.ContentAlignment;
using Timer = System.Timers.Timer;

namespace NPC_MakerSpace_CheckIn
{
    class Visitor
    {
        public enum VisitorDirection : ushort
        {
            In = 0, 
            Out = 1
        };
        private string           _id;              // Student ID (prefered), DL#, Name, etc...
        private string           _reason;          // From the list of reasons
        private string           _reasonDetail;    // Only applies for the class(department, course number, description, instructor name) and the student organization(org name)
        private long             _time;            // Epoch time
        private int              _estHours;        // Estimated time being in the space
        private bool             _autoLogOut;      // Visitor had to be logged out using default hours
        private VisitorDirection _direction;       // Visitor coming in or leaving

        public Visitor() {}
        
        public Visitor(string id, string reason, string reasonDetail, long time, int estHours, VisitorDirection direction)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _reason = reason ?? throw new ArgumentNullException(nameof(reason));
            _reasonDetail = reasonDetail ?? throw new ArgumentNullException(nameof(reasonDetail));
            _time = time;
            _estHours = estHours;
            _autoLogOut = false;
            _direction = direction;
        }

        // Copy Constructor:
        public Visitor(Visitor v)
        {
           _id = v._id;
           _reason = v._reason;
           _reasonDetail = v._reasonDetail;
           _time = v._time;
           _estHours = v._estHours;
           _autoLogOut = v.AutoLogOut;
           _direction = v._direction;
        }
        
        // Copy Modify Constructor:
        public Visitor(Visitor v, long t, VisitorDirection d, bool a)
        {
           _id = v._id;
           _reason = v._reason;
           _reasonDetail = v._reasonDetail;
           _time = t;
           _estHours = v._estHours;
           _autoLogOut = a;
           _direction = d;
        }

        public static Visitor GetNewInstance(Visitor v, long t, VisitorDirection d, bool a)
        {
            return new Visitor(v, t, d, a);
        }
        
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string Reason
        {
            get => _reason;
            set => _reason = value;
        }

        public string ReasonDetail
        {
            get => _reasonDetail;
            set => _reasonDetail = value;
        }

        public long Time
        {
            get => _time;
            set => _time = value;
        }

        public int EstHours
        {
            get => _estHours;
            set => _estHours = value;
        }

        public bool AutoLogOut
        {
            get => _autoLogOut;
            set => _autoLogOut = value;
        }

        public VisitorDirection Direction
        {
            get => _direction;
            set => _direction = value;
        }
    }

    class NPC_Class
    {
        private string  _academicOrganization;       // Department
        private int     _classNumber;                // 
        private string  _courseDescription;          // 
        private string  _courseId;                   // 
        private float   _creditHours;                // (Not listed/named in the header)
        private string  _instructorLastName;         // 
        private string  _courseType;                 // [ Blended, IndepStudy, Internship, Lab, Online, Tradition, Tradtnl, Web-Enhanc ]
        private string  _meetingDay;                 // 
        private string  _startTime;                  // 
        private string  _endTime;                    // 
        private string  _facilityId;                 // 
        private string  _startDate;                  // 
        private string  _endDate;                    // 
        private bool    _enrolmentStatus;            // [ Open, Closed ] Course is open/closed for this semester
        private int     _capEnrolment;               // Total Students that can enrol for the class
        private int     _totalEnrolled;              // Total Students Enrolled for the class
        private int     _waitTotal;                  // Total Students Waiting for the class
        private string  _session;                    // [ REG, 8W1, 8W2 ] Regular 16 Weeks, 8-Weeks First, 8-Weeks Last
        private string  _location;                   // [ BUSINESS, HIGHSCHOOL, HSTECH, NPCC, ONLINE, STUDNTHOME ]
        private string  _mode;                       // UNKNOWN

        // Partial Constructor with pertinent data only:
        public NPC_Class(string academicOrganization, string courseDescription, string courseId, string instructorLastName)
        {
            _academicOrganization = academicOrganization;
            _courseDescription = courseDescription;
            _courseId = courseId;
            _instructorLastName = instructorLastName;
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
            _academicOrganization = academicOrganization;
            _classNumber = classNumber;
            _courseDescription = courseDescription;
            _courseId = courseId;
            _creditHours = creditHours;
            _instructorLastName = instructorLastName;
            _courseType = courseType;
            _meetingDay = meetingDay;
            _startTime = startTime;
            _endTime = endTime;
            _facilityId = facilityId;
            _startDate = startDate;
            _endDate = endDate;
            _enrolmentStatus = enrolmentStatus;
            _capEnrolment = capEnrolment;
            _totalEnrolled = totalEnrolled;
            _waitTotal = waitTotal;
            _session = session;
            _location = location;
            _mode = mode;
        }

        public string GetAcademicOrganization => _academicOrganization;

        public string GetCourseDescription => _courseDescription;

        public string GetCourseId => _courseId;

        public string GetInstructorLastName => _instructorLastName;
    }
    
    public partial class Form1 : Form
    {
        private string[] _orgs;
        private List<NPC_Class> class_list = new List<NPC_Class>();
        private List<Visitor> visitor_list = new List<Visitor>();
        private List<NPC_Class> classQryResult = new List<NPC_Class>();
        
        TimeSpan midnight;
        double millisecondsTillMidnight;

        enum Reasons : ushort
        {
            Null = 0,
            Consultation = 1,
            PersonalProject = 2,
            Class = 3,
            StudentOrgProject = 4,
            Workshop = 5,
            Commission = 6,
            Max = 7
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
                // Define when to run the action in seconds. USED FOR TESTING:
                //DateTime midnight2 = DateTime.Now.AddSeconds(45);
                
                // Define when to run the action (midnight):
                midnight = DateTime.Today.AddDays(1.0) - DateTime.Now;
                
                // Get the milliseconds till the action needs to be executed:
                millisecondsTillMidnight = midnight.TotalMilliseconds;

                // Create a timer set for midnight:
                Timer checkForTime = new Timer(millisecondsTillMidnight);
                
                // Set the event handler to run based on the timer:
                checkForTime.Elapsed += new ElapsedEventHandler(Scheduler);
                
                // Enable the timer:
                checkForTime.Enabled = true;
                checkForTime.Start();
            }

            {
                // // TODO: REMOVE TEMP VISITORS!!!
                // Visitor tempVisitor0_in  = new Visitor("0", "0 In", "Reason Detail 0", long.Parse("1677000000"), 1, Visitor.VisitorDirection.In);
                // Visitor tempVisitor0_out = new Visitor("0", "0 Out", "Reason Detail 0", long.Parse("1677000001"), 1, Visitor.VisitorDirection.Out);
                // visitor_list.Add(tempVisitor0_in);
                // AddVisitorAvatarButton("0");
                // visitor_list.Add(tempVisitor0_out);
                //
                // Visitor tempVisitor1_in  = new Visitor("1", "1 In", "Reason Detail 1", long.Parse("1677000010"), 2, Visitor.VisitorDirection.In);
                // Visitor tempVisitor1_out = new Visitor("1", "1 Out", "Reason Detail 1", long.Parse("1677000011"), 2, Visitor.VisitorDirection.Out);
                // visitor_list.Add(tempVisitor1_in);
                // AddVisitorAvatarButton("1");
                // visitor_list.Add(tempVisitor1_out);
                //
                // Visitor tempVisitor2_in  = new Visitor("2", "2 In", "Reason Detail 2", long.Parse("1677000020"), 3, Visitor.VisitorDirection.In);
                // Visitor tempVisitor2_out = new Visitor("2", "2 Out", "Reason Detail 2", long.Parse("1677000021"), 3, Visitor.VisitorDirection.Out);
                // visitor_list.Add(tempVisitor2_in);
                // AddVisitorAvatarButton("2");
                // visitor_list.Add(tempVisitor2_out);
                //
                // Visitor tempVisitor3_in  = new Visitor("3", "3 In", "Reason Detail 3", long.Parse("1677000030"), 4, Visitor.VisitorDirection.In);
                // Visitor tempVisitor3_out = new Visitor("3", "3 Out", "Reason Detail 3", long.Parse("1677000031"), 4, Visitor.VisitorDirection.Out);
                // visitor_list.Add(tempVisitor3_in);
                // AddVisitorAvatarButton("3");
                // //visitor_list.Add(tempVisitor3_out);
                //
                // Visitor tempVisitor4_in = new Visitor("4", "4 In", "Reason Detail 4", long.Parse("1677000040"), 5, Visitor.VisitorDirection.In);
                // Visitor tempVisitor4_out = new Visitor("4", "4 Out", "Reason Detail 4", long.Parse("1677000041"), 5, Visitor.VisitorDirection.Out);
                // visitor_list.Add(tempVisitor4_in);
                // AddVisitorAvatarButton("4");
                // visitor_list.Add(tempVisitor4_out);
                //
                // Visitor tempVisitor01_in  = new Visitor("1", "01 In", "Reason Detail 01", long.Parse("1677000210"), 1, Visitor.VisitorDirection.In);
                // Visitor tempVisitor01_out = new Visitor("1", "01 Out", "Reason Detail 01", long.Parse("1677000211"), 1, Visitor.VisitorDirection.Out);
                // visitor_list.Add(tempVisitor01_in);
                // AddVisitorAvatarButton("01");
                // //visitor_list.Add(tempVisitor1_out);
            }
        }

        // Read the files for the classes and the orgs into global variables:
        private void ReadFiles()
        {
            try
            {
                // Open the Student Organization File:
                _orgs    = File.ReadAllLines(@"C:\Users\decyple\student_orgs.dat");
                
                // Read the new class list file:
                int i = 1;
                var classFile = File.ReadAllLines(@"C:\Users\decyple\class_list_new.dat");
                foreach ( var course in classFile )
                {
                    // Split the line via the tilda delimiter:
                    var classDataColumn = course.Split('~');
                    
                    // Instantiate the new NPC_Class object:
                    NPC_Class tempClass = new NPC_Class(classDataColumn[0], 
                                                        classDataColumn[2], 
                                                        classDataColumn[3], 
                                                        classDataColumn[5]);
                    
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
            for (int i = 0; i < (int)Reasons.Max - 1; i++)
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
                if (string.IsNullOrEmpty(tbSearch.Text))
                {
                    MessageBox.Show(@"Please enter a search term...");
                    ActiveControl = tbSearch;
                }
                else
                {
                    // Clear the list to start afresh:
                    lbClassList.Items.Clear();

                    // If we are searching for a class:
                    if (cbReason.SelectedIndex == (int)Reasons.Class)
                    {
                        // Query the class list using the search term and find all entries that match: 
                        classQryResult = class_list.FindAll(course =>
                            course.GetCourseDescription.ToLower().Contains(tbSearch.Text.ToLower()));
                        
                        // If we found classes, add them. Otherwise, notify the user no entries were found:
                        if (classQryResult.Count != 0)
                            foreach (var classElement in classQryResult)
                                lbClassList.Items.Add(FormatListBoxEntry(classElement.GetCourseDescription, classElement.GetInstructorLastName));
                        else
                            lbClassList.Items.Add(@"No results found...");
                    }
                    // If we are searching for a Student Organization:
                    else if (cbReason.SelectedIndex == (int)Reasons.StudentOrgProject)
                    {
                        // Query the orgs list using the search term and find all entries that match: 
                        string[] orgsQryResult = Array.FindAll(_orgs, course => course.ToLower().Contains(tbSearch.Text.ToLower()));
                        
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
            Close();
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
            string writeText = v.Id + '~';

            string dir;
            if (v.Direction == Visitor.VisitorDirection.In)
                dir = "in" + '~';
            else
                dir = "out" + '~';
            
            writeText += dir + v.Reason + '~' + v.ReasonDetail + '~' + v.Time + '~' + v.EstHours + '~' + v.AutoLogOut + Environment.NewLine;
            
            // Write the text to a new file named "NPC_MakerSpace_Check_In_Out.dat".
            File.AppendAllText(Path.Combine(docPath, "NPC_MakerSpace_Check_In_Out.dat"), writeText);
        }

        // This function will check several factors to verify the data enter is correct and in the form needed to be
        // stored later:
        private bool DataValidation(Visitor v)
        {
            // Check for the ID:
            if (String.IsNullOrEmpty(v.Id))
                MessageBox.Show(@"[ERROR] Data Validation: Need the Id...");
            
            // Check for the Reason:
            if (String.IsNullOrEmpty(v.Reason))
                MessageBox.Show(@"[ERROR] Data Validation: Need the Reason...");
            
            // Check for the Reason Detail:
            if (String.IsNullOrEmpty(v.ReasonDetail) && (v.Reason == @"Class" || v.Reason == @"Student Org Project"))
                MessageBox.Show(@"[ERROR] Data Validation: Need the Reason Detail...");
            
            // Check for the Estimated Hours:
            if (String.IsNullOrEmpty(v.EstHours.ToString()))
                MessageBox.Show(@"[ERROR] Data Validation: Need the Estimated Hours...");
            
            // Check for the Direction:
            if (v.Direction.ToString() == @"In/Out")
                MessageBox.Show(@"[ERROR] Data Validation: Need the Direction...");
            
            return true;
        }

        bool FormValidation()
        {
            // Check for the ID:
            if (String.IsNullOrEmpty(tbID.Text))
            {
                MessageBox.Show(@"Need the Id...");
                ActiveControl = tbID;
                return false;
            }
            
            // Check for the Reason:
            if (cbReason.SelectedIndex == -1 || cbReason.SelectedIndex == 0)
            {
                MessageBox.Show(@"Need the Reason...");
                ActiveControl = cbReason;
                return false;
            }
            
            // Check for the Reason Detail:
            if ((lbClassList.SelectedIndex == -1) && (cbReason.SelectedItem.ToString() == @"Class" || cbReason.SelectedItem.ToString() == @"Student Org Project"))
            {
                MessageBox.Show(@"Need the Reason Detail");
                ActiveControl = lbClassList;
                return false;
            }
            
            // Check for the Estimated Hours:
            if (string.IsNullOrEmpty(tbEstHours.Text))
            {
                MessageBox.Show(@"Need the Estimated Hours...");
                ActiveControl = tbEstHours;
                return false;
            }
            
            // Check for the Direction:
            if (cbInOut.Text == @"In/Out")
            {
                MessageBox.Show(@"Need the Direction...");
                ActiveControl = cbInOut;
                return false;
            }

            return true;
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            // Validate the users input:
            if (!FormValidation())
                return;
            
            // Get the epoch timestamp:
            var unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            
            // Check if there is already a visitor with that ID in the system that's logged in:
            int i = -1;
            Button tempButton = null;
            // Loop through the buttons in the FlowLayoutPanel:
            foreach (Button control in flpCheckOutList.Controls)
            {
                // If the ID's match:
                if (control.Text.ToLower() == tbID.Text.ToLower())
                {
                    // Get the count of matching ID's:
                    i++;
                    
                    // Get the button control to pass to UserCheckOut:
                    // TODO: Either eliminate the i variable or verify that the control is actually for the correct visitor:
                    tempButton = control;
                }
            }
            if (i > -1 && cbInOut.Checked == true)
            {
                MessageBox.Show(@"Warning: There is already a user with that name...I will still proceed with added numbers to the end of the name");
                tbID.Text += '_' + unixTimestamp.ToString();
            }
            
            // Get the class:
            string reason_detail = "";
            if (cbReason.SelectedIndex == (int)Reasons.Class)
            {
                // Detect if no class was selected by the user:
                if (lbClassList.SelectedIndex == -1)
                {
                    MessageBox.Show(@"No class has been selected...");
                    ActiveControl = lbClassList;
                    return;
                }

                reason_detail = classQryResult[lbClassList.SelectedIndex].GetCourseDescription;
            }
            // Get the Student Organization:
            else if (cbReason.SelectedIndex == (int)Reasons.StudentOrgProject)
            {
                reason_detail = lbClassList.Items[lbClassList.SelectedIndex].ToString();
            }
            
            if (cbInOut.Text == @"In")
            {
                // Create the visitor class object:
                Visitor visitor = new Visitor(tbID.Text, cbReason.Items[cbReason.SelectedIndex].ToString(), reason_detail, unixTimestamp, int.Parse(tbEstHours.Text), Visitor.VisitorDirection.In);
                
                // Validate all fields are populated:
                if (DataValidation(visitor))
                {
                    // Write the data to the file:
                    WriteData(visitor);
                    
                    // Add the visitor to the check-out list:
                    AddVisitorAvatarButton(tbID.Text);
                    // Button tempBTN = new Button();
                    // tempBTN.Text = tbID.Text;
                    // tempBTN.BackgroundImage = Image.FromFile(@"C:\Users\decyple\RiderProjects\NPC-MakerSpace-CheckIn\user64_v2a.png");
                    // tempBTN.BackgroundImageLayout = ImageLayout.Zoom;
                    // int width = 108; 
                    // int height = 108;
                    // tempBTN.Size = new Size(width, height);
                    // tempBTN.TextAlign = ContentAlignment.BottomCenter;
                    // tempBTN.TextImageRelation = TextImageRelation.ImageAboveText;
                    // tempBTN.Parent = this;
                    // tempBTN.Click += new EventHandler(UserCheckOut);
                    // flpCheckOutList.Controls.Add(tempBTN);
                    
                    // Add the visitor to the list:
                    visitor_list.Add(visitor);
                }
            }
            else if (cbInOut.Text == @"Out")
            {
                // Validate the users input, then check the visitor out:
                if (FormValidation())
                    // Log the visitor out by passing tbID as the sender to UserCheckOut. That way we can log the visitor
                    // out AND destroy the button at the same time:
                    UserCheckOut(tempButton, e);
                else
                    return;
            }
            else
            {
                MessageBox.Show(@"There was an error completing the request...Check the visitors direction (In/Out)");
                return;
            }
            
            // Reset the form to the default values and return control to tbID:
            ResetForm();
            ActiveControl = tbID;
        }

        private void AddVisitorAvatarButton(string id)
        {
            // Add the visitor to the check-out list via a flow layout panel button:
            Button tempBTN = new Button();
            tempBTN.Text = id;
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
        }

        private void RemoveVisitorAvatarButton(object sender, EventArgs e)
        {
            (sender as Button).Dispose();
        }
        
        private void UserCheckOut(object sender, EventArgs e)
        {
            // Find the visitor in the list by Querying the visitor_in_space_list list: 
            Visitor visitorQryResult = visitor_list.Find(element => element.Id.ToLower().Contains((sender as Button).Text.ToLower()));

            // Check the visitor out by removing them from the list:
            if (visitorQryResult == null)
                // Cant find the user:
                MessageBox.Show(@"We got problems...");
            else
            {
                // Get the epoch timestamp:
                var unixTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                
                // Create a copy of the original check-in:
                Visitor tempVisitor = Visitor.GetNewInstance(   visitorQryResult, 
                                                                unixTimestamp, 
                                                                Visitor.VisitorDirection.Out, 
                                                                false);

                // Add the new visitor entry to the visitor_list:
                visitor_list.Add(tempVisitor);
                
                // Remove the button by disposing of it:
                RemoveVisitorAvatarButton(sender, e);
                
                // Write the data out:
                WriteData(tempVisitor);
            }
        }

        // This provides the same functionality as pressing the search button by entering the enter key:
        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            // If the enter key has been pressed, perform the search:
            if (e.KeyValue == (char)13)
                btnSearch.PerformClick();
        }

        private void onScreenKeyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe");
        }

        private void GenerateReport()
        {
            //MessageBox.Show(@"REPORT BEING GENERATED!");

            // foreach (var visitor in visitor_list)
            // {
            //     WriteData(visitor);
            // }
        }

        private void CheckOutAll()
        {
            // Find all unique visitor ids:
            var distinctList = visitor_list.Select(v => v.Id).Distinct();
            
            // Create a list of the IDs that will be removed from the list later:
            List<Visitor> visitorsToCheckOut = new List<Visitor>();

            // Remove all of the avatars:
            Invoke(new Action(() =>
            {
                flpCheckOutList.Controls.Clear();
            }));

            // Loop through each unique visitor and find all entries for the visitor:
            foreach (var visitor in distinctList)
            {
                // Find all instances of the current visitor to get the instances of visitation:
                List<Visitor> visitorQryResult = visitor_list.FindAll(element => element.Id.ToLower().Contains(visitor.ToLower()));
                
                // If the number of visits is odd, then the visitor hasn't been checked out:
                if (visitorQryResult.Count % 2 != 0)
                {
                    // Create a copy of the original check-in:
                    Visitor tempVisitor = Visitor.GetNewInstance( visitorQryResult.Last(), 
                                                                    visitorQryResult.Last().Time += visitorQryResult.Last().EstHours * 3600, 
                                                                    Visitor.VisitorDirection.Out, true);

                    // Add the new visitor entry to the visitor_list:
                    visitorsToCheckOut.Add(tempVisitor);
                    
                    // Write the data out:
                    WriteData(tempVisitor);
                }
            }

            // Add the list of visitors that needed checking out to the visitor list:
            if (visitorsToCheckOut.Count > 0)
                visitor_list.AddRange(visitorsToCheckOut);
        }
        
        void Scheduler(object sender, ElapsedEventArgs e)
        {
            //MessageBox.Show(@"Scheduler running");
            
            // Log out all user that are still logged in.
            CheckOutAll();
            
            // Generate the daily report:
            GenerateReport();
            
            MessageBox.Show(@"Scheduler has completed running");
        }

        private void generateEndofDayReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckOutAll();
            GenerateReport();

            MessageBox.Show(@"Report Generation Complete...");
        }
        
        private void tbID_Leave(object sender, EventArgs e)
        {
            // Find the visitor in the list by Querying the visitor_list list: 
            Visitor visitorQryResult = visitor_list.Find(element => element.Id.ToLower().Contains((sender as TextBox).Text.ToLower()));

            // Check the visitor out by removing them from the list:
            if (visitorQryResult == null)
            {
                // Cant find the user. So, we proceed normally:
                cbInOut.Checked = true;
                cbInOut.Text = @"In";
            }
        }
    }
}
