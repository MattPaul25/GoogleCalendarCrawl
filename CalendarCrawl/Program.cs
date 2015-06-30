using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CalendarCrawl;

namespace CalendarQuickstart
{
    class Program
    {
        static void Main()
        {
            var calInfo = new CalendarConnect();
            Console.ReadLine();
        }
    }
    class CalendarConnect
    {
        private string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        private string ApplicationName = "Google Calendar API Quickstart";
        public CalendarConnect()
        {
            getCalendarInfo();
        }
        private void getCalendarInfo()
        {
            UserCredential credential = getCredentials();
            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            CreateAbsenseObjects(request);
        }

        private void CreateAbsenseObjects(EventsResource.ListRequest request)
        {
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    List<string> attendees = new List<string>();
                    for (int i = 0; i < eventItem.Attendees.Count; i++)
                    {
                        attendees.Add(eventItem.Attendees[i].DisplayName);
                    }
                    var absObj = new AbsenceObject()
                    {
                        EventInvites = attendees,
                        EventCreator = eventItem.Creator.DisplayName,
                        EventStartTime = System.Convert.ToDateTime(eventItem.Start.DateTime.Value.ToLongTimeString()),
                        EventEndTime = System.Convert.ToDateTime(eventItem.End.DateTime.Value.ToLongTimeString())

                    };
                }
            }
            else
            {
                Console.WriteLine("No upcoming events found.");
            }
        }

        private UserCredential getCredentials()
        {
            UserCredential credential;
            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets, Scopes, "user", CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }
            return credential;
        }
    }
}
                    