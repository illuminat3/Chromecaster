using Chromecaster.Datatypes;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Zeroconf;

namespace Chromecaster.Services
{
    public class ChromecastLocator
    {
        public async Task<List<ChromecastDevice>> DiscoverDevicesAsync()
        {
            List<ChromecastDevice> devices = new List<ChromecastDevice>();

            IReadOnlyList<IZeroconfHost> chromecastHosts = await ZeroconfResolver.ResolveAsync("_googlecast._tcp.local.");

            foreach (var host in chromecastHosts)
            {
                string deviceName = host.DisplayName;
                string ipAddress = host.IPAddress;

                devices.Add(new ChromecastDevice { Name = deviceName, IPAddress = ipAddress });
            }

            return devices;
        }
    }
}