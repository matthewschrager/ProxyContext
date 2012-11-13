using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProxyContext
{
    class ProxyContext : IDisposable
    {
        //===============================================================
        public ProxyContext(String workingDirectory = null)
        {
            WorkingDirectory = workingDirectory;

            var setupInfo = new AppDomainSetup { ApplicationBase = WorkingDirectory };
            AppDomain = AppDomain.CreateDomain("Proxy Context Domain", null, setupInfo);
        }
        //===============================================================
        public AppDomain AppDomain { get; private set; }
        //===============================================================
        public String WorkingDirectory { get; private set; }
        //===============================================================
        public T CreateProxy<T>(IEnumerable<AssemblyName> additionalAssemblies = null)
        {
            if (additionalAssemblies != null)
            {
                foreach (var assembly in additionalAssemblies)
                    AppDomain.Load(assembly);
            }

            return (T)AppDomain.CreateInstanceAndUnwrap(typeof(T).Assembly.FullName, typeof(T).FullName);
        }
        //===============================================================
        public void Dispose()
        {
            AppDomain.Unload(AppDomain);
        }
        //===============================================================
    }
}
