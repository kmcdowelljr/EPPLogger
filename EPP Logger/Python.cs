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


namespace EPP_Logger
{
    public partial class Python_Window : Form
    {
        public Python_Window()
        {
            InitializeComponent();

                 
        }

        private void Python_Load(object sender, EventArgs e)
        {
          
            
            //Step 1:
            // Create a new script runtime
            string script = "pytool-gimped.py";
            var engine = Python.CreateEngine();
            var scope = engine.CreateScope();
            var source = engine.CreateScriptSourceFromString(script, SourceCodeKind.Statements);
            var compiled = source.Compile();
            var result = compiled.Execute(scope);
            
           
           
            try{
                // Step 2: Load python file into memory
                dynamic loadscript = engine.ExecuteFile(script);
                
                
            }
            catch (Exception ex) {
                python_TextBox.Text = ex.Message.ToString();
                                 }
            //Console.ReadKey(true);

        }
    }
}
