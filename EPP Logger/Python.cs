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
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;
using IronPython.Hosting;
using IronPython.Modules;
using System.Diagnostics;
using System.IO;
using IronPython;


namespace EPP_Logger
{
    public partial class Python_Window : Form
    {
        string ApplicationPath = System.Environment.CurrentDirectory.ToString();
      

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
            ScriptRuntimeSetup setup = Python.CreateRuntimeSetup(null);
            ScriptRuntime runtime = new ScriptRuntime(setup);
            ScriptEngine engine = Python.GetEngine(runtime);
            dynamic scope = engine.CreateScope();
            
            python_TextBox.Text = @ApplicationPath + "\br";
                      
            var py = Python.CreateEngine();
            //py.Runtime.Globals.SetVariable("epp", "Test");
           py.Runtime.IO.SetOutput(new ScriptOutputStream(python_TextBox), Encoding.GetEncoding(1252));
            string Engine = py.LanguageVersion.ToString() + "\n";
            python_TextBox.Text = "Iron Python" + " " + Engine +"\n";
         

            try
            {
               // py.Execute(parseargs);
                //py.Execute(@"C:\Users\kmcdowel\Documents\Visual Studio 2013\Projects\EPP Logger\EPP Logger\bin\Debug\mainprogram.py");
                py.ExecuteFile(ApplicationPath +"\\pytool-gimped.py",scope);
               // python_TextBox.Text = file;
            }
            catch (Exception ex) {
                    
                python_TextBox.Text = ex.Message.ToString(); }

            
          }
        }
    }

