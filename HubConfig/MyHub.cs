using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SingalRAngular1.EFModels;
using SingalRAngular1.HubModels;

namespace SingalRAngular1.HubConfig
{
    public class MyHub: Hub
    {
        //2-Added
        //SignalrContext is a classname
        private readonly SignalrContext ctx;

        public MyHub(SignalrContext context)
        {
            ctx = context;
        }

        //2-Added
        public async Task authMe(PersonInfo personinfo)
        {
            string currSignalrID = Context.ConnectionId;
            Person tempPerson = ctx.Person.Where(p => p.Username == personinfo.userName && p.Password == personinfo.password)
                .SingleOrDefault();
            //"Where" filtered collections(list/array) and return based on criteria/condition given
            //SingleOrDefault() return the value if the collections(list/array) contains 0(default) or 1(single) index
            if (tempPerson != null) //if credentials are correct
            {
                Console.WriteLine("\n" + tempPerson.Name + " logged in" + "\nSignalrID: " + currSignalrID);

                Connection currUser = new Connection
                {
                    PersonId = tempPerson.Id,
                    SignalrId = currSignalrID,
                    TimeStamp = DateTime.Now
                };
                await ctx.Connections.AddAsync(currUser);
                ctx.SaveChanges();
                await Clients.Caller.SendAsync("authMeResponseSuccess", tempPerson);
            }
            else
            {
                await Clients.Caller.SendAsync("authMeResponseFail");
            }
        }
        /*
        //1-added
        public async Task askServer(string someTextFromClient)
        {
            string tempString;
            //1-added
            //this program triggered by the function from client "signalrservice.ts" askServer()
            //open your console to check the msg
            if (someTextFromClient == "hey")
            {
                tempString = "message was 'hey'";
            }
            else
            {
                tempString = "message was something else";

            }

            await Clients.Clients(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        }
        */
    }
}

//https://www.youtube.com/watch?v=QENOe4EMIQ0 REF