using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScriptEngine;
using ScriptEngine.Environment;
using ScriptEngine.HostedScript;

namespace WordCounter
{
    class Program
    {
        static HostedScriptEngine _world;

        static void Main(string[] args)
        {
            CreateWorld();
            var counter = CreateCounter();

            bool wantAgain = false;
            do
            {
                Console.WriteLine("Введите любую строку и я посчитаю что в ней.");
                var line = Console.ReadLine();
                counter.ProcessInput(line);

                Console.WriteLine("Хотите еще? y/n");
                var answer = Console.ReadLine();
                wantAgain = answer.Equals("Y", StringComparison.OrdinalIgnoreCase);

            } while (wantAgain);
        }

        private static ScriptableCounter CreateCounter()
        {
            var curDir = Directory.GetCurrentDirectory();
            var module = Path.Combine(curDir, "counter.os");
            ICodeSource src;
            if (File.Exists(module))
            {
                src = _world.Loader.FromFile(module);
            }
            else
            {
                src = _world.Loader.FromString("");
            }

            var compiler = _world.GetCompilerService();
            var image = compiler.CreateModule(src);
            var loadedModule = _world.EngineInstance.LoadModuleImage(image);

            var counter = new ScriptableCounter(loadedModule);
            _world.EngineInstance.InitializeSDO(counter);
            return counter;
        }

        private static void CreateWorld()
        {
            _world = new HostedScriptEngine();
            var thisAsm = System.Reflection.Assembly.GetExecutingAssembly();
            var asmDir = Path.GetDirectoryName(thisAsm.Location);
            var libDir = Path.Combine(asmDir, "lib");
            if (Directory.Exists(libDir))
            {
                _world.InitExternalLibraries(libDir, new string[0]);
            }

            _world.SetGlobalEnvironment(new ConsoleHost(), null);
            _world.AttachAssembly(thisAsm);
            _world.Initialize();
        }
    }
}
