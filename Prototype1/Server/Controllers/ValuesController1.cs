using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class BedeemStoreController : ApiController
    {

        public BedeemStoreController()
        {
            string json;
            using (StreamReader r = new StreamReader("file.json"))
            {
                json = r.ReadToEnd();
            }
            
            _bedeems = JsonConvert.DeserializeObject<List<Bedeem>>(json);
            
        }

        List<Bedeem> _bedeems = new List<Bedeem>();

        // GET api/<controller>
        public List<Bedeem> Get()
        {
            return _bedeems;
        }

        // GET api/<controller>/5
        public List<Bedeem> Get(string id)
        {
            return _bedeems.Where(x => x.Target == id).ToList();
        }

        // POST api/<controller>
        public void Post([FromBody]Bedeem value)
        {
            _bedeems.Add(value);
            string json = JsonConvert.SerializeObject(_bedeems);
            using (StreamWriter r = new StreamWriter("file.json"))
            {
                 r.Write(json);
            }
            
        }
    }

    public class Bedeem
    {
        public TagCloudTag Tag;
        public BedeemScore Score;
        public string Target;
        public Hotness Lifecycle;
        public DateTime Created;
        public string CreatedBy;
    }

    public enum Hotness
    {
        Flash,DayFeeder,Classy,StoneColdThuth
    }

    public enum BedeemScore
    {
        BestInClass,NewsWorthy,SpamOfTheDay,Crap
    }
}