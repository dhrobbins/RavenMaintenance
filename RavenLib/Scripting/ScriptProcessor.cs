using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RavenLib.Utils;
using System.IO;
using CSScriptLibrary;
using Raven.Client.Document;
using Raven.Abstractions;

namespace RavenLib.Scripting
{
    public class ScriptProcessor
    {
        private string scriptPath;
        private Dictionary<string, object> parameters;

        public ScriptProcessor() { }

        public ScriptProcessor(string scriptPath, Dictionary<string, object> parameters) 
        {
            this.scriptPath = scriptPath;
            this.parameters = parameters;
        }

        public ScriptProcessor SetPath(string path)
        {
            this.scriptPath = path;

            return this;
        }

        public ScriptProcessor SetAParameters(Dictionary<string, object> parameters)
        {
            this.parameters = parameters;

            return this;
        }

        public void Execute()
        {
            string scriptBody = ReadScriptFromDisk();
            var processRunner = CSScript.LoadMethod(scriptBody)
                                            .GetStaticMethod();

            Enforce.ArgumentNotNull<Dictionary<string, object>>(parameters, 
                                 "ScriptProcessor.Execute - parameters can not be null");

            processRunner(this.parameters);
        }

        public void EvaluateScriptStub(string scriptBody)
        {
            Enforce.That((string.IsNullOrEmpty(scriptBody) == false), 
                                "ScriptProcesor.EvaluateScriptStub - scriptBody can not be null.");

            var processRunner = CSScript.LoadMethod(scriptBody)
                                            .GetStaticMethod();
            processRunner(this.parameters);
        }

        private string ReadScriptFromDisk()
        {
            Enforce.That((string.IsNullOrEmpty(scriptPath) == false), 
                                "ScriptProcessor.ReadScriptFromDisk - scriptPath can not be null.");

            string scriptBody = string.Empty;

            var fileInfo = new FileInfo(this.scriptPath);

            if (fileInfo.Exists)
            {
                var sr = fileInfo.OpenText();
                scriptBody = sr.ReadToEnd();
                sr.Close();
            }
            else
            {
                throw new FileNotFoundException("ScriptProcessor.ReadScriptFromDisk - " + scriptPath 
                                                    + " was not found.");
            }

            return scriptBody;
        }

        private string PrepSnippetForExecution(string snippet)
        {
            string header = @"//css_reference ApprovaFlow
//css_reference Newtonsoft.Json

using System;
using System.Collections.Generic;
using System.Linq;
using ApprovaFlow;
using ApprovaFlow.Data;
using ApprovaFlow.Users;
using ApprovaFlow.Workflow;
using Raven.Client.Document;
using Raven.Client;
using Raven.Client.Linq;
using Raven.Abstractions.Data;
using Newtonsoft.Json;
using RavenLib.Data;

public class Script
{
    public static void Process(Dictionary<string, object> parameters)
    {";
            string footer = @"}
}";

            return header + "\n\n" + snippet + "\n\n";

        }

        public List<string> GetScriptFileNames(string scriptPath)
        {
            Enforce.That((string.IsNullOrEmpty(scriptPath) == false),
                                "ScriptProcessor.GetScriptFileNames - scriptPath can not be null");

            var scriptFileNames = new List<string>();

            var dirInfo = new DirectoryInfo(scriptPath);

            if (dirInfo.Exists)
            {
                scriptFileNames = dirInfo.GetFiles().Select(x => x.Name)
                                                    .ToList() ?? new List<string>();
            }
            else
            {
                throw new DirectoryNotFoundException("ScriptProcessor.ReadScriptNames - " + scriptPath
                                                    + " not found.");
            }

            return scriptFileNames;
        }
    }
}
