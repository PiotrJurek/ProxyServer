using ServerLibrary.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    internal class IpHelper
    {
        public static IPAddress[] GetListenIPAddresses(string[] listenAddresses)
        {
            HashSet<IPAddress> addresses = new HashSet<IPAddress>();

            if (listenAddresses.Contains("*"))
            {
                addresses.Add(IPAddress.Any);
                return addresses.ToArray();
            }

            foreach (string listenAddress in listenAddresses)
            {
                if (listenAddress == "localhost")
                {
                    addresses.Add(IPAddress.Loopback);
                }
                else
                {
                    addresses.Add(IPAddress.Parse(listenAddress));
                }
            }
            
            return addresses.ToArray();
        }

        public static bool IsClientAllowed(IPAddress clientAddress, string[] allowedIPAddresses)
        {
            if (allowedIPAddresses.Contains("any"))
            {
                return true;
            }

            foreach (var cidr in allowedIPAddresses)
            {
                if (IsInSubnet(clientAddress, cidr))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsInSubnet(IPAddress address, string cidr)
        {
            var parts = cidr.Split('/');
            var ip = IPAddress.Parse(parts[0]);
            var prefixLength = int.Parse(parts[1]);

            var addressBytes = address.GetAddressBytes();
            var ipBytes = ip.GetAddressBytes();

            int byteCount = prefixLength / 8;
            int bitCount = prefixLength % 8;

            for (int i = 0; i < byteCount; i++)
            {
                if (addressBytes[i] != ipBytes[i])
                {
                    return false;
                }
            }

            if (bitCount > 0)
            {
                int mask = 0xFF << (8 - bitCount);
                if ((addressBytes[byteCount] & mask) != (ipBytes[byteCount] & mask))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
