using System;
using System.Collections;
using System.IO;
using System.Text;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core.Publishers;
using ThoughtWorks.CruiseControl.Core.Util;

namespace ThoughtWorks.CruiseControl.Core.Tasks
{
    [ReflectorType("merge")]
    public class MergeFilesTask : ITask
    {
		[ReflectorArray("files")] 
		public string[] MergeFiles = new string[0];

        public void Run(IntegrationResult result)
        {
            foreach (string mergeFile in MergeFiles)
            {
                WildCardPath path = new WildCardPath(mergeFile);
                FileInfo[] files = path.GetFiles();
                foreach (FileInfo fileInfo in files)
                {
                    Log.Info("Merging file: " + fileInfo);
                    if (fileInfo.Exists)
                    {
                        result.TaskResults.Add((new FileTaskResult(fileInfo)));
                    }
                    else
                    {
                        Log.Warning("File not Found: " + fileInfo);
                    }
                }
            }
        }

        public bool ShouldRun(IntegrationResult result)
        {
            return true;
        }

    	public string MergeFilesForPresentation
    	{
    		get
    		{
    			StringBuilder combined = new StringBuilder();
				bool isFirst = true;
    			foreach (string file in MergeFiles)
    			{
    				if (! isFirst)
    				{
    					combined.Append(Environment.NewLine);
    				}
					combined.Append(file);
					isFirst = false;
    			}
				return combined.ToString();
    		}
			set
			{
				if (value == null || value == string.Empty)
				{
					MergeFiles = new string[0];
					return;
				}
				ArrayList files = new ArrayList();
				using (StringReader reader = new StringReader(value))
				{
					while(true)
					{
						string line = reader.ReadLine();
						if(line != null)
						{
							files.Add(line);
						}
						else
						{
							break;
						}
					}
				}
				MergeFiles = (string[]) files.ToArray(typeof (string));
			}
    	}
    }
}