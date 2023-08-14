using RecruitmentTask.Entities;

namespace RecruitmentTask.Data
{
    public class DataProviders
    {
        private readonly DataDbContext _dbContext;

        public DataProviders(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddData()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Categories.Any())
                {
                    var categories = CreateNewCategories();
                    _dbContext.Categories.AddRange(categories);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Contacts.Any())
                {
                    var contacts = CreateNewContacts();
                    _dbContext.Contacts.AddRange(contacts);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Contact> CreateNewContacts()
        {
            var contacts = new Contact[]
            {
               new Contact{Name="Jan", Surname="Kowalski", Email="Jan.Kowalski@gmail.com",PhoneNumber = 123456789,CategoryId=1},
               new Contact{Name="Adam", Surname="Nowak", Email="Adam.Nowak@gmail.com", PhoneNumber = 123456789,CategoryId=2},
               new Contact{Name="Anna", Surname="Kowalska", Email="Anna.Kowalska@gmail.com",PhoneNumber = 123456789,CategoryId=3},
            };

            return contacts;
        }

        private IEnumerable<Category> CreateNewCategories()
        {
            var category = new Category[]
            {
               new Category{CategoryId=1,Name="Służbowy"},    
               new Category{CategoryId=2,Name="Prywatny"},
               new Category{CategoryId=3,Name="Inny"}
            };

            return category;
        }
    }
}