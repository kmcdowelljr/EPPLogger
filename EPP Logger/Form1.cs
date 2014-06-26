using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
//Adding Python lib to program build
using Python.Runtime;
using IronPython.Compiler;
using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using Microsoft.Scripting;
//


namespace EPP_Logger
{
    public partial class Form1 : Form
    {
        #region FakeData
        public int sizeof_fakedata = 0;

        public string[] fakedata = {
 "Test : 1",
"Login Completed Successfully\n",
"Test : 2",
"Test not available",
"Test : 3",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20009, text:--CONTACT_SUCCESSFULLY_ADDED] DATA = {roid=TOGLOD01CONT1, creationdate=1377080899}",
"Test : 4",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20027, text:--CHECK_COMMAND_COMPLETED_SUCCESSFULLY] DATA = {toglod01cont1=false}",
"Test : 5",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20027, text:--CHECK_COMMAND_COMPLETED_SUCCESSFULLY] DATA = {toglod01cont99=true}",
"Test : 6",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20010, text:--CONTACT_SUCCESSFULLY_QUERIED] DATA = {street2=Building One, street1=3375 Clear Creek Drive, postalcode=50398-001, authinfo=mysecret, fax=+1.7653455566, countrycode=US, extra=[], telno=+1.7035555555, telnoext=12, state=Florida, createdby=30614086, org=Cobolt Boats, Inc., creationdate=1377080899, city=Clearwater, clientid=30614086, roid=TOGLOD01CONT1, email=janderson@worldnet.biz, name=Jim L. Anderson}",
"Test : 7",
"CODE = 1001 DESCRIPTION = Command completed successfully; action pending STATUS = pending ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20039, text:--CONTACT_SUCCESSFULLY_TRANSFERRED_ACTION_PENDING, text:Command completed successfully; action pending] DATA = {requestdate=2013-08-21T10:28:22.0Z, id=TOGLOD01CONT1, actionclientid=30614086, requestclientid=306140862, actiondate=2013-08-26T10:28:22.0Z, type=contact:trnData, transferstatus=pending}",
"Test : 8",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20022, text:--CONTACT_SUCCESSFULLY_TRANSFERRED, text:Query] DATA = {requestdate=2013-08-21T10:28:22.0Z, id=TOGLOD01CONT1, actionclientid=30614086, requestclientid=306140862, actiondate=2013-08-26T10:28:22.0Z, type=contact:trnData, transferstatus=pending}",
"Test : 9",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20022, text:--CONTACT_SUCCESSFULLY_TRANSFERRED, text:Transfer Request Client Cancelled] DATA = {requestdate=2013-08-21T10:28:22.0Z, id=TOGLOD01CONT1, actionclientid=306140862, requestclientid=306140862, actiondate=2013-08-21T10:28:23.0Z, type=contact:trnData, transferstatus=clientCancelled}}",
"Test : 10",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20011, text:--CONTACT_SUCCESSFULLY_UPDATED] DATA = {}",
"TEST : 11",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20011, text:--CONTACT_SUCCESSFULLY_UPDATED] DATA = {}",
"Test : 12",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20008, text:--CONTACT_SUCCESSFULLY_DELETED] DATA = {}",
"Test : 13",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20009, text:--CONTACT_SUCCESSFULLY_ADDED] DATA = {roid=TOGLOD01CONT2, creationdate=1377080905}",
"Test : 14",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20001, text:--DOMAIN_SUCCESSFULLY_ADDED] DATA = {expirationdate=1440115199, name=TOGLOD01TEST-01.BIZ, creationdate=1377080905}",
"Test : 15",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20005, text:--NAMESERVER_SUCCESSFULLY_ADDED] DATA = {name=NS1.TOGLOD01TEST-01.BIZ, creationdate=1377080906}",
"Test : 16",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20005, text:--NAMESERVER_SUCCESSFULLY_ADDED] DATA = {name=ABCDEFGHIJKLMNOPQRSTUVWXYZ-ABCDEFGHIJKLMNOPQR.TOGLOD01TEST-01.BIZ, creationdate=1377080907}",
"Test : 17",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20027, text:--CHECK_COMMAND_COMPLETED_SUCCESSFULLY] DATA = {ns1.toglod01test-01.biz=false}",
"Test : 18",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20027, text:--CHECK_COMMAND_COMPLETED_SUCCESSFULLY] DATA = {ns1.toglod01test-99.biz=true}",
"Test : 19",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20006, text:--NAMESERVER_SUCCESSFULLY_QUERIED] DATA = {clientid=30614086, roid=H6467009-BIZ, iplist=[192.1.2.3], name=NS1.TOGLOD01TEST-01.BIZ, createdby=30614086, creationdate=1377080906, hoststatus_list=[ok]}",
"Test : 20",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20007, text:--NAMESERVER_SUCCESSFULLY_UPDATED] DATA = {}",
"Test : 21",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20007, text:--NAMESERVER_SUCCESSFULLY_UPDATED] DATA = {}",
"Test : 22",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20004, text:--NAMESERVER_SUCCESSFULLY_DELETED] DATA = {}",
"Test : 23",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20005, text:--NAMESERVER_SUCCESSFULLY_ADDED] DATA = {name=NS2.TOGLOD01TEST-01.BIZ, creationdate=1377080910}",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20005, text:--NAMESERVER_SUCCESSFULLY_ADDED] DATA = {name=NS3.TOGLOD01TEST-01.BIZ, creationdate=1377080911}",
"Test : 24",
"CODE = 2306 DESCRIPTION = Parameter value policy error STATUS = parameter value policy error ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 5010, text:SRS Minor Code: 50112, text:--INVALID_NUMBER_OF_BILL_CONTACTS, text:toglod01test-02.biz] DATA = null",
"Test : 25",
"CODE = 2306 DESCRIPTION = Parameter value policy error STATUS = parameter value policy error ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 5010, text:SRS Minor Code: 50112, text:--INVALID_NUMBER_OF_BILL_CONTACTS, text:toglod01test-02.biz] DATA = null",
"Test : 26",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20001, text:--DOMAIN_SUCCESSFULLY_ADDED] DATA = {expirationdate=1440115199, name=TOGLOD01TEST-02.BIZ, creationdate=1377080912}",
"Test : 27",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20001, text:--DOMAIN_SUCCESSFULLY_ADDED] DATA = {expirationdate=1692575999, name=TOGLOD01TEST-03.BIZ, creationdate=1377080913}",
"Test : 28",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20001, text:--DOMAIN_SUCCESSFULLY_ADDED] DATA = {expirationdate=1440115199, name=TOGLOD01TEST-99-ABCDEFGHIJKLMNOPQRSTUVWXYZ-ABCDEFGHIJKLMNOPQRST.BIZ, creationdate=1377080913}",
"Test : 29",
"CODE = 2005 DESCRIPTION = Parameter value syntax error STATUS = parameter value policy error ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 5100, text:SRS Minor Code: 51002, text:--INVALID_DOMAIN_NAME, text:invalid domain name: in-,amp;valid--.biz] DATA = null",
"Test : 30",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20027, text:--CHECK_COMMAND_COMPLETED_SUCCESSFULLY] DATA = {toglod01test-03.biz=false, toglod01test-01.biz=false, toglod01test-02.biz=false}",
"Test : 31",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20027, text:--CHECK_COMMAND_COMPLETED_SUCCESSFULLY] DATA = {toglod01test-97.biz=true, toglod01test-98.biz=true, toglod01test-99.biz=true}",
"Test : 32",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20002, text:--DOMAIN_SUCCESSFULLY_QUERIED, text:Domain successfully queried] DATA = {authinfo=mysecret, expirationdate=1440115199, createdby=30614086, domainstatus_list=[ok], creationdate=1377080912, tech=TOGLOD01CONT2, clientid=30614086, roid=D6467016-BIZ, billing=TOGLOD01CONT2, nameserverslist=[NS2.TOGLOD01TEST-01.BIZ, NS3.TOGLOD01TEST-01.BIZ], registrant=TOGLOD01CONT2, admin=TOGLOD01CONT2, name=TOGLOD01TEST-02.BIZ}",
"Test : 33",
"CODE = 1001 DESCRIPTION = Command completed successfully; action pending STATUS = pending ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20038, text:--DOMAIN_SUCCESSFULLY_TRANSFERRED_ACTION_PENDING, text:Command completed successfully; action pending] DATA = {requestdate=2013-08-21T10:28:35.0Z, actionclientid=30614086, requestclientid=306140862, expirationdate=1471737599, name=TOGLOD01TEST-02.BIZ, actiondate=2013-08-26T10:28:35.0Z, type=domain:trnData, transferstatus=pending}",
"Test : 34",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20021, text:--DOMAIN_SUCCESSFULLY_TRANSFERRED, text:Query] DATA = {requestdate=2013-08-21T10:28:35.0Z, actionclientid=30614086, requestclientid=306140862, expirationdate=1471737599, name=TOGLOD01TEST-02.BIZ, actiondate=2013-08-26T10:28:35.0Z, type=domain:trnData, transferstatus=pending}",
"Test : 35",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20021, text:--DOMAIN_SUCCESSFULLY_TRANSFERRED, text:Transfer Request Client Cancelled] DATA = {requestdate=2013-08-21T10:28:35.0Z, actionclientid=306140862, requestclientid=306140862, expirationdate=1440115199, name=TOGLOD01TEST-02.BIZ, actiondate=2013-08-21T10:28:37.0Z, type=domain:trnData, transferstatus=clientCancelled}",
"Test : 36",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20002, text:--DOMAIN_SUCCESSFULLY_QUERIED, text:Domain successfully queried] DATA = {authinfo=mysecret, expirationdate=1440115199, createdby=30614086, domainstatus_list=[inactive], creationdate=1377080905, hostslist=[ABCDEFGHIJKLMNOPQRSTUVWXYZ-ABCDEFGHIJKLMNOPQR.TOGLOD01TEST-01.BIZ, NS3.TOGLOD01TEST-01.BIZ, NS2.TOGLOD01TEST-01.BIZ], tech=TOGLOD01CONT2, clientid=30614086, roid=D6467007-BIZ, billing=TOGLOD01CONT2, registrant=TOGLOD01CONT2, admin=TOGLOD01CONT2, name=TOGLOD01TEST-01.BIZ}",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20026, text:--DOMAIN_SUCCESSFULLY_RENEWED] DATA = {expirationdate=1597967999, name=TOGLOD01TEST-01.BIZ}",
"Test : 37",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20002, text:--DOMAIN_SUCCESSFULLY_QUERIED, text:Domain successfully queried] DATA = {authinfo=mysecret, expirationdate=1597967999, domainstatus_list=[inactive], createdby=30614086, updateddate=1377080918, tech=TOGLOD01CONT2, creationdate=1377080905, hostslist=[ABCDEFGHIJKLMNOPQRSTUVWXYZ-ABCDEFGHIJKLMNOPQR.TOGLOD01TEST-01.BIZ, NS3.TOGLOD01TEST-01.BIZ, NS2.TOGLOD01TEST-01.BIZ], clientid=30614086, roid=D6467007-BIZ, billing=TOGLOD01CONT2, registrant=TOGLOD01CONT2, updatedby=30614086, admin=TOGLOD01CONT2, name=TOGLOD01TEST-01.BIZ}",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20026, text:--DOMAIN_SUCCESSFULLY_RENEWED] DATA = {expirationdate=1692575999, name=TOGLOD01TEST-01.BIZ}",
"Test : 38",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20003, text:--DOMAIN_SUCCESSFULLY_UPDATED] DATA = {}",
"Test 39",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20009, text:--CONTACT_SUCCESSFULLY_ADDED] DATA = {roid=TOGLOD01CONT3, creationdate=1377080920}",
"Test : 40",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20003, text:--DOMAIN_SUCCESSFULLY_UPDATED] DATA = {}",
"Test : 41",
"CODE = 1000 DESCRIPTION = Command completed successfully STATUS = success ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 2000, text:SRS Minor Code: 20003, text:--DOMAIN_SUCCESSFULLY_UPDATED] DATA = {}",
"Test : 42",
"CODE = 2305 DESCRIPTION = Object association prohibits operation STATUS = association prohibits opeation ERRORMSG = null EXTRAINFO = [text:SRS Major Code: 5170, text:SRS Minor Code: 51711, text:--DOMAIN_HAS_SUBORDINATE_HOSTS, text:Domain cannot be deleted until hosts are removed] DATA = null",
"Test : 43",
"-  [358979031] : Entering logout with this = com.logicboxes.rtk.epp.biz.TransportExt@1629ce8c",
"-  [358979031] : Leaving logout with this = com.logicboxes.rtk.epp.biz.TransportExt@1629ce8c" 
        };

        #endregion
        public Form1()
        {
            InitializeComponent();
            // Display EPP Logger information
            InputBox.Text = "Neustars, Inc © 2013-2014 EPP Logger\n";
            InputBox.Text = "Python Simulator Build version 1.00\n";
            // Hidden the progress bar
            updateProgressBar.Visible = false;   // Turn on when performing a process.

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this exits the application without saving any data t
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Handles Open file option to read .EPP loggs
            Stream myStream = null;
            OpenFileDialog openDialog = new OpenFileDialog(); // Create a new OpenFileDialog instance
            // Configure  openDialog options

            openDialog.InitialDirectory = "c:\\"; // assign a default directory
            openDialog.Filter = "txt files (*.epp) | *.epp | All Files (*.*) | *.*";
            openDialog.FilterIndex = 2;
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openDialog.OpenFile()) != null)
                    {
                        System.IO.StreamReader reader = new System.IO.StreamReader(openDialog.FileName); // open the selected file name to read
                        // now display the stream of data to the InputOut window
                        InputBox.Text = reader.ReadToEnd();
                        // then write it out
                        OutputFakeData();
                    }
                }

                catch (Exception ex) { MessageBox.Show("Error: Could not read file from disk.  Original error: " + ex.Message); }
            }
        }

        private void OutputFakeData()
        {

            OutputText_Box.AppendText(fakedata.ToString());
         
        }

        private void pythonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var py = IronPython.Hosting.Python.CreateEngine();
            try
            {
                py.ExecuteFile("");
            }
            catch (Exception ex) { }
        } 
    }
}

