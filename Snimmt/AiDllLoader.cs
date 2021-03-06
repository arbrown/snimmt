﻿using SnimmtPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snimmt
{
    internal class AiDllLoader
    {
        public string SearchPath { get; set; }
        private IDictionary<string,Type> AIs { get; set; }


        public AiDllLoader()
        {
            AIs = new Dictionary<string, Type>();
            LoadAiDlls();
        }

        public AiDllLoader(string path)
        {
            AIs = new Dictionary<string, Type>();
            SearchPath = path;
            LoadAiDlls();
        }



        private void LoadAiDlls()
        {
            var dllPaths = Directory.GetFiles(SearchPath, "*.dll");

            var dlls = new List<Assembly>();

            foreach (var file in dllPaths)
            {
                var an = AssemblyName.GetAssemblyName(file);
                var dll = Assembly.Load(an);
                dlls.Add(dll);
            }

            var playerType = typeof(ISnimmtPlayer);

            foreach (var dll in dlls)
            {
                if (dll != null)
                {
                    var types = dll.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(playerType.FullName) != null)
                            {
                                try
                                {
                                    var ai = (ISnimmtPlayer)Activator.CreateInstance(type);
                                    AIs.Add(ai.Name, type);
                                }
                                catch (ArgumentException)
                                {
                                    // Log duplicate named AI somehow
                                }
                            }
                        }
                    }
                }
            }
        }

        public IEnumerable<string> GetNames()
        {
            return AIs.Keys;
        }

        public bool TryGetAi(string name, out ISnimmtPlayer ai)
        {
            if (AIs.TryGetValue(name, out Type type))
            {
                ai = (ISnimmtPlayer)Activator.CreateInstance(type);
                return true;
            }

            ai = null;
            return false;

        }
    }
}
