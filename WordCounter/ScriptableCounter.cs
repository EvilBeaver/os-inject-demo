using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptEngine.Environment;
using ScriptEngine.Machine;
using ScriptEngine.Machine.Contexts;

namespace WordCounter
{
    class ScriptableCounter : ScriptDrivenObject
    {
        public ScriptableCounter(LoadedModuleHandle module) : base(module)
        {
        }

        public ScriptableCounter(LoadedModuleHandle module, bool deffered) : base(module, deffered)
        {
        }

        protected override int GetOwnVariableCount()
        {
            return 0;
        }

        protected override int GetOwnMethodCount()
        {
            return 0;
        }

        protected override void UpdateState()
        {
            
        }

        public void ProcessInput(string line)
        {
            var result = DoCounting(line);
            ShowAnswer(result);
        }

        private int DoCounting(string line)
        {
            var methodId = GetScriptMethod("ПриВыполненииПодсчета", "OnPerformCounting");
            if(methodId == -1)
                return DefaultCounter(line);

            var lineVar = ValueFactory.Create(line);
            var boolTrue = ValueFactory.Create(true);
            var stdProcessFlag = Variable.Create(boolTrue, "stdProcessing");

            var arguments = new IValue[2] {lineVar, stdProcessFlag};
            var result = CallScriptMethod(methodId, arguments);

            if (stdProcessFlag.AsBoolean())
            {
                return DefaultCounter(line);
            }
            else if(result.DataType == DataType.Number)
            {
                return (int)result.AsNumber();
            }
            else
            {
                Console.WriteLine("Неверный тип аргумента был получен из скрипта");
                return DefaultCounter(line);
            }
        }

        private int DefaultCounter(string line)
        {
            return line.Split(' ').Length;
        }

        private void ShowAnswer(int value)
        {
            var methodId = GetScriptMethod("ПриВыводеРезультата", "OnResultOutput");
            if (methodId == -1)
            {
                DefaultAnswer(value);
                return;
            }

            var lineVar = ValueFactory.Create(value);
            var boolTrue = ValueFactory.Create(true);
            var stdProcessFlag = Variable.Create(boolTrue, "stdProcessing");

            var arguments = new IValue[2] { lineVar, stdProcessFlag };
            CallScriptMethod(methodId, arguments);
            if(stdProcessFlag.AsBoolean())
                DefaultAnswer(value);
        }

        private void DefaultAnswer(int value)
        {
            Console.WriteLine("В строке {0} слов", value);
        }
    }
}
