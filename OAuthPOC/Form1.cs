using Microsoft.Azure.Management.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using Microsoft.Rest;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Management.Logic.Models;
using Microsoft.WindowsAzure.Management.ServiceBus;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace OAuthPOC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static string azureActiveDirectoryInstance = System.Configuration.ConfigurationManager.AppSettings["AzureActiveDirectoryInstance"];
        private static string tenantID = System.Configuration.ConfigurationManager.AppSettings["TenantID"];
        private static string clientID = System.Configuration.ConfigurationManager.AppSettings["ClientID"];
        private static string appKey = System.Configuration.ConfigurationManager.AppSettings["AppKey"];
        private static string resource = System.Configuration.ConfigurationManager.AppSettings["Resource"];
        private static string subscriptionID = System.Configuration.ConfigurationManager.AppSettings["SubscriptionID"];

        static string authority = azureActiveDirectoryInstance + tenantID;

        private void button1_Click(object sender, EventArgs e)
        {
            using (LogicManagementClient client = GetLogicManagementClient())
            {
                var workflowPage = client.Workflows.ListBySubscription();
                List<Workflow> workflows = workflowPage.ToList();

                foreach (var workflow in workflows)
                {
                    listBox1.Items.Add(workflow.Name);
                }
            }
        }

        private string GetToken()
        {
            var httpClient = new HttpClient();
            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(clientID, appKey);
            var result = authContext.AcquireToken(resource, clientCredential);
            return result.AccessToken;
        }

        private string GetAuthToken()
        {
            var httpClient = new HttpClient();
            var authContext = new AuthenticationContext(authority);
            var clientCredential = new ClientCredential(clientID, appKey);
            var result = authContext.AcquireToken(resource, clientCredential);
            var token = result.CreateAuthorizationHeader();
            return token;
        }

        private LogicManagementClient GetLogicManagementClient()
        {
            var credentials = new TokenCredentials(GetToken());
            var client = new LogicManagementClient(credentials);
            client.SubscriptionId = subscriptionID;
            return client;
        }

    }
}
