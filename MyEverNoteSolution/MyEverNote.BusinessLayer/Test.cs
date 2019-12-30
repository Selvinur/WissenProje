using MyEverNote.DataAccessLayer;
using MyEverNote.DataAccessLayer.EntityFramework;
using MyEverNote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.BusinessLayer
{
    public class Test
    {
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<EverNoteUser> repo_user = new Repository<EverNoteUser>();
        public Test()
        {
            //DatabaseContext db = new DatabaseContext();
            ////db.Database.CreateIfNotExists();
            //db.EverNoteUsers.ToList();
            List<Category> categories = repo_category.list();
        }  
        public void InsertTest()
        {
            int result = repo_user.Insert(new EverNoteUser()
            {
                Name = "Medine",
                Surname="Yağışan",
                Email="mygsn@gmail.com",
                ActivateGuid=Guid.NewGuid(),
                IsActive =false,
                IsAdmin =false,
                UserName ="MdneY",
                Password="12345",
                CreatedOn =DateTime.Now,
                ModifiedOn =DateTime.Now.AddMinutes(5),
                ModifiedUserName ="MY"
            });
        }
        public void UpdateTest()
        {
            EverNoteUser user = repo_user.Find(x=>x.Name=="Medine");
            if (user != null)
            {
                user.UserName = "MdnY";
                repo_user.Update(user);
            }
        
}
        public void DeleteTest()
        {
            EverNoteUser user = repo_user.Find(x => x.UserName == "MdnY");
            if (user != null)
            {
                repo_user.Delete(user);
            }
        }
    }
}
