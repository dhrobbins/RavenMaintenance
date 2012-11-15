using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RavenLib.Scripting;
using Nancy;

namespace RavenLib
{
    public class ScriptingModule : RavenModuleBase
    {
        public ScriptingModule(DefaultRootPathProvider rootPathProvider)
            : base("Scripting")
        {
            Post["/ExecuteScript/{scriptName}"] = parameters =>
                {   
                    string scriptPath = rootPathProvider.GetRootPath() + @"\ConfigScripts\"
                                                           + parameters.scriptName;
                    var scriptParameters = new Dictionary<string, object>();
                    scriptParameters.Add("DocumentSession", DocumentSession);

                    var scriptProcessor = new ScriptProcessor(scriptPath, scriptParameters);
                    scriptProcessor.Execute();

                    return scriptParameters["ResultsJson"].ToString();
                };

            Post["/ExecuteSnippet"] = parameters =>
                {
                    string snippet = parameters.Forms["snippet"].ToString();
                    var scriptParameters = new Dictionary<string, object>();
                    scriptParameters.Add("DocumentSession", DocumentSession);

                    var scriptProcessor = new ScriptProcessor();
                    scriptProcessor.SetAParameters(scriptParameters);
                    scriptProcessor.EvaluateScriptStub(snippet);

                    return scriptParameters["ResultsJson"].ToString();
                };

            Post["/GetFileScriptNames"] = fsn =>
            {
                var scriptProcessor = new ScriptProcessor();
                string scriptPath = rootPathProvider.GetRootPath() + @"\ConfigScripts\";

                return Response.AsJson(scriptProcessor.GetScriptFileNames(scriptPath));
            };

            Get["/Tool"] = parameters =>
                {
                    return View["Scripting.htm"];                        
                };
        }
    }
}
