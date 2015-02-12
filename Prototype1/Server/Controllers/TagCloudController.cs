using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers
{
    public class TagCloudController : ApiController
    {
        List<TagCloudTag> _tags;
        public TagCloudController()
        {
            _tags = new List<TagCloudTag>();
            //_tags.Add(new TagCloudTag(1, "Nyheter", "Samfunn","Aktuelt"));
            //_tags.Add(new TagCloudTag(2, "Wrestling", "Sport","Entertainment","Show","Professional"));
            //_tags.Add(new TagCloudTag(3, "Development", "Software", "IT", "Programming", "Computer"));

            DataTable table;

            FileStream stream = File.Open("c:\\users\\oddhelge\\documents\\visual studio 2013\\Projects\\TagCloud\\WebApplication1\\App_Data\\tags.xlsx", FileMode.Open, FileAccess.Read);
            using (IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                excelReader.IsFirstRowAsColumnNames = false;
                table = excelReader.AsDataSet().Tables[0]; ;
                excelReader.Close();
            }

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var tag = new TagCloudTag();
                for(int j =0;j<table.Columns.Count;j++)
                {
                    if (j == 0)
                        tag.Id = Convert.ToInt32(table.Rows[i][j]);
                    else if (j == 1)
                        tag.Word = (string)table.Rows[i][j];
                    else
                        if(table.Rows[i][j] is System.String )
                            tag.RelatedWords.Add((string)table.Rows[i][j]);
                }
                _tags.Add(tag);
            }
               
        }


      

        // GET: api/TagCloud
        public List<TagCloudTag> Get()
        {
            return _tags;
        }

        // GET: api/TagCloud/5
        public TagCloudTag Get(int id)
        {
            foreach(var item in _tags)
            {
                if (item.Id == id)
                    return item;
            }
            return null;
        }

        [HttpGet]
        public List<TagCloudTag> Get(string name)
        {
            var returns = new List<TagCloudTag>();
            foreach (var item in _tags)
            {
                if (item.Word.ToLower().Contains(name.ToLower()))
                    if(!returns.Contains(item))
                        returns.Add(item);
                foreach(var related in item.RelatedWords)
                {
                    if (related.ToLower().Contains(name.ToLower()))
                        if (!returns.Contains(item))
                            returns.Add(item);
                }
            }
            return returns;
 
        }

      
      
    }

    public class TagCloudTag
    {
        public TagCloudTag() { }

        public TagCloudTag(int id, string word, string linkedword = "", string linkedword2 = "", string linkedword3 = "", string linkedword4 = "")
        {
            Id = id;
            Word = word;
            if (!string.IsNullOrWhiteSpace(linkedword)) RelatedWords.Add(linkedword);
            if (!string.IsNullOrWhiteSpace(linkedword2)) RelatedWords.Add(linkedword2);
            if (!string.IsNullOrWhiteSpace(linkedword3)) RelatedWords.Add(linkedword3);
            if (!string.IsNullOrWhiteSpace(linkedword4)) RelatedWords.Add(linkedword4);

        }

        public int Id;
        public string Word;
        public List<string> RelatedWords = new List<string>();
    }
}
