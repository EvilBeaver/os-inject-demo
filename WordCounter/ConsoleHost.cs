using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptEngine.HostedScript;
using ScriptEngine.HostedScript.Library;
using ScriptEngine.Machine.Contexts;

namespace WordCounter
{
    [ContextClass("СчетчикСлов")]
    class ConsoleHost : IHostApplication
    {
        public void Echo(string str, MessageStatusEnum status = MessageStatusEnum.Ordinary)
        {
            Console.WriteLine(str);
        }

        public void ShowExceptionInfo(Exception exc)
        {
            Echo(exc.ToString());
        }

        public bool InputString(out string result, int maxLen)
        {
            result = Console.ReadLine();
            return true;
        }

        public string[] GetCommandLineArguments()
        {
            return new string[0];
        }
    }
}
