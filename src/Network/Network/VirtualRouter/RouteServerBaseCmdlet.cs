﻿using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using CNM = Microsoft.Azure.Commands.Network.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.Network
{
    public class RouteServerBaseCmdlet : NetworkBaseCmdlet
    {
        // Gateway ASN which gets populated for VirtualHub as RS resides inside GW
        public const int GatewayAsn = 65515;

        public void AddBgpConnectionsToPSVirtualHub(CNM.PSVirtualHub virtualHubModel,
                                                  string resourceGroupName,
                                                  string routerName)
        {
            var bgpConnections = this.NetworkClient.NetworkManagementClient.VirtualHubBgpConnections.List(resourceGroupName, routerName);
            var bgpConnectionList = ListNextLink<BgpConnection>.GetAllResourcesByPollingNextLink(bgpConnections, this.NetworkClient.NetworkManagementClient.VirtualHubBgpConnections.ListNext);
            foreach (var connection in bgpConnectionList)
            {
                virtualHubModel.BgpConnections.Add(NetworkResourceManagerProfile.Mapper.Map<CNM.PSBgpConnection>(connection));
            }
        }

        public void AddIpConfigurtaionToPSVirtualHub(CNM.PSVirtualHub virtualHubModel,
                                                    string resourceGroupName,
                                                    string routerName)
        {
            var ipConfigModels = this.NetworkClient.NetworkManagementClient.VirtualHubIPConfiguration.List(resourceGroupName, routerName);
            var ipConfigList = ListNextLink<HubIpConfiguration>.GetAllResourcesByPollingNextLink(ipConfigModels, this.NetworkClient.NetworkManagementClient.VirtualHubIPConfiguration.ListNext);
            HubIpConfiguration ipConfigModel = null;
            if (ipConfigList.Count > 0)
            {
                ipConfigModel = ipConfigList[0];
            }
            var ipconfig = NetworkResourceManagerProfile.Mapper.Map<CNM.PSHubIpConfiguration>(ipConfigModel);
            virtualHubModel.IpConfigurations = new List<CNM.PSHubIpConfiguration>();
            virtualHubModel.IpConfigurations.Add(ipconfig);
        }
    }
}