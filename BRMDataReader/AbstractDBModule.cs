using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;

namespace Business.DataModule
{
    public class AbstractDBModule
    {
        public static ArrayList LoadedModules = new ArrayList();

        public static bool isLoaded(Type t)
        {
            return LoadedModules.Contains(t);
        }

        public AbstractDBModule()
        {            
            Type t = this.GetType();
            LoadedModules.Add(t);
        }

        public static void UnloadModules()
        {
            LoadedModules.Clear();
        }
    }
}