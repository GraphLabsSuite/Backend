using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphLabs.Backend.DAL;
using GraphLabs.Backend.Domain;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GraphLabs.Backend.Api.Controllers
{
    [ODataRoutePrefix("subjects")]
    public class SubjectsController : Controller
    {
        private readonly GraphLabsContext _db;

        public SubjectsController(GraphLabsContext context)
        {
            _db = context;
        }

        [EnableQuery]
        public IQueryable<Subject> Get()
        {
            return _db.Subjects;
        }

        [ODataRoute("({key})")]
        [EnableQuery]
        public SingleResult<Subject> Get(long key)
        {
            return SingleResult.Create(_db.Subjects.Where(t => t.Id == key));
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var subject = new Subject();
            var id = TryExecute(() => (_db.Subjects.Last().Id) + 1, "Не удалось извлечь subject id");
            subject.Id = id;

            var json = await Request.GetBodyAsString();
            var jsonData = TryExecute(() => JObject.Parse(json), "Не удалось распарсить данные.");

            var name = TryExecute(() => jsonData["name"].Value<string>(), "Не удалось прочитать значение subject name");
            if (name == null || name == "")
            {
                throw new SubjectConvertException("Полученное имя предмета пусто");
            }
            subject.Name = name;

            var description = TryExecute(() => jsonData["description"].Value<string>(), "Не удалось прочитать значение subject description");
            if (description == null || description == "")
            {
                throw new SubjectConvertException("Полученное описание предмета пусто");
            }
            subject.Description = description;
            
            _db.Subjects.Add(subject);
            await _db.SaveChangesAsync();

            return Ok();
        }

        private class SubjectConvertException : Exception
        {
            public SubjectConvertException(string error) : base(error)
            {
            }

            public SubjectConvertException(string error, Exception inner) : base(error, inner)
            {
            }
        }

        private static T TryExecute<T>(Func<T> f, string errorMessage)
        {
            try
            {
                return f();
            }
            catch (Exception e)
            {
                throw new SubjectConvertException(errorMessage, e);
            }
        }
    }
}