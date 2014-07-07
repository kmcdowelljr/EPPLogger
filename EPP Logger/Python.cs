using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Utils;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Debugging;
using IronPython;
using IronPython.Runtime.Exceptions;
using IronPython.Runtime.Operations;
using IronPython.Runtime;
using IronPython.Hosting;
using IronPython.Modules;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "IronPython.Runtime.Exceptions.TraceBackFrame..ctor(System.Object,System.Object,System.Object)", MessageId = "0#globals")]
[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Scope = "member", Target = "IronPython.Runtime.Exceptions.TraceBackFrame.Globals", MessageId = "Globals")]

namespace EPP_Logger
{
    public partial class Python_Window : Form
    {
        string ApplicationPath = System.Environment.CurrentDirectory.ToString();
        string IronPython_Path = @"C:\\Program Files (x86)\\IronPython 2.7\\";
        string IronPython_File = "ipy.exe";
        TextWriter _writer = null;

        public Python_Window()
        {
            InitializeComponent();
        }
               
    
        private class ScriptOutputStream : Stream
        {
            #region Fields
            private readonly TextBox _control;
            #endregion

            #region Constructors
            public ScriptOutputStream(TextBox control)
            {
                _control = control;
            }
            #endregion

            #region Properties
            public override bool CanRead
            {
                get { return false; }
            }

            public override bool CanSeek
            {
                get { return false; }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override long Length
            {
                get { throw new NotImplementedException(); }
            }

            public override long Position
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
            #endregion

            #region Exposed Members
            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                _control.Text += Encoding.GetEncoding(1252).GetString(buffer, offset, count);
            }
            #endregion
        }

  
        private void Python_Load(object sender, EventArgs e)
        {
            
            #region oldcode
          
            ScriptRuntimeSetup setup = Python.CreateRuntimeSetup(null);
            ScriptRuntime runtime = new ScriptRuntime(setup);
            ScriptEngine engine = Python.GetEngine(runtime);

            _writer = new TextBoxStreamWriter(python_TextBox);
            Console.SetOut(_writer);

            python_TextBox.Text = @ApplicationPath + "\r\n";
                      
            var py = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            py.Runtime.IO.RedirectToConsole();
            engine.SetSearchPaths(new string[] { "C:\\Program Files (x86)\\IronPython 2.7\\Lib", "C:\\Python27\\Lib\\" });
            py.Runtime.IO.SetOutput(new ScriptOutputStream(python_TextBox), Encoding.GetEncoding(1252));
            string Engine = py.LanguageVersion.ToString() + "\n";
            python_TextBox.Text = "Iron Python" + " " + Engine +"\r\n";
            
             string _tld ="";
                        
            try
            {
               

               py.ExecuteFile(ApplicationPath + "\\pytool-gimped.py", scope);
                     
                   
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.ToString());
                
            }

        
            #endregion
            // Revised code
            # region Command Prompt
            try
     {
        
       System.Diagnostics.ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd.exe", "/c");
       // The following commands are needed to redirect the standard output.
       // This means that it will be redirected to the Process.StandardOutput StreamReader.
        
        procStartInfo.RedirectStandardOutput = false;
        procStartInfo.UseShellExecute = true;
        // Do not create the black window.
        procStartInfo.CreateNoWindow = true;
        procStartInfo.RedirectStandardInput = true;
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.RedirectStandardError = true;
        procStartInfo.WorkingDirectory = IronPython_Path;
        procStartInfo.FileName = IronPython_File;

        Process process1 = new Process();
        process1.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
        process1.ErrorDataReceived += new DataReceivedEventHandler(ErrorHandler);

      
        // Now we create a process, assign its ProcessStartInfo and start it
        try
        {
            process1.StartInfo = procStartInfo;
            process1.SynchronizingObject = python_TextBox;

            process1.Start();
            
            process1.BeginOutputReadLine();

            process1.WaitForExit();
        }
        catch
        {
        }

            #endregion

        #region oldcode2
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        // Get the output into a string
        string result = proc.StandardOutput.ReadToEnd();
        // Display the command output.
        python_TextBox.Text = result;
        _writer = new TextBoxStreamWriter(python_TextBox);
        Console.SetOut(_writer);

        }
      catch (Exception objException)
      {
      // Log the exception
      }
        #endregion
        }
        private void OutputHandler(Object source, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output. 
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                python_TextBox.AppendText(outLine.Data + "\r\n");
            }
        }

        private void ErrorHandler(Object source, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output. 
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                python_TextBox.AppendText(outLine.Data + "\r\n");
            }
        }  
     }
    }

