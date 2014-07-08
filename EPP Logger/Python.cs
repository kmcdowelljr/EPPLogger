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
        //string IronPython_Path = @"C:\\Program Files (x86)\\IronPython 2.7\\";
        //string IronPython_File = "ipy.exe";
        TextWriter _writer = null;

        public Python_Window()
        {
            InitializeComponent();

        }

        private void Python_Load(object sender, EventArgs e)
        {
            ScriptRuntimeSetup setup = Python.CreateRuntimeSetup(null);
            ScriptRuntime runtime = new ScriptRuntime(setup);
            ScriptEngine engine = Python.GetEngine(runtime);
            
             var py = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            py.Runtime.IO.RedirectToConsole();
            engine.SetSearchPaths(new string[] { "C:\\Program Files (x86)\\IronPython 2.7\\Lib", "C:\\Python27\\Lib\\" });
           
            string Engine = py.LanguageVersion.ToString() + "\n";
            python_TextBox.Text = "Iron Python" + " " + Engine + "\r\n";


            try
            {
               //engine.Execute(ApplicationPath +"\\pytool-gimped.py",scope);
               py.Execute(@"pytool-gimped.py", scope);
               //py.ExecuteFile(ApplicationPath + "\\pytool-gimped.py", scope)

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }
        }
    }
}
        
     
        
 
